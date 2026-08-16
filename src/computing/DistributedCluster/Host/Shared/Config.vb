Imports System.IO

Namespace Shared

    ''' <summary>
    ''' 集群运行配置。命令行参数覆盖默认值。
    ''' </summary>
    Public Class Config

        ''' <summary>
        ''' 运行角色：headnode / node / worker。
        ''' </summary>
        Public Property mode As String = "headnode"

        ''' <summary>
        ''' HTTP 监听端口。
        ''' </summary>
        Public Property httpPort As Integer = 8080

        ''' <summary>
        ''' SMB 挂载根目录。
        ''' </summary>
        Public Property smbRoot As String = "/mnt/cluster"

        ''' <summary>
        ''' 节点轮询间隔（毫秒）。
        ''' </summary>
        Public Property pollInterval As Integer = 1000

        ''' <summary>
        ''' 数据块失败重试上限。
        ''' </summary>
        Public Property maxRetry As Integer = 3

        ''' <summary>
        ''' 集群显示名称。
        ''' </summary>
        Public Property clusterName As String = "Darwinism Cluster"

        ''' <summary>
        ''' 远程关闭令牌（用于 OPTIONS /ctrl/kill）。
        ''' </summary>
        Public Property shutdownToken As String = "darwinism-shutdown"

        ''' <summary>
        ''' 头结点地址（节点守护进程使用，形如 http://headnode:8080）。
        ''' </summary>
        Public Property headNodeUrl As String = "http://127.0.0.1:8080"

        ''' <summary>
        ''' 当前节点 id（默认机器名）。
        ''' </summary>
        Public Property nodeId As String = Environment.MachineName

        ''' <summary>
        ''' worker 子进程（当前 exe）的完整路径，用于 Process.Start。
        ''' </summary>
        Public ReadOnly Property WorkerExecutable As String
            Get
                Return Process.GetCurrentProcess().MainModule.FileName
            End Get
        End Property

        ''' <summary>
        ''' 从命令行参数解析配置。支持 --key=value 形式。
        ''' </summary>
        Public Shared Function Parse(args As String()) As Config
            Dim cfg As New Config()

            For Each arg As String In args
                If Not arg.StartsWith("--") Then
                    Continue For
                End If

                Dim kv = arg.Substring(2).Split(New Char() {"="c}, 2)
                Dim key = kv(0).ToLower()
                Dim value = If(kv.Length > 1, kv(1), "")

                Select Case key
                    Case "mode" : cfg.mode = value
                    Case "port" : Integer.TryParse(value, cfg.httpPort)
                    Case "smb" : cfg.smbRoot = value
                    Case "poll" : Integer.TryParse(value, cfg.pollInterval)
                    Case "retry" : Integer.TryParse(value, cfg.maxRetry)
                    Case "name" : cfg.clusterName = value
                    Case "token" : cfg.shutdownToken = value
                    Case "head" : cfg.headNodeUrl = value.TrimEnd("/"c)
                    Case "node" : cfg.nodeId = value
                End Select
            Next

            Return cfg
        End Function
    End Class
End Namespace
