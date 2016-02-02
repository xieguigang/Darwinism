Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace Statements.Tokens

    Public Class SelectConstruct : Inherits Tokens.Token
        Friend Expression As CodeDom.CodeExpression
        Friend SelectMethod As System.Reflection.MethodInfo

        Sub New(Statement As LINQStatement)
            MyBase.Statement = Statement
            Call Me.TryParse()
        End Sub

        Private Sub TryParse()
            Dim str = Regex.Match(Statement._original, " select .+", RegexOptions.IgnoreCase).Value
            For Each key In Options.OptionList
                str = Regex.Split(str, String.Format(" {0}\s?", key), RegexOptions.IgnoreCase).First
            Next
            str = Mid(str, 9)
            MyBase._original = str
            If String.IsNullOrEmpty(str) Then
                Throw New SyntaxErrorException("Not SELECT statement token, can not procedure the query operation!")
            End If
            Me.Expression = New LINQ.Parser.Parser().ParseExpression(str)
        End Sub

        Public Sub Initialzie()
            SelectMethod = DynamicInvoke.GetMethod(MyBase.Statement.ILINQProgram, SelectConstructCompiler.SelectMethodName)
        End Sub
    End Class
End Namespace