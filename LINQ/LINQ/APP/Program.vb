Module Program

    ''' <summary>
    ''' DO_NOTHING
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Main() As Integer

        Dim s As String = "From var As Type In $source->parallel Let x = var -> aa(d,""g ++ "") where x -> test2(test3(xx),var) is true select new varType(var,x), x+3"
        Dim t As New TokenIcer.TokenParser
        t.InputString = s
        Dim tk As TokenIcer.Token = Nothing
        Dim ide As Integer = 0

        Do While Not t.GetToken.ShadowCopy(tk) Is Nothing
            Call $"[{tk.TokenName}]{tk.TokenValue}".__DEBUG_ECHO(ide)
            If tk.TokenName = TokenIcer.TokenParser.Tokens.LPair Then
                ide += 1
            ElseIf tk.TokenName = TokenIcer.TokenParser.Tokens.RPair Then
                ide -= 1
            ElseIf tk.TokenName = TokenIcer.TokenParser.Tokens.From OrElse
                tk.TokenName = TokenIcer.TokenParser.Tokens.In OrElse
                tk.TokenName = TokenIcer.TokenParser.Tokens.Select OrElse
                tk.TokenName = TokenIcer.TokenParser.Tokens.Let OrElse
                tk.TokenName = TokenIcer.TokenParser.Tokens.Where Then
                ide += 2
            End If
        Loop


        Return GetType(CLI).RunCLI(App.CommandLine, AddressOf __exeEmpty)
    End Function

    Private Function __exeEmpty() As Integer
        Call Console.WriteLine("{0}!{1}", GetType(Program).Assembly.Location, GetType(LINQ.Framework.LQueryFramework).FullName)
        Return 0
    End Function
End Module
