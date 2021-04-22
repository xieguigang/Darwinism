Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Emit.Delegates

Namespace Interpreter.Query

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

End Namespace