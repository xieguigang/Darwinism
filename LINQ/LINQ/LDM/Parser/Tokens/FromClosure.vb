Imports Microsoft.VisualBasic.LINQ.Framework
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.Framework.LQueryFramework

Namespace LDM.Statements.Tokens

    ''' <summary>
    ''' The init variable.
    ''' </summary>
    Public Class FromClosure : Inherits Closure

        ''' <summary>
        ''' 变量的名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String
        ''' <summary>
        ''' 变量的类型标识符
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TypeId As String

        Sub New(tokens As ClosureTokens(), parent As LinqStatement)
            Call MyBase.New(TokenIcer.Tokens.From, tokens, parent)

            Name = Source.Tokens(Scan0).TokenValue
            TypeId = Source.Tokens(2).TokenValue
        End Sub

        Public Overridable Function ToFieldDeclaration() As CodeDom.CodeMemberField
            Dim CodeMemberField = New CodeDom.CodeMemberField(TypeId, Name)
            CodeMemberField.Attributes = CodeDom.MemberAttributes.Public

            Return CodeMemberField
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("Dim {0} As {1}", Name, TypeId)
        End Function
    End Class
End Namespace