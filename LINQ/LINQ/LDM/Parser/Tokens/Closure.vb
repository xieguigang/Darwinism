Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace Statements.Tokens

    Public MustInherit Class Closure

        Protected _statement As LINQStatement
        Protected _source As ClosureTokens

        Sub New(type As TokenParser.Tokens, tokens As ClosureTokens(), parent As LINQStatement)
            _source = GetTokens(type, from:=tokens)
            _statement = parent

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

        Sub New(token As ClosureTokens, parent As LINQStatement)
            _source = token
            _statement = parent
        End Sub

        Const MissingRequiredField As String = "Missing the required LINQ statement token {0}!"

        Public Overrides Function ToString() As String
            Return _source.ToString
        End Function

        Public Shared Function GetTokens(type As TokenParser.Tokens, from As ClosureTokens()) As ClosureTokens
            Dim LQuery = (From x As ClosureTokens In from Where x.Token = type Select x).FirstOrDefault
            Return LQuery
        End Function
    End Class
End Namespace