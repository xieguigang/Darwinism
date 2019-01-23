Imports Darwinism
Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()
        Dim ps As New Docker.PowerShell

        ' Call Console.WriteLine(ps.RunScript("docker ps"))

        '   Call Console.WriteLine(Docker.PS.ToArray.GetJson)

        '    Call Console.WriteLine(Docker.Search("centos").ToArray.GetJson)


        Call Console.WriteLine(Docker.Run("centos", "echo ""hello world"""))
        Call Console.WriteLine(Docker.Run("centos", "ls -l /mnt/ntfs", New Docker.Mount With {.local = "D:\test", .virtual = "/mnt/ntfs"}))

        For Each line In Docker.CommandHistory
            Call Console.WriteLine(line)
        Next

        Pause()
    End Sub

End Module
