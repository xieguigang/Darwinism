Imports System.CodeDom
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq.Statements.TokenIcer
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace LDM.Parser

    ''' <summary>
    ''' Divides the string into tokens.
    ''' </summary>
    Public Class Tokenizer

        ReadOnly Tokens As Iterator(Of Token(Of Tokens))

        Dim _IsInvalid As Boolean = False
        Dim _PrevToken As Token = Token.NullToken

        ''' <summary>
        ''' A tokenizer is always constructed on a single string.  Create one tokenizer per string.
        ''' </summary>
        ''' <param name="s">string to tokenize</param>
        Public Sub New(s As String)
            Call Me.New(Statements.TokenIcer.GetTokens(s))
        End Sub

        Sub New(tokens As IEnumerable(Of Token(Of Tokens)))
            Me.Tokens = New Iterator(Of Token(Of Tokens))(tokens)
            MoveNext()
        End Sub

        ''' <summary>
        ''' Moves to the next character.  If there are no more characters, then the tokenizer is
        ''' invalid.
        ''' </summary>
        Private Sub MoveNext()
            If Not Tokens.MoveNext() Then
                _IsInvalid = True
            End If
        End Sub

        ''' <summary>
        ''' Allows access to the token most recently parsed.
        ''' </summary>
        Public ReadOnly Property Current() As Token(Of Tokens)
            Get
                Return _PrevToken
            End Get
        End Property

        ''' <summary>
        ''' Indicates that there are no more characters in the string and tokenizer is finished.
        ''' </summary>
        Public ReadOnly Property IsInvalid() As Boolean
            Get
                Return _IsInvalid
            End Get
        End Property

        ''' <summary>
        ''' Is the current character a letter or underscore?
        ''' </summary>
        Public ReadOnly Property IsChar() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Dim Current = Tokens.GetCurrent
                Return Current.TokenName = Statements.TokenIcer.Tokens.String
            End Get
        End Property

        ''' <summary>
        ''' Is the current character a dot (".")?
        ''' </summary>
        Public ReadOnly Property IsDot() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Return Tokens.GetCurrent.TokenName = Statements.TokenIcer.Tokens.CallFunc
            End Get
        End Property

        ''' <summary>
        ''' Is the current character a comma?
        ''' </summary>
        Public ReadOnly Property IsComma() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Return Tokens.GetCurrent.TokenName = Statements.TokenIcer.Tokens.ParamDeli
            End Get
        End Property

        ''' <summary>
        ''' Is the current character a number?
        ''' </summary>
        Public ReadOnly Property IsNumber() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Dim t As Tokens = Tokens.GetCurrent.TokenName
                Return t = Statements.TokenIcer.Tokens.Integer OrElse t = Statements.TokenIcer.Tokens.Float
            End Get
        End Property

        ''' <summary>
        ''' Is the current character a whitespace character?
        ''' </summary>
        Public ReadOnly Property IsSpace() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Return Tokens.GetCurrent.TokenName = Statements.TokenIcer.Tokens.WhiteSpace
            End Get
        End Property

        ''' <summary>
        ''' Is the current character an operator?
        ''' </summary>
        Public ReadOnly Property IsOperator() As Boolean
            Get
                If _IsInvalid Then
                    Return False
                End If
                Select Case Tokens.GetCurrent.TokenName
                    Case Statements.TokenIcer.Tokens.Slash,
                         Statements.TokenIcer.Tokens.RPair,
                         Statements.TokenIcer.Tokens.Plus,
                         Statements.TokenIcer.Tokens.Or,
                         Statements.TokenIcer.Tokens.Not,
                         Statements.TokenIcer.Tokens.Minus,
                         Statements.TokenIcer.Tokens.LPair,
                         Statements.TokenIcer.Tokens.Is,
                         Statements.TokenIcer.Tokens.Equals,
                         Statements.TokenIcer.Tokens.And
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property

        ''' <summary>
        ''' Gets the next token in the string.  Reads as many characters as necessary to retrieve
        ''' that token.
        ''' </summary>
        ''' <returns>next token</returns>
        Public Function GetNextToken() As Token
            If _IsInvalid Then
                Return Token.NullToken
            End If

            Dim token__1 As Token(Of Tokens)
            If IsChar Then
                token__1 = GetString()
            ElseIf IsComma Then
                token__1 = New Token(",", Statements.TokenIcer.Tokens.ParamDeli, TokenPriority.None)
                MoveNext()
            ElseIf IsDot Then
                token__1 = New Token(".", Statements.TokenIcer.Tokens.CallFunc, TokenPriority.None)
                MoveNext()
            ElseIf IsNumber Then
                token__1 = GetNumber()
            ElseIf IsSpace Then
                ' Eat space and do recursive call.
                MoveNext()
                token__1 = GetNextToken()
            ElseIf IsOperator Then
                token__1 = GetOperator()
            Else
                token__1 = Token.NullToken
                MoveNext()
            End If

            _PrevToken = token__1
            Return token__1
        End Function

        ''' <summary>
        ''' Anything that starts with a character is considered a string.  This could be a 
        ''' primitive quoted string, a primitive expression, or an identifier
        ''' </summary>
        ''' <returns></returns>
        Private Function GetString() As Token
            Dim s As String = Tokens.GetCurrent.TokenValue
            ' "false" or "true" is a primitive expression.
            If s = "false" OrElse s = "true" Then
                Return New Token([Boolean].Parse(s), Statements.TokenIcer.Tokens.String, TokenPriority.None)
            End If

            ' The previous token was a quote, so this is a primitive string.
            Return New Token(Tokens.GetCurrent, TokenPriority.None)
        End Function

        ''' <summary>
        ''' A token that starts with a number can be an integer, a long, or a double.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' An integer is the default for numbers.  Numbers can also be followed by a
        ''' l, L, d, or D character to indicate a long or a double value respectively.
        ''' Any numbers containing a dot (".") are considered doubles.
        ''' </remarks>
        Private Function GetNumber() As Token
            Dim Current = Tokens.GetCurrent
            Dim s As String = Current.TokenValue

            If Current.TokenName = Statements.TokenIcer.Tokens.Float Then
                Return New Token([Double].Parse(s), Statements.TokenIcer.Tokens.String, TokenPriority.None)
            End If
            Return New Token(Int32.Parse(s), Statements.TokenIcer.Tokens.String, TokenPriority.None)
        End Function

        ''' <summary>
        ''' Some operators take more than one character.  Also, the tokenizer is able to 
        ''' categorize the token's priority based on what kind of operator it is.
        ''' </summary>
        ''' <returns></returns>
        Private Function GetOperator() As Token
            Dim Current = Tokens.GetCurrent

            Select Case Current.TokenName
                Case Statements.TokenIcer.Tokens.Equals OrElse Statements.TokenIcer.Tokens.Is
                    MoveNext()
                    Return New Token(Current.TokenValue, Statements.TokenIcer.Tokens.Is, TokenPriority.Equality)
                Case Statements.TokenIcer.Tokens.Minus
                    MoveNext()
                    If _PrevToken.TokenName = Primitive OrElse _PrevToken.Type = Statements.TokenIcer.Tokens.Identifier Then
                        Return New Token(Current.TokenValue, Statements.TokenIcer.Tokens.Minus, TokenPriority.PlusMinus)
                    Else
                        Return New Token(Current.TokenValue, Statements.TokenIcer.Tokens.Minus, TokenPriority.UnaryMinus)
                    End If
                Case Statements.TokenIcer.Tokens.Plus
                    MoveNext()
                    Return New Token(Current.TokenValue, Statements.TokenIcer.Tokens.Plus, TokenPriority.PlusMinus)
                Case Statements.TokenIcer.Tokens.Not
                    MoveNext()
                    If Tokens.GetCurrent.TokenName = Statements.TokenIcer.Tokens.Equals Then
                        MoveNext()
                        Return New Token(Tokens.GetCurrent.TokenValue, Statements.TokenIcer.Tokens.Equals, TokenPriority.Equality)
                    Else
                        Return New Token(Tokens.GetCurrent.TokenValue, Statements.TokenIcer.Tokens.Not, TokenPriority.[Not])
                    End If
                Case Statements.TokenIcer.Tokens.Asterisk OrElse Statements.TokenIcer.Tokens.Slash
                    MoveNext()
                    Return New Token(Tokens.GetCurrent.TokenValue, Tokens.GetCurrent.TokenName, TokenPriority.MulDiv)
                Case Statements.TokenIcer.Tokens.Mod
                    MoveNext()
                    Return New Token("%", Statements.TokenIcer.Tokens.Mod, TokenPriority.[Mod])
                Case Statements.TokenIcer.Tokens.Or
                    MoveNext()
                    Return New Token(Tokens.GetCurrent.TokenValue, Statements.TokenIcer.Tokens.Or, TokenPriority.[Or])
                Case Statements.TokenIcer.Tokens.And
                    MoveNext()
                    Return New Token(Tokens.GetCurrent, Statements.TokenIcer.Tokens.And, TokenPriority.[And])
                Case Statements.TokenIcer.Tokens.LPair
                    MoveNext()
                    Return New Token("(", OpenParens, TokenPriority.None)
                Case Statements.TokenIcer.Tokens.RPair
                    MoveNext()
                    Return New Token(")", CloseParens, TokenPriority.None)
                Case Statements.TokenIcer.Tokens.OpenBracket
                    MoveNext()
                    Return New Token("[", Statements.TokenIcer.Tokens.OpenBracket, TokenPriority.None)
                Case Statements.TokenIcer.Tokens.CloseBracket
                    MoveNext()
                    Return New Token("]", Statements.TokenIcer.Tokens.CloseBracket, TokenPriority.None)
                Case Else
                    ' When we detect a quote, we can just ignore it since the user doesn't really need to know about it.
                    MoveNext()
                    _PrevToken = New Token(Tokens.GetCurrent.TokenValue, Statements.TokenIcer.Tokens.String, TokenPriority.None)
                    Return GetString()
            End Select
            Return Token.NullToken
        End Function
    End Class
End Namespace