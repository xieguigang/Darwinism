Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Language

    Public Class Token : Inherits CodeToken(Of Tokens)

        Sub New(name As Tokens, Optional text As String = Nothing)
            Call MyBase.New(name, text)
        End Sub

        Public Overloads Shared Operator =(t As Token, c As (name As Tokens, text As String)) As Boolean
            Return t.name = c.name AndAlso t.text = c.text
        End Operator

        Public Overloads Shared Operator <>(t As Token, c As (name As Tokens, text As String)) As Boolean
            Return Not t = c
        End Operator

    End Class
End Namespace