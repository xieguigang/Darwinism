Imports System.IO
Imports System.Net.Sockets
Imports RQL.HttpInternal

''' <summary>
''' 在线查询服务提供模块
''' </summary>
Public Class RESTProvider : Inherits HttpInternal.HttpServer

    Public ReadOnly Property Repository As Repository

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="portal"></param>
    ''' <param name="repo">需要在这里将url转换为Long以进行protocol的绑定操作</param>
    Sub New(portal As Integer, repo As Repository)
        Call MyBase.New(portal)
        Me.Repository = repo
    End Sub

    ''' <summary>
    ''' http://linq.gcmodeller.org/kegg/pathways?from x in $ let nn = expression(x) where nn.test select nn,pathway=x
    ''' </summary>
    ''' <param name="p"></param>
    Public Overrides Sub handleGETRequest(p As HttpProcessor)
        Dim url As String = p.http_url

    End Sub

    Public Overrides Sub handlePOSTRequest(p As HttpProcessor, inputData As StreamReader)
        Call p.writeFailure("Method not allowed!")
    End Sub

    Protected Overrides Function __httpProcessor(client As TcpClient) As HttpProcessor
        Return New HttpProcessor(client, Me)
    End Function
End Class
