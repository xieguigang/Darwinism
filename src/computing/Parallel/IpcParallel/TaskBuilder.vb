#Region "Microsoft.VisualBasic::c7c31f6a783ed4128dc814f766dfce31, src\computing\Parallel\IpcParallel\TaskBuilder.vb"

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

    '   Total Lines: 265
    '    Code Lines: 178 (67.17%)
    ' Comment Lines: 49 (18.49%)
    '    - Xml Docs: 85.71%
    ' 
    '   Blank Lines: 38 (14.34%)
    '     File Size: 9.39 KB


    ' Class TaskBuilder
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: FromStream, GetArgumentValue, GetArgumentValueNumber, GetMethod, GetParameters
    '               Initialize, PostError, Run
    ' 
    '     Sub: PostFinished
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Darwinism.HPC.Parallel.IpcStream
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.ApplicationServices.Development.NetCoreApp
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

''' <summary>
''' Run on slave node
''' </summary>
Public Class TaskBuilder : Implements ITaskDriver

    ReadOnly masterPort As Integer
    ReadOnly masterHost As String
    ReadOnly emit As New StreamEmit
    ReadOnly timeout_sec As Double = 15
    ReadOnly verbose_echo As Boolean = False

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="port"></param>
    ''' <param name="master"></param>
    ''' <param name="timeout">
    ''' the tcp request timeout value in time data unit ``seconds``.
    ''' </param>
    <DebuggerStepThrough>
    Sub New(port As Integer,
            Optional master As String = "localhost",
            Optional timeout As Double = 15,
            Optional verbose As Boolean = False)

        verbose_echo = verbose
        masterPort = port
        masterHost = master
        timeout_sec = timeout
    End Sub

    ''' <summary>
    ''' get parameter values
    ''' </summary>
    ''' <param name="params"></param>
    ''' <param name="n"></param>
    ''' <returns></returns>
    Private Iterator Function GetParameters(params As ParameterInfo(), n As Integer) As IEnumerable(Of Object)
        For i As Integer = 0 To n - 1
            Dim par As Object = GetArgumentValue(i)
            Dim targetType As Type = params(i).ParameterType

            If par Is Nothing Then
                Yield par
            ElseIf par.GetType.IsInheritsFrom(targetType) Then
                Yield Conversion.CTypeDynamic(par, targetType)
            ElseIf par.GetType Is GetType(SocketRef) Then
                ' is a common parameter value between
                ' the parallel batchs
                Dim socket As SocketRef = DirectCast(par, SocketRef)

                Using buffer As ObjectStream = socket.Open
                    Yield FromStream(buffer)
                End Using
            Else
                Yield par
            End If
        Next
    End Function

    ''' <summary>
    ''' load task method delegate function and request parameters from remote master for run the task 
    ''' </summary>
    ''' <param name="api"></param>
    ''' <param name="target"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Private Function Initialize(<Out> ByRef api As MethodInfo,
                                <Out> ByRef target As Object,
                                <Out> ByRef args As Object()) As Integer

        Dim task As IDelegate = GetMethod()
        Dim params As ParameterInfo()
        Dim emits As EmitStreamAttribute()

        ' .net clr reflection code for get task context
        api = task.GetMethod
        emits = api.GetCustomAttributes(Of EmitStreamAttribute).ToArray
        params = api.GetParameters
        target = task.GetMethodTarget

        If Not emits.IsNullOrEmpty Then
            For Each type As EmitStreamAttribute In emits
                If Not type.Target Is Nothing Then
                    Call emit.Emit(type.Target, StreamEmit.CreateHandler(type))
                End If
            Next
        End If

        Dim n As Integer = GetArgumentValueNumber()
        Dim argList As New List(Of Object)(GetParameters(params, n))

        If verbose_echo Then
            Call VBDebugger.EchoLine("run task:")
            Call VBDebugger.EchoLine(task.GetJson(indent:=False, simpleDict:=True))
        End If

        ' fix for optional parameter values
        For i As Integer = n To params.Length - 1
            If Not params(i).IsOptional Then
                Return PostError(New Exception($"missing parameter value for [{i}]{params(i).Name}!"))
            Else
                Call argList.Add(params(i).DefaultValue)
            End If
        Next

        args = argList.ToArray

        Return 0
    End Function

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim api As MethodInfo = Nothing
        Dim target As Object = Nothing
        Dim args As Object() = Nothing

        Try
            Dim i As New Value(Of Integer)

            If 0 <> (i = Initialize(api, target, args)) Then
                Return i
            End If
        Catch ex As Exception
            Call PostError(ex)
            Return 500
        End Try

        ' send debug message
        Call New TcpRequest(masterHost, masterPort) _
            .SetTimeOut(timespan:=TimeSpan.FromSeconds(timeout_sec)) _
            .SetVerbose(verbose_echo) _
            .SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.PostStart))

        Try
            Call PostFinished(api.Invoke(target, args), Protocols.PostResult)
        Catch ex As Exception
            Call PostError(ex)
        Finally
            If verbose_echo Then
                Call VBDebugger.EchoLine("job done!")
            End If
        End Try

        Return 0
    End Function

    ''' <summary>
    ''' just get the number of the arguments that passed.
    ''' (due to the reason of optional parameters) 
    ''' </summary>
    ''' <returns></returns>
    Private Function GetArgumentValueNumber() As Integer
        Dim resp = New TcpRequest(masterHost, masterPort) _
            .SetTimeOut(timespan:=TimeSpan.FromSeconds(timeout_sec)) _
            .SetVerbose(verbose_echo) _
            .SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.GetArgumentNumber))
        Dim n As Integer = BitConverter.ToInt32(resp.ChunkBuffer, Scan0)

        Return n
    End Function

    ''' <summary>
    ''' get target task function from the master node via TCP request
    ''' </summary>
    ''' <returns></returns>
    Private Function GetMethod() As IDelegate
        Dim resp = New TcpRequest(masterHost, masterPort) _
            .SetTimeOut(timespan:=TimeSpan.FromSeconds(timeout_sec)) _
            .SetVerbose(verbose_echo) _
            .SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.GetTask))
        Dim json As String = resp.GetUTF8String
        Dim target As IDelegate = json.LoadJSON(Of IDelegate)

        Return target
    End Function

    ''' <summary>
    ''' object is created via the module symbol: <see cref="emit"/>
    ''' </summary>
    ''' <param name="stream"></param>
    ''' <returns></returns>
    Private Function FromStream(stream As ObjectStream) As Object
        Dim type As Type = stream.type.GetType(
            knownFirst:=True,
            searchPath:={stream.type.assembly}
        )

#If NETCOREAPP Then
        Call deps.TryHandleNetCore5AssemblyBugs(
            package:=type, external_libloc:=Nothing)
#End If

        Using buf As MemoryStream = stream.openMemoryBuffer
            Return emit.handleCreate(buf, type, stream.method)
        End Using
    End Function

    ''' <summary>
    ''' <see cref="SocketRef"/> -> target
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    Private Function GetArgumentValue(i As Integer) As Object
        Dim request As New RequestStream(IPCSocket.Protocol, Protocols.GetArgumentByIndex, BitConverter.GetBytes(i))
        Dim resp = New TcpRequest(masterHost, masterPort) _
            .SetTimeOut(timespan:=TimeSpan.FromSeconds(timeout_sec)) _
            .SetVerbose(verbose_echo) _
            .SendMessage(request)
        Dim stream As New ObjectStream(resp.ChunkBuffer)

        If stream.IsNothing Then
            ' 20210516 object value is nothing?
            Return Nothing
        Else
            Dim socket As SocketRef = SocketRef.GetSocket(stream)

            Using buffer As ObjectStream = socket.Open
                Return FromStream(buffer)
            End Using
        End If
    End Function

    Private Function PostError(err As Exception) As Integer
        Call PostFinished(New IPCError(err), Protocols.PostError)

        Return 500
    End Function

    Private Sub PostFinished(result As Object, protocol As Protocols)
        Dim socket As SocketRef = SocketRef.WriteBuffer(result, emit)

        Using buf As ObjectStream = emit.handleSerialize(socket)
            Dim request As New RequestStream(
                protocolCategory:=IPCSocket.Protocol,
                protocol:=protocol,
                buffer:=buf.Serialize
            )

            If verbose_echo Then
                If TypeOf result Is IPCError Then
                    Call VBDebugger.EchoLine($"post error...")
                Else
                    Call VBDebugger.EchoLine($"post result...")
                End If
            End If

            Call New TcpRequest(masterHost, masterPort) _
                .SetTimeOut(timespan:=TimeSpan.FromSeconds(timeout_sec)) _
                .SetVerbose(verbose_echo) _
                .SendMessage(request)
        End Using
    End Sub
End Class
