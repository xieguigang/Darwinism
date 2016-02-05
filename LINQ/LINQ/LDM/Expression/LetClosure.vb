Imports System.Text.RegularExpressions
Imports System.Text

Namespace LDM.Expression

    ''' <summary>
    ''' Object declared using a LET expression.(使用Let语句所声明的只读对象)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LetClosure : Inherits Closure

        Friend Expression As CodeDom.CodeExpression

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Sub New(token As ClosureTokens, parent As LINQStatement)
            Call MyBase.New(token, parent)
            'Dim Name As String = Regex.Match([Declare], ".+?\=").Value
            'MyBase.Name = Name.Replace("=", "").Trim
            'MyBase.TypeId = Mid([Declare], Len(Name) + 1).Trim
            'Me.Expression = Parser.ParseExpression(MyBase.TypeId)
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

        End Function

        Public Function GetAfterDeclare(tokens As ClosureTokens(), parent As LINQStatement) As LetClosure()

        End Function
    End Module
End Namespace