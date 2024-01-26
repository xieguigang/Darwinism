Imports batch
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.GraphTheory.KNearNeighbors
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports Parallel.IpcStream

Public Class KNearNeighbors

    Public Property Target As TagVector
    Public Property KNeighbors As KNeighbors

    Public ReadOnly Property RowIndex As Integer
        Get
            Return Target.index
        End Get
    End Property

    Private Shared Function FindNeighbors(v As TagVector(), matrix As TagVector(), k As Integer, cutoff As Double, score As ScoreMetric) As KNearNeighbors()
        Dim export As KNearNeighbors() = New KNearNeighbors(v.Length) {}
        Dim llinks As (TagVector, w As Double)()

        score.cutoff = cutoff

        For i As Integer = 0 To v.Length - 1
            llinks = KNN.FindNeighbors(v(i), matrix, k, score)
            export(i) = New KNearNeighbors With {
                .KNeighbors = New KNeighbors With {
                    .size = llinks.Length,
                    .indices = llinks.Select(Function(ki) ki.Item1.index).ToArray,
                    .weights = llinks.Select(Function(ki) ki.w).ToArray
                },
                .Target = v(i)
            }
        Next

        Return export
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="score"></param>
    ''' <param name="cutoff"></param>
    ''' <param name="k"></param>
    ''' <returns>
    ''' the result collection keeps the original element order with 
    ''' the <paramref name="data"/> matrix rows.
    ''' </returns>
    Public Shared Function FindNeighbors(data As GeneralMatrix, score As ScoreMetric, cutoff As Double, Optional k As Integer = 30) As IEnumerable(Of KNeighbors)
        Dim matrix As TagVector() = data.PopulateVectors.ToArray
        Dim pool As SocketRef = SocketRef.WriteBuffer(matrix)
        Dim task As New Func(Of TagVector(), TagVector(), Integer, Double, ScoreMetric, KNearNeighbors())(AddressOf FindNeighbors)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim nParts = matrix.Split(CInt(matrix.Length / env.n_threads / 2))
        Dim score_ref As SocketRef = SocketRef.WriteBuffer(score)
        Dim dist = Host.ParallelFor(Of TagVector(), KNearNeighbors())(env, task, nParts, pool, k, cutoff, score_ref).ToArray

        Return dist.IteratesALL.OrderBy(Function(ki) ki.RowIndex).Select(Function(v) v.KNeighbors)
    End Function
End Class