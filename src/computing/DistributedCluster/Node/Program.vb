Imports System.IO
Imports System.Threading
Imports ClusterShared

Module Program

    Sub Main(args As String())
        Dim cfg = Config.Parse(args)

        ' 解析 worker 可执行文件路径（支持 --worker 参数 / WORKER_EXE 环境变量 / 同目录默认）。
        Dim workerPath = cfg.WorkerExecutable

        If Not File.Exists(workerPath) Then
            Console.Error.WriteLine($"[node] 无法定位 worker 可执行文件: {workerPath}")
            Console.Error.WriteLine($"[node] 请通过 --worker=<path> 参数或 WORKER_EXE 环境变量指定 worker 路径。")
            Environment.ExitCode = 2
            Return
        End If

        Console.WriteLine($"Darwinism Distributed Cluster :: Node (worker={Path.GetFileName(workerPath)})")
        Call RunNode(cfg)
    End Sub

    ''' <summary>
    ''' 启动计算节点守护进程：每秒轮询头结点拉取任务并执行。
    ''' </summary>
    Private Sub RunNode(cfg As Config)
        Dim daemon = New Daemon(cfg)

        AddHandler Console.CancelKeyPress, Sub(s, e)
                                               e.Cancel = True
                                               Call daemon.Stop()
                                           End Sub

        Call daemon.Start()
    End Sub
End Module
