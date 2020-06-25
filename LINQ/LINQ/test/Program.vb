Public Module Program

    Sub Main()
        Call parserTest()
    End Sub

    Sub parserTest()
        Dim script = "from x as double in [1,2,3,4,5,6,7,8,9] where x > 5 select x ^ 2 # this is comment text"
        Dim tokens = LINQ.Language.GetTokens(script).ToArray

        Pause()
    End Sub
End Module
