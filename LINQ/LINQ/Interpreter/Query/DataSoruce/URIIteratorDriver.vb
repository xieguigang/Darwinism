Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    Public Class URIIteratorDriver : Inherits DataSet

        ReadOnly uri As String

        Sub New(query As QueryExpression, uri As String, env As Environment)
            MyBase.New(query.symbol, env)
            Me.uri = uri
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            Dim driver As DataSourceDriver = env.GlobalEnvir.GetDriverByCode(symbolDeclare.type)

            For Each item As Object In driver.ReadFromUri(uri)
                Yield item
            Next
        End Function
    End Class
End Namespace