Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.Statements.TokenIcer.Parser
Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Statements.Tokens

    Public Class WhereClosure : Inherits Closure

        Public ReadOnly Property Expression As Func(Of TokenIcer.Tokens)

        Sub New(tokens As ClosureTokens(), parent As LINQStatement)
            Call MyBase.New(TokenIcer.Tokens.Where, tokens, parent)
            Expression = _source.Tokens.Parsing(stackt)
        End Sub

        Public Sub Initialize()
            '   Me.TestMethod = DynamicInvoke.GetMethod(_statement.ILINQProgram, WhereConditionTestCompiler.FunctionName)
        End Sub
    End Class
End Namespace