Imports Microsoft.VisualBasic.Data.Repository

''' <summary>
''' A hashcode bucketes in-memory key-value database
''' </summary>
Public Class Buckets

    ReadOnly partitions As Integer
    ReadOnly database_dir As String
    ReadOnly hotCache As New Dictionary(Of UInteger)


    Sub New(database_dir As String, Optional partitions As Integer = 64)
        Me.partitions = partitions
        Me.database_dir = database_dir
    End Sub

    Public Function [Get](key As String) As Byte()

    End Function

    Public Sub Put(key As String, data As Byte())

    End Sub

    Private Sub HashKey(ByRef key As Byte(), ByRef hashcode As UInteger, ByRef bucket As UInteger)
        hashcode = MurmurHash.MurmurHashCode3_x86_32(key, &HFFFFFFFFUI)
        bucket = (hashcode Mod CUInt(partitions)) + 1
    End Sub

End Class
