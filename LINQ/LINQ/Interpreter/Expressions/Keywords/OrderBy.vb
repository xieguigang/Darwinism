Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class OrderBy : Inherits KeywordExpression

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Order By"
            End Get
        End Property

        Dim key As Expression

        Sub New(key As Expression)
            Me.key = key
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"order by {key}"
        End Function
    End Class
End Namespace