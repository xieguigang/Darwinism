Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace Statements.Tokens

    Public Class WhereCondition : Inherits Token

        Friend Expression As CodeExpression
        Friend TestMethod As MethodInfo

        Sub New(Statement As LINQStatement)
            Me.Statement = Statement

            Dim Parser As LINQ.Parser.Parser = New LINQ.Parser.Parser
            Dim str = GetStatement(Statement._original, New String() {"where", "let"}, False)
            If String.IsNullOrEmpty(str) Then
                str = GetStatement(Statement._original, New String() {"where", "select"}, False)
            End If

            Expression = Parser.ParseExpression(str)
        End Sub

        Public Sub Initialize()
            Me.TestMethod = DynamicInvoke.GetMethod(Statement.ILINQProgram, WhereConditionTestCompiler.FunctionName)
        End Sub
    End Class
End Namespace