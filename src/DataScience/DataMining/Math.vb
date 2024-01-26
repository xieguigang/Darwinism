Imports System.Runtime.CompilerServices
Imports batch
Imports Microsoft.VisualBasic.DataMining.Clustering
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Linq
Imports Parallel
Imports Parallel.IpcStream

Public Module VectorMath

    Sub New()

    End Sub

    <EmitStream(GetType(ClusterVectorFile), Target:=GetType(ClusterEntity()))>
    Private Function totalDistance(parts As ClusterEntity(), alldata As ClusterEntity()) As Double()
        Dim sum_total As Double() = New Double(parts.Length - 1) {}

        For i As Integer = 0 To parts.Length - 1
            sum_total(i) = CanopyBuilder.TotalDistance(parts(i), alldata)
        Next

        Return sum_total
    End Function

    ''' <summary>
    ''' this function calculate the average distance between each points of 
    ''' the given data vectors, result could be used for:
    ''' 
    ''' 1. calculate the T2 threshold for <see cref="CanopyBuilder"/>
    ''' 2. 
    ''' </summary>
    ''' <param name="points"></param>
    ''' <returns></returns>
    <Extension>
    Public Function AverageDistance(points As IEnumerable(Of ClusterEntity)) As Double
        Dim alldata As ClusterEntity() = points.ToArray
        Dim pool As SocketRef = SocketRef.WriteBuffer(alldata, StreamEmit.Custom(Of ClusterEntity())(New ClusterVectorFile))
        Dim task As New Func(Of ClusterEntity(), ClusterEntity(), Double())(AddressOf totalDistance)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim nParts = alldata.Split(CInt(alldata.Length / env.n_threads / 2))
        Dim dist = Host.ParallelFor(Of ClusterEntity(), Double())(env, task, nParts, pool).ToArray

        Return CanopyBuilder.AverageDistance(alldata.Length, parts:=dist.IteratesALL.ToArray)
    End Function
End Module
