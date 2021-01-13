Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

<Protocol(GetType(Protocols))>
Public Class IPCSocket : Implements ITaskDriver

    Public Shared ReadOnly Property Protocol As Long = New ProtocolAttribute(GetType(Protocols)).EntryPoint

    ReadOnly socket As TcpServicesSocket
    ReadOnly target As IDelegate

    Public ReadOnly Property HostPort As Integer
        Get
            Return socket.LocalPort
        End Get
    End Property

    Public Property handlePOSTResult As Action(Of Stream)
    Public Property nargs As Integer
    Public Property handleGetArgument As Func(Of Integer, ObjectStream)

    Sub New(target As IDelegate, Optional debug As Integer? = Nothing)
        Me.socket = New TcpServicesSocket(If(debug, GetFirstAvailablePort()))
        Me.socket.ResponseHandler = AddressOf New ProtocolHandler(Me).HandleRequest
        Me.target = target
    End Sub

    Private Function GetFirstAvailablePort() As Integer
#If netcore5 = 1 Then
        If Not "/bin/bash".FileExists Then
            Return TCPExtensions.GetFirstAvailablePort()
        End If

        ' 为了避免高并发的时候出现端口占用的情况，在这里使用随机数来解决一些问题
        Dim BEGIN_PORT = randf.NextInteger(MAX_PORT - 1)
        Dim stdout As String = CommandLine.Call("/bin/bash", "-c ""netstat -tulpn""")
        Dim usedPorts As Index(Of Integer) = stdout.LineTokens _
            .Select(Function(line) line.StringSplit("\s+").ElementAt(3)) _
            .Where(Function(n) n.IsPattern(".+[:]\d+")) _
            .Select(Function(i) Integer.Parse(i.Split(":"c).Last)) _
            .Indexing

        For i As Integer = BEGIN_PORT To MAX_PORT - 1
            If Not i Like usedPorts Then
                Return i
            End If
        Next

        Return -1
#Else
        ' PlatformNotSupportedException: The information requested is unavailable on the current platform.
        ' on UNIX .net 5
        Return TCPExtensions.GetFirstAvailablePort()
#End If
    End Function

    Public Function Run() As Integer Implements ITaskDriver.Run
        Return socket.Run
    End Function

    <Protocol(Protocols.GetTask)>
    Public Function GetTask(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Call Console.WriteLine($"[{GetHashCode.ToHexString}] get parallel task entry.")
        Return New DataPipe(Encoding.UTF8.GetBytes(target.GetJson))
    End Function

    <Protocol(Protocols.GetArgumentByIndex)>
    Public Function GetArgumentByIndex(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Dim i As Integer = BitConverter.ToInt32(request.ChunkBuffer, Scan0)
        Dim buf As ObjectStream = _handleGetArgument(i)
        Dim pipe As New DataPipe(buf)

        Return pipe
    End Function

    <Protocol(Protocols.PostStart)>
    Public Function PostStart(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Call Console.WriteLine($"[{GetHashCode.ToHexString}] started!")
        Return New DataPipe(Encoding.UTF8.GetBytes("OK!"))
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
