Imports System.Net.Sockets
Imports SMRUCC.WebCloud.HTTPInternal.Core.WebSocket

Public Class InterfaceSocket : Inherits WsProcessor

    Public Sub New(tcp As TcpClient)
        MyBase.New(tcp)
    End Sub
End Class
