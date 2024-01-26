Imports System.IO
Imports Microsoft.VisualBasic.Data.GraphTheory.KdTree.ApproximateNearNeighbor
Imports Microsoft.VisualBasic.Data.IO
Imports Parallel

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
