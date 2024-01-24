Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Correlations
Imports rnd = Microsoft.VisualBasic.Math.RandomExtensions

Module clr_parallelTask_demo

    Public Function compute_function(i As vectorData, pool As vectorData()) As Double()
        Dim sum As Double() = New Double(pool.Length - 1) {}

        For offset As Integer = 0 To pool.Length - 1
            sum(offset) = i.DistanceTo(pool(offset))
        Next

        Return sum
    End Function

    Sub Main()
        Dim width As Integer = 1000
        Dim pool As vectorData() = New vectorData(1000) {}
        Dim v As Double()

        For i As Integer = 0 To pool.Length - 1
            v = Enumerable.Range(0, width).Select(Function(any) rnd.NextDouble(0, any + 1)).ToArray
            pool(i) = New vectorData With {.Data = v}
        Next


    End Sub
End Module

Public Class vectorData : Implements IVector

    Public Property Data As Double() Implements IVector.Data

End Class