#Region "Microsoft.VisualBasic::2248825b67980049b6e4b5f64228dfa5, src\computing\Parallel\IpcParallel\SlaveTask.vb"

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

    '   Total Lines: 193
    '    Code Lines: 126 (65.28%)
    ' Comment Lines: 35 (18.13%)
    '    - Xml Docs: 91.43%
    ' 
    '   Blank Lines: 32 (16.58%)
    '     File Size: 7.07 KB


    ' Delegate Function
    ' 
    ' 
    ' Class SlaveTask
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: (+2 Overloads) Emit, GetValueFromStream, handleGET, handlePOST, RunTask
    '               startSocket
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Threading
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports options = Darwinism.HPC.Parallel.Extensions

''' <summary>
''' generates the commandline string not contains the executable file path
''' </summary>
''' <param name="processor"></param>
''' <param name="port"></param>
''' <returns></returns>
Public Delegate Function ISlaveTask(processor As InteropService, port As Integer) As String

''' <summary>
''' the master node of the slave node
''' </summary>
Public Class SlaveTask

    ReadOnly processor As InteropService
    ReadOnly builder As ISlaveTask
    ReadOnly debugPort As Integer?
    ReadOnly ignoreError As Boolean

    Friend ReadOnly streamBuf As New StreamEmit

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="processor"></param>
    ''' <param name="cli">function delegate to generates a commandline for run 
    ''' a parallel task. this commandline string should not contains the 
    ''' executable file path.</param>
    ''' <param name="debugPort"></param>
    ''' <param name="ignoreError"></param>
    <DebuggerStepThrough>
    Sub New(processor As InteropService, cli As ISlaveTask,
            Optional debugPort As Integer? = Nothing,
            Optional ignoreError As Boolean = False)

        Me.builder = cli
        Me.processor = processor
        Me.debugPort = debugPort
        Me.ignoreError = ignoreError
    End Sub

    Public Function Emit(Of T)(streamAs As Func(Of T, Stream)) As SlaveTask
        Call streamBuf.Emit(streamAs)
        Return Me
    End Function

    Public Function Emit(Of T)(fromStream As Func(Of Stream, T)) As SlaveTask
        Call streamBuf.Emit(fromStream)
        Return Me
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="buf">buffer is an <see cref="ObjectStream"/></param>
    ''' <param name="type"></param>
    ''' <param name="debugCode"></param>
    ''' <returns></returns>
    Private Function handlePOST(buf As Stream, type As Type, debugCode As Integer) As Object
        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{debugCode.ToHexString}] task finished!")
        End If

        Return GetValueFromStream(buf, type, streamBuf)
    End Function

    Friend Shared Function GetValueFromStream(buf As Stream, type As Type, streamBuf As StreamEmit) As Object
        Dim obj As New ObjectStream(buf)
        Dim socket As SocketRef = SocketRef.GetSocket(obj)

        obj = socket.Open

        Using file As MemoryStream = obj.openMemoryBuffer
            Return streamBuf.handleCreate(file, type, obj.method)
        End Using
    End Function

    Private Function handleGET(param As Object, i As Integer, debugCode As Integer) As ObjectStream
        Dim socket As SocketRef = SocketRef.WriteBuffer(param, streamBuf)

        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{debugCode.ToHexString}] get argument[{i + 1}]...")
        End If

        Return streamBuf.handleSerialize(socket)
    End Function

    Private Function startSocket(entry As [Delegate], parameters As Object()) As IPCSocket
        Dim target As New IDelegate(entry)
        Dim resultType As Type = entry.Method.ReturnType

        Return New IPCSocket(target, debugPort) With {
            .host = Me,
            .handlePOSTResult =
                Function(buf, host)
                    Return handlePOST(buf, resultType, host.GetHashCode)
                End Function,
            .nargs = parameters.Length,
            .handleGetArgument =
                Function(i, host)
                    Return handleGET(parameters(i), i, host.GetHashCode)
                End Function
        }
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="entry"></param>
    ''' <param name="parameters">
    ''' the common parameter value between the parallel batches 
    ''' can be handle by the pre-cached <see cref="SocketRef"/>.
    ''' </param>
    ''' <returns></returns>
    Public Function RunTask(Of T)(entry As [Delegate], ParamArray parameters As Object()) As T
RE0:
        Dim host As IPCSocket = startSocket(entry, parameters)
        Dim result As Object = Nothing
        Dim hostIndex As Integer = host.GetHashCode

        Call Microsoft.VisualBasic.Parallel.RunTask(AddressOf host.Run)
        Call Thread.Sleep(100)

        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{hostIndex.ToHexString}] port:{host.HostPort}")
        End If

        Dim commandlineArgvs As String = builder(processor, host.HostPort)
        Dim stdout As String

        If Not debugPort Is Nothing Then
            Console.WriteLine(commandlineArgvs)
            Pause()
        End If

        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{hostIndex.ToHexString}] [EXEC] {processor} {commandlineArgvs}")
        End If

#If NET48 Then
        stdout = CommandLine.Call(processor, commandlineArgvs)
#Else
        stdout = CommandLine.Call(processor, commandlineArgvs, dotnet:=True, debug:=Not debugPort Is Nothing)
#End If

        Call host.Stop()

        If Not host.handleSetResult Then
            If options.Verbose AndAlso host.socketExitCode <> 0 Then
                Call VBDebugger.EchoLine($"[{Me.GetHashCode}/{hostIndex.ToHexString}] socket have non-ZERO exit status({host.socketExitCode}), retry...")
                Call VBDebugger.EchoLine($"[{Me.GetHashCode}/{hostIndex.ToHexString}] {host.GetLastError}")
            ElseIf options.Verbose Then
                Call VBDebugger.EchoLine($"[{Me.GetHashCode}] slave process echo:")
                Call VBDebugger.EchoLine(stdout)
                Call VBDebugger.EchoLine("--------- end echo ----------")
            End If

            GoTo RE0
        End If

        If options.Verbose Then
            Call VBDebugger.EchoLine($"[{hostIndex.ToHexString}] thread exit...")
        End If

        result = host.result

        If TypeOf result Is IPCError Then
            If ignoreError Then
                With DirectCast(result, IPCError)
                    For Each msg As String In .GetAllErrorMessages
                        Call Console.WriteLine($"[error] {msg}")
                    Next
                    For Each frame As StackFrame In .GetSourceTrace
                        Call Console.WriteLine($"[{hostIndex.ToHexString}] {frame.ToString}")
                    Next
                End With

                Return App.LogException(IPCError.CreateError(DirectCast(result, IPCError)))
            Else
                Throw IPCError.CreateError(DirectCast(result, IPCError))
            End If
        Else
            Return result
        End If
    End Function
End Class
