#Region "Microsoft.VisualBasic::cd557dbf8e19b518beacd8dd3e9754ca, G:/GCModeller/src/runtime/Darwinism/src/Darwinism//Math.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 87
    '    Code Lines: 57
    ' Comment Lines: 20
    '   Blank Lines: 10
    '     File Size: 3.34 KB


    ' Module Math
    ' 
    '     Function: averageDistance, FindNeighbors
    ' 
    ' /********************************************************************************/

#End Region

Imports Darwinism.DataScience.DataMining
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' darwinism IPC parallel math
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
    Public Function averageDistance(<RRawVectorArgument> x As Object, Optional env As Environment = Nothing) As Object
        Dim pull = DataMiningDataSet.getDataModel(x, check_class:=False, env)

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="k"></param>
    ''' <param name="cutoff"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' this function only supports the cosine similarity score function.
    ''' </remarks>
    <ExportAPI("knn")>
    <RApiReturn(GetType(KNeighbors))>
    Public Function FindNeighbors(<RRawVectorArgument> x As Object,
                                  Optional k As Integer = 16,
                                  Optional cutoff As Double = 0.8,
                                  Optional env As Environment = Nothing) As Object

        Dim m As GeneralMatrix

        If x Is Nothing Then
            Call env.AddMessage("the given raw data matrix is nothing!")
            Return Nothing
        ElseIf TypeOf x Is dataframe Then
            m = New NumericMatrix(DirectCast(x, dataframe) _
               .forEachRow _
               .Select(Function(r)
                           Return CLRVector.asNumeric(r.value)
                       End Function))
        ElseIf x.GetType.ImplementInterface(Of GeneralMatrix) Then
            m = x
        ElseIf x.GetType.ImplementInterface(Of INumericMatrix) Then
            m = New NumericMatrix(DirectCast(x, INumericMatrix))
        ElseIf TypeOf x Is ClusterEntity() Then
            m = New NumericMatrix(DirectCast(x, ClusterEntity()).Select(Function(v) v.entityVector))
        Else
            Return Message.InCompatibleType(GetType(GeneralMatrix), x.GetType, env)
        End If

        Return KNearNeighbors.FindNeighbors(m, cutoff, k).ToArray
    End Function

End Module

