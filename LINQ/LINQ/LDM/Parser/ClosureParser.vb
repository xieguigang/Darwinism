Imports Microsoft.VisualBasic.LINQ.TokenIcer

Public Module ClosureParser

    Public Function TryParse(st As String) As ClosureTokens()
        Dim parser As TokenParser = New TokenParser With {.InputString = st}
        Dim token As Token = Nothing
        Dim parts As New List(Of ClosureTokens)
        Dim tmp As New List(Of Token)
        Dim current As TokenParser.Tokens
        Dim closure As ClosureTokens

        Do While Not parser.GetToken.ShadowCopy(token) Is Nothing
            Select Case token.TokenName
                Case TokenParser.Tokens.Imports,
                     TokenParser.Tokens.In,
                     TokenParser.Tokens.Let,
                     TokenParser.Tokens.Select,
                     TokenParser.Tokens.Where

                    closure = New ClosureTokens With {
                        .Token = current,
                        .Tokens = tmp.ToArray
                    }
                    Call parts.Add(closure)
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

        closure = New ClosureTokens With {
            .Token = current,
            .Tokens = tmp.ToArray
        }
        Call parts.Add(closure)

        Return parts.ToArray
    End Function
End Module
