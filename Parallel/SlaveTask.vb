Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.Scripting.MetaData

Public Delegate Function ISlaveTask(processor As InteropService, port As Integer) As String

Public Class SlaveTask

    ReadOnly toBuffers As New Dictionary(Of Type, Func(Of Object, Stream))
    ReadOnly fromBuffer As New Dictionary(Of Type, Func(Of Stream, Object))
    ReadOnly processor As InteropService
    ReadOnly builder As ISlaveTask
    ReadOnly debugPort As Integer?

    Sub New(processor As InteropService, cli As ISlaveTask, Optional debugPort As Integer? = Nothing)
        Me.builder = cli
        Me.processor = processor
        Me.debugPort = debugPort
    End Sub

    Public Function Emit(Of T)(streamAs As Func(Of T, Stream)) As SlaveTask
        toBuffers(GetType(T)) = Function(obj) streamAs(obj)
        Return Me
    End Function

    Public Function Emit(Of T)(fromStream As Func(Of Stream, T)) As SlaveTask
        fromBuffer(GetType(T)) = Function(buf) fromStream(buf)
        Return Me
    End Function

    Private Function handlePOST(buf As Stream, type As Type) As Object
        If fromBuffer.ContainsKey(type) Then
            Return fromBuffer(type)(buf)
        Else
            Return BSONFormat.Load(buf).CreateObject(type)
        End If
    End Function

    Private Function handleGET(param As Object) As ObjectStream
        Dim type As Type = param.GetType

        If toBuffers.ContainsKey(type) Then
            Return New ObjectStream(New TypeInfo(type), StreamMethods.Emit, toBuffers(type)(param))
        Else
            Dim element = type.GetJsonElement(param, New JSONSerializerOptions)
            Dim buf As Stream = BSONFormat.SafeGetBuffer(element)

            Return New ObjectStream(New TypeInfo(type), StreamMethods.BSON, buf)
        End If
    End Function

    Public Function RunTask(entry As [Delegate], ParamArray parameters As Object()) As Object
        Dim target As New IDelegate(entry)
        Dim result As Object = Nothing
        Dim host As New IPCSocket(target, debugPort) With {
            .handlePOSTResult = Sub(buf) result = handlePOST(buf, entry.Method.ReturnType),
            .nargs = parameters.Length,
            .handleGetArgument = Function(i) handleGET(parameters(i))
        }

        Call New Thread(AddressOf host.Run).Start()
        Call Thread.Sleep(100)

        'If Not debugPort Is Nothing Then
        '    Pause()
        'End If

        Call CommandLine.Call(processor, builder(processor, host.HostPort))

        Return result
    End Function

End Class
