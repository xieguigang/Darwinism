Imports System.IO
Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Flute.Http.Configurations
Imports Flute.Http.Core
Imports Flute.Http.FileSystem

Module Program

    Sub Main(args As String())
        Dim cfg = Config.Parse(args)
        Console.WriteLine($"Darwinism Distributed Cluster :: HeadNode")

        Call RunHeadNode(cfg)
    End Sub

    ''' <summary>
    ''' 启动头结点：HttpRouter 注册 ClusterController，HttpSocket 监听并提供仪表盘。
    ''' </summary>
    Private Sub RunHeadNode(cfg As Config)
        Dim scheduler = New Scheduler(cfg)
        Dim controller = New ClusterController(scheduler, cfg)
        Dim wfs As New WebFileSystemListener(Path.Combine(App.HOME, "wwwroot"))
        Dim router As New HttpRouter(controller)
        Dim settings As New Configuration With {
            .shutdown_token = cfg.shutdownToken,
            .silent = False
        }

        Dim socket As New HttpSocket(router.MountFs(wfs), cfg.httpPort, configs:=settings)

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
End Module
