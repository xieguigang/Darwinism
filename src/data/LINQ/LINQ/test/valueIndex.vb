Imports LINQ
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Public Module valueIndex

    Sub Main()
        Dim pool = Enumerable.Range(0, 10000000).Select(Function(a) randf.NextDouble(-99, 10000)).ToArray
        Dim index As RangeIndex(Of Double) = DoubleIndex().IndexData(pool)

        Dim search1 = index.Search(100).ToArray
        Dim search2 = index.Search(90, 300).ToArray
        Dim search3 = index.Search(-50).ToArray

        Pause()
    End Sub
End Module
