#Region "Microsoft.VisualBasic::390a8342e7e80bbeb22a4b73795c864b, src\data\LINQ\LINQ\Interpreter\Script\Builders\BinaryBuilder.vb"

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


    ' Code Statistics:

    '   Total Lines: 71
    '    Code Lines: 60 (84.51%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 11 (15.49%)
    '     File Size: 2.70 KB


    '     Module BinaryBuilder
    ' 
    '         Function: ParseBinary, ShrinkTokens
    ' 
    '         Sub: JoinBinary
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Language
Imports LINQ.Script
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language

Namespace Script.Builders

    Module BinaryBuilder

        ''' <summary>
        ''' 运算符的优先级分组：从最高优先级到最低优先级依次进行归约
        ''' </summary>
        ReadOnly orders As String()() = {
            New String() {"^"},
            New String() {"/", "*", "%"},
            New String() {"+", "-"},
            New String() {"&"},
            New String() {"=", "<>", ">", "<", ">=", "<="},
            New String() {"and"},
            New String() {"or"}
        }

        <Extension>
        Public Function ParseBinary(tokenList As Token()) As Expression
            Dim shrinks As List(Of [Variant](Of String, Expression)) = tokenList.SplitOperators.ShrinkTokens.AsList

            If shrinks = 3 Then
                Return New BinaryExpression(shrinks(0), shrinks(2), shrinks(1))
            End If

            For i As Integer = 0 To orders.Length - 1
                Call shrinks.JoinBinary(listOp:=orders(i))

                If orders(i)(Scan0) = "=" Then
                    ' not一元运算符的优先级比比较运算符以及逻辑运算符都低
                    ' 但是比and/or运算符的优先级高，所以在这里进行比较运算的归约之后
                    ' 再对not运算符进行归约
                    Call shrinks.JoinNot()
                End If
            Next

            If shrinks = 3 Then
                Return New BinaryExpression(shrinks(0), shrinks(2), shrinks(1))
            ElseIf shrinks = 1 Then
                Return shrinks(Scan0)
            Else
                Throw New SyntaxErrorException
            End If
        End Function

        <Extension>
        Private Sub JoinBinary(ByRef shrinks As List(Of [Variant](Of String, Expression)), listOp As Index(Of String))
            For i As Integer = 1 To shrinks.Count - 1
                If i >= shrinks.Count Then
                    Return
                End If

                If shrinks(i) Like GetType(String) AndAlso listOp.IndexOf(shrinks(i).TryCast(Of String)) > -1 Then
                    Dim op As String = shrinks(i).TryCast(Of String)
                    Dim left As Expression = shrinks(i - 1).TryCast(Of Expression)
                    Dim right As Expression = shrinks(i + 1).TryCast(Of Expression)

                    If left Is Nothing OrElse right Is Nothing Then
                        Throw New SyntaxErrorException($"invalid operand for the operator '{op}': {shrinks(i - 1)} {op} {shrinks(i + 1)}")
                    End If

                    Dim bin As New BinaryExpression(left, right, op)

                    shrinks.RemoveRange(i - 1, 3)
                    shrinks.Insert(i - 1, bin)
                End If
            Next
        End Sub

        ''' <summary>
        ''' 将``not &lt;expression>``组合为一元逻辑取反表达式
        ''' </summary>
        <Extension>
        Private Sub JoinNot(ByRef shrinks As List(Of [Variant](Of String, Expression)))
            Dim i As Integer = 0

            Do While i < shrinks.Count - 1
                If shrinks(i) Like GetType(String) AndAlso
                    String.Equals(shrinks(i).TryCast(Of String), "not", StringComparison.OrdinalIgnoreCase) Then

                    Dim operand As Expression = shrinks(i + 1).TryCast(Of Expression)

                    If operand Is Nothing Then
                        Throw New SyntaxErrorException("missing operand for the 'not' operator!")
                    End If

                    shrinks.RemoveRange(i, 2)
                    shrinks.Insert(i, New NotExpression(operand))
                Else
                    i += 1
                End If
            Loop
        End Sub

        <Extension>
        Private Iterator Function ShrinkTokens(blocks As IEnumerable(Of Token())) As IEnumerable(Of [Variant](Of String, Expression))
            For Each block As Token() In blocks.Where(Function(b) Not b.IsNullOrEmpty)
                If block.Length = 1 AndAlso block(Scan0).name = Tokens.Operator Then
                    Yield block(Scan0).text
                ElseIf block.Length = 3 AndAlso block(1).name = Tokens.Reference Then
                    Yield New MemberReference(ParseToken(block(0)), ParseToken(block(2)))
                Else
                    Yield block.ParseExpression
                End If
            Next
        End Function
    End Module
End Namespace
