Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

<Protocol(GetType(Protocols))>
Public Class IPCSocket : Implements ITaskDriver

    ReadOnly socket As New TcpServicesSocket(GetFirstAvailablePort)
    ReadOnly target As IDelegate

    Public ReadOnly Property HostPort As Integer
        Get
            Return socket.LocalPort
        End Get
    End Property

    Public Property handlePOSTResult As Action(Of Stream)
    Public Property nargs As Integer
    Public Property handleGetArgument As Func(Of Integer, Stream)

    Sub New(target As IDelegate)
        Me.socket.ResponseHandler = AddressOf New ProtocolHandler(Me).HandleRequest
        Me.target = target
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Return socket.Run
    End Function

    <Protocol(Protocols.GetTask)>
    Public Function GetTask(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Return New DataPipe(Encoding.UTF8.GetBytes(target.GetJson))
    End Function

    <Protocol(Protocols.GetArgumentByIndex)>
    Public Function GetArgumentByIndex(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Dim i As Integer = BitConverter.ToInt32(request.ChunkBuffer, Scan0)
        Dim buf As Stream = _handleGetArgument(i)
        Dim pipe As New StreamPipe(buf)

        Return pipe
    End Function

    <Protocol(Protocols.GetArgumentNumber)>
    Public Function GetArgumentNumber(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Return New DataPipe(BitConverter.GetBytes(nargs))
    End Function

    <Protocol(Protocols.PostResult)>
    Public Function PostResult(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Using ms As New MemoryStream(request.ChunkBuffer)
            Call _handlePOSTResult(ms)
        End Using

        Return New DataPipe(Encoding.ASCII.GetBytes("OK!"))
    End Function
End Class
