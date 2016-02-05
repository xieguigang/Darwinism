Module Program

    ''' <summary>
    ''' DO_NOTHING
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Main() As Integer

        Dim s As String = "From var As Type In ""$source->parallel"" Let x = var -> aa(d,""g ++ "") Let x = var -> aa(d,""g ++ "") where x -> test2(test3(xx),var) is true Let x = var -> aa(d,""g ++ "") Let x = var -> aa(d,""g ++ "") Let x = var -> aa(d,""g ++ "") select new varType(var,x), x+3"
        Dim expr = LINQ.Statements.LINQStatement.TryParse(s)

        Return GetType(CLI).RunCLI(App.CommandLine, AddressOf __exeEmpty)
    End Function

    Private Function __exeEmpty() As Integer
        Call Console.WriteLine("{0}!{1}", GetType(Program).Assembly.Location, GetType(LINQ.Framework.LQueryFramework).FullName)
        Return 0
    End Function
End Module
