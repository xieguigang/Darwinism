Imports System.IO
Imports System.Net.Sockets
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Serialization
Imports SMRUCC.HTTPInternal.Core

''' <summary>
''' 在线查询服务提供模块，在这个模块之中只负责进行url参数的解析工作
''' </summary>
Public Class RESTProvider : Inherits HttpServer

    Public ReadOnly Property Repository As Repository
    Public ReadOnly Property LinqProvider As LinqAPI = New LinqAPI

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="portal"></param>
    ''' <param name="repo">需要在这里将url转换为Long以进行protocol的绑定操作</param>
    Sub New(portal As Integer, repo As Repository)
        Call MyBase.New(portal)
        Me.Repository = repo
    End Sub

    Sub New()
        Call Me.New(80, RQL.Repository.LoadDefault)
    End Sub

    ''' <summary>
    ''' http://linq.gcmodeller.org/kegg/pathways?where=test_expr(pathway)
    ''' 测试条件里面的对象实例的标识符使用资源url里面的最后一个标识符为变量名
    ''' 测试条件表达式使用VisualBasic的语法
    ''' 测试条件必须以where起头开始
    ''' </summary>
    ''' <param name="p"></param>
    ''' <return>返回一个网络终点IpEndPoint</return>
    Public Overrides Sub handleGETRequest(p As HttpProcessor)
        If p.IsWWWRoot Then
            ' 返回帮助信息
        Else
            Call __apiInvoke(p)
        End If
    End Sub

    Private Sub __apiInvoke(p As HttpProcessor)
        Dim url As String = p.http_url
        Dim pos As Integer = InStr(url, "?")
        Dim expr As String = ""
        If pos = 0 Then
            ' expr为空
        Else
            expr = Mid(url, pos + 1).Trim  ' 参数里面可能含有转意字符，还需要进行转意
            expr = expr.URLEscapes
            url = Mid(url, 1, pos - 1)
        End If

        Dim source As IEnumerable = Repository.GetRepository(url, expr) ' expr为空的话，则没有where测试，则返回所有数据
        Dim linq As IPEndPoint = LinqProvider.OpenQuery(source, Repository.GetType(url))
        Call p.outputStream.WriteLine(linq.GetJson)
    End Sub

    Public Overrides Sub handlePOSTRequest(p As HttpProcessor, inputData As MemoryStream)
        Call p.writeFailure("Method not allowed!")
    End Sub

    Protected Overrides Function __httpProcessor(client As TcpClient) As HttpProcessor
        Return New HttpProcessor(client, Me)
    End Function
End Class
