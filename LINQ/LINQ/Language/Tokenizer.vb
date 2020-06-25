Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Text.Parser

Namespace Language

    Public Class Tokenizer

        Dim buffer As New CharBuffer
        Dim text As CharPtr
        Dim escapes As New Escaping

        Friend Class Escaping

            Public [string] As Boolean = False
            Public strWrapper As Char
            Public comment As Boolean = False

        End Class

        Sub New(code As String)
            text = code
        End Sub

        Public Iterator Function GetTokens() As IEnumerable(Of Token)
            Dim token As New Value(Of Token)

            Do While text
                If Not token = walkChar(++text) Is Nothing Then
                    Yield token
                End If
            Loop
        End Function

        ReadOnly keywords As Index(Of String) = {"select", "from", "let", "as", "distinct", "group", "by", "order"}
        ReadOnly operators As Index(Of String) = {"+", "-", "*", "/", "\", "%", "=", "<>", ">", "<", ">=", "<="}
        ReadOnly literal As Index(Of String) = {"true", "false"}
        ReadOnly logicals As Index(Of String) = {"not", "and", "or"}

        Private Function walkChar(c As Char) As Token
            If escapes.string Then
                buffer += c

                If c = escapes.strWrapper Then
                    escapes.string = False
                    Return New Token(Tokens.Literal, buffer.PopAllChars.CharString)
                Else
                    Return Nothing
                End If
            ElseIf escapes.comment Then
                If c = ASCII.CR OrElse c = ASCII.LF Then
                    escapes.comment = False
                    Return New Token(Tokens.Comment, buffer.PopAllChars.CharString)
                Else
                    buffer += c
                    Return Nothing
                End If
            ElseIf c = "'"c OrElse c = """"c OrElse c = "`"c Then
                escapes.string = True
                escapes.strWrapper = c
                Return Nothing
            ElseIf c = "#"c Then
                escapes.comment = True
                Return Nothing
            ElseIf c = " "c OrElse c = ASCII.TAB Then
                If buffer <> 0 Then
                    Return createToken()
                End If
            End If
        End Function

        Private Function createToken() As Token
            Dim text As String = buffer.PopAllChars.CharString
            Dim textLower As String = text.ToLower

            If textLower Like keywords Then
                Return New Token(Tokens.keyword, text)
            ElseIf textLower Like operators Then
                Return New Token(Tokens.Operator, text)
            ElseIf textLower Like literal Then
                Return New Token(Tokens.Boolean, text)
            ElseIf textLower Like logicals Then
                Return New Token(Tokens.Operator, text)
            ElseIf textLower.IsPattern("\d+") Then
                Return New Token(Tokens.Integer, text)
            ElseIf textLower.IsNumeric Then
                Return New Token(Tokens.Number, text)
            Else
                Return New Token(Tokens.Invalid, text)
            End If
        End Function
    End Class
End Namespace