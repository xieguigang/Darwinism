Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.Linq.LDM
Imports Microsoft.VisualBasic.Scripting.TokenIcer
Imports Microsoft.VisualBasic.Linq.LDM.Statements.TokenIcer.Parser

Namespace LDM.Statements.Tokens

    Public Class SelectClosure : Inherits Tokens.Closure
        Implements IProjectProvider

        ''' <summary>
        ''' 通过Select表达式所产生的数据投影
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Projects As String() Implements IProjectProvider.Projects

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="tokens">使用逗号分隔数据投影</param>
        ''' <param name="parent"></param>
        Sub New(tokens As ClosureTokens(), parent As LinqStatement)
            Call MyBase.New(TokenIcer.Tokens.Select, tokens, parent)
            ' Projects = Source.Tokens.Parsing(stackT).Args
        End Sub
    End Class
End Namespace