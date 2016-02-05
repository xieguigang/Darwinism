Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace Statements.Tokens

    Public MustInherit Class Token

        Protected _statement As LINQStatement
        Protected _tokens As StatementToken()
        ''' <summary>
        ''' <see cref="_tokens"/>的第一个元素
        ''' </summary>
        Protected _source As StatementToken

        Sub New(type As TokenParser.Tokens, tokens As StatementToken())
            _tokens = GetTokens(type, from:=tokens)
            _source = _tokens.FirstOrDefault
            If _source Is Nothing Then
                If type = TokenParser.Tokens.From OrElse
                    type = TokenParser.Tokens.In OrElse
                    type = TokenParser.Tokens.Select Then
                    ' LET and SELECT is optional
                    ' But From, In and Select is required
                    ' If missing, then syntax error, throw exception
                    Dim msg As String = String.Format(MissingRequiredField, type)
                    Throw New SyntaxErrorException(msg)
                End If
            End If
        End Sub

        Const MissingRequiredField As String = "Missing the required LINQ statement token {0}!"

        Public Overrides Function ToString() As String
            Return _tokens.ToString
        End Function

        Public Shared Function GetTokens(type As TokenIcer.TokenParser.Tokens, from As StatementToken()) As StatementToken()
            Dim LQuery = (From x As StatementToken In from Where x.Token = type Select x).ToArray
            Return LQuery
        End Function
    End Class
End Namespace