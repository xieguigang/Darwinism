Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Language

Namespace Script

    Public Module SyntaxImplements

        <Extension>
        Public Function PopulateQueryExpression(tokens As IEnumerable(Of Token)) As Expression
            Dim blocks = tokens.SplitByTopLevelStack.ToArray


        End Function

        Public Function ParseExpression(tokens As IEnumerable(Of Token)) As Expression

        End Function
    End Module
End Namespace