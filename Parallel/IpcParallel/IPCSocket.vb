#Region "Microsoft.VisualBasic::bf0b81e52519071ade9c98ff767d4828, Parallel\IpcParallel\IPCSocket.vb"

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
'     Properties: handleGetArgument, handlePOSTResult, handleSetResult, host, HostPort
'                 nargs, Protocol, result, socketExitCode
' 
'     Constructor: (+1 Overloads) Sub New
' 
'     Function: GetArgumentByIndex, GetArgumentNumber, GetFirstAvailablePort, GetLastError, GetTask
'               PostError, PostResult, PostStart, Run
' 
'     Sub: [Stop]
' 
' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Parallel.IpcStream
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

''' <summary>
''' IPC parallel socket for master node
''' </summary>
<Protocol(GetType(Protocols))>
Public Class IPCSocket : Implements ITaskDriver

    Public Shared ReadOnly Property Protocol As Long = New ProtocolAttribute(GetType(Protocols)).EntryPoint

    ReadOnly socket As TcpServicesSocket
    ReadOnly target As IDelegate

    ''' <summary>
    ''' running in verbose(debug) mode?
    ''' </summary>
    ReadOnly verbose As Boolean

    Public ReadOnly Property HostPort As Integer
        Get
            Return socket.LocalPort
        End Get
    End Property

    Public ReadOnly Property result As Object = Nothing

    Public Property handlePOSTResult As Func(Of Stream, IPCSocket, Object)
    Public Property nargs As Integer
    Public Property handleGetArgument As Func(Of Integer, IPCSocket, ObjectStream)
    Public Property host As SlaveTask

    Public ReadOnly Property handleSetResult As Boolean = False
    ''' <summary>
    ''' error code of <see cref="TcpServicesSocket.Run()"/>
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property socketExitCode As Integer

    Sub New(target As IDelegate, Optional debug As Integer? = Nothing, Optional verbose As Boolean = False)
        Me.socket = New TcpServicesSocket(If(debug, GetFirstAvailablePort()), debug:=verbose OrElse Not debug Is Nothing)
        Me.socket.ResponseHandler = AddressOf New ProtocolHandler(Me).HandleRequest
        Me.target = target
        Me.verbose = verbose
        Me.result = Nothing
        Me.handleSetResult = False
    End Sub

    Public Shared Function GetFirstAvailablePort() As Integer
        Call Thread.Sleep(randf.NextInteger(1000))

        If Environment.OSVersion.Platform = PlatformID.Unix Then
#If NETCOREAPP Then
            If Not "/bin/bash".FileExists Then
                Return TCPExtensions.GetFirstAvailablePort(-1)
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
        Return TCPExtensions.GetFirstAvailablePort(-1)
#End If
        Else
            Return TCPExtensions.GetFirstAvailablePort(-1)
        End If
    End Function

    Public Function GetLastError() As String
        If socket Is Nothing Then
            Return "[Warning] Please run socket at first!"
        Else
            Return socket.LastError
        End If
    End Function

    Public Sub [Stop]()
        Call socket.Dispose()
    End Sub

    ''' <summary>
    ''' socket run
    ''' </summary>
    ''' <returns></returns>
    Public Function Run() As Integer Implements ITaskDriver.Run
        _socketExitCode = socket.Run
        Return socketExitCode
    End Function

    ''' <summary>
    ''' get task function
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.GetTask)>
    Public Function GetTask(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        If verbose Then
            Call Console.WriteLine($"[{GetHashCode.ToHexString}] get parallel task entry.")
        End If

        Return New DataPipe(Encoding.UTF8.GetBytes(target.GetJson))
    End Function

    ''' <summary>
    ''' get argument value by index
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.GetArgumentByIndex)>
    Public Function GetArgumentByIndex(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Dim i As Integer = BitConverter.ToInt32(request.ChunkBuffer, Scan0)

        Using buf As ObjectStream = _handleGetArgument(i, Me)
            Return New DataPipe(buf)
        End Using
    End Function

    ''' <summary>
    ''' show a signal of task run
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.PostStart)>
    Public Function PostStart(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        If verbose Then
            Call Console.WriteLine($"[{GetHashCode.ToHexString}] started!")
        End If

        Return New DataPipe(Encoding.UTF8.GetBytes("OK!"))
    End Function

    ''' <summary>
    ''' get count of argument value input
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.GetArgumentNumber)>
    Public Function GetArgumentNumber(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Return New DataPipe(BitConverter.GetBytes(nargs))
    End Function

    ''' <summary>
    ''' recive the error message from the slave node
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.PostError)>
    Public Function PostError(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Using ms As New MemoryStream(request.ChunkBuffer)
            _result = DirectCast(SlaveTask.GetValueFromStream(ms, GetType(IPCError), host.streamBuf), IPCError)
            _handleSetResult = True
        End Using

        Return New DataPipe(Encoding.ASCII.GetBytes("OK!"))
    End Function

    ''' <summary>
    ''' recive the result data from the slave node
    ''' </summary>
    ''' <param name="request"></param>
    ''' <param name="remoteAddress"></param>
    ''' <returns></returns>
    <Protocol(Protocols.PostResult)>
    Public Function PostResult(request As RequestStream, remoteAddress As System.Net.IPEndPoint) As BufferPipe
        Using ms As New MemoryStream(request.ChunkBuffer)
            _result = _handlePOSTResult(ms, Me)
            _handleSetResult = True
        End Using

        Return New DataPipe(Encoding.ASCII.GetBytes("OK!"))
    End Function
End Class
