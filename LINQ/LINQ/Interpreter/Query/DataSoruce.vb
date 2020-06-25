Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    Public MustInherit Class DataSet

        Public MustOverride Function PopulatesData(source As Object, driver As SymbolReference, env As Environment) As IEnumerable(Of Object)

    End Class

    Public Class SymbolIterator : Inherits DataSet

        Public Overrides Function PopulatesData(source As Object, driver As SymbolReference, env As Environment) As IEnumerable(Of Object)
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class URIIteratorDriver : Inherits DataSet

        Public Overrides Function PopulatesData(source As Object, driver As SymbolReference, env As Environment) As IEnumerable(Of Object)
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace