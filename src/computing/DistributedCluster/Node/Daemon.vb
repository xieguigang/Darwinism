Imports System.Net.Http
Imports System.Threading
Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Microsoft.VisualBasic.Serialization.JSON

''' <summary>
''' 计算节点守护进程：每秒向头结点轮询拉取任务，领取后启动 worker 子进程执行，
''' 并依据回执结果继续下一轮轮询。空闲时仅发送心跳保持存活。
''' </summary>
Public Class Daemon

    Private ReadOnly cfg As Config
    Private ReadOnly runner As ProcessRunner
    Private ReadOnly http As New HttpClient()
    Private running As Boolean = True

    Sub New(cfg As Config)
        Me.cfg = cfg
        Me.runner = New ProcessRunner(cfg)
    End Sub

    ''' <summary>
    ''' 启动守护主循环，直到进程被终止。
    ''' </summary>
    Public Sub Start()
        Console.WriteLine($"[node] 守护进程启动，节点 id={cfg.nodeId}，头结点={cfg.headNodeUrl}")
        Console.WriteLine($"[node] 每秒轮询间隔={cfg.pollInterval}ms，SMB 根={cfg.smbRoot}")

        While running
            Try
                Dim block = PullTask()

                If block Is Nothing Then
                    ' 空闲：发送一次心跳保持存活。
                    Call SendIdleHeartbeat()
                Else
                    Console.WriteLine($"[node] 领取数据块 {block.blockId} (job={block.jobId})，启动 worker。")
                    Call runner.RunBlock(block)
                    Console.WriteLine($"[node] 数据块 {block.blockId} 处理完毕。")
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

    Private Sub SendIdleHeartbeat()
        Try
            Dim url = $"{cfg.headNodeUrl}/api/heartbeat?nodeid={Uri.EscapeDataString(cfg.nodeId)}&currentblock=&cores={Environment.ProcessorCount}&log=idle"
            Call http.PostAsync(New Uri(url), Nothing).Wait(cfg.pollInterval)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' 请求停止主循环（通常由 Ctrl+C 触发）。
    ''' </summary>
    Public Sub [Stop]()
        running = False
    End Sub
End Class
