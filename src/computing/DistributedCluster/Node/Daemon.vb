Imports System.Net.Http
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Microsoft.VisualBasic.Serialization.JSON

''' <summary>
''' 计算节点守护进程：每秒向头结点轮询拉取任务，领取后启动 worker 子进程执行，
''' 并依据回执结果继续下一轮轮询。空闲时发送携带资源指标的心跳保持存活。
''' </summary>
Public Class Daemon

    Private ReadOnly cfg As Config
    Private ReadOnly runner As ProcessRunner
    Private ReadOnly metrics As INodeMetrics
    Private ReadOnly http As New HttpClient()
    Private running As Boolean = True

    Sub New(cfg As Config)
        Me.cfg = cfg
        Me.runner = New ProcessRunner(cfg)
        Me.metrics = CreateMetricsReader()
    End Sub

    ''' <summary>
    ''' 按运行时操作系统选择指标采集实现（Windows / Linux 双 API 兼容）。
    ''' </summary>
    Private Shared Function CreateMetricsReader() As INodeMetrics
        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            Return New WindowsMetricsReader()
        ElseIf RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            Return New LinuxMetricsReader()
        Else
            ' 其他平台（如 macOS）暂退回 Linux 解析实现，采集失败会安全降级。
            Console.WriteLine("[node] 未识别操作系统，尝试使用 Linux 指标采集器。")
            Return New LinuxMetricsReader()
        End If
    End Function

    ''' <summary>
    ''' 启动守护主循环，直到进程被终止。
    ''' </summary>
    Public Sub Start()
        Console.WriteLine($"[node] 守护进程启动，节点 id={cfg.nodeId}，头结点={cfg.headNodeUrl}")
        Console.WriteLine($"[node] 每秒轮询间隔={cfg.pollInterval}ms，SMB 根={cfg.smbRoot}")

        While running
            Try
                ' 每轮先采集一次节点资源指标（两次采样求差，首轮为 0）。
                Dim m = metrics.Sample()

                Dim block = PullTask()

                If block Is Nothing Then
                    ' 空闲：发送一次携带资源指标的心跳保持存活。
                    Call SendHeartbeat(m, "idle")
                Else
                    Console.WriteLine($"[node] 领取数据块 {block.blockId} (job={block.jobId})，启动 worker。")
                    Call runner.RunBlock(block)
                    Console.WriteLine($"[node] 数据块 {block.blockId} 处理完毕。")
                    ' 工作块结束后也上报一次指标（携带当前计算块标识）。
                    Call SendHeartbeat(m, block.blockId)
                End If
            Catch ex As Exception
                Console.Error.WriteLine($"[node] 轮询异常: {ex.Message}")
            End Try

            Thread.Sleep(cfg.pollInterval)
        End While
    End Sub

    ''' <summary>
    ''' 向头结点拉取一个待处理数据块。
    ''' </summary>
    Private Function PullTask() As TaskBlock
        Dim url = $"{cfg.headNodeUrl}/api/task/pull?nodeid={Uri.EscapeDataString(cfg.nodeId)}&cores={Environment.ProcessorCount}"
        Dim json = http.GetStringAsync(New Uri(url)).Result

        If json.IndexOf("""available"":false", StringComparison.OrdinalIgnoreCase) >= 0 Then
            Return Nothing
        End If

        Return json.LoadJSON(Of TaskBlock)()
    End Function

    ''' <summary>
    ''' 构造 NodeHeartbeat 并通过 POST JSON Body 上报到统一心跳接口。
    ''' 首次采样时 cpuUsage / 网络速率可能为 0，属正常（需两次采样求差）。
    ''' </summary>
    Private Sub SendHeartbeat(m As NodeMetrics, log As String)
        Try
            Dim hb As New NodeHeartbeat With {
                .nodeId = cfg.nodeId,
                .timestamp = DateTime.UtcNow.Ticks,
                .currentBlock = If(log = "idle", Nothing, log),
                .log = log,
                .cores = m.cpuCores,
                .ipAddress = m.ipAddress,
                .machineName = m.machineName,
                .cpuUsage = m.cpuUsage,
                .totalMemoryMB = m.totalMemoryMB,
                .memoryUsage = m.memoryUsage,
                .netUploadRate = m.netUploadRate,
                .netDownloadRate = m.netDownloadRate
            }

            Dim content = New StringContent(hb.GetJson(), Text.Encoding.UTF8, "application/json")
            Call http.PostAsync(New Uri($"{cfg.headNodeUrl}/api/heartbeat"), content).Wait(cfg.pollInterval)
        Catch ex As Exception
            Console.Error.WriteLine($"[node] 心跳上报失败: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' 请求停止主循环（通常由 Ctrl+C 触发）。
    ''' </summary>
    Public Sub [Stop]()
        running = False
    End Sub
End Class
