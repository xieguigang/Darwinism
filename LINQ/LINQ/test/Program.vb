Imports LINQ.Script
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports LINQ.Interpreter.Query

Public Module Program

    Dim y = 0
    Dim test = From x As Double In {(1 + y) * 8, 2, 3, 4, 5, 6, 7, 8, 9}
               Where x ^ 3 > (5 * x)
               Select x = x ^ 2 + 99, y = x * 2
               Order By y

    Sub Main()
        Call parserTest()
    End Sub

    Sub parserTest()
        Dim script = "
from x as double in [(1+y)*8,2,3,4,5,6,7,8,9]  # this is comment text
where x^3 > (5 *x)
select x = x ^ 2+99 , y = x*2
order by y
"
        Dim tokens = LINQ.Language.GetTokens(script).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim env As New GlobalEnvironment(New Registry, New NamedValue(Of Object)("y", 1))

        Dim result = query.Exec(env)

        Pause()
    End Sub
End Module
