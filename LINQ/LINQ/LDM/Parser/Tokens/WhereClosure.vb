Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.Linq.LDM.Statements.TokenIcer.Parser
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace LDM.Statements.Tokens

    Public Class WhereClosure : Inherits Closure
        Implements ICodeProvider

        Public ReadOnly Property Code As String Implements ICodeProvider.Code

        Sub New(tokens As ClosureTokens(), parent As LinqStatement)
            Call MyBase.New(TokenIcer.Tokens.Where, tokens, parent)
            Code = Source.Tokens.ToArray(Function(x) x.TokenValue).JoinBy(" ")
        End Sub

        Public Shared Function CreateLinqWhere(Expr As String, type As Type) As ITest

        End Function

        Public Delegate Function ITest(x As Object) As Boolean
    End Class
End Namespace