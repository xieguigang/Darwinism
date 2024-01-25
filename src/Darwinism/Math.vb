Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
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
    <RApiReturn(TypeCodes.double)>
    Public Function averageDistance(<RRawVectorArgument> x As Object, Optional env As SMRUCC.Rsharp.Runtime.Environment = Nothing) As Object
        Dim pull = DataMiningDataSet.getDataModel(x, env)

        If pull Like GetType(Message) Then
            Return pull.TryCast(Of Message)
        End If

        Dim maps As New DataSetConvertor(pull.TryCast(Of EntityClusterModel()))
        Dim bigDataset As ClusterEntity() = maps _
            .GetVectors(pull.TryCast(Of EntityClusterModel())) _
            .ToArray
        Dim dist As Double = VectorMath.AverageDistance(bigDataset)

        Return dist
    End Function

End Module
