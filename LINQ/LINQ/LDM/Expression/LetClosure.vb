Imports System.Text.RegularExpressions
Imports System.Text
Imports System.CodeDom

Namespace LDM.Expression

    ''' <summary>
    ''' Object declared using a LET expression.(使用Let语句所声明的只读对象)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LetClosure : Inherits Closure

        Friend Expression As CodeDom.CodeExpression

        Sub New(source As Statements.Tokens.LetClosure)
            Call MyBase.New(source)


        End Sub

        Public Function ToFieldDeclaration() As CodeDom.CodeMemberField
            'Dim CodeMemberField = New CodeDom.CodeMemberField("System.Object", Name)
            'CodeMemberField.Attributes = CodeDom.MemberAttributes.Public
            'Return CodeMemberField
        End Function

        Public Overrides Function ToString() As String
            '      Return String.Format("Let {0} = {1}", Name, MyBase.TypeId)
        End Function

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace