Module Module1

    Sub Main()
        Dim ps As New Docker.PowerShell

        Call Console.WriteLine(ps.RunScript("docker ps"))

        Pause()
    End Sub

End Module
