Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' math helpers
''' </summary>
<Package("Math")>
<RTypeExport("entity_vector", GetType(ClusterEntity))>
Module Math

    ''' <summary>
    ''' measure the average distance for the given dataset
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("average_distance")>
    Public Function averageDistance(<RRawVectorArgument> x As Object, Optional env As Environment = Nothing) As Double
        Dim bigDataset As ClusterEntity()
        Dim dist As Double = VectorMath.AverageDistance(bigDataset)

        Return dist
    End Function

End Module
