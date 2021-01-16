
Imports LINQ.Language
Imports LINQ.Runtime
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Expressions

    Public Class Literals : Inherits Expression

        Public Property value As Object

        Public ReadOnly Property type As Type
            Get
                If value Is Nothing Then
                    Return GetType(Void)
                Else
                    Return value.GetType
                End If
            End Get
        End Property

        Sub New(t As Token)
            Select Case t.name
                Case Tokens.Boolean : value = t.text.ParseBoolean
                Case Tokens.Integer : value = t.text.ParseInteger
                Case Tokens.Number : value = t.text.ParseDouble
                Case Else
                    Throw New InvalidCastException
            End Select
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Return value
        End Function

        Public Overrides Function ToString() As String
            Return $"({type.Name.ToLower}) {any.ToString(value, "null")}"
        End Function
    End Class
End Namespace