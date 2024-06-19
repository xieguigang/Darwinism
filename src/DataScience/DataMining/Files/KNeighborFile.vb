#Region "Microsoft.VisualBasic::fc4d84d2353e33ce2a58c435e65ad139, src\DataScience\DataMining\Files\KNeighborFile.vb"

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

'   Total Lines: 67
'    Code Lines: 50
' Comment Lines: 3
'   Blank Lines: 14
'     File Size: 2.35 KB


' Class KNeighborFile
' 
'     Function: BufferInMemory, ReadBuffer, ReadSingle, (+2 Overloads) WriteBuffer
' 
'     Sub: WriteSingle
' 
' /********************************************************************************/

#End Region

Imports System.IO
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' file handler for array of <see cref="KNearNeighbors"/>
''' </summary>
Public Class KNeighborFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim knn As KNearNeighbors() = obj
        Dim bin As New BinaryDataWriter(file) With {.ByteOrder = ByteOrder.LittleEndian}

        Call bin.Write(knn.Length)

        For Each ki As KNearNeighbors In knn
            Call WriteSingle(ki, bin)
        Next

        Call bin.Flush()

        Return True
    End Function

    Private Shared Sub WriteSingle(knn As KNearNeighbors, bin As BinaryDataWriter)
        Call VectorFile.WriteSingle(bin, knn.Target)
        Call bin.Write(knn.KNeighbors.size)
        Call bin.Write(knn.KNeighbors.indices)
        Call bin.Write(knn.KNeighbors.weights)
    End Sub

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim ms As New MemoryStream
        Call WriteBuffer(obj, ms)
        Call ms.Seek(0, SeekOrigin.Begin)
        Return ms
    End Function

    Private Shared Function ReadSingle(bin As BinaryDataReader) As KNearNeighbors
        Dim vector As TagVector = VectorFile.ReadSingle(bin)
        Dim size As Integer = bin.ReadInt32
        Dim indices As Integer() = bin.ReadInt32s(size)
        Dim weights As Double() = bin.ReadDoubles(size)

        Return New KNearNeighbors With {
            .Target = vector,
            .KNeighbors = New KNeighbors(size, indices, weights)
        }
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim bin As New BinaryDataReader(file) With {.ByteOrder = ByteOrder.LittleEndian}
        Dim n As Integer = bin.ReadInt32
        Dim load As KNearNeighbors() = New KNearNeighbors(n - 1) {}

        For i As Integer = 0 To n - 1
            load(i) = ReadSingle(bin)
        Next

        Return load
    End Function
End Class
