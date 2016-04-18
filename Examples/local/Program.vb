Imports System.IO
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Serialization

Module Program

    Sub Main()

        Dim nnnn As Integer() = {3, 424, 2324, 88, 2, 54, 46, 7, 57, 5, -1111, 86, 7, 87, 97, 55}
        Dim value As New Microsoft.VisualBasic.ComputingServices.SharedMemory.HashValue(NameOf(nnnn), nnnn)
        Call value.__DEBUG_ECHO






        Dim remoteMachine As New TaskHost(New IPEndPoint("127.0.0.1", 1234))
        Dim func As Func(Of Stream, String, String()) = AddressOf AnalysisExample.API.LongTest1
        Dim path As String = "E:\Microsoft.VisualBasic.Parallel\trunk\Examples\local\local.vbproj"
        Dim localfile As New ComputingServices.FileSystem.IO.RemoteFileStream(path, FileMode.Open, remoteMachine.FileSystem)
        Dim array As String() = remoteMachine.Invoke(func, {localfile, "this is the message from local machine!"})
        ' remote linq

        Call array.Length.__DEBUG_ECHO

        localfile = New ComputingServices.FileSystem.IO.RemoteFileStream(path, FileMode.Open, remoteMachine.FileSystem)
        Dim source = remoteMachine.AsLinq(Of String)(func, {localfile, "this is the remote linq example!"})
        Dim array2 = (From s As String In source Where InStr(s, "Include=") > 0 Select s)

        For Each line As String In array2
            Call Console.WriteLine(line)
        Next


        Call Pause()
    End Sub

    Sub test()
        Dim info = GetType(Program).AddressOf(NameOf(Main))
        Dim resulkt = info.Invoke(Nothing)
    End Sub
End Module
