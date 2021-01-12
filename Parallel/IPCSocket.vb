Imports System.IO
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel

Public Class IPCSocket : Implements ITaskDriver

    ReadOnly socket As New TcpServicesSocket(GetFirstAvailablePort) With {
        .ResponseHandler = AddressOf DataRequestHandler
    }

    Public ReadOnly Property HostPort As Integer
        Get
            Return socket.LocalPort
        End Get
    End Property

    Public Property handlePOSTResult As Action(Of Stream)
    Public Property nargs As Integer
    Public Property handleGetArgument As Func(Of Integer, Stream)

    Sub New()
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Return socket.Run
    End Function

    Private Function DataRequestHandler(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe

    End Function
End Class
