#Region "Microsoft.VisualBasic::f1713f277fa30ae9819f2b9346516005, src\DataScience\DataMining\KNearNeighbors.vb"

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

    '   Total Lines: 64
    '    Code Lines: 45 (70.31%)
    ' Comment Lines: 11 (17.19%)
    '    - Xml Docs: 90.91%
    ' 
    '   Blank Lines: 8 (12.50%)
    '     File Size: 2.82 KB


    ' Class KNearNeighbors
    ' 
    '     Properties: KNeighbors, RowIndex, Target
    ' 
    '     Function: (+2 Overloads) FindNeighbors
    ' 
    ' /********************************************************************************/

#End Region

Imports batch
Imports Darwinism.HPC.Parallel
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.GraphTheory.KNearNeighbors
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix

<EmitStream(GetType(KNeighborFile))>
Public Class KNearNeighbors

    Public Property Target As TagVector
    Public Property KNeighbors As KNeighbors

    Public ReadOnly Property RowIndex As Integer
        Get
            Return Target.index
        End Get
    End Property

    <EmitStream(GetType(KNeighborFile), Target:=GetType(KNearNeighbors()))>
    <EmitStream(GetType(VectorFile), Target:=GetType(TagVector()))>
    Private Shared Function FindNeighbors(v As TagVector(), matrix As TagVector(), k As Integer, cutoff As Double) As KNearNeighbors()
        Dim export As KNearNeighbors() = New KNearNeighbors(v.Length) {}
        Dim llinks As (TagVector, w As Double)()
        Dim score As New Cosine() With {.cutoff = cutoff}

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
    Public Shared Function FindNeighbors(data As GeneralMatrix, cutoff As Double, Optional k As Integer = 30) As IEnumerable(Of KNeighbors)
        Dim matrix As TagVector() = data.PopulateVectors.ToArray
        Dim pool As SocketRef = SocketRef.WriteBuffer(matrix, StreamEmit.Custom(Of TagVector())(New VectorFile))
        Dim task As New Func(Of TagVector(), TagVector(), Integer, Double, KNearNeighbors())(AddressOf FindNeighbors)
        Dim env As Argument = DarwinismEnvironment.GetEnvironmentArguments
        Dim nParts = matrix.Split(CInt(matrix.Length / env.n_threads / 2))
        Dim dist = Host.ParallelFor(Of TagVector(), KNearNeighbors())(env, task, nParts, pool, k, cutoff).ToArray

        Return dist.IteratesALL.OrderBy(Function(ki) ki.RowIndex).Select(Function(v) v.KNeighbors)
    End Function
End Class
