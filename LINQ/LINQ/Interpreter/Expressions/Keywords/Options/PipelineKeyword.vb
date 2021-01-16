Imports Microsoft.VisualBasic.My.JavaScript
Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class PipelineKeyword : Inherits KeywordExpression

        Public MustOverride Overloads Function Exec(result As IEnumerable(Of JavaScriptObject), env As Environment) As IEnumerable(Of JavaScriptObject)

    End Class
End Namespace