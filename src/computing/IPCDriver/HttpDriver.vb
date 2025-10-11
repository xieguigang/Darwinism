Imports System.Net.Sockets
Imports Darwinism.IPC.Networking.HTTP
Imports Darwinism.IPC.Networking.Protocols.Reflection
Imports Flute.Http.Configurations
Imports Flute.Http.Core
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class HttpDriver : Inherits HttpServer

    ReadOnly app As ProtocolHandler
    ReadOnly flags As Dictionary(Of String, Long)
    ReadOnly protocol As Type

    Public Sub New(app As ProtocolHandler, port As Integer,
                   Optional threads As Integer = -1,
                   Optional configs As Configuration = Nothing)

        MyBase.New(port, threads, configs)
        Me.app = app
        Me.protocol = app.DeclaringType
        Me.flags = protocol.GetFields _
            .Where(Function(f) f.FieldType Is protocol) _
            .ToDictionary(Function(a) a.Name.ToLower,
                          Function(a)
                              Return CLng(a.GetValue(Nothing))
                          End Function)
    End Sub

    Public Overrides Sub handleGETRequest(p As HttpProcessor)
        Call FlushData(p, app.HandleRequest(TranslateRequest(p), Nothing))
    End Sub

    Public Overrides Sub handlePOSTRequest(p As HttpProcessor, inputData As String)
        Call FlushData(p, app.HandleRequest(TranslateRequest(p), Nothing))
    End Sub

    Public Overrides Sub handleOtherMethod(p As HttpProcessor)
        Call FlushData(p, app.HandleRequest(TranslateRequest(p), Nothing))
    End Sub

    Public Sub FlushData(p As HttpProcessor, data As BufferPipe)

    End Sub

    Public Function TranslateRequest(p As HttpProcessor) As RequestStream
        Dim url As New URL(p.http_url)
        Dim name As String = Strings.LCase(url.path.BaseName)

        If Not flags.ContainsKey(name) Then
            Return NetResponse.RFC_METHOD_NOT_ALLOWED
        End If

        Return New RequestStream(app.ProtocolEntry, flags(name), url.query.GetJson)
    End Function

    Protected Overrides Function getHttpProcessor(client As TcpClient, bufferSize As Integer) As HttpProcessor
        Return New HttpProcessor(client, Me, bufferSize * 4, _settings)
    End Function
End Class
