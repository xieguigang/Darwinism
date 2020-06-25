Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Language

Namespace Script

    Public Module SyntaxImplements

        <Extension>
        Public Function PopulateQueryExpressions(tokens As IEnumerable(Of Token)) As IEnumerable(Of Expression)

        End Function

        Public Function ParseExpression(tokens As IEnumerable(Of Token)) As Expression

        End Function
    End Module
End Namespace