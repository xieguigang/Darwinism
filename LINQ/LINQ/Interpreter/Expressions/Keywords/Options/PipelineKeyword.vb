Imports Microsoft.VisualBasic.My.JavaScript
Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class PipelineKeyword : Inherits KeywordExpression

        Public MustOverride Overloads Function Exec(result As IEnumerable(Of JavaScriptObject), context As ExecutableContext) As IEnumerable(Of JavaScriptObject)

        ''' <summary>
        ''' 将字符串常量表示转换为变量引用
        ''' </summary>
        ''' <param name="expr"></param>
        ''' <returns></returns>
        Protected Shared Function FixLiteral(expr As Expression) As Expression
            If TypeOf expr Is BinaryExpression Then
                Dim bin As BinaryExpression = DirectCast(expr, BinaryExpression)

                bin.left = FixLiteral(bin.left)
                bin.right = FixLiteral(bin.right)
            ElseIf TypeOf expr Is Literals Then
                expr = New SymbolReference(DirectCast(expr, Literals).value)
            ElseIf TypeOf expr Is FuncEval Then
                DirectCast(expr, FuncEval).parameters = DirectCast(expr, FuncEval).parameters _
                    .Select(AddressOf FixLiteral) _
                    .ToArray
            End If

            Return expr
        End Function

    End Class
End Namespace