Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Microsoft.VisualBasic.Language

Public Delegate Function ISlaveTask(processor As InteropService, port As Integer) As String

Public Class SlaveTask

    ReadOnly processor As InteropService
    ReadOnly builder As ISlaveTask
    ReadOnly debugPort As Integer?
    ReadOnly streamBuf As New StreamEmit

    Sub New(processor As InteropService, cli As ISlaveTask, Optional debugPort As Integer? = Nothing)
        Me.builder = cli
        Me.processor = processor
        Me.debugPort = debugPort
    End Sub

    Public Function Emit(Of T)(streamAs As Func(Of T, Stream)) As SlaveTask
        Call streamBuf.Emit(streamAs)
        Return Me
    End Function

    Public Function Emit(Of T)(fromStream As Func(Of Stream, T)) As SlaveTask
        Call streamBuf.Emit(fromStream)
        Return Me
    End Function

    Private Function handlePOST(buf As Stream, type As Type, debugCode As Integer) As Object
        Call Console.WriteLine($"[{debugCode.ToHexString}] task finished!")
        Return streamBuf.handleCreate(buf, type, StreamMethods.Auto)
    End Function

    Private Function handleGET(param As Object, i As Integer, debugCode As Integer) As ObjectStream
        Call Console.WriteLine($"[{debugCode.ToHexString}] get argument[{i + 1}]...")
        Return streamBuf.handleSerialize(param)
    End Function

    Public Function RunTask(Of T)(entry As [Delegate], ParamArray parameters As Object()) As T
        Dim target As New IDelegate(entry)
        Dim result As Object = Nothing
        Dim host As IPCSocket = Nothing
        Dim resultType As Type = entry.Method.ReturnType

        host = New IPCSocket(target, debugPort) With {
            .handlePOSTResult = Sub(buf) result = handlePOST(buf, resultType, host.GetHashCode),
            .nargs = parameters.Length,
            .handleGetArgument = Function(i) handleGET(parameters(i), i, host.GetHashCode)
        }

        Call Microsoft.VisualBasic.Parallel.RunTask(AddressOf host.Run)

        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] port:{host.HostPort}")
        Call Thread.Sleep(100)

        ' Dim resultStream As MemoryStream
        Dim commandlineArgvs As String = builder(processor, host.HostPort)

        If Not debugPort Is Nothing Then
            Console.WriteLine(commandlineArgvs)
            Pause()
        End If

#If netcore5 = 0 Then
        Call CommandLine.Call(processor, commandlineArgvs)
#Else
        Call CommandLine.Call(processor, commandlineArgvs, dotnet:=True)
#End If

        Call host.Stop()
        ' Call Console.WriteLine(If(result Is Nothing, "null", result.ToString))
        Call Console.WriteLine($"[{host.GetHashCode.ToHexString}] thread exit...")

        ' result = decomposingStdoutput(resultStream, resultType, host.GetHashCode)
        ' resultStream.Close()
        ' resultStream.Dispose()

        Return result
    End Function
End Class
