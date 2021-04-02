#Region "Microsoft.VisualBasic::421ee975643ae542543f35de0068b5bc, Parallel\IpcParallel\SlaveTask.vb"

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

    ' Delegate Function
    ' 
    ' 
    ' Class SlaveTask
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: (+2 Overloads) Emit, handleGET, handlePOST, RunTask
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Diagnostics
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Parallel.IpcStream

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
        Call Console.WriteLine($"[{debugCode.ToHexString}] task finished!")
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
        Call Console.WriteLine($"[{debugCode.ToHexString}] get argument[{i + 1}]...")
        Return streamBuf.handleSerialize(socket)
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
        Dim target As New IDelegate(entry)
        Dim result As Object = Nothing
        Dim host As IPCSocket = Nothing
        Dim resultType As Type = entry.Method.ReturnType

        host = New IPCSocket(target, debugPort) With {
            .host = Me,
            .handlePOSTResult =
                Sub(buf)
                    result = handlePOST(buf, resultType, host.GetHashCode)
                End Sub,
            .nargs = parameters.Length,
            .handleGetArgument =
                Function(i)
                    Return handleGET(parameters(i), i, host.GetHashCode)
                End Function,
            .handleError = Sub(ex) result = ex
        }

        Call Microsoft.VisualBasic.Parallel.RunTask(AddressOf host.Run)

        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] port:{host.HostPort}")
        Call Thread.Sleep(100)

        Dim commandlineArgvs As String = builder(processor, host.HostPort)

        'If Not debugPort Is Nothing Then
        '    Console.WriteLine(commandlineArgvs)
        '    Pause()
        'End If

#If netcore5 = 0 Then
        Call CommandLine.Call(processor, commandlineArgvs)
#Else
        Call CommandLine.Call(processor, commandlineArgvs, dotnet:=True)
#End If

        Call host.Stop()
        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] thread exit...")

        If TypeOf result Is IPCError Then
            If ignoreError Then
                With DirectCast(result, IPCError)
                    For Each msg As String In .GetAllErrorMessages
                        Call Console.WriteLine($"[error] {msg}")
                    Next
                    For Each frame As StackFrame In .GetSourceTrace
                        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] {frame.ToString}")
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

