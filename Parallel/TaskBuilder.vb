Imports System.IO
Imports System.Reflection
#If netcore5 = 1 Then
Imports Microsoft.VisualBasic.ApplicationServices.Development.NetCore5
#End If
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.Net.Tcp
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class TaskBuilder : Implements ITaskDriver

    ReadOnly masterPort As Integer

    Sub New(port As Integer)
        masterPort = port
    End Sub

    Public Function Run() As Integer Implements ITaskDriver.Run
        Dim task As IDelegate = GetMethod()
        Dim n As Integer = GetArgumentValueNumber()
        Dim args As New List(Of Object)

        For i As Integer = 0 To n - 1
            args.Add(GetArgumentValue(i))
        Next

        Dim api As MethodInfo = task.GetMethod
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
        Call PostFinished(api.Invoke(Nothing, args.ToArray))

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
        Dim resp = New TcpRequest(masterPort).SendMessage(New RequestStream(IPCSocket.Protocol, Protocols.GetArgumentByIndex, BitConverter.GetBytes(i)))
        Dim stream As New ObjectStream(resp.ChunkBuffer)
        Dim type As Type = stream.type.GetType(knownFirst:=True)

#If netcore5 = 1 Then
        Call deps.TryHandleNetCore5AssemblyBugs(package:=type)
#End If

        If stream.method = StreamMethods.BSON Then
            Return BSONFormat.Load(stream.stream).CreateObject(type)
        Else
            Throw New NotImplementedException
        End If
    End Function

    Private Function PostError(err As Exception) As Integer


        Return 500
    End Function

    Private Sub PostFinished(result As Object)
        Dim type As Type = result.GetType
        Dim element = type.GetJsonElement(result, New JSONSerializerOptions)
        Dim buf As Stream = BSONFormat.SafeGetBuffer(element)
        Dim request As New RequestStream(IPCSocket.Protocol, Protocols.PostResult, New StreamPipe(buf).Read)

        Call New TcpRequest(masterPort).SendMessage(request)
    End Sub
End Class
