Imports System.Collections.Concurrent
Imports System.IO
Imports Darwinism.HPC.DistributedCluster.[Shared]
Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Microsoft.VisualBasic.Serialization.JSON

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
    ''' 当指定了 datasetDir 时，优先解析其目录内的 dataset.ini / dataset.json 输入源，
    ''' 将其展开为多个数据块（沿用现有 chunk 拆分与 SMB 写入机制），Node/Worker 契约不变。
    ''' </summary>
    Public Function SubmitJob(submit As JobSubmit) As String
        Dim jobId = Guid.NewGuid().ToString("N")
        Call smb.EnsureJobDirs(jobId)

        Dim chunkSize = If(submit.chunkSize <= 0, 1024 * 1024L, submit.chunkSize)

        ' 优先处理 dataset 输入源
        If Not String.IsNullOrEmpty(submit.datasetDir) Then
            Dim dir = ResolveDatasetDir(submit.datasetDir)

            If String.IsNullOrEmpty(dir) Then
                Call AppendLog($"[submit] 数据输入目录不存在: {submit.datasetDir}")
            Else
                Dim expanded = ExpandDataset(jobId, submit, dir, chunkSize)

                If expanded > 0 Then
                    SyncLock lockObj
                        totalJobs += 1
                    End SyncLock
                    Call AppendLog($"[submit] 新任务 {jobId} 已提交，dataset 展开 {expanded} 个数据块。")
                    Return jobId
                Else
                    Call AppendLog($"[warn] dataset 目录 {dir} 未解析出任何输入数据块。")
                End If
            End If
        End If

        Dim inputs = If(submit.inputFiles, New String() {})

        If inputs.Length = 0 Then
            ' 没有输入文件时仍创建一个占位块，便于演示纯计算方法。
            Call EnqueueBlock(jobId, submit, Nothing, chunkSize)
        Else
            For Each file In inputs
                If Not System.IO.File.Exists(file) Then
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

    ''' <summary>
    ''' 将相对 webRoot 的 dataset 目录路径解析为完整路径，越界或不存在返回空串。
    ''' </summary>
    Private Function ResolveDatasetDir(relOrFull As String) As String
        Dim root = System.IO.Path.GetFullPath(cfg.webRoot)
        Dim dir As String

        If System.IO.Path.IsPathRooted(relOrFull) Then
            dir = System.IO.Path.GetFullPath(relOrFull)
        Else
            dir = System.IO.Path.GetFullPath(System.IO.Path.Combine(root, relOrFull.TrimStart("/"c)))
        End If

        If Not dir.StartsWith(root, StringComparison.OrdinalIgnoreCase) OrElse Not System.IO.Directory.Exists(dir) Then
            Return ""
        End If

        Return dir
    End Function

    ''' <summary>
    ''' 解析 dataset 目录，展开为数据块入队，返回展开的数据块数量。
    ''' </summary>
    Private Function ExpandDataset(jobId As String, submit As JobSubmit, dir As String, chunkSize As Long) As Integer
        Dim type = If(submit.datasetType, "auto").ToLower()

        If type = "auto" Then
            If System.IO.File.Exists(System.IO.Path.Combine(dir, "dataset.json")) Then
                type = "json"
            ElseIf System.IO.File.Exists(System.IO.Path.Combine(dir, "dataset.ini")) Then
                type = "ini"
            Else
                Return 0
            End If
        End If

        If type = "json" Then
            Return SplitDatasetJson(jobId, submit, dir, chunkSize)
        ElseIf type = "ini" Then
            Return SplitDatasetIni(jobId, submit, dir, chunkSize)
        End If

        Return 0
    End Function

    ''' <summary>
    ''' 解析 dataset.ini：枚举匹配后缀的数据文件，每个文件按 chunkSize 拆分。
    ''' dataset.ini 示例：
    '''   [dataset]
    '''   ext=*.dat
    '''   description=任务描述
    ''' </summary>
    Private Function SplitDatasetIni(jobId As String, submit As JobSubmit, dir As String, chunkSize As Long) As Integer
        Dim iniPath = System.IO.Path.Combine(dir, "dataset.ini")
        Dim ext = "*.dat"
        Dim description = ""

        For Each line In System.IO.File.ReadAllLines(iniPath)
            Dim l = line.Trim()
            If l.Length = 0 OrElse l.StartsWith("["c) Then
                Continue For
            End If
            Dim eq = l.IndexOf("="c)
            If eq < 0 Then
                Continue For
            End If
            Dim key = l.Substring(0, eq).Trim().ToLower()
            Dim val = l.Substring(eq + 1).Trim()
            If key = "ext" Then
                ext = val
            ElseIf key = "description" Then
                description = val
            End If
        Next

        If Not ext.Contains("*"c) Then
            ext = "*." & ext.TrimStart("."c)
        End If

        Dim files = System.IO.Directory.GetFiles(dir, ext)
        Dim count = 0

        For Each f In files
            Call SplitFile(jobId, submit, f, chunkSize)
            count += 1
        Next

        Call AppendLog($"[dataset.ini] {description} 匹配到 {count} 个后缀 {ext} 数据文件。")
        Return count
    End Function

    ''' <summary>
    ''' 解析 dataset.json：将大文件按 chunks 的 offset/size 读取段写入 SMB 数据块，每个 chunk 一个 TaskBlock。
    ''' </summary>
    Private Function SplitDatasetJson(jobId As String, submit As JobSubmit, dir As String, chunkSize As Long) As Integer
        Dim jsonPath = System.IO.Path.Combine(dir, "dataset.json")
        Dim info = LoadJSON(Of DatasetJsonInfo)(System.IO.File.ReadAllText(jsonPath))

        If info Is Nothing OrElse String.IsNullOrEmpty(info.datafile) OrElse info.chunks Is Nothing Then
            Call AppendLog($"[dataset.json] 配置无效: {jsonPath}")
            Return 0
        End If

        Dim bigFile = System.IO.Path.Combine(dir, info.datafile)
        If Not System.IO.File.Exists(bigFile) Then
            Call AppendLog($"[dataset.json] 大文件不存在: {bigFile}")
            Return 0
        End If

        Dim count = 0

        Using fs = System.IO.File.OpenRead(bigFile)
            For Each c In info.chunks
                If c.size <= 0 Then
                    Continue For
                End If
                Dim buffer(CInt(Math.Min(c.size, 64 * 1024 * 1024L)) - 1) As Byte
                Dim remaining = c.size
                Dim copied = 0L

                fs.Seek(c.offset, SeekOrigin.Begin)

                While remaining > 0
                    Dim toRead = CInt(Math.Min(buffer.Length, remaining))
                    Dim read = fs.Read(buffer, 0, toRead)
                    If read <= 0 Then
                        Exit While
                    End If

                    Dim guid As String = System.Guid.NewGuid().ToString("N")
                    Dim blockFile = smb.BlockPath(jobId, guid)
                    Using out = System.IO.File.Create(blockFile)
                        Call out.Write(buffer, 0, read)
                    End Using
                    Call EnqueueBlock(jobId, submit, guid, chunkSize)
                    copied += read
                    remaining -= read
                End While

                count += 1
            Next
        End Using

        Call AppendLog($"[dataset.json] {info.description} 展开 {count} 个 chunk 数据块。")
        Return count
    End Function

    ''' <summary>
    ''' 预览指定 dataset 目录的数据输入源（供 web 页面惰性展示）。
    ''' </summary>
    Public Function PreviewDataset(dir As String) As DatasetPreview
        Dim preview As New DatasetPreview()

        If System.IO.File.Exists(System.IO.Path.Combine(dir, "dataset.json")) Then
            preview.kind = "json"
            Try
                preview.json = Path.Combine(dir, "dataset.json").ReadAllText.LoadJSON(Of DatasetJsonInfo)
            Catch ex As Exception
                preview.error = ex.Message
            End Try
            Return preview
        End If

        If System.IO.File.Exists(System.IO.Path.Combine(dir, "dataset.ini")) Then
            preview.kind = "ini"
            preview.ini = ParseIniPreview(dir)
            Return preview
        End If

        preview.kind = "none"
        Return preview
    End Function

    ''' <summary>
    ''' 解析 dataset.ini 供预览（提取后缀 + 描述 + 匹配文件清单）。
    ''' </summary>
    Private Function ParseIniPreview(dir As String) As DatasetIniInfo
        Dim iniPath = System.IO.Path.Combine(dir, "dataset.ini")
        Dim ext = "*.dat"
        Dim description = ""
        Dim lines = System.IO.File.ReadAllLines(iniPath)

        For Each line In lines
            Dim l = line.Trim()
            If l.Length = 0 OrElse l.StartsWith("["c) Then
                Continue For
            End If
            Dim eq = l.IndexOf("="c)
            If eq < 0 Then
                Continue For
            End If
            Dim key = l.Substring(0, eq).Trim().ToLower()
            Dim val = l.Substring(eq + 1).Trim()
            If key = "ext" Then
                ext = val
            ElseIf key = "description" Then
                description = val
            End If
        Next

        If Not ext.Contains("*"c) Then
            ext = "*." & ext.TrimStart("."c)
        End If

        Dim files = System.IO.Directory.GetFiles(dir, ext) _
                                .Select(Function(p) System.IO.Path.GetFileName(p)) _
                                .ToArray()

        Return New DatasetIniInfo With {
            .ext = ext,
            .description = description,
            .files = files
        }
    End Function

    Private Sub SplitFile(jobId As String, submit As JobSubmit, file As String, chunkSize As Long)
        Dim buffer(chunkSize - 1) As Byte
        Dim blockIndex = 0

        Using fs = System.IO.File.OpenRead(file)
            Dim read As Integer
            Dim offset = 0

            While (read = fs.Read(buffer, offset, CInt(Math.Min(chunkSize, fs.Length - fs.Position)))) > 0
                Dim guid As String = System.Guid.NewGuid().ToString("N")
                Dim blockFile = smb.BlockPath(jobId, guid)
                Using out = System.IO.File.OpenWrite(blockFile)
                    Call out.Write(buffer, 0, read)
                End Using
                Call EnqueueBlock(jobId, submit, guid, chunkSize)
                blockIndex += 1
            End While
        End Using

        Call AppendLog($"[split] 文件 {System.IO.Path.GetFileName(file)} 拆分为 {blockIndex} 块。")
    End Sub

    Private Sub EnqueueBlock(jobId As String, submit As JobSubmit, blockGuid As String, chunkSize As Long)
        ' 当没有输入文件时，blockGuid 为 Nothing，表示纯计算方法块。
        Dim guid As String = If(String.IsNullOrEmpty(blockGuid), System.Guid.NewGuid().ToString("N"), blockGuid)

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
        If hb Is Nothing OrElse String.IsNullOrEmpty(hb.nodeId) Then
            ' 拒绝空节点标识，避免 ConcurrentDictionary 写入空键异常。
            Return
        End If
        heartbeats(hb.nodeId) = hb
        If Not String.IsNullOrEmpty(hb.log) Then
            If hb.log <> Flags.idle Then
                Call AppendLog($"[{hb.nodeId}] {hb.log}")
            End If
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

        If System.IO.File.Exists(logPath) Then
            Try
                logText = System.IO.File.ReadAllText(logPath)
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
        Dim totalMemoryMB As Long = 0

        ' 标准算力参考基线：1 个标准节点取 64 逻辑核心 / 256 GB 内存。
        Const REF_CORES As Integer = 64
        Const REF_MEM_GB As Integer = 256

        For Each kv In heartbeats
            Dim hb = kv.Value
            Dim online = hb.timestamp >= onlineThreshold
            If online Then
                totalCores += Math.Max(1, hb.cores)
                totalMemoryMB += Math.Max(0, hb.totalMemoryMB)
            End If

            nodeList.Add(New NodeStatus With {
                .nodeId = hb.nodeId,
                .online = online,
                .lastHeartbeat = hb.timestamp,
                .currentBlock = hb.currentBlock,
                .cores = hb.cores,
                .lastLog = hb.log,
                .ipAddress = hb.ipAddress,
                .machineName = hb.machineName,
                .cpuUsage = hb.cpuUsage,
                .totalMemoryMB = hb.totalMemoryMB,
                .memoryUsage = hb.memoryUsage,
                .netUploadRate = hb.netUploadRate,
                .netDownloadRate = hb.netDownloadRate
            })
        Next

        Dim onlineCount = nodeList.Where(Function(n) n.online).Count()
        Dim runningCount = running.Count
        Dim pendingCount = queue.Count
        Dim failedList = failed.Values.ToArray()
        Dim logs = logBuffer.ToArray()

        ' 算力指数：综合总核心数与总物理内存，基准归一化后取几何平均。
        '   标准节点(64 核 / 256 GB) 指数 ≈ 100，便于横向比较集群规模。
        Dim cpuScore = CDbl(totalCores) / REF_CORES
        Dim memScore = (CDbl(totalMemoryMB) / 1024.0) / REF_MEM_GB
        Dim powerIndex = CInt(Math.Round(Math.Sqrt(cpuScore * memScore) * 100))

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
            .totalMemoryMB = totalMemoryMB,
            .powerIndex = powerIndex,
            .nodes = nodeList.ToArray(),
            .failures = failedList,
            .logs = logs,
            .serverTime = DateTime.UtcNow.Ticks
        }
    End Function
End Class
