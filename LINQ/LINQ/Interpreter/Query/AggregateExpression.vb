Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    ''' <summary>
    ''' aggregate ... into ...
    ''' </summary>
    Public Class AggregateExpression : Inherits QueryExpression

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression))
            Call MyBase.New(symbol, sequence, exec)
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace