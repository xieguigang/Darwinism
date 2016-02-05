Imports Microsoft.VisualBasic.LINQ.TokenIcer

Public Module StatementParser

    Public Function TryParse(st As String) As StatementParser.StatementToken()
        Dim parser As TokenIcer.TokenParser = New TokenParser With {.InputString = st}
        Dim token As Token = Nothing
        Dim parts As New List(Of StatementToken)
        Dim tmp As New List(Of Token)
        Dim current As TokenParser.Tokens

        Do While Not parser.GetToken.ShadowCopy(token) Is Nothing
            Select Case token.TokenName
                Case TokenParser.Tokens.Imports, TokenParser.Tokens.In, TokenParser.Tokens.Let, TokenParser.Tokens.Select, TokenParser.Tokens.Where
                    Call parts.Add(New StatementToken With {.Token = current, .Tokens = tmp.ToArray})
                    Call tmp.Clear()
                    current = token.TokenName
                Case TokenParser.Tokens.From
                    current = TokenParser.Tokens.From
                Case TokenParser.Tokens.WHITESPACE
                    ' Do Nothing
                Case Else
                    Call tmp.Add(token)
            End Select
        Loop

        Call parts.Add(New StatementToken With {.Token = current, .Tokens = tmp.ToArray})

        Return parts.ToArray
    End Function

    Public Class StatementToken
        Public Property Token As TokenParser.Tokens
        Public Property Tokens As Token()

        Public Overrides Function ToString() As String
            Return $"[{Token}] {Tokens.ToArray(Function(x) x.TokenValue).JoinBy(" ")}"
        End Function
    End Class
End Module
