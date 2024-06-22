#Region "Microsoft.VisualBasic::fab52badebe4aa3ae9c85fa859268503, src\DataScience\DataMining\Files\VectorFile.vb"

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

    '   Total Lines: 63
    '    Code Lines: 46 (73.02%)
    ' Comment Lines: 3 (4.76%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 14 (22.22%)
    '     File Size: 2.23 KB


    ' Class VectorFile
    ' 
    '     Function: BufferInMemory, ReadBuffer, ReadSingle, (+2 Overloads) WriteBuffer
    ' 
    '     Sub: WriteSingle
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Darwinism.HPC.Parallel
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' read/write <see cref="TagVector()"/> vector data
''' </summary>
Public Class VectorFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Dim vecs As TagVector() = obj
        Dim bin As New BinaryDataWriter(file) With {.ByteOrder = ByteOrder.LittleEndian}

        Call bin.Write(vecs.Length)

        For Each vi As TagVector In vecs
            Call WriteSingle(bin, vi)
        Next

        Call bin.Flush()

        Return True
    End Function

    Public Shared Sub WriteSingle(bin As BinaryDataWriter, v As TagVector)
        Call bin.Write(v.index)
        Call bin.Write(If(v.tag, ""), BinaryStringFormat.UInt32LengthPrefix)
        Call bin.Write(v.vector.Length)
        Call bin.Write(v.vector)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Return Me.DefaultWriteMemory(obj)
    End Function

    Public Shared Function ReadSingle(bin As BinaryDataReader) As TagVector
        Dim index As Integer = bin.ReadInt32
        Dim tag As String = bin.ReadString(BinaryStringFormat.UInt32LengthPrefix)
        Dim width As Integer = bin.ReadInt32
        Dim vector As Double() = bin.ReadDoubles(width)

        Return New TagVector(index, tag, vector)
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Dim bin As New BinaryDataReader(file) With {.ByteOrder = ByteOrder.LittleEndian}
        Dim n As Integer = bin.ReadInt32
        Dim load As TagVector() = New TagVector(n - 1) {}

        For i As Integer = 0 To n - 1
            load(i) = ReadSingle(bin)
        Next

        Return load
    End Function
End Class
