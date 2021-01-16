Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace Interpreter.Query

    Public MustInherit Class DataSet

        Protected ReadOnly env As Environment
        Protected ReadOnly symbolDeclare As SymbolDeclare

        Sub New(symbolDeclare As SymbolDeclare, env As Environment)
            Me.env = env
            Me.symbolDeclare = symbolDeclare
        End Sub

        Public MustOverride Function PopulatesData() As IEnumerable(Of Object)

        Public Shared Function CreateDataSet(query As QueryExpression, env As Environment) As DataSet
            If query.IsURISource Then
                Return New URIIteratorDriver(query, query.GetSeqValue(Nothing), env)
            Else
                Dim seqVal As Object = query.GetSeqValue(env)

                If TypeOf seqVal Is String Then
                    Return New URIIteratorDriver(query, seqVal, env)
                Else
                    Return New SymbolIterator(query, seqVal, env)
                End If
            End If
        End Function

    End Class

    Public Class SymbolIterator : Inherits DataSet

        ReadOnly objVal As Object

        Sub New(query As QueryExpression, obj As Object, env As Environment)
            MyBase.New(query.symbol, env)
            Me.objVal = obj
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            If objVal Is Nothing Then
                Throw New NullReferenceException
            End If

            If objVal.GetType.IsArray Then
                With DirectCast(objVal, Array)
                    For i As Integer = 0 To .Length - 1
                        Yield .GetValue(i)
                    Next
                End With
            ElseIf objVal.GetType.ImplementInterface(GetType(IEnumerable)) Then
                For Each item As Object In DirectCast(objVal, IEnumerable)
                    Yield item
                Next
            Else
                Throw New InvalidCastException
            End If
        End Function
    End Class

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