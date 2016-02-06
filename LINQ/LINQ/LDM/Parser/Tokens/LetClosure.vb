Imports System.Text.RegularExpressions
Imports System.Text
Imports Microsoft.VisualBasic.LINQ.LDM
Imports Microsoft.VisualBasic.LINQ.TokenIcer

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

        Public Property Expression As Func

        ''' <summary>
        ''' Let var = expression
        ''' </summary>
        ''' <remarks></remarks>
        Sub New(token As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(token, parent)

            Name = _source.Tokens.First.TokenValue

            Dim sk As Integer

            If _source.Tokens(1).TokenName = TokenParser.Tokens.Equals Then
                sk = 2                ' 没有申明类型
            Else
                If _source.Tokens(1).TokenName = TokenParser.Tokens.As Then
                    Type = _source.Tokens(2).TokenValue
                    sk = 3
                Else
                    Throw New SyntaxErrorException
                End If
            End If

            Dim expr = _source.Tokens.Skip(2).ToArray
            Expression = New Queue(Of Token)(expr).Parsing
        End Sub

        Public Function ToFieldDeclaration() As CodeDom.CodeMemberField
            'Dim CodeMemberField = New CodeDom.CodeMemberField("System.Object", Name)
            'CodeMemberField.Attributes = CodeDom.MemberAttributes.Public
            'Return CodeMemberField
        End Function

        Public Overrides Function ToString() As String
            '      Return String.Format("Let {0} = {1}", Name, MyBase.TypeId)
        End Function
    End Class

    Public Module Parser

        Public Function GetPreDeclare(tokens As ClosureTokens(), parent As LINQStatement) As LetClosure()
            Dim i As Integer = 2
            Dim current As ClosureTokens = Nothing
            Dim list As New List(Of ClosureTokens)

            Do While tokens.Read(i, current).Token = TokenIcer.TokenParser.Tokens.Let
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

            Do While tokens.Read(i, current).Token <> TokenIcer.TokenParser.Tokens.Where
                If i = tokens.Length Then
                    Exit Do
                End If
            Loop
            Do While tokens.Read(i, current).Token = TokenIcer.TokenParser.Tokens.Let
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