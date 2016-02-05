Imports Microsoft.VisualBasic.LINQ.TokenIcer
Imports Microsoft.VisualBasic.LINQ.TokenIcer.TokenParser

Public Class ClosureTokens
    Public Property Token As Tokens
    Public Property Tokens As Token()

    Public Overrides Function ToString() As String
        Return $"[{Token}] {Tokens.ToArray(Function(x) x.TokenValue).JoinBy(" ")}"
    End Function
End Class