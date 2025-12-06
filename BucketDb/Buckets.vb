Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Data.Repository

''' <summary>
''' A hashcode bucketes in-memory key-value database
''' </summary>
Public Class Buckets

    ReadOnly partitions As Integer
    ReadOnly database_dir As String
    ReadOnly hotCache As New Dictionary(Of UInteger, HotData)
    ReadOnly buckets As Dictionary(Of Integer, BinaryDataReader)

    Sub New(database_dir As String, Optional partitions As Integer = 64)
        Me.partitions = partitions
        Me.database_dir = database_dir

        For i As Integer = 1 To partitions
            buckets(i) = New BinaryDataReader($"{database_dir}/bucket{i}.db".Open(FileMode.OpenOrCreate, doClear:=False, [readOnly]:=True))
        Next
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function [Get](key As String) As Byte()
        Return [Get](Encoding.UTF8.GetBytes(key))
    End Function

    Public Function [Get](keydata As Byte()) As Byte()
        Dim hashcode As UInteger
        Dim bucket As UInteger

        Call HashKey(keydata, hashcode, bucket)

        If hotCache.ContainsKey(hashcode) Then
            Return hotCache(hashcode).data
        End If

        Dim bucketfile As BinaryDataReader = buckets(bucket)
        ' get binary file offset via hashcode
        Dim bufSize As Integer
        Dim offset As Long = GetOffset(bucketfile, hashcode, bufSize)

        bucketfile.Position = offset

        Dim data As New HotData With {.bucket = bucket, .data = bucketfile.ReadBytes(bufSize), .hashcode = hashcode, .hits = 1}
        Call hotCache.Add(hashcode, data)
        Return data.data
    End Function

    Private Shared Function GetOffset(bucket As BinaryDataReader, hashcode As UInteger, <Out> ByRef bufSize As Integer) As Long

    End Function

    Public Sub Put(keybuf As Byte(), data As Byte())
        Dim hashcode As UInteger
        Dim bucket As UInteger

        Call HashKey(keyBuf, hashcode, bucket)

        If hotCache.ContainsKey(hashcode) Then
            hotCache(hashcode).data = data
        End If

        Dim bucketfile As BinaryDataReader = buckets(bucket)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub Put(key As String, data As Byte())
        Call Put(Encoding.UTF8.GetBytes(key), data)
    End Sub

    Private Sub HashKey(ByRef key As Byte(), <Out> ByRef hashcode As UInteger, <Out> ByRef bucket As UInteger)
        hashcode = MurmurHash.MurmurHashCode3_x86_32(key, &HFFFFFFFFUI)
        bucket = (hashcode Mod CUInt(partitions)) + 1 ' bucket id start from 1
    End Sub

    Public Class HotData

        Public hashcode As UInteger
        Public bucket As UInteger
        Public hits As Integer
        Public data As Byte()

    End Class

End Class
