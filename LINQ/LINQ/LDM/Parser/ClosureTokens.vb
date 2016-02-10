Imports Microsoft.VisualBasic.LINQ.Statements
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Statements

    Public Class ClosureTokens
        Public Property Token As TokenIcer.Tokens
        Public Property Tokens As Token(Of TokenIcer.Tokens)()

        Public Overrides Function ToString() As String
            Return $"[{Token}] {Tokens.ToArray(Function(x) x.TokenValue).JoinBy(" ")}"
        End Function
    End Class
End Namespace