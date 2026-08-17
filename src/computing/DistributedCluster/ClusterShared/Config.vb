Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ClusterShared

    ''' <summary>
    ''' 集群运行配置。命令行参数覆盖默认值。
    ''' </summary>
    Public Class Config

        ''' <summary>
        ''' 运行角色：headnode / node / worker。
        ''' </summary>
        <Opt("--mode", "-m")> Public Property mode As String = "headnode"

        ''' <summary>
        ''' HTTP 监听端口。
        ''' </summary>
        <Opt("--port", "-p")> Public Property httpPort As Integer = 8080

        ''' <summary>
        ''' SMB 挂载根目录（集群数据中枢，jobs 数据块存放处）。
        ''' </summary>
        <Opt("--smb", "-s")> Public Property smbRoot As String = "/mnt/cluster"

        ''' <summary>
        ''' 暴露给 web 管理页面的集群文件系统根目录（一般为 /mnt/ 下的 smb 共享目录）。
        ''' 任务提交时，dll 文件树与计算数据目录树仅能在该目录子树内浏览，避免暴露 jobs 内部目录。
        ''' </summary>
        <Opt("--smb-web-root", "-b")> Public Property webRoot As String = "/mnt/smb"

        ''' <summary>
        ''' 节点轮询间隔（毫秒）。
        ''' </summary>
        <Opt("--poll-interval", "-t")> Public Property pollInterval As Integer = 1000

        ''' <summary>
        ''' 数据块失败重试上限。
        ''' </summary>
        <Opt("--max-retry", "-r")> Public Property maxRetry As Integer = 3

        ''' <summary>
        ''' 集群显示名称。
        ''' </summary>
        <Opt("--name", "-n")> Public Property clusterName As String = "Darwinism Cluster"

        ''' <summary>
        ''' 远程关闭令牌（用于 OPTIONS /ctrl/kill）。
        ''' </summary>
        ''' <remarks>
        ''' 默认会生成随机令牌，即不允许通过url远程关闭集群
        ''' </remarks>
        <Opt("--shutdown-token", "-u")> Public Property shutdownToken As String = "darwinism-shutdown" & "-" & RandomExtensions.GetBytes(width:=64).ToHexString

        ''' <summary>
        ''' 头结点地址（节点守护进程使用，形如 http://headnode:8080）。
        ''' </summary>
        <Opt("--head", "-l")> Public Property headNodeUrl As String = "http://127.0.0.1:8080"

        ''' <summary>
        ''' 当前节点 id（默认机器名）。
        ''' </summary>
        <Opt("--id", "-i")> Public Property nodeId As String = Environment.MachineName & "-" & RandomExtensions.GetBytes(8).Select(Function(b) b.ToString("x2")).JoinBy("")

        ''' <summary>
        ''' worker 子进程的可执行文件路径，由 --worker= 参数显式指定；
        ''' 为空时 WorkerExecutable 会回退到默认值或环境变量。
        ''' </summary>
        <Opt("--worker", "-w")> Public Property workerExe As String = ""

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

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        ''' <summary>
        ''' 从命令行参数解析配置。支持 --key=value 形式。
        ''' </summary>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Parse(args As String()) As Config
            Return CommandLine.BuildFromArguments(args, NoSubCommand:=True).CreateOpts(Of Config)
        End Function
    End Class
End Namespace
