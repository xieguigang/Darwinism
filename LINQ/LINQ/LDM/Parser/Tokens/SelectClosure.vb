Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace Statements.Tokens

    Public Class SelectClosure : Inherits Tokens.Closure
        Friend Expression As CodeDom.CodeExpression
        Friend SelectMethod As System.Reflection.MethodInfo

        ''' <summary>
        ''' 通过Select表达式所产生的数据投影
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Projects As Project()

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tokens">使用逗号分隔数据投影</param>
        ''' <param name="parent"></param>
        Sub New(tokens As ClosureTokens(), parent As LINQStatement)
            Call MyBase.New(TokenIcer.TokenParser.Tokens.Select, tokens, parent)

        End Sub

        Private Shared Function __isDelimiter(x As TokenIcer.Token) As Boolean
            Return x.TokenName = TokenIcer.TokenParser.Tokens.paramDeli
        End Function

        Public Sub Initialzie()
            SelectMethod = DynamicInvoke.GetMethod(MyBase._statement.ILINQProgram, SelectConstructCompiler.SelectMethodName)
        End Sub
    End Class

    Public Class Project
        Public Property value As TokenIcer.Token()
    End Class
End Namespace