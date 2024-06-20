Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.Parallel

Module Program
    Sub Main(args As String())
        Dim cmdl = App.CommandLine

        If cmdl.Name = "-s" Then
            Call runServer()
        Else
            Call runClient()
        End If
    End Sub

    Dim listen As Integer = 11003

    Private Sub runClient()
        Dim socket As New TcpRequest("127.0.0.1", listen)
        Dim msg As New RequestStream(0, 0, "hello")

        socket.SendMessage(msg)
    End Sub

    Private Sub runServer()
        Dim socket As New TcpServicesSocket(listen)
        socket.Run()
    End Sub
End Module
