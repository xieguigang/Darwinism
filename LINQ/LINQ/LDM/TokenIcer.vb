' This file was Auto Generated with TokenIcer
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Statements.TokenIcer

    ' This is our token enumeration. It holds every token defined in the grammar
    ''' <summary>
    ''' Tokens is an enumeration of all possible token values.
    ''' </summary>
    Public Enum Tokens
        UNDEFINED = 0
        CallFunc = 1
        [Integer] = 2
        Float = 3
        ArrayType = 4
        ParamDeli = 5
        WhiteSpace = 6
        [Let] = 7
        Equals = 8
        StringIntrop = 9
        [String] = 10
        VbCrLf = 11
        LPair = 12
        RPair = 13
        Asterisk = 14
        Slash = 15
        Plus = 16
        Minus = 17
        Pretend = 18
        cor = 19
        [And] = 20
        [Not] = 21
        [Or] = 22
        [Is] = 23
        [As] = 24
        From = 25
        Where = 26
        [Select] = 27
        [Imports] = 28
        [In] = 29
        var = 30
        varRef = 31
    End Enum

    Public Module Parser

        Public ReadOnly Property stackT As StackTokens(Of Tokens) =
            New StackTokens(Of Tokens)(Function(a, b) a = b) With {
                .LPair = Tokens.LPair,
                .ParamDeli = Tokens.ParamDeli,
                .Pretend = Tokens.Pretend,
                .RPair = Tokens.RPair,
                .WhiteSpace = Tokens.WhiteSpace
        }

        Private Function __getParser() As TokenParser(Of Tokens)
            Dim _tokens As New Dictionary(Of Tokens, String)()

            ' These lines add each grammar rule to the dictionary
            _tokens.Add(Tokens.CallFunc, "->\s*[a-zA-Z_][a-zA-Z0-9_]*")
            _tokens.Add(Tokens.[Integer], "[0-9]+")
            _tokens.Add(Tokens.Float, "[0-9]+\.+[0-9]+")
            _tokens.Add(Tokens.ArrayType, "[a-zA-Z_][a-zA-Z0-9_]*\(\)")
            _tokens.Add(Tokens.ParamDeli, ",")
            _tokens.Add(Tokens.WhiteSpace, "[ \t]+")
            _tokens.Add(Tokens.[Let], "[Ll][Ee][Tt]")
            _tokens.Add(Tokens.Equals, "=")
            _tokens.Add(Tokens.StringIntrop, "\$"".*?""")
            _tokens.Add(Tokens.[String], """.*?""")
            _tokens.Add(Tokens.VbCrLf, "[\r\n]+")
            _tokens.Add(Tokens.LPair, "\(")
            _tokens.Add(Tokens.RPair, "\)")
            _tokens.Add(Tokens.Asterisk, "\*")
            _tokens.Add(Tokens.Slash, "\/")
            _tokens.Add(Tokens.Plus, "\+")
            _tokens.Add(Tokens.Minus, "\-")
            _tokens.Add(Tokens.Pretend, "Pretend")
            _tokens.Add(Tokens.cor, "[nN][eE][wW]")
            _tokens.Add(Tokens.[And], "[aA][nN][dD]")
            _tokens.Add(Tokens.[Not], "[nN][oO][tT]")
            _tokens.Add(Tokens.[Or], "[oO][rR]")
            _tokens.Add(Tokens.[Is], "[iI][sS]")
            _tokens.Add(Tokens.[As], "[aA][sS]")
            _tokens.Add(Tokens.From, "[fF][rR][oO][mM]")
            _tokens.Add(Tokens.Where, "[wW][hH][eE][rR][eE]")
            _tokens.Add(Tokens.[Select], "[sS][eE][lL][eE][cC][tT]")
            _tokens.Add(Tokens.[Imports], "^[Ii][mM][pP][oO][rR][tT][sS]")
            _tokens.Add(Tokens.[In], "[iI][nN]")
            _tokens.Add(Tokens.var, "[a-zA-Z_][a-zA-Z0-9_]*")
            _tokens.Add(Tokens.varRef, "\$[a-zA-Z0-9_]*")

            Return New TokenParser(Of Tokens)(_tokens, Tokens.UNDEFINED)
        End Function

        ' Our Constructor, which simply initializes values
        ''' <summary>
        ''' Default Constructor
        ''' </summary>
        ''' <remarks>
        ''' The constructor initalizes memory and adds all of the tokens to the token dictionary.
        ''' </remarks>
        Public Function GetTokens(expr As String) As Token(Of Tokens)()
            Return __getParser.GetTokens(expr)
        End Function
    End Module
End Namespace

