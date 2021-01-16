Imports LINQ.Interpreter.Query
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.ApplicationServices.Terminal
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.My.JavaScript

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine, executeFile:=AddressOf RunQuery)
    End Function

    Private Function RunQuery(file As String, args As CommandLine) As Integer
        Dim tokens = LINQ.Language.GetTokens(file.ReadAllText).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim env As New GlobalEnvironment(New Registry)
        Dim result As JavaScriptObject() = query.Exec(env)
        Dim table As DataFrame = result.CreateTableDataSet
        Dim text As String()() = table _
            .csv _
            .Select(Function(c) c.ToArray) _
            .ToArray

        Call text.PrintTable

        Return 0
    End Function
End Module
