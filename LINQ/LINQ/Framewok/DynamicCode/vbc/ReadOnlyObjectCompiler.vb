Imports Microsoft.VisualBasic.Linq.LDM

Namespace Framework.DynamicCode.VBC

    Public Class ReadOnlyObjectCompiler : Inherits TokenCompiler

        Public [Object] As Statements.Tokens.FromClosure
        Public ReadOnlyObjects As Statements.Tokens.LetClosure()
        Dim Target As Statements.Tokens.LetClosure

        Sub New(Target As Statements.Tokens.LetClosure)
            Me.Target = Target
        End Sub

        Public Function Compile() As CodeDom.CodeStatement
            'Dim AssignStatement = New CodeDom.CodeAssignStatement(New CodeDom.CodeVariableReferenceExpression(Target.Name), Target.Expression)  '生成赋值语句，设置函数返回值
            'Return AssignStatement
        End Function

        Public Overrides Function ToString() As String
            Return Target.ToString
        End Function
    End Class
End Namespace