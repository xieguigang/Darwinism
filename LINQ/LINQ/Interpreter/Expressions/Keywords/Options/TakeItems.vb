
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    Public Class TakeItems : Inherits PipelineKeyword

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Take"
            End Get
        End Property

        Dim n As Expression

        Sub New(n As Expression)
            Me.n = n
        End Sub

        Public Overrides Function Exec(result As IEnumerable(Of JavaScriptObject), env As Environment) As IEnumerable(Of JavaScriptObject)
            Return result.Take(count:=CInt(Exec(env)))
        End Function

        Public Overrides Function Exec(env As Environment) As Object
            Return n.Exec(env)
        End Function
    End Class
End Namespace