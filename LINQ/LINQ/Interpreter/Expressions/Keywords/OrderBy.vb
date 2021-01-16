Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    Public Class OrderBy : Inherits KeywordExpression

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

        Public Function Sort(result As IEnumerable(Of JavaScriptObject), env As Environment) As IEnumerable(Of JavaScriptObject)
            If desc Then
                Return result _
                    .OrderByDescending(Function(obj)
                                           For Each key As String In obj
                                               env.FindSymbol(key).value = obj(key)
                                           Next

                                           Return Exec(env)
                                       End Function)
            Else
                Return result _
                    .OrderBy(Function(obj)
                                 For Each key As String In obj
                                     env.FindSymbol(key).value = obj(key)
                                 Next

                                 Return Exec(env)
                             End Function)
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"order by {key}"
        End Function
    End Class
End Namespace