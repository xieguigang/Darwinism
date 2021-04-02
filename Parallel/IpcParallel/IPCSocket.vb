#Region "Microsoft.VisualBasic::3cd47c9347dfb0bebb90110fe2139a7c, Parallel\IpcParallel\IPCSocket.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Class IPCSocket
    ' 
    '     Properties: handleError, handleGetArgument, handlePOSTResult, host, HostPort
    '                 nargs, Protocol
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetArgumentByIndex, GetArgumentNumber, GetFirstAvailablePort, GetTask, PostError
    '               PostResult, PostStart, Run
    ' 
    '     Sub: [Stop]
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
#If netcore5 = 1 Then
Imports Microsoft.VisualBasic.ComponentModel.Collection
#End If
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Linq
Imports Parallel.IpcStream

#If netcore5 = 1 Then
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions
#End If

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
    Public Property handleError As Action(Of IPCError)
    Public Property nargs As Integer
    Public Property handleGetArgument As Func(Of Integer, ObjectStream)
    Public Property host As SlaveTask

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

    Public Sub [Stop]()
        Call socket.Dispose()
    End Sub

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

        Using buf As ObjectStream = _handleGetArgument(i)
            Return New DataPipe(buf)
        End Using
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

    <Protocol(Protocols.PostError)>
    Public Function PostError(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Using ms As New MemoryStream(request.ChunkBuffer)
            Call DirectCast(SlaveTask.GetValueFromStream(ms, GetType(IPCError), host.streamBuf), IPCError).DoCall(_handleError)
        End Using

        Return New DataPipe(Encoding.ASCII.GetBytes("OK!"))
    End Function

    <Protocol(Protocols.PostResult)>
    Public Function PostResult(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Using ms As New MemoryStream(request.ChunkBuffer)
            Call _handlePOSTResult(ms)
        End Using

        Return New DataPipe(Encoding.ASCII.GetBytes("OK!"))
    End Function
End Class
