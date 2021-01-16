Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class WhereFilter : Inherits Expression

        Dim filter As Expression

        Sub New(filter As Expression)
            Me.filter = filter
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace