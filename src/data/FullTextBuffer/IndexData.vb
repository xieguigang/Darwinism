Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO

Public Class IndexData

    Public name As String
    Public n As Integer
    Public id As Integer()

    Public Function GetBytes() As Byte()
        Dim ms As New MemoryStream
        Dim w As New BinaryDataWriter(ms, Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}
        Dim nameBuf As Byte() = Encoding.UTF8.GetBytes(name)

        Call w.Write(nameBuf.Length)
        Call w.Write(nameBuf)
        Call w.Write(n)
        Call w.Write(id)
        Call w.Flush()

        Return ms.ToArray
    End Function

    Public Shared Function Parse(buff As Byte()) As IndexData
        Dim rd As New BinaryDataReader(New MemoryStream(buff), Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}
        Dim nameSize As Integer = rd.ReadInt32
        Dim nameBuf As Byte() = rd.ReadBytes(nameSize)
        Dim name As String = Encoding.UTF8.GetString(nameBuf)
        Dim size As Integer = rd.ReadInt32
        Dim id As Integer() = rd.ReadInt32s(size)

        Return New IndexData With {
            .name = name,
            .n = size,
            .id = id
        }
    End Function

End Class