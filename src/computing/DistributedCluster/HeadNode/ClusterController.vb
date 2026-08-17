Imports System.IO
Imports Darwinism.HPC.DistributedCluster.Host.ClusterShared
Imports Darwinism.HPC.DistributedCluster.[Shared]
Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Flute.Http.Core.Message
Imports Flute.Http.Core.Message.HttpHeader

''' <summary>
''' 头结点 REST 控制器。基于 Flute 的 HttpRouter 反射路由，注册以下接口：
'''   GET  /api/status          集群状态快照（仪表盘轮询）
'''   GET  /api/submit          提交作业（query: assemblyPath,methodName,name,chunkSize,inputs 逗号分隔,datasetDir,datasetType）
'''   GET  /api/files/tree      分层增量返回 webRoot 子树（query: dir 相对路径，默认根）
'''   GET  /api/assembly/scan   反射扫描 dll 方法+XML 注释（query: assemblyPath 相对 webRoot 路径或完整路径）
'''   GET  /api/dataset/preview 预览 dataset 目录数据输入（query: dir 相对 webRoot 路径）
'''   GET  /api/task/pull       节点拉取任务（query: nodeId）
'''   POST /api/heartbeat       节点心跳（query: nodeId,currentBlock,log,cores）
'''   POST /api/task/done       节点报告完成（query: blockId,jobId,nodeId）
'''   POST /api/task/failed     节点报告失败（query: blockId,jobId,nodeId,errorMessage,stackTrace）
'''   GET  /                    仪表盘静态页
'''   GET  /app.js /style.css   静态资源
''' </summary>
Public Class ClusterController

    ReadOnly scheduler As Scheduler
    ReadOnly cfg As Config

    Sub New(scheduler As Scheduler, cfg As Config)
        Me.scheduler = scheduler
        Me.cfg = cfg
    End Sub

    ' ============ 状态快照 ============

    <HttpGet("/api/status")>
    Public Sub Status(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        res.WriteJSON(Of ClusterStatus)(scheduler.Snapshot(), indent:=False)
    End Sub

    ' ============ 文件树（分层懒加载） ============

    ''' <summary>
    ''' 分层增量返回 webRoot 子树的直接子节点。前端点开目录节点才请求下一层，
    ''' 避免一次性递归整棵 smb 目录（共享繁忙时扫描极慢）。
    ''' 
    ''' - "dir": 相对 webRoot 的子目录（以 / 开头或为空=根）。
    ''' </summary>
    <HttpGet("/api/files/tree")>
    Public Sub FilesTree(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Try
            Dim rel = If(CType(req.Argument("dir"), String), "").Trim()
            Dim root = Path.GetFullPath(cfg.webRoot)
            Dim target As String = root

            If rel.Length > 0 Then
                ' 规范化相对路径，阻止目录穿越
                target = Path.GetFullPath(Path.Combine(root, rel.TrimStart("/"c)))
            End If

            If Not target.StartsWith(root, StringComparison.OrdinalIgnoreCase) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure("路径越界"), indent:=False)
                Return
            End If

            If Not Directory.Exists(target) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure($"目录不存在: {rel}"), indent:=False)
                Return
            End If

            Dim nodes As New List(Of FileNode)

            For Each d In Directory.GetDirectories(target)
                Dim info = New DirectoryInfo(d)
                Dim children = info.GetFileSystemInfos()
                Dim hasDll = children.Any(Function(c) (Not c.Attributes.HasFlag(FileAttributes.Directory)) AndAlso c.Extension.ToLower() = ".dll")
                Dim hasDataset = children.Any(Function(c) Not c.Attributes.HasFlag(FileAttributes.Directory) AndAlso (c.Name.ToLower() = "dataset.ini" OrElse c.Name.ToLower() = "dataset.json"))

                nodes.Add(New FileNode With {
                    .name = info.Name,
                    .isDir = True,
                    .fullPath = NormalizeRel(root, d),
                    .isDll = False,
                    .hasDllChildren = hasDll,
                    .hasDataset = hasDataset
                })
            Next

            For Each f In Directory.GetFiles(target, "*.dll")
                Dim info = New FileInfo(f)
                nodes.Add(New FileNode With {
                    .name = info.Name,
                    .isDir = False,
                    .fullPath = NormalizeRel(root, f),
                    .isDll = True,
                    .hasDllChildren = False,
                    .hasDataset = False
                })
            Next

            res.WriteJSON(Of FileNode())(nodes.ToArray(), indent:=False)
        Catch ex As Exception
            res.WriteJSON(Of ApiResult)(ApiResult.Failure(ex.Message), indent:=False)
        End Try
    End Sub

    ''' <summary>
    ''' 相对 webRoot 的规范路径（以 / 开头）。
    ''' </summary>
    Private Function NormalizeRel(root As String, full As String) As String
        Dim rel = full.Substring(root.Length).TrimStart("\"c, "/"c)
        Return "/" & rel.Replace("\"c, "/"c)
    End Function

    ' ============ Assembly 反射扫描 ============

    ''' <summary>
    ''' 反射加载目标 dll，扫描符合 worker 调用约定的方法并从同名 XML 注释取 summary/remarks，随后卸载。
    ''' 
    ''' - assemblyPath: 相对 webRoot 的路径或完整路径。
    ''' </summary>
    ''' 
    <HttpGet("/api/assembly/scan")>
    Public Sub AssemblyScan(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Try
            Dim raw = If(CType(req.Argument("assemblypath"), String), "").Trim()

            If String.IsNullOrEmpty(raw) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure("missing assemblyPath"), indent:=False)
                Return
            End If

            Dim full = ResolveWebRootPath(raw)

            If Not full.StartsWith(Path.GetFullPath(cfg.webRoot), StringComparison.OrdinalIgnoreCase) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure("程序集路径越界"), indent:=False)
                Return
            End If

            Dim msg As String = ""
            Dim methods As AssemblyMethod() = AssemblyScanner.Scan(full, msg)

            If methods.Length = 0 AndAlso Not String.IsNullOrEmpty(msg) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure(msg), indent:=False)
                Return
            End If

            res.WriteJSON(Of AssemblyScan)(New AssemblyScan With {
                .methods = methods,
                .message = msg
            }, indent:=False)
        Catch ex As Exception
            res.WriteJSON(Of ApiResult)(ApiResult.Failure(ex.Message), indent:=False)
        End Try
    End Sub

    ''' <summary>
    ''' 将相对 webRoot 的路径或完整路径解析为完整路径，并阻止越界。
    ''' </summary>
    Private Function ResolveWebRootPath(raw As String) As String
        Dim root = Path.GetFullPath(cfg.webRoot)

        If Path.IsPathRooted(raw) AndAlso raw.StartsWith(root.Replace("\", "/").Trim("/"c)) Then
            Return Path.GetFullPath(raw)
        End If

        Return Path.GetFullPath(Path.Combine(root, raw.TrimStart("/"c)))
    End Function

    ' ============ dataset 预览 ============

    ''' <summary>
    ''' 预览所选目录下的计算数据输入源（dataset.ini / dataset.json）。
    ''' 
    ''' - dir: 相对 webRoot 的目录路径。
    ''' </summary>
    ''' 
    <HttpGet("/api/dataset/preview")>
    Public Sub DatasetPreview(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Try
            Dim rel = If(CType(req.Argument("dir"), String), "").Trim()
            Dim root = Path.GetFullPath(cfg.webRoot)
            Dim dir = If(String.IsNullOrEmpty(rel), root, Path.GetFullPath(Path.Combine(root, rel.TrimStart("/"c))))

            If Not dir.StartsWith(root, StringComparison.OrdinalIgnoreCase) OrElse Not Directory.Exists(dir) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure("路径越界或不存在"), indent:=False)
                Return
            End If

            Dim preview = scheduler.PreviewDataset(dir)
            res.WriteJSON(Of DatasetPreview)(preview, indent:=False)
        Catch ex As Exception
            res.WriteJSON(Of ApiResult)(ApiResult.Failure(ex.Message), indent:=False)
        End Try
    End Sub

    ' ============ 作业提交 ============

    <HttpGet("/api/submit")>
    Public Sub Submit(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Try
            Dim submit As New JobSubmit()
            If req.HasValue("name") Then submit.name = CType(req.Argument("name"), String)
            If req.HasValue("assemblypath") Then submit.assemblyPath = CType(req.Argument("assemblypath"), String)
            If req.HasValue("methodname") Then submit.methodName = CType(req.Argument("methodname"), String)
            If req.HasValue("chunksize") Then Long.TryParse(CType(req.Argument("chunksize"), String), submit.chunkSize)
            If req.HasValue("datasetdir") Then submit.datasetDir = CType(req.Argument("datasetdir"), String)
            If req.HasValue("datasettype") Then submit.datasetType = CType(req.Argument("datasettype"), String)

            ' inputFiles 通过逗号分隔的字符串传入（已 URL 编码）。
            If req.HasValue("inputs") Then
                Dim inputs = CType(req.Argument("inputs"), String)
                If Not String.IsNullOrEmpty(inputs) Then
                    submit.inputFiles = inputs.Split(","c).Select(Function(s) s.Trim()).Where(Function(s) s.Length > 0).ToArray()
                End If
            End If

            If String.IsNullOrEmpty(submit.assemblyPath) Then
                res.WriteJSON(Of ApiResult)(ApiResult.Failure("missing assemblyPath"), indent:=False)
                Return
            End If

            Dim jobId = scheduler.SubmitJob(submit)
            res.WriteJSON(Of ApiResult)(ApiResult.Success(jobId), indent:=False)
        Catch ex As Exception
            res.WriteJSON(Of ApiResult)(ApiResult.Failure(ex.Message), indent:=False)
        End Try
    End Sub

    ' ============ 节点拉取任务 ============

    <HttpGet("/api/task/pull")>
    Public Sub Pull(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        Dim nodeId = CType(req.Argument("nodeid"), String)

        Dim block = scheduler.PullBlock(nodeId)

        If block Is Nothing Then
            res.WriteJSON(Of ApiResult)(ApiResult.NoTask(), indent:=False)
        Else
            res.WriteJSON(Of TaskBlock)(block, indent:=False)
        End If
    End Sub

    ' ============ 节点心跳 ============

    ''' <summary>
    ''' 大小写不敏感地读取请求参数。
    ''' JSON Body（DataContract 序列化）使用 PascalCase 字段名（如 nodeId），
    ''' 而历史 query 参数使用小写名（如 nodeid），此处两者均尝试以保持兼容。
    ''' </summary>
    Private Function Arg(req As HttpPOSTRequest, pascalName As String) As Object
        Dim v As String = req.Argument(pascalName)
        If v Is Nothing OrElse String.IsNullOrEmpty(v) Then
            v = req.Argument(pascalName.ToLower())
        End If
        Return v
    End Function

    <HttpPost("/api/heartbeat")>
    Public Sub Heartbeat(req As HttpPOSTRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Dim cores As Integer = Environment.ProcessorCount
        If Not Integer.TryParse(CType(Arg(req, "cores"), String), cores) Then
            cores = Environment.ProcessorCount
        End If

        Dim cpuUsage As Double = 0
        Double.TryParse(CType(Arg(req, "cpuUsage"), String), cpuUsage)

        Dim totalMemoryMB As Long = 0
        Long.TryParse(CType(Arg(req, "totalMemoryMB"), String), totalMemoryMB)

        Dim memoryUsage As Double = 0
        Double.TryParse(CType(Arg(req, "memoryUsage"), String), memoryUsage)

        Dim netUploadRate As Double = 0
        Double.TryParse(CType(Arg(req, "netUploadRate"), String), netUploadRate)

        Dim netDownloadRate As Double = 0
        Double.TryParse(CType(Arg(req, "netDownloadRate"), String), netDownloadRate)

        Dim nodeId = CType(Arg(req, "nodeId"), String)
        If String.IsNullOrEmpty(nodeId) Then
            ' 防止空键导致后续字典写入异常。
            nodeId = "unknown-node"
        End If

        Dim hb As New NodeHeartbeat With {
            .nodeId = nodeId,
            .timestamp = DateTime.UtcNow.Ticks,
            .currentBlock = CType(Arg(req, "currentBlock"), String),
            .log = CType(Arg(req, "log"), String),
            .cores = cores,
            .ipAddress = CType(Arg(req, "ipAddress"), String),
            .machineName = CType(Arg(req, "machineName"), String),
            .cpuUsage = cpuUsage,
            .totalMemoryMB = totalMemoryMB,
            .memoryUsage = memoryUsage,
            .netUploadRate = netUploadRate,
            .netDownloadRate = netDownloadRate
        }

        Call scheduler.ReceiveHeartbeat(hb)
        res.WriteJSON(Of ApiResult)(ApiResult.Success(), indent:=False)
    End Sub

    ' ============ 节点回执：完成 ============

    <HttpPost("/api/task/done")>
    Public Sub Done(req As HttpPOSTRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Dim result As New TaskResult With {
            .blockId = CType(req.Argument("blockid"), String),
            .jobId = CType(req.Argument("jobid"), String),
            .nodeId = CType(req.Argument("nodeid"), String),
            .success = True
        }

        Call scheduler.ReportDone(result)
        res.WriteJSON(Of ApiResult)(ApiResult.Success(), indent:=False)
    End Sub

    ' ============ 节点回执：失败 ============

    <HttpPost("/api/task/failed")>
    Public Sub Failed(req As HttpPOSTRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Dim result As New TaskResult With {
            .blockId = CType(req.Argument("blockid"), String),
            .jobId = CType(req.Argument("jobid"), String),
            .nodeId = CType(req.Argument("nodeid"), String),
            .success = False,
            .errorMessage = CType(req.Argument("errormessage"), String),
            .stackTrace = CType(req.Argument("stacktrace"), String)
        }

        Call scheduler.ReportFailed(result)
        res.WriteJSON(Of ApiResult)(ApiResult.Success(), indent:=False)
    End Sub
End Class
