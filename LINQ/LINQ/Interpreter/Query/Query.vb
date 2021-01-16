Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    Public MustInherit Class QueryExpression : Inherits Expression

        ReadOnly sequence As Expression

        Friend ReadOnly symbol As SymbolDeclare
        Friend ReadOnly executeQueue As Expression()

        Public ReadOnly Property IsURISource As Boolean
            Get
                Return TypeOf sequence Is Literals AndAlso DirectCast(sequence, Literals).type = GetType(String)
            End Get
        End Property

        Sub New(symbol As SymbolDeclare, sequence As Expression, execQueue As IEnumerable(Of Expression))
            Me.symbol = symbol
            Me.sequence = sequence
            Me.executeQueue = execQueue.ToArray
        End Sub

        Public Function GetSeqValue(env As Environment) As Object
            Return sequence.Exec(env)
        End Function

        Protected Function GetDataSet(env As Environment) As DataSet
            Return DataSet.CreateDataSet(Me, env)
        End Function
    End Class
End Namespace