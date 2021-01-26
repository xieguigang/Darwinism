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

Public Class TaskBuilder : Implements ITaskDriver

    ReadOnly masterPort As Integer
    ReadOnly emit As New StreamEmit

    Sub New(port As Integer)
        masterPort = port
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim task As IDelegate = GetMethod()
        Dim api As MethodInfo = task.GetMethod
        Dim target As Object = task.GetMethodTarget
        Dim n As Integer = GetArgumentValueNumber()
        Dim args As New List(Of Object)

        For i As Integer = 0 To n - 1
            args.Add(GetArgumentValue(i))
        Next

        Call Console.WriteLine("run task:")
        Call Console.WriteLine(task.GetJson(indent:=False, simpleDict:=True))

        Dim params As ParameterInfo() = api.GetParameters

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
            Call PostFinished(api.Invoke(target, args.ToArray))
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

    Private Function GetArgumentValue(i As Integer) As Object
        Dim request As New RequestStream(IPCSocket.Protocol, Protocols.GetArgumentByIndex, BitConverter.GetBytes(i))
        Dim resp = New TcpRequest(masterPort).SendMessage(request)
        Dim stream As New ObjectStream(resp.ChunkBuffer)
        Dim type As Type = stream.type.GetType(knownFirst:=True)

#If netcore5 = 1 Then
        Call deps.TryHandleNetCore5AssemblyBugs(package:=type)
#End If

        Using buf As MemoryStream = stream.openMemoryBuffer
            Return emit.handleCreate(buf, type, stream.method)
        End Using
    End Function

    Private Function PostError(err As Exception) As Integer
        Call PostFinished(New IPCError(err))

        Return 500
    End Function

    Private Sub PostFinished(result As Object)
        Using buf As Stream = emit.handleSerialize(result).openMemoryBuffer
            Dim request As New RequestStream(
                protocolCategory:=IPCSocket.Protocol,
                protocol:=Protocols.PostResult,
                buffer:=New StreamPipe(buf).Read
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
