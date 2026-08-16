Imports System.Collections.Concurrent
Imports System.IO
Imports Microsoft.VisualBasic.App
Imports Microsoft.VisualBasic.Serialization.JSON
Imports ClusterShared

''' <summary>
''' 头结点任务调度器：维护数据块队列、节点心跳、重试逻辑与失败日志提取。
''' 所有状态均为内存态，配合 SMB 文件系统作为数据与日志中枢。
''' </summary>
Public Class Scheduler

    Private ReadOnly cfg As Config
    Private ReadOnly smb As SmbPaths

    ''' <summary>待分发数据块队列。</summary>
    Private ReadOnly queue As New ConcurrentQueue(Of TaskBlock)()

    ''' <summary>运行中数据块：blockId -> block。</summary>
    Private ReadOnly running As New ConcurrentDictionary(Of String, TaskBlock)()

    ''' <summary>已完成数据块数。</summary>
    Private completed As Integer = 0

    ''' <summary>失败（重试达上限）数据块。</summary>
    Private ReadOnly failed As New ConcurrentDictionary(Of String, FailureInfo)()

    ''' <summary>已提交任务数。</summary>
    Private totalJobs As Integer = 0

    ''' <summary>节点心跳：nodeId -> 最近心跳。</summary>
    Private ReadOnly heartbeats As New ConcurrentDictionary(Of String, NodeHeartbeat)()

    ''' <summary>滚动日志流（最近 200 条）。</summary>
    Private ReadOnly logBuffer As New ConcurrentQueue(Of String)()

    ''' <summary>线程安全锁，用于聚合计数。</summary>
    Private ReadOnly lockObj As New Object()

    Sub New(cfg As Config)
        Me.cfg = cfg
        Me.smb = New SmbPaths(cfg.smbRoot)
    End Sub

    ' ============ 任务提交与拆分 ============

    ''' <summary>
    ''' 提交一个新作业，将输入文件按 chunkSize 拆分为独立数据块并写入 SMB。
    ''' </summary>
    Public Function SubmitJob(submit As JobSubmit) As String
        Dim jobId = Guid.NewGuid().ToString("N")
        Call smb.EnsureJobDirs(jobId)

        Dim chunkSize = If(submit.chunkSize <= 0, 1024 * 1024L, submit.chunkSize)
        Dim inputs = If(submit.inputFiles, New String() {})

        If inputs.Length = 0 Then
            ' 没有输入文件时仍创建一个占位块，便于演示纯计算方法。
            Call EnqueueBlock(jobId, submit, Nothing, chunkSize)
        Else
            For Each file In inputs
                If Not File.Exists(file) Then
                    Call AppendLog($"[warn] 输入文件不存在，已跳过: {file}")
                    Continue For
                End If
                Call SplitFile(jobId, submit, file, chunkSize)
            Next
        End If

        SyncLock lockObj
            totalJobs += 1
        End SyncLock

        Call AppendLog($"[submit] 新任务 {jobId} 已提交，输入文件 {inputs.Length} 个。")
        Return jobId
    End Function

    Private Sub SplitFile(jobId As String, submit As JobSubmit, file As String, chunkSize As Long)
        Dim buffer(chunkSize - 1) As Byte
        Dim blockIndex = 0

        Using fs = File.OpenRead(file)
            Dim read As Integer
            Dim offset = 0

            While (read = fs.Read(buffer, offset, CInt(Math.Min(chunkSize, fs.Length - fs.Position)))) > 0
                Dim guid = Guid.NewGuid().ToString("N")
                Dim blockFile = smb.BlockPath(jobId, guid)
                Using out = File.OpenWrite(blockFile)
                    Call out.Write(buffer, 0, read)
                End Using
                Call EnqueueBlock(jobId, submit, guid, chunkSize)
                blockIndex += 1
            End While
        End Using

        Call AppendLog($"[split] 文件 {Path.GetFileName(file)} 拆分为 {blockIndex} 块。")
    End Sub

    Private Sub EnqueueBlock(jobId As String, submit As JobSubmit, blockGuid As String, chunkSize As Long)
        ' 当没有输入文件时，blockGuid 为 Nothing，表示纯计算方法块。
        Dim guid = If(blockGuid, Guid.NewGuid().ToString("N"))

        Dim block As New TaskBlock With {
            .blockId = guid,
            .jobId = jobId,
            .assemblyPath = submit.assemblyPath,
            .methodName = submit.methodName,
            .jobRoot = smb.JobRoot(jobId),
            .retryCount = 0
        }
        queue.Enqueue(block)
    End Sub

    ' ============ 节点拉取任务 ============

    ''' <summary>
    ''' 节点拉取一个待处理数据块；若无可用块返回 Nothing。
    ''' </summary>
    Public Function PullBlock(nodeId As String) As TaskBlock
        Dim block As TaskBlock = Nothing

        If queue.TryDequeue(block) Then
            Dim existing As TaskBlock = Nothing
            If running.TryGetValue(block.blockId, existing) Then
                block.retryCount = existing.retryCount
            End If
            running(block.blockId) = block
            Call AppendLog($"[pull] 节点 {nodeId} 领取块 {block.blockId} (job={block.jobId})。")
            Return block
        End If

        Return Nothing
    End Function

    ' ============ 心跳 ============

    Public Sub ReceiveHeartbeat(hb As NodeHeartbeat)
        heartbeats(hb.nodeId) = hb
        If Not String.IsNullOrEmpty(hb.log) Then
            Call AppendLog($"[{hb.nodeId}] {hb.log}")
        End If
    End Sub

    ' ============ 回执 ============

    ''' <summary>
    ''' 节点报告数据块计算完成。
    ''' </summary>
    Public Sub ReportDone(result As TaskResult)
        Dim removedBlock As TaskBlock = Nothing
        Dim removed = running.TryRemove(result.blockId, removedBlock)

        If removed Then
            SyncLock lockObj
                completed += 1
            End SyncLock
            Call AppendLog($"[done] 块 {result.blockId} 由节点 {result.nodeId} 完成。")
        End If
    End Sub

    ''' <summary>
    ''' 节点报告数据块计算失败：重试或标记最终失败。
    ''' </summary>
    Public Sub ReportFailed(result As TaskResult)
        Dim block As TaskBlock = Nothing

        If Not running.TryRemove(result.blockId, block) Then
            ' 可能已被其他路径移除，忽略。
            Return
        End If

        block.retryCount += 1

        If block.retryCount <= cfg.maxRetry Then
            ' 重新入队重试。
            queue.Enqueue(block)
            Call AppendLog($"[retry] 块 {result.blockId} 失败，第 {block.retryCount} 次重试入队。")
        Else
            ' 重试达上限，标记失败并提取 SMB 日志。
            Dim info = ExtractFailure(block, result)
            failed(block.blockId) = info
            Call AppendLog($"[failed] 块 {result.blockId} 重试耗尽，标记为失败。")
        End If
    End Sub

    Private Function ExtractFailure(block As TaskBlock, result As TaskResult) As FailureInfo
        Dim logPath = smb.LogPath(block.jobId, block.blockId)
        Dim logText = ""

        If File.Exists(logPath) Then
            Try
                logText = File.ReadAllText(logPath)
            Catch ex As Exception
                logText = $"[无法读取日志] {ex.Message}"
            End Try
        End If

        Return New FailureInfo With {
            .blockId = block.blockId,
            .jobId = block.jobId,
            .retryCount = block.retryCount,
            .message = result.errorMessage,
            .stackTrace = result.stackTrace,
            .logPath = logPath
        }
    End Function

    ' ============ 日志 ============

    Private Sub AppendLog(line As String)
        Dim stamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        logBuffer.Enqueue($"[{stamp}] {line}")
        ' 限制缓冲长度。
        If logBuffer.Count > 200 Then
            Dim dummy As String = Nothing
            logBuffer.TryDequeue(dummy)
        End If
    End Sub

    ' ============ 状态快照 ============

    ''' <summary>
    ''' 生成仪表盘所需的集群状态快照。
    ''' </summary>
    Public Function Snapshot() As ClusterStatus
        Dim onlineThreshold = DateTime.UtcNow.AddMilliseconds(-cfg.pollInterval * 5).Ticks
        Dim nodeList As New List(Of NodeStatus)()
        Dim totalCores = 0

        For Each kv In heartbeats
            Dim hb = kv.Value
            Dim online = hb.timestamp >= onlineThreshold
            If online Then
                totalCores += Math.Max(1, hb.cores)
            End If

            nodeList.Add(New NodeStatus With {
                .nodeId = hb.nodeId,
                .online = online,
                .lastHeartbeat = hb.timestamp,
                .currentBlock = hb.currentBlock,
                .cores = hb.cores,
                .lastLog = hb.log
            })
        Next

        Dim onlineCount = nodeList.Count(Function(n) n.online)
        Dim runningCount = running.Count
        Dim pendingCount = queue.Count
        Dim failedList = failed.Values.ToArray()
        Dim logs = logBuffer.ToArray()

        Return New ClusterStatus With {
            .clusterName = cfg.clusterName,
            .smbRoot = cfg.smbRoot,
            .httpPort = cfg.httpPort,
            .pollInterval = cfg.pollInterval,
            .totalJobs = totalJobs,
            .pendingBlocks = pendingCount,
            .completedBlocks = completed,
            .failedBlocks = failedList.Length,
            .runningBlocks = runningCount,
            .onlineNodes = onlineCount,
            .totalCores = totalCores,
            .powerIndex = onlineCount * Math.Max(1, totalCores \ Math.Max(1, onlineCount)),
            .nodes = nodeList.ToArray(),
            .failures = failedList,
            .logs = logs,
            .serverTime = DateTime.UtcNow.Ticks
        }
    End Function
End Class
