Imports System.IO
Imports System.Linq
Imports Flute.Http.Core.Message
Imports Flute.Http.Core.Message.HttpHeader
Imports Microsoft.VisualBasic.Serialization.JSON
Imports ClusterShared

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

    Private ReadOnly scheduler As Scheduler
    Private ReadOnly cfg As Config
    Private ReadOnly wwwroot As String

    Sub New(scheduler As Scheduler, cfg As Config)
        Me.scheduler = scheduler
        Me.cfg = cfg
        Dim exeDir = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().Location)
        Me.wwwroot = Path.Combine(exeDir, "wwwroot")
    End Sub

    ' ============ 仪表盘静态托管 ============

    <HttpGet("/")>
    Public Sub Index(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        Dim f = Path.Combine(wwwroot, "index.html")
        If System.IO.File.Exists(f) Then
            res.SendFile(f)
        Else
            res.WriteHTML("<h1>Distributed Cluster</h1><p>wwwroot/index.html not found.</p>")
        End If
    End Sub

    <HttpGet("/app.js")>
    Public Sub AppJs(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        Dim f = Path.Combine(wwwroot, "app.js")
        If System.IO.File.Exists(f) Then res.SendFile(f) Else res.WriteError(404, "app.js")
    End Sub

    <HttpGet("/style.css")>
    Public Sub StyleCss(req As HttpRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"
        Dim f = Path.Combine(wwwroot, "style.css")
        If System.IO.File.Exists(f) Then res.SendFile(f) Else res.WriteError(404, "style.css")
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
            submit.name = CType(req.Argument("name"), String)
            submit.assemblyPath = CType(req.Argument("assemblyPath"), String)
            submit.methodName = CType(req.Argument("methodName"), String)
            Long.TryParse(CType(req.Argument("chunkSize"), String), submit.chunkSize)

            ' inputFiles 通过逗号分隔的字符串传入（已 URL 编码）。
            Dim inputs = CType(req.Argument("inputs"), String)
            If Not String.IsNullOrEmpty(inputs) Then
                submit.inputFiles = inputs.Split(","c).Select(Function(s) s.Trim()).Where(Function(s) s.Length > 0).ToArray()
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
        Dim nodeId = CType(req.Argument("nodeId"), String)

        Dim block = scheduler.PullBlock(nodeId)

        If block Is Nothing Then
            res.WriteJSON(Of ApiResult)(ApiResult.NoTask(), indent:=False)
        Else
            res.WriteJSON(Of TaskBlock)(block, indent:=False)
        End If
    End Sub

    ' ============ 节点心跳 ============

    <HttpPost("/api/heartbeat")>
    Public Sub Heartbeat(req As HttpPOSTRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Dim cores As Integer = Environment.ProcessorCount
        If Not Integer.TryParse(CType(req.Argument("cores"), String), cores) Then
            cores = Environment.ProcessorCount
        End If

        Dim hb As New NodeHeartbeat With {
            .nodeId = CType(req.Argument("nodeId"), String),
            .timestamp = DateTime.UtcNow.Ticks,
            .currentBlock = CType(req.Argument("currentBlock"), String),
            .log = CType(req.Argument("log"), String),
            .cores = cores
        }

        Call scheduler.ReceiveHeartbeat(hb)
        res.WriteJSON(Of ApiResult)(ApiResult.Success(), indent:=False)
    End Sub

    ' ============ 节点回执：完成 ============

    <HttpPost("/api/task/done")>
    Public Sub Done(req As HttpPOSTRequest, res As HttpResponse)
        res.AccessControlAllowOrigin = "*"

        Dim result As New TaskResult With {
            .blockId = CType(req.Argument("blockId"), String),
            .jobId = CType(req.Argument("jobId"), String),
            .nodeId = CType(req.Argument("nodeId"), String),
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
            .blockId = CType(req.Argument("blockId"), String),
            .jobId = CType(req.Argument("jobId"), String),
            .nodeId = CType(req.Argument("nodeId"), String),
            .success = False,
            .errorMessage = CType(req.Argument("errorMessage"), String),
            .stackTrace = CType(req.Argument("stackTrace"), String)
        }

        Call scheduler.ReportFailed(result)
        res.WriteJSON(Of ApiResult)(ApiResult.Success(), indent:=False)
    End Sub
End Class
