Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Language

    Public Class Token : Inherits CodeToken(Of Tokens)

        Sub New(name As Tokens, Optional text As String = Nothing)
            Call MyBase.New(name, text)
        End Sub
    End Class
End Namespace