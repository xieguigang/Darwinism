Imports System.Runtime.CompilerServices
Imports batch
Imports Microsoft.VisualBasic.DataMining.Clustering
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Parallel.IpcStream

Public Module VectorMath

    ''' <summary>
    ''' a global parameters for the parallel computing
    ''' </summary>
    Dim par As Argument

    Public Sub SetEnvironment(par As Argument)
        VectorMath.par = par
    End Sub

    Public Sub SetThreads(n_threads As Integer)
        If par Is Nothing Then
            par = New Argument(n_threads)
        Else
            par.n_threads = n_threads
        End If
    End Sub

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
        Dim pool As SocketRef = SocketRef.WriteBuffer(alldata)
        Dim task As New Func(Of ClusterEntity, ClusterEntity(), Double)(AddressOf CanopyBuilder.TotalDistance)
        Dim dist = Host.ParallelFor(Of ClusterEntity, Double)(par, task, alldata, pool).ToArray

        Return CanopyBuilder.AverageDistance(alldata.Length, parts:=dist)
    End Function
End Module
