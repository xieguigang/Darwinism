Imports batch
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Correlations
Imports Microsoft.VisualBasic.MIME.application.json
Imports Parallel
Imports Parallel.IpcStream
Imports rnd = Microsoft.VisualBasic.Math.RandomExtensions

Module clr_parallelTask_demo

    Public Function compute_function(i As vectorData, pool As vectorData()) As Double()
        Dim sum As Double() = New Double(pool.Length - 1) {}

        For offset As Integer = 0 To pool.Length - 1
            sum(offset) = i.DistanceTo(pool(offset))
        Next

        Return sum
    End Function

    Sub Main2()
        Call runParallel()
    End Sub

    Private Function generateDemoData() As vectorData()
        Dim width As Integer = 1000
        Dim pool As vectorData() = New vectorData(1000) {}
        Dim v As Double()

        For i As Integer = 0 To pool.Length - 1
            v = Enumerable.Range(0, width).Select(Function(any) rnd.NextDouble(0, any + 1)).ToArray
            pool(i) = New vectorData With {.Data = v}
        Next

        Call Console.WriteLine("memory data create job done!")

        Return pool
    End Function

    Private Sub runParallel()
        Dim pool = generateDemoData()

        Call Console.WriteLine("create parallrl task...")

        Dim args As New Argument(8) With {.ignoreError = False, .verbose = True}
        Dim memory_symbol1 As SocketRef = SocketRef.WriteBuffer(pool)
        Dim result = Host.ParallelFor(Of vectorData, Double())(args, New Func(Of vectorData, vectorData(), Double())(AddressOf compute_function), pool, memory_symbol1)

        For Each item In result
            Call Console.WriteLine(item.GetJson)
        Next
    End Sub
End Module

Public Class vectorData : Implements IVector

    Public Property Data As Double() Implements IVector.Data

End Class