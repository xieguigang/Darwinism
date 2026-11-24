Imports LINQ.Language

''' <summary>
''' 脚本词法调试工具
''' </summary>
Module Probe

    Public Sub DumpTokens(script As String)
        For Each t As Token In LINQ.Language.GetTokens(script)
            Call Console.WriteLine($"[{t.name}]	{t.text}")
        Next
    End Sub
End Module
