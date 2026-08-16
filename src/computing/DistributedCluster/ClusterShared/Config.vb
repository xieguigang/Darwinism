Imports System.IO
Imports Microsoft.VisualBasic.Math

Namespace ClusterShared

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
        Public Property nodeId As String = Environment.MachineName & "-" & RandomExtensions.GetBytes(8).Select(Function(b) b.ToString("x2")).JoinBy("")

        ''' <summary>
        ''' worker 子进程的可执行文件路径，由 --worker= 参数显式指定；
        ''' 为空时 WorkerExecutable 会回退到默认值或环境变量。
        ''' </summary>
        Public Property workerExe As String = ""

        ''' <summary>
        ''' worker 子进程可执行文件的完整路径，用于 Process.Start。
        ''' 解析顺序：--worker 参数 → WORKER_EXE 环境变量 → 与当前 exe 同目录的 worker(.exe)。
        ''' 若均不可定位，返回同目录下的兜底路径，由调用方检测存在性并报错。
        ''' </summary>
        Public ReadOnly Property WorkerExecutable As String
            Get
                ' 1. 显式命令行参数
                If Not String.IsNullOrWhiteSpace(workerExe) AndAlso File.Exists(workerExe) Then
                    Return workerExe
                End If

                ' 2. 环境变量
                Dim env = Environment.GetEnvironmentVariable("WORKER_EXE")
                If Not String.IsNullOrWhiteSpace(env) AndAlso File.Exists(env) Then
                    Return env
                End If

                ' 3. 与当前进程同目录下的 worker 可执行文件
                Dim baseDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)
                For Each candidate In {Path.Combine(baseDir, "worker.exe"), Path.Combine(baseDir, "worker")}
                    If File.Exists(candidate) Then
                        Return candidate
                    End If
                Next

                ' 兜底：返回同目录下的默认文件名，由调用方检测并给出明确错误
                Return Path.Combine(baseDir, "worker.exe")
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
                    Case "worker" : cfg.workerExe = value
                End Select
            Next

            Return cfg
        End Function
    End Class
End Namespace
