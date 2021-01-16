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
            Me.key = FixLiteral(key)
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
            Dim raw As JavaScriptObject() = result.ToArray
            Dim keys As Object()
            Dim i As Integer()

            If TypeOf key Is Literals Then
                Dim keyName As String = key.Exec(Nothing)

                keys = raw.Select(Function(obj) obj(keyName)).ToArray
            Else
                keys = raw _
                    .Select(Function(obj)
                                Return GetOrderKey(obj, env)
                            End Function) _
                    .ToArray
            End If

            If keys.All(Function(xi) xi.GetType Is GetType(Double)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, Double)))
            ElseIf keys.All(Function(xi) xi.GetType Is GetType(Integer)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, Integer)))
            ElseIf keys.All(Function(xi) xi.GetType Is GetType(String)) Then
                i = DoOrder(keys.Select(Function(k) DirectCast(k, String)))
            Else
                Throw New NotImplementedException
            End If

            Return i.Select(Function(index) raw(index))
        End Function

        Private Function DoOrder(Of T As IComparable(Of T))(keys As IEnumerable(Of T)) As Integer()
            If desc Then
                Return keys.Select(Function(key, i) (key, i)).OrderByDescending(Function(ti) ti.key).Select(Function(ti) ti.i).ToArray
            Else
                Return keys.Select(Function(key, i) (key, i)).OrderBy(Function(ti) ti.key).Select(Function(ti) ti.i).ToArray
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"order by {key}"
        End Function
    End Class
End Namespace