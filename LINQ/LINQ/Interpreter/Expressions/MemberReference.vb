Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class MemberReference : Inherits Expression

        ReadOnly symbol As Expression
        ReadOnly memberName As Expression

        Sub New(symbol As Expression, memberName As Expression)
            Me.symbol = symbol
            Me.memberName = memberName
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"{symbol}->{memberName}"
        End Function
    End Class
End Namespace