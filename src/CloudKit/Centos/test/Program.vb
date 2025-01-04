Imports System.IO
Imports Darwinism.Centos.proc.net

Module Program
    Sub Main(args As String())
        Console.WriteLine("Hello World!")
        parser_test1()
    End Sub

    Private Sub parser_test1()
        Dim file As Stream = "G:\GCModeller\src\runtime\Darwinism\test\linux\tcp.txt".Open
        Dim tcplist = tcp.Parse(file).ToArray

        Pause()
    End Sub

End Module
