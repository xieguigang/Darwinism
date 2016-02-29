Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.VisualBasic.LINQ.Statements.TokenIcer
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Statements.Tokens

    ''' <summary>
    ''' Object declared using a LET expression.(使用Let语句所声明的只读对象)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LetClosure : Inherits Closure

        ''' <summary>
        ''' Variable name
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String

        ''' <summary>
        ''' Optional
        ''' </summary>
        ''' <returns></returns>
        Public Property Type As String

        Public Property Expression As Func(Of TokenIcer.Tokens)

        ''' <summary>
        ''' Let var = expression
        ''' </summary>
        ''' <remarks></remarks>
        Sub New(token As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(token, parent)

            Name = Source.Tokens.First.TokenValue

            Dim sk As Integer

            If Source.Tokens(1).TokenName = TokenIcer.Tokens.Equals Then
                sk = 2                ' 没有申明类型
            Else
                If Source.Tokens(1).TokenName = TokenIcer.Tokens.As Then
                    Type = Source.Tokens(2).TokenValue
                    sk = 4
                Else
                    Throw New SyntaxErrorException
                End If
            End If

            Dim expr As IEnumerable(Of Token(Of TokenIcer.Tokens)) = Source.Tokens.Skip(sk)
            Expression = expr.Parsing(stackt)
        End Sub

        Public Function ToFieldDeclaration() As CodeDom.CodeMemberField
            'Dim CodeMemberField = New CodeDom.CodeMemberField("System.Object", Name)
            'CodeMemberField.Attributes = CodeDom.MemberAttributes.Public
            'Return CodeMemberField
        End Function

        Public Overrides Function ToString() As String
            Return Source.ToString
        End Function
    End Class

    Public Module Parser

        Public Function GetPreDeclare(tokens As ClosureTokens(), parent As LINQStatement) As LetClosure()
            Dim i As Integer = 2
            Dim current As ClosureTokens = Nothing
            Dim list As New List(Of ClosureTokens)

            Do While tokens.Read(i, current).Token = TokenIcer.Tokens.Let
                Call list.Add(current)
                If i = tokens.Length Then
                    Exit Do
                End If
            Loop

            Dim value = list.ToArray(Function(x) New LetClosure(x, parent))
            Return value
        End Function

        Public Function GetAfterDeclare(tokens As ClosureTokens(), parent As LINQStatement) As LetClosure()
            Dim i As Integer = 2
            Dim current As ClosureTokens = Nothing
            Dim list As New List(Of ClosureTokens)

            Do While tokens.Read(i, current).Token <> TokenIcer.Tokens.Where
                If i = tokens.Length Then
                    Exit Do
                End If
            Loop
            Do While tokens.Read(i, current).Token = TokenIcer.Tokens.Let
                Call list.Add(current)
                If i = tokens.Length Then
                    Exit Do
                End If
            Loop

            Dim value = list.ToArray(Function(x) New LetClosure(x, parent))
            Return value
        End Function
    End Module
End Namespace