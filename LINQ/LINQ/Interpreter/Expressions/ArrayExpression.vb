Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class ArrayExpression : Inherits Expression

        Dim seq As Expression()

        Sub New(seq As IEnumerable(Of Expression))
            Me.seq = seq.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim list As New List(Of Object)

            For Each item In seq
                list.Add(item.Exec(context))
            Next

            Return list.ToArray
        End Function

        Public Overrides Function ToString() As String
            Return $"[{seq.JoinBy(", ")}]"
        End Function
    End Class
End Namespace