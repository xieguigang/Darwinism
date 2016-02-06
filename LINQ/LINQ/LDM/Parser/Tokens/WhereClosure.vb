Imports System.CodeDom
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC
Imports Microsoft.VisualBasic.LINQ.LDM
Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace Statements.Tokens

    Public Class WhereClosure : Inherits Closure

        Public ReadOnly Property Expression As Func

        Sub New(tokens As ClosureTokens(), parent As LINQStatement)
            Call MyBase.New(TokenParser.Tokens.Where, tokens, parent)

            Dim source = New Queue(Of Token)(_source.Tokens)
            Expression = source.Parsing
        End Sub

        Public Sub Initialize()
            '   Me.TestMethod = DynamicInvoke.GetMethod(_statement.ILINQProgram, WhereConditionTestCompiler.FunctionName)
        End Sub
    End Class
End Namespace