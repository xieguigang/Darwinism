Imports System.IO
Imports System.Net.Sockets
Imports SMRUCC.WebCloud.HTTPInternal.Core.WebSocket

Public Class InterfaceSocket : Inherits WsProcessor

    Public Sub New(tcp As TcpClient)
        MyBase.New(tcp)
    End Sub

    Private Sub InterfaceSocket_onClientBinaryMessage(sender As WsProcessor, data As MemoryStream, responseStream As NetworkStream) Handles Me.onClientBinaryMessage

    End Sub

    Private Sub InterfaceSocket_onClientTextMessage(sender As WsProcessor, data As String, responseStream As NetworkStream) Handles Me.onClientTextMessage

    End Sub

    Private Sub InterfaceSocket_onClientDisconnect(sender As Object) Handles Me.onClientDisconnect

    End Sub
End Class
