Imports System.Threading
Imports Flute.Http.Configurations
Imports Flute.Http.Core
Imports ClusterShared

Module Program

    Sub Main(args As String())
        Dim cfg = Config.Parse(args)
        Console.WriteLine($"Darwinism Distributed Cluster :: mode={cfg.mode}")

        Select Case cfg.mode.ToLower
            Case "headnode"
                Call RunHeadNode(cfg)
            Case "node"
                Call RunNode(cfg)
            Case "worker"
                ' worker 由节点守护进程通过 Process.Start 启动，参数为：
                ' --mode=worker {blockId} {jobId} {assemblyPath} {methodName} {jobRoot}
                Environment.ExitCode = ReflectHost.Run(args)
            Case Else
                Console.WriteLine("未知模式，请使用 --mode=headnode|node|worker")
                Console.WriteLine("示例:")
                Console.WriteLine("  管理头结点: Host.exe --mode=headnode --port=8080 --smb=/mnt/cluster")
                Console.WriteLine("  计算节点:   Host.exe --mode=node --head=http://headnode:8080 --node=worker1")
                Console.WriteLine("  反射 worker: Host.exe --mode=worker <blockId> <jobId> <assembly> <method> <jobRoot>")
        End Select
    End Sub

    ''' <summary>
    ''' 启动头结点：HttpRouter 注册 ClusterController，HttpSocket 监听并提供仪表盘。
    ''' </summary>
    Private Sub RunHeadNode(cfg As Config)
        Dim scheduler = New Scheduler(cfg)
        Dim controller = New ClusterController(scheduler, cfg)
        Dim router As New HttpRouter(controller)

        Dim settings As New Configuration With {
            .shutdown_token = cfg.shutdownToken,
            .silent = False
        }

        Dim socket As New HttpSocket(AddressOf router.AppHandler, cfg.httpPort, configs:=settings)

        Console.WriteLine($"[headnode] 监听端口 {cfg.httpPort}，SMB 根 {cfg.smbRoot}")
        Console.WriteLine($"[headnode] 仪表盘: http://localhost:{cfg.httpPort}/")
        Console.WriteLine($"[headnode] 远程关闭: OPTIONS /ctrl/kill (X-Shutdown-Token: {cfg.shutdownToken})")

        AddHandler Console.CancelKeyPress, Sub(s, e)
                                                 e.Cancel = True
                                                 Call socket.Shutdown()
                                             End Sub

        ' Run() 会阻塞当前线程直到服务器关闭。
        Call socket.Run()
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
