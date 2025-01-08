#Region "Microsoft.VisualBasic::7e4f86546d30c8214f26fbaaeb652b62, src\computing\Parallel\IpcParallel\IPCSocket.vb"

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


    ' Code Statistics:

    '   Total Lines: 201
    '    Code Lines: 122 (60.70%)
    ' Comment Lines: 52 (25.87%)
    '    - Xml Docs: 90.38%
    ' 
    '   Blank Lines: 27 (13.43%)
    '     File Size: 7.56 KB


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
Imports Darwinism.Centos
Imports Darwinism.Centos.proc.net
Imports Darwinism.HPC.Parallel.IpcStream
Imports Darwinism.IPC.Networking.Protocols.Reflection
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports options = Darwinism.HPC.Parallel.Extensions
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

''' <summary>
''' IPC parallel socket for master node
''' </summary>
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

    Sub New(target As IDelegate, Optional debug As Integer? = Nothing)
        Me.socket = New TcpServicesSocket(If(debug, GetFirstAvailablePort()), debug:=Verbose OrElse Not debug Is Nothing) With {
            .KeepsAlive = False
        }
        Me.socket.ResponseHandler = AddressOf New ProtocolHandler(Me).HandleRequest
        Me.target = target
        Me.result = Nothing
        Me.handleSetResult = False
    End Sub

    Public Shared Function GetFirstAvailablePort(Optional delay As Integer = 1000) As Integer
        Call Thread.Sleep(randf.NextInteger(delay))

        If Environment.OSVersion.Platform = PlatformID.Unix Then
#If NETCOREAPP Then
            If Not "/bin/bash".FileExists Then
                Return TCPExtensions.GetFirstAvailablePort(-1)
            End If

            ' 为了避免高并发的时候出现端口占用的情况，在这里使用随机数来解决一些问题
            ' port range start from nearby 10000
            ' for avoid port number conflicts
            Dim BEGIN_PORT = randf.NextInteger(MAX_PORT / 7, MAX_PORT - 1)
            Dim usedPorts As Index(Of Integer) = PortIsUsed(Verbose)

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

    ''' <summary>
    ''' work on linux
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function PortIsUsed(verbose As Boolean) As Integer()
        Dim stdout As String = Interaction.Shell("netstat", "-tulpn", verbose:=False)
        Dim usedPorts As Integer() = netstat.tulnp(stdout) _
            .Select(Function(t) t.LocalListenPort) _
            .ToArray

        If verbose Then
            Call VBDebugger.EchoLine("try to get tcp port listen binding status via `netstat` tool.")
            Call VBDebugger.EchoLine($"usedPorts: {usedPorts.JoinBy(", ")}.")

            If usedPorts.IsNullOrEmpty Then
                Call VBDebugger.EchoLine("none?")
            End If
        End If

        If usedPorts.Length = 0 Then
            If verbose Then
                Call VBDebugger.EchoLine("but the `netstat` tool is missing or not working, read/decode of the `/proc/net/tcp` file directly!")
            End If

            ' fallback method, read /proc/net/tcp file
            ' 可能存在权限问题，在netstat不存在的时候才会进行读取
            stdout = "/proc/net/tcp".ReadAllText
            usedPorts = tcp.Parse(New StringReader(stdout)) _
                .Select(Function(t) t.GetLocalAddress.port) _
                .ToArray

            If verbose Then
                Call VBDebugger.EchoLine($"parse {usedPorts.Length} tcp ports from the output:")
                Call VBDebugger.EchoLine(stdout)
            End If
        End If

        Return usedPorts
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
        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{GetHashCode.ToHexString}] get parallel task entry.")
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
        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{GetHashCode.ToHexString}] started!")
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
