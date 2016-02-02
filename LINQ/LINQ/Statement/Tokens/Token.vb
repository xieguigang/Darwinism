Namespace Statements.Tokens

    Public MustInherit Class Token

        Protected Statement As LINQStatement
        Protected Friend _original As String

        Public Overrides Function ToString() As String
            Return _original
        End Function
    End Class
End Namespace