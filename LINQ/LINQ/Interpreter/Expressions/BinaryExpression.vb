Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class BinaryExpression : Inherits Expression

        Friend left, right As Expression
        Dim op As String

        Public ReadOnly Property LikeValueAssign As Boolean
            Get
                If op <> "=" Then
                    Return False
                Else
                    Return TypeOf left Is SymbolReference
                End If
            End Get
        End Property

        Sub New(left As Expression, right As Expression, op As String)
            Me.left = left
            Me.right = right
            Me.op = op
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim x As Object = left.Exec(context)
            Dim y As Object = right.Exec(context)

            Select Case op
                Case "+" : Return x + y
                Case "-" : Return x - y
                Case "*" : Return x * y
                Case "/" : Return x / y
                Case "^" : Return x ^ y
                Case ">" : Return x > y
                Case "<" : Return x < y
                Case "=" : Return x = y

                Case "&" : Return x & y

                Case ">=" : Return x >= y
                Case "<=" : Return x <= y
                Case "<>" : Return x <> y

                Case Else
                    Throw New NotImplementedException
            End Select
        End Function

        Public Overrides Function ToString() As String
            Return $"({left} {op} {right})"
        End Function
    End Class
End Namespace