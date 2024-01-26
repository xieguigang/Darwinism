Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.IO
Imports Parallel

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
