Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Language
Imports LINQ.Script
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language

Module BinaryBuilder

    ReadOnly orders As String()() = {
        New String() {"^"},
        New String() {"/", "*", "%"},
        New String() {"+", "-"},
        New String() {"and", "or"}
    }

    <Extension>
    Public Function ParseBinary(tokenList As Token()) As Expression
        Dim shrinks As List(Of [Variant](Of String, Expression)) = tokenList.SplitOperators.ShrinkTokens.AsList

        If shrinks = 3 Then
            Return New BinaryExpression(shrinks(0), shrinks(2), shrinks(1))
        End If

        For Each level As String() In orders
            Call shrinks.JoinBinary(listOp:=level)
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
                Dim bin As New BinaryExpression(shrinks(i - 1), shrinks(i + 1), shrinks(i))

                shrinks.RemoveRange(i - 1, 3)
                shrinks.Insert(i - 1, bin)
            End If
        Next
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
