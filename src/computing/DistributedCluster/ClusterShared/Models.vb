Imports System
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ClusterShared

    ''' <summary>
    ''' 计算数据块的运行时描述，由头结点拆分任务后写入队列并分发到计算节点。
    ''' </summary>
    Public Class TaskBlock

        ''' <summary>
        ''' 数据块的唯一标识 guid，用于定位 SMB 上的块/结果/日志文件。
        ''' </summary>
        Public Property blockId As String

        ''' <summary>
        ''' 所属任务 id。
        ''' </summary>
        Public Property jobId As String

        ''' <summary>
        ''' 待加载执行的 CLR assembly 在 SMB 上的完整路径。
        ''' </summary>
        Public Property assemblyPath As String

        ''' <summary>
        ''' 待反射调用的 CLR 方法名（形如 Namespace.Class.Method）。
        ''' </summary>
        Public Property methodName As String

        ''' <summary>
        ''' 任务根目录（SMB 上的 jobs/{jobId}）。
        ''' </summary>
        Public Property jobRoot As String

        ''' <summary>
        ''' 当前已重试次数。
        ''' </summary>
        Public Property retryCount As Integer

        ''' <summary>
        ''' 序列化本对象为 json 字符串。
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Class

    ''' <summary>
    ''' 计算节点向头结点上报的心跳信息，包含存活时间戳与当前工作块。
    ''' </summary>
    Public Class NodeHeartbeat

        ''' <summary>
        ''' 节点唯一 id（通常为机器名或节点名）。
        ''' </summary>
        Public Property nodeId As String

        ''' <summary>
        ''' 节点上报时间（UTC 时间刻度）。
        ''' </summary>
        Public Property timestamp As Long

        ''' <summary>
        ''' 节点当前正在计算的数据块 id，空闲为 null。
        ''' </summary>
        Public Property currentBlock As String

        ''' <summary>
        ''' 节点最近一次 stdout / 日志快照。
        ''' </summary>
        Public Property log As String

        ''' <summary>
        ''' 节点逻辑核心数，用于算力估算。
        ''' </summary>
        Public Property cores As Integer

        ''' <summary>
        ''' 节点 IP 地址（IPv4），用于仪表盘展示节点网络位置。
        ''' </summary>
        Public Property ipAddress As String

        ''' <summary>
        ''' 节点计算机名称，用于仪表盘展示节点身份。
        ''' </summary>
        Public Property machineName As String

        ''' <summary>
        ''' 当前 CPU 使用率（0-100，百分比）。由节点端两次采样求差计算后上报。
        ''' </summary>
        Public Property cpuUsage As Double

        ''' <summary>
        ''' 物理内存总量（单位 MB）。
        ''' </summary>
        Public Property totalMemoryMB As Long

        ''' <summary>
        ''' 当前内存使用率（0-100，百分比）。由节点端计算后上报。
        ''' </summary>
        Public Property memoryUsage As Double

        ''' <summary>
        ''' 网络上传速率（字节/秒），由节点端两次采样求差计算后上报。
        ''' </summary>
        Public Property netUploadRate As Double

        ''' <summary>
        ''' 网络下载速率（字节/秒），由节点端两次采样求差计算后上报。
        ''' </summary>
        Public Property netDownloadRate As Double

        Public Sub New()
            cores = Environment.ProcessorCount
        End Sub
    End Class

    ''' <summary>
    ''' 节点回执：数据块计算完成 / 失败。
    ''' </summary>
    Public Class TaskResult

        ''' <summary>
        ''' 完成的数据块 id。
        ''' </summary>
        Public Property blockId As String

        ''' <summary>
        ''' 所属任务 id。
        ''' </summary>
        Public Property jobId As String

        ''' <summary>
        ''' 执行节点 id。
        ''' </summary>
        Public Property nodeId As String

        ''' <summary>
        ''' 是否成功：True 表示 ExitCode=0。
        ''' </summary>
        Public Property success As Boolean

        ''' <summary>
        ''' 失败时从 SMB 日志提取的异常描述。
        ''' </summary>
        Public Property errorMessage As String

        ''' <summary>
        ''' 失败时从 SMB 日志提取的栈追踪。
        ''' </summary>
        Public Property stackTrace As String
    End Class

    ''' <summary>
    ''' 头结点对外暴露的集群状态快照，由仪表盘每 1-2 秒拉取。
    ''' </summary>
    Public Class ClusterStatus

        ''' <summary>
        ''' 集群名称。
        ''' </summary>
        Public Property clusterName As String

        ''' <summary>
        ''' SMB 根目录。
        ''' </summary>
        Public Property smbRoot As String

        ''' <summary>
        ''' HTTP 监听端口。
        ''' </summary>
        Public Property httpPort As Integer

        ''' <summary>
        ''' 心跳轮询间隔（毫秒）。
        ''' </summary>
        Public Property pollInterval As Integer

        ''' <summary>
        ''' 已经提交的任务总数。
        ''' </summary>
        Public Property totalJobs As Integer

        ''' <summary>
        ''' 队列中待处理的数据块数量。
        ''' </summary>
        Public Property pendingBlocks As Integer

        ''' <summary>
        ''' 已完成的数据块数量。
        ''' </summary>
        Public Property completedBlocks As Integer

        ''' <summary>
        ''' 失败（重试达上限）的数据块数量。
        ''' </summary>
        Public Property failedBlocks As Integer

        ''' <summary>
        ''' 当前正在计算的数据块数量。
        ''' </summary>
        Public Property runningBlocks As Integer

        ''' <summary>
        ''' 在线节点数量。
        ''' </summary>
        Public Property onlineNodes As Integer

        ''' <summary>
        ''' 集群总逻辑核心数（在线节点之和）。
        ''' </summary>
        Public Property totalCores As Integer

        ''' <summary>
        ''' 集群总物理内存（单位 MB，在线节点之和）。
        ''' </summary>
        Public Property totalMemoryMB As Long

        ''' <summary>
        ''' 集群算力指数。
        ''' 综合集群总逻辑核心数与总物理内存量，采用基准归一化后的几何平均：
        '''   cpuScore = totalCores / REF_CORES
        '''   memScore = totalMemoryMB / 1024 / REF_MEM_GB
        '''   powerIndex = Round(Sqrt(cpuScore * memScore) * 100)
        ''' 几何平均保证 CPU 与内存任一维度瓶颈都会拉低指数；×100 使标准配置（64 核 / 256 GB）≈ 100。
        ''' </summary>
        Public Property powerIndex As Integer

        ''' <summary>
        ''' 各节点实时状态。
        ''' </summary>
        Public Property nodes As NodeStatus()

        ''' <summary>
        ''' 失败数据块调试信息（含日志片段）。
        ''' </summary>
        Public Property failures As FailureInfo()

        ''' <summary>
        ''' 最近的日志流（节点心跳 / stdout 快照）。
        ''' </summary>
        Public Property logs As String()

        ''' <summary>
        ''' 当前服务器时间（UTC ticks），用于仪表盘显示。
        ''' </summary>
        Public Property serverTime As Long
    End Class

    ''' <summary>
    ''' 单个节点的精简状态（用于仪表盘节点列表）。
    ''' </summary>
    Public Class NodeStatus

        Public Property nodeId As String
        Public Property online As Boolean
        Public Property lastHeartbeat As Long
        Public Property currentBlock As String
        Public Property cores As Integer
        Public Property lastLog As String

        ''' <summary>
        ''' 节点 IP 地址（IPv4）。
        ''' </summary>
        Public Property ipAddress As String

        ''' <summary>
        ''' 节点计算机名称。
        ''' </summary>
        Public Property machineName As String

        ''' <summary>
        ''' 当前 CPU 使用率（0-100，百分比）。
        ''' </summary>
        Public Property cpuUsage As Double

        ''' <summary>
        ''' 物理内存总量（单位 MB）。
        ''' </summary>
        Public Property totalMemoryMB As Long

        ''' <summary>
        ''' 当前内存使用率（0-100，百分比）。
        ''' </summary>
        Public Property memoryUsage As Double

        ''' <summary>
        ''' 网络上传速率（字节/秒）。
        ''' </summary>
        Public Property netUploadRate As Double

        ''' <summary>
        ''' 网络下载速率（字节/秒）。
        ''' </summary>
        Public Property netDownloadRate As Double
    End Class

    ''' <summary>
    ''' 失败数据块的调试信息（用于失败调试面板展开）。
    ''' </summary>
    Public Class FailureInfo

        Public Property blockId As String
        Public Property jobId As String
        Public Property retryCount As Integer
        Public Property message As String
        Public Property stackTrace As String
        Public Property logPath As String
    End Class

    ''' <summary>
    ''' 任务提交请求：用户向头结点提交一个需要拆分计算的作业。
    ''' </summary>
    Public Class JobSubmit

        ''' <summary>
        ''' 任务名称（可选）。
        ''' </summary>
        Public Property name As String

        ''' <summary>
        ''' 输入的原始数据块文件（SMB 上的路径列表）。
        ''' </summary>
        Public Property inputFiles As String()

        ''' <summary>
        ''' 待加载执行的 CLR assembly 路径。
        ''' </summary>
        Public Property assemblyPath As String

        ''' <summary>
        ''' 待反射调用的 CLR 方法名。
        ''' </summary>
        Public Property methodName As String

        ''' <summary>
        ''' 单个数据块的目标大小（字节），用于拆分。
        ''' </summary>
        Public Property chunkSize As Long

        ''' <summary>
        ''' 计算数据输入源目录（位于 webRoot 下的 SMB 子目录）。
        ''' 当指定 datasetDir 时，头结点会解析该目录内的 dataset.ini / dataset.json，
        ''' 将匹配的输入数据展开为多个数据块（沿用现有 chunk 拆分与 SMB 写入机制）。
        ''' </summary>
        Public Property datasetDir As String

        ''' <summary>
        ''' 数据输入源类型：auto（自动探测 ini/json）/ ini / json。默认 auto。
        ''' </summary>
        Public Property datasetType As String = "auto"
    End Class

    ''' <summary>
    ''' 文件树节点：用于 web 页面惰性展示 webRoot 子树。
    ''' 后端按目录层级增量返回（仅一层直接子项），前端点开目录才请求下一层。
    ''' </summary>
    Public Class FileNode

        ''' <summary>
        ''' 节点名称（文件名或目录名）。
        ''' </summary>
        Public Property name As String

        ''' <summary>
        ''' 是否为目录。
        ''' </summary>
        Public Property isDir As Boolean

        ''' <summary>
        ''' 相对 webRoot 的路径（以 / 开头），前端选点时回传该值拼接 webRoot 得到完整路径。
        ''' </summary>
        Public Property fullPath As String

        ''' <summary>
        ''' 是否为可被选中作为 CLR Assembly 的 *.dll 文件（仅当 isDir=False 时可能为 True）。
        ''' </summary>
        Public Property isDll As Boolean

        ''' <summary>
        ''' 该目录节点是否含有 dll 直接子文件（用于前端图标提示，仅目录节点有意义）。
        ''' </summary>
        Public Property hasDllChildren As Boolean

        ''' <summary>
        ''' 目录节点是否含有 dataset.ini / dataset.json（用于前端提示可作为数据输入源，仅目录节点有意义）。
        ''' </summary>
        Public Property hasDataset As Boolean
    End Class

    ''' <summary>
    ''' 反射扫描得到的单个 CLR 方法（符合 worker 计算进程调用约定），及其 XML 注释。
    ''' </summary>
    Public Class AssemblyMethod

        ''' <summary>
        ''' 方法所在命名空间。
        ''' </summary>
        Public Property [namespace] As String

        ''' <summary>
        ''' 方法所在类型（class）短名称。
        ''' </summary>
        Public Property [class] As String

        ''' <summary>
        ''' 方法短名称。
        ''' </summary>
        Public Property method As String

        ''' <summary>
        ''' 方法完整签名，形如 "MyNs.MyClass.Run(Byte[], String, String)"。
        ''' </summary>
        Public Property signature As String

        ''' <summary>
        ''' 从目标 dll 的 .NET XML 注释文档中解析的 &lt;summary&gt; 文本（缺失则为空串）。
        ''' </summary>
        Public Property summary As String

        ''' <summary>
        ''' 从目标 dll 的 .NET XML 注释文档中解析的 &lt;remarks&gt; 文本（缺失则为空串）。
        ''' </summary>
        Public Property remarks As String
    End Class

    ''' <summary>
    ''' dataset.ini 解析结果：匹配后缀的数据输入文件清单。
    ''' </summary>
    Public Class DatasetIniInfo

        ''' <summary>
        ''' 数据文件后缀（含通配符，如 *.dat），来自 [dataset] ext=。
        ''' </summary>
        Public Property ext As String

        ''' <summary>
        ''' 任务描述，来自 [dataset] description=。
        ''' </summary>
        Public Property description As String

        ''' <summary>
        ''' 匹配后缀的输入数据文件（相对 dataset 目录的路径列表）。
        ''' </summary>
        Public Property files As String()
    End Class

    ''' <summary>
    ''' dataset.json 解析结果：大文件 + 数据块偏移列表。
    ''' </summary>
    Public Class DatasetJsonInfo

        ''' <summary>
        ''' 大型数据文件名（相对 dataset 目录）。
        ''' </summary>
        Public Property datafile As String

        ''' <summary>
        ''' 任务描述。
        ''' </summary>
        Public Property description As String

        ''' <summary>
        ''' 数据块偏移/大小列表，每项含 offset 与 size（字节）。
        ''' </summary>
        Public Property chunks As BlockDef()

        ''' <summary>
        ''' 数据块定义（偏移 + 大小，单位字节）。
        ''' </summary>
        Public Class BlockDef
            Public Property offset As Long
            Public Property size As Long
        End Class
    End Class

    ''' <summary>
    ''' 计算数据输入源预览：供 web 页面在选择 dataset 目录时惰性展示。
    ''' </summary>
    Public Class DatasetPreview

        ''' <summary>
        ''' 数据源类型：ini / json / none（所选目录不含 dataset 配置）。
        ''' </summary>
        Public Property kind As String

        ''' <summary>
        ''' ini 模式下的解析结果。
        ''' </summary>
        Public Property ini As DatasetIniInfo

        ''' <summary>
        ''' json 模式下的解析结果。
        ''' </summary>
        Public Property json As DatasetJsonInfo

        ''' <summary>
        ''' 预览读取失败时的错误信息（如文件损坏）。
        ''' </summary>
        Public Property [error] As String
    End Class

    ''' <summary>
    ''' 通用 API 响应封装，用于成功/失败与任务提交回执。
    ''' </summary>
    Public Class ApiResult

        Public Property ok As Boolean
        Public Property jobId As String
        Public Property message As String
        Public Property available As Boolean

        Public Shared Function Success(Optional jobId As String = "") As ApiResult
            Return New ApiResult With {.ok = True, .jobId = jobId}
        End Function

        Public Shared Function Failure(msg As String) As ApiResult
            Return New ApiResult With {.ok = False, .message = msg}
        End Function

        Public Shared Function NoTask() As ApiResult
            Return New ApiResult With {.ok = True, .available = False}
        End Function
    End Class
End Namespace
