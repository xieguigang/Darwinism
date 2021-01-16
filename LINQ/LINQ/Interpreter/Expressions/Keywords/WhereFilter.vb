Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class WhereFilter : Inherits KeywordExpression

        Dim filter As Expression

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Where"
            End Get
        End Property

        Sub New(filter As Expression)
            Me.filter = filter
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"where {filter}"
        End Function
    End Class
End Namespace