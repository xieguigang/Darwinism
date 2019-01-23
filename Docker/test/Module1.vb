Imports Microsoft.VisualBasic.Serialization.JSON

Module Module1

    Sub Main()
        Dim ps As New Docker.PowerShell

        Call Console.WriteLine(ps.RunScript("docker ps"))

        Call Console.WriteLine(Docker.PS.ToArray.GetJson)

        Call Console.WriteLine(Docker.Search("centos").ToArray.GetJson)

        Pause()
    End Sub

End Module
