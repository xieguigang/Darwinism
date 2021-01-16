Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class BinaryExpression : Inherits Expression

        Dim left, right As Expression
        Dim op As String

        Sub New(left As Expression, right As Expression, op As String)
            Me.left = left
            Me.right = right
            Me.op = op
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"({left} {op} {right})"
        End Function
    End Class
End Namespace