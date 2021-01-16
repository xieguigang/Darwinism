Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class ArrayExpression : Inherits Expression

        Dim seq As Expression()

        Sub New(seq As IEnumerable(Of Expression))
            Me.seq = seq.ToArray
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace