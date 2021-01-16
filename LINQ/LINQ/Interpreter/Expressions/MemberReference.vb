Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Expressions

    Public Class MemberReference : Inherits Expression

        Friend ReadOnly symbol As Expression
        Friend ReadOnly memberName As String

        Sub New(symbol As Expression, memberName As Expression)
            Me.symbol = symbol

            If TypeOf memberName Is SymbolReference Then
                Me.memberName = DirectCast(memberName, SymbolReference).symbolName
            ElseIf TypeOf memberName Is Literals Then
                Me.memberName = any.ToString(memberName.Exec(Nothing))
            Else
                Throw New InvalidExpressionException(memberName.name)
            End If
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim symbol As Object = Me.symbol.Exec(context)

            If symbol Is Nothing Then
                Throw New NullReferenceException
            End If

            If TypeOf symbol Is JavaScriptObject Then
                Return DirectCast(symbol, JavaScriptObject)(memberName)
            End If

            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"{symbol}->{memberName}"
        End Function
    End Class
End Namespace