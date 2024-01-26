Imports System.IO
Imports Microsoft.VisualBasic.DataMining.ComponentModel.Serialization
Imports Microsoft.VisualBasic.DataMining.KMeans
Imports Parallel

''' <summary>
''' file stream handler for <see cref="ClusterEntity()"/>.
''' </summary>
Public Class ClusterVectorFile : Implements IEmitStream

    Public Function BufferInMemory(obj As Object) As Boolean Implements IEmitStream.BufferInMemory
        Return True
    End Function

    Public Function WriteBuffer(obj As Object, file As Stream) As Boolean Implements IEmitStream.WriteBuffer
        Call EntityVectorFile.SaveVector(DirectCast(obj, ClusterEntity()), file)
        Call file.Flush()
        Return True
    End Function

    Public Function WriteBuffer(obj As Object) As Stream Implements IEmitStream.WriteBuffer
        Dim ms As New MemoryStream
        Call EntityVectorFile.SaveVector(DirectCast(obj, ClusterEntity()), ms)
        Call ms.Flush()
        Call ms.Seek(0, SeekOrigin.Begin)
        Return ms
    End Function

    Public Function ReadBuffer(file As Stream) As Object Implements IEmitStream.ReadBuffer
        Return EntityVectorFile.LoadVectors(file).ToArray
    End Function
End Class
