#Region "Microsoft.VisualBasic::dbc901e9e714bd16246386794001bf15, src\DataScience\DataMining\Math.vb"

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

    '   Total Lines: 46
    '    Code Lines: 29
    ' Comment Lines: 9
    '   Blank Lines: 8
    '     File Size: 1.85 KB


    ' Module VectorMath
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: AverageDistance, totalDistance
    ' 
    ' /********************************************************************************/

#End Region

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
