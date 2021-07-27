#Region "Microsoft.VisualBasic::9a027e0ed7f92384ff7b28a566c8517e, LINQ\LINQ\Language\Tokenizer.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class Tokenizer
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: createToken, GetTokens, walkChar
    '         Class Escaping
    ' 
    ' 
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Text.Parser

Namespace Language

    Friend Class Tokenizer

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
                    If buffer <> 0 Then
                        Yield createToken(Nothing)
                    End If

                    Yield token
                End If
            Loop

            If buffer > 0 Then
                Yield createToken(Nothing)
            End If
        End Function

        ReadOnly keywords As Index(Of String) = {
            "imports",
            "select", "from", "in", "let", "as", "distinct", "group", "by", "order", "aggregate",
            "where", "take", "skip", "into",
            "descending", "ascending"
        }
        ReadOnly operators As Index(Of String) = {"+", "-", "*", "/", "\", "%", "=", "<>", ">", "<", ">=", "<=", "^", "&"}
        ReadOnly literal As Index(Of String) = {"true", "false"}
        ReadOnly logicals As Index(Of String) = {"not", "and", "or"}

        Private Function walkChar(c As Char) As Token
            If escapes.string Then
                If c = escapes.strWrapper Then
                    Dim str As String = buffer.PopAllChars.CharString

                    escapes.string = False

                    If str.StartsWith("?") Then
                        str = str.Substring(1, str.Length - 1)
                        Return New Token(Tokens.CommandLineArgument, str)
                    Else
                        Return New Token(Tokens.Literal, str)
                    End If
                Else
                    buffer += c
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
            ElseIf c = "?"c Then
                Dim tmp As Token

                If buffer <> 0 Then
                    tmp = createToken(Nothing)
                Else
                    tmp = Nothing
                End If

                buffer += c

                Return tmp
            ElseIf c = " "c OrElse c = ASCII.TAB Then
                If buffer <> 0 Then
                    Return createToken(Nothing)
                Else
                    Return Nothing
                End If
            ElseIf c = ASCII.CR OrElse c = ASCII.LF Then
                Return New Token(Tokens.Terminator, vbCrLf)
            ElseIf c = "<"c OrElse c = ">"c OrElse c = "="c Then
                Return createToken(bufferNext:=c)
            ElseIf c = "["c OrElse c = "("c Then
                Return New Token(Tokens.Open, c)
            ElseIf c = ")"c OrElse c = "]"c Then
                Return New Token(Tokens.Close, c)
            ElseIf c Like operators Then
                Return New Token(Tokens.Operator, c)
            ElseIf c = ","c Then
                Return New Token(Tokens.Comma, c)
            ElseIf c = "."c Then
                If Not buffer.isInteger Then
                    Return New Token(Tokens.Reference, c)
                Else
                    buffer += c
                End If
            Else
                buffer += c
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' 这个函数在创建单词的同时还会将缓存之中的信息清理干净
        ''' </summary>
        ''' <param name="bufferNext"></param>
        ''' <returns></returns>
        Private Function createToken(bufferNext As Char?) As Token
            Dim text As String = buffer.PopAllChars.CharString
            Dim textLower As String

            If escapes.comment Then
                escapes.comment = False
                Return New Token(Tokens.Comment, text)
            Else
                textLower = text.ToLower
            End If

            If Not bufferNext Is Nothing Then
                Dim test As String = text & bufferNext.Value

                If test Like operators Then
                    Return New Token(Tokens.Operator, test)
                Else
                    buffer += bufferNext
                End If
            End If

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
            ElseIf textLower.IsPattern("[a-z_][a-z_0-9]*") Then
                Return New Token(Tokens.Symbol, text)
            Else
                Return New Token(Tokens.Invalid, text)
            End If
        End Function
    End Class
End Namespace
