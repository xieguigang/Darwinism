Imports LINQ.Runtime
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Interpreter.Expressions

    Public Class OutputProjection : Inherits Expression

        Public Property fields As NamedValue(Of Expression)()

        Sub New(fields As IEnumerable(Of NamedValue(Of Expression)))
            Me.fields = fields.ToArray
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"new {{{fields.Select(Function(a) $"{a.Name} = {a.Value}").JoinBy(", ")}}}"
        End Function
    End Class
End Namespace