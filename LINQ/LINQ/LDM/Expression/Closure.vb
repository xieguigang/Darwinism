Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace LDM.Expression

    Public MustInherit Class Closure

        Protected _expression As CodeDom.CodeExpression
        Protected _source As Statements.Tokens.Closure

        Sub New(source As Statements.Tokens.Closure)
            _source = source
            _expression = __parsing()
        End Sub

        Protected MustOverride Function __parsing() As CodeDom.CodeExpression

        Public Overrides Function ToString() As String
            Return _source.ToString
        End Function
    End Class
End Namespace