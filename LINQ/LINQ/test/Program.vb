Imports LINQ.Script

Public Module Program

    Sub Main()
        Call parserTest()
    End Sub

    Sub parserTest()
        Dim script = "
from x as double in [(1+y)*8,2,3,4,5,6,7,8,9]  # this is comment text
where x^3 > (5 *x)
select x ^ 2+99 , y = x*2
"
        Dim tokens = LINQ.Language.GetTokens(script).ToArray
        Dim query = tokens.PopulateQueryExpression


        Pause()
    End Sub
End Module
