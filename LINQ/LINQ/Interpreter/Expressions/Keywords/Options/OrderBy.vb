Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    Public Class OrderBy : Inherits PipelineKeyword

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Order By"
            End Get
        End Property

        Dim key As Expression
        Dim desc As Boolean

        Sub New(key As Expression)
            Me.key = key
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Return key.Exec(env)
        End Function

        Private Function GetOrderKey(obj As JavaScriptObject, env As Environment) As Object
            For Each key As String In obj
                env.FindSymbol(key).value = obj(key)
            Next

            Return Exec(env)
        End Function

        Public Overrides Function Exec(result As IEnumerable(Of JavaScriptObject), env As Environment) As IEnumerable(Of JavaScriptObject)
            If desc Then
                Return result.OrderByDescending(Function(obj) GetOrderKey(obj, env))
            Else
                Return result.OrderBy(Function(obj) GetOrderKey(obj, env))
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"order by {key}"
        End Function
    End Class
End Namespace