Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.Language
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

    Private Function handlePOST(buf As Stream, type As Type, debugCode As Integer) As Object
        Call Console.WriteLine($"[{debugCode.ToHexString}] task finished!")

        If fromBuffer.ContainsKey(type) Then
            Return fromBuffer(type)(buf)
        Else
            Return BSONFormat.Load(buf).CreateObject(type)
        End If
    End Function

    Private Function handleGET(param As Object, i As Integer, debugCode As Integer) As ObjectStream
        Dim type As Type = param.GetType

        Call Console.WriteLine($"[{debugCode.ToHexString}] get argument[{i + 1}]...")

        If toBuffers.ContainsKey(type) Then
            Return New ObjectStream(New TypeInfo(type, fullpath:=True), StreamMethods.Emit, toBuffers(type)(param))
        Else
            Dim element = type.GetJsonElement(param, New JSONSerializerOptions)
            Dim buf As Stream = BSONFormat.SafeGetBuffer(element)

            Return New ObjectStream(New TypeInfo(type, fullpath:=True), StreamMethods.BSON, buf)
        End If
    End Function

    Public Function RunTask(entry As [Delegate], ParamArray parameters As Object()) As Object
        Dim target As New IDelegate(entry)
        Dim result As Object = Nothing
        Dim host As IPCSocket = Nothing
        Dim resultType As Type = entry.Method.ReturnType

        host = New IPCSocket(target, debugPort) With {
            .handlePOSTResult = Sub(buf) result = handlePOST(buf, resultType, host.GetHashCode),
            .nargs = parameters.Length,
            .handleGetArgument = Function(i) handleGET(parameters(i), i, host.GetHashCode)
        }

        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] port:{host.HostPort}")

        Call New Thread(AddressOf host.Run).Start()
        Call Thread.Sleep(100)

        Dim resultStream As MemoryStream
        Dim commandlineArgvs As String = builder(processor, host.HostPort)

        'If Not debugPort Is Nothing Then
        '    Pause()
        'End If

#If netcore5 = 0 Then
        resultStream = CommandLine.CallDotNetCorePipeline(processor, commandlineArgvs)
#Else
        resultStream = CommandLine.CallDotNetCorePipeline(processor, commandlineArgvs)
#End If

        Call host.Stop()
        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] thread exit...")

        result = decomposingStdoutput(resultStream, resultType, host.GetHashCode)
        resultStream.Close()
        resultStream.Dispose()

        Return result
    End Function

    Private Function decomposingStdoutput(buffer As MemoryStream, type As Type, host As Integer) As Object
        Using reader As New StreamReader(buffer)
            Do While reader.ReadLine <> TaskBuilder.streamDelimiter
            Loop

            Dim dataSize As Integer = Integer.Parse(reader.ReadLine)
            Dim chunkBuffer As Byte() = New Byte(dataSize - 1) {}

            buffer.Seek(buffer.Length - dataSize, SeekOrigin.Begin)
            buffer.Read(chunkBuffer, Scan0, chunkBuffer.Length)

            Using resultBuffer As New MemoryStream(chunkBuffer)
                Return handlePOST(resultBuffer, type, host)
            End Using
        End Using
    End Function

End Class
