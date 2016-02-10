Imports System.IO
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub Main()
        Dim remoteMachine As New TaskHost(New IPEndPoint("127.0.0.1", 1234))
        Dim func As Func(Of Stream, String, String()) = AddressOf AnalysisExample.API.LongTest1
        Dim path As String = "E:\Microsoft.VisualBasic.Parallel\trunk\Examples\local\local.vbproj"
        Dim localfile As New ComputingServices.FileSystem.IO.RemoteFileStream(path, FileMode.Open, remoteMachine.FileSystem)
        Dim array As String() = remoteMachine.Invoke(func, {localfile, "this is the message from local machine!"})
        ' remote linq

        Dim source = remoteMachine.AsLinq(Of String)(func, {localfile, "this is the remote linq example!"})
        Dim array2 = (From s As String In source.AsParallel Where InStr(s, "+") Select s).ToArray


        Call Pause()
    End Sub
End Module
