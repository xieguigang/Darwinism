Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq.Statements.TokenIcer

Namespace LDM.Parser

    Module Extensions

        Public ReadOnly Property Primitive As Tokens = Tokens.String
        Public ReadOnly Property OpenParens As Tokens = Tokens.LPair
        Public ReadOnly Property CloseParens As Tokens = Tokens.RPair

        ''' <summary>
        ''' Current tokens is a operator?
        ''' </summary>
        ''' <param name="token"></param>
        ''' <returns></returns>
        <Extension> Public Function IsOperator(token As Tokens) As Boolean
            Select Case token
                Case Statements.TokenIcer.Tokens.Slash,
                     Statements.TokenIcer.Tokens.RPair,
                     Statements.TokenIcer.Tokens.Plus,
                     Statements.TokenIcer.Tokens.Or,
                     Statements.TokenIcer.Tokens.Not,
                     Statements.TokenIcer.Tokens.Minus,
                     Statements.TokenIcer.Tokens.LPair,
                     Statements.TokenIcer.Tokens.Is,
                     Statements.TokenIcer.Tokens.Equals,
                     Statements.TokenIcer.Tokens.And,
                     Tokens.GT,
                     Tokens.GT_EQ,
                     Tokens.LT,
                     Tokens.LT_EQ,
                     Tokens.Mod,
                     Tokens.ModSlash,
                     Tokens.Shift

                    Return True
                Case Else
                    Return False
            End Select
        End Function
    End Module
End Namespace