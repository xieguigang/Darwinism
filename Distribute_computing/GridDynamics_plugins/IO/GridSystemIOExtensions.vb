#Region "Microsoft.VisualBasic::20c22b0b0d9c962c490f706d4fbc4010, Distribute_computing\GridDynamics_plugins\IO\GridSystemIOExtensions.vb"

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

    ' Module GridSystemIOExtensions
    ' 
    '     Function: LoadGridSystem, PopVectorStream
    ' 
    '     Sub: Serialize
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Module GridSystemIOExtensions

    <Extension>
    Public Sub Serialize(grid As GridSystem, save As Stream, Optional chunkSize% = 1024)
        Dim A As Stream = grid.A.PopVectorStream(chunkSize)
        Dim B As Stream = grid.C _
            .Select(Function(cor) cor.BC) _
            .AsVector _
            .PopVectorStream(chunkSize)

        Call save.Seek(Scan0, SeekOrigin.Begin)

        Using writer As New BinaryDataWriter(save)
            Call writer.Write(grid.C.Length) ' i32
            Call writer.Write(grid.AC)       ' f64
            Call writer.Write(A.Length)      ' i64
            Call writer.Write(A)             ' bytes 
            Call writer.Write(B.Length)      ' i64
            Call writer.Write(B)             ' bytes

            Call A.Dispose()
            Call B.Dispose()

            For Each factor As MemoryStream In grid.C _
                .Select(Function(cor)
                            Return cor.B.PopVectorStream(chunkSize)
                        End Function)

                Call writer.Write(factor.Length) ' i64
                Call writer.Write(factor)        ' bytes
                Call factor.Dispose()
            Next
        End Using
    End Sub

    <Extension>
    Private Function PopVectorStream(v As Vector, chunkSize%) As MemoryStream
        Dim ms As New MemoryStream
        Call v.Serialize(save:=ms, chunkSize:=chunkSize)
        Return ms
    End Function

    <Extension>
    Public Function LoadGridSystem(stream As Stream) As GridSystem
        Using reader As New BinaryDataReader(stream)
            Dim width% = reader.ReadInt32
            Dim A_const# = reader.ReadDouble
            Dim subSize&
            Dim subChunk As Byte()
            Dim grid As New GridSystem With {.AC = A_const}
            Dim cor As New List(Of Correlation)
            Dim grid_B As Vector

            ' A
            subSize = reader.ReadInt64
            subChunk = reader.ReadBytes(subSize)
            grid.A = New MemoryStream(subChunk).LoadVector

            ' B
            subSize = reader.ReadInt64
            subChunk = reader.ReadBytes(subSize)
            grid_B = New MemoryStream(subChunk).LoadVector

            ' read correlation matrix
            For i As Integer = 0 To width - 1
                subSize = reader.ReadInt64
                subChunk = reader.ReadBytes(subSize)

                cor += New Correlation With {
                    .BC = grid_B(i),
                    .B = New MemoryStream(subChunk).LoadVector
                }
            Next

            grid.C = cor

            Return grid
        End Using
    End Function
End Module

