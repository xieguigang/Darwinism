#Region "Microsoft.VisualBasic::88c85fda6cd5e5bf2705b987d084e35b, Parallel\IpcParallel\TaskBuilder.vb"

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

    ' Class TaskBuilder
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: GetArgumentValue, GetArgumentValueNumber, GetMethod, PostError, Run
    ' 
    '     Sub: PostFinished
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Reflection
#If netcore5 = 1 Then
Imports Microsoft.VisualBasic.ApplicationServices.Development.NetCore5
#End If
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Parallel.IpcStream

''' <summary>
''' Run on slave node
''' </summary>
Public Class TaskBuilder : Implements ITaskDriver

    ReadOnly masterPort As Integer
    ReadOnly emit As New StreamEmit

    Sub New(port As Integer)
        masterPort = port
    End Sub

    Private Iterator Function GetParameters(params As ParameterInfo(), n As Integer) As IEnumerable(Of Object)
        For i As Integer = 0 To n - 1
            Dim par As Object = GetArgumentValue(i)
            Dim targetType As Type = params(i).ParameterType

            If par.GetType.IsInheritsFrom(targetType) Then
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

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim task As IDelegate = GetMethod()
        Dim api As MethodInfo = task.GetMethod
        Dim params As ParameterInfo() = api.GetParameters
        Dim target As Object = task.GetMethodTarget
        Dim n As Integer = GetArgumentValueNumber()
        Dim args As New List(Of Object)(GetParameters(params, n))

        Call Console.WriteLine("run task:")
        Call Console.WriteLine(task.GetJson(indent:=False, simpleDict:=True))

        ' fix for optional parameter values
        For i As Integer = n To params.Length - 1
            If Not params(i).IsOptional Then
                Return PostError(New Exception($"missing parameter value for [{i}]{params(i).Name}!"))
            Else
                args.Add(params(i).DefaultValue)
            End If
        Next

        ' send debug message
        Call New TcpRequest(masterPort).SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.PostStart))

        Try
            Call PostFinished(api.Invoke(target, args.ToArray), Protocols.PostResult)
        Catch ex As Exception
            Call PostError(ex)
        Finally
            Call Console.WriteLine("job done!")
        End Try

        Return 0
    End Function

    Private Function GetArgumentValueNumber() As Integer
        Dim resp = New TcpRequest(masterPort).SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.GetArgumentNumber))
        Dim n As Integer = BitConverter.ToInt32(resp.ChunkBuffer, Scan0)

        Return n
    End Function

    Private Function GetMethod() As IDelegate
        Dim resp = New TcpRequest(masterPort).SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.GetTask))
        Dim json As String = resp.GetUTF8String
        Dim target As IDelegate = json.LoadJSON(Of IDelegate)

        Return target
    End Function

    Private Function FromStream(stream As ObjectStream) As Object
        Dim type As Type = stream.type.GetType(knownFirst:=True)

#If netcore5 = 1 Then
        Call deps.TryHandleNetCore5AssemblyBugs(package:=type)
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
        Dim resp = New TcpRequest(masterPort).SendMessage(request)
        Dim stream As New ObjectStream(resp.ChunkBuffer)
        Dim socket As SocketRef = SocketRef.GetSocket(stream)

        Using buffer As ObjectStream = socket.Open
            Return FromStream(buffer)
        End Using
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

            If TypeOf result Is IPCError Then
                Call Console.WriteLine($"post error...")
            Else
                Call Console.WriteLine($"post result...")
            End If

            Call New TcpRequest(masterPort).SendMessage(request)
        End Using
    End Sub
End Class

