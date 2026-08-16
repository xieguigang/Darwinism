Imports Darwinism.HPC.DistributedCluster.Shared.ClusterShared
Imports Flute.Http.Core.Message
Imports Flute.Http.Core.Message.HttpHeader

''' <summary>
''' 头结点 REST 控制器。基于 Flute 的 HttpRouter 反射路由，注册以下接口：
'''   GET  /api/status          集群状态快照（仪表盘轮询）
'''   GET  /api/submit          提交作业（query: assemblyPath,methodName,name,chunkSize,inputs 逗号分隔）
'''   GET  /api/task/pull       节点拉取任务（query: nodeId）
'''   POST /api/heartbeat       节点心跳（query: nodeId,currentBlock,log,cores）
'''   POST /api/task/done       节点报告完成（query: blockId,jobId,nodeId）
'''   POST /api/task/failed     节点报告失败（query: blockId,jobId,nodeId,errorMessage,stackTrace）
'''   GET  /                     仪表盘静态页
'''   GET  /app.js /style.css   静态资源
''' </summary>
Public Class ClusterController

    ReadOnly scheduler As Scheduler

    Sub New(scheduler As Scheduler, cfg As Config)
        Me.scheduler = scheduler
    End Sub

    ' ============ 状态快照 ============

    <HttpGet("/api/status")>
    Public Sub Status(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        res.WriteJSON(Of ClusterStatus)(scheduler.Snapshot(), indent:=False)
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
        Dim v = req.Argument(pascalName)
        If v Is Nothing OrElse String.IsNullOrEmpty(CStr(v)) Then
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
