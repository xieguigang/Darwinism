Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text
Imports LINQ
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Public Module Buffer

    Public Function ReadIndex(file As Stream, <Out> ByRef offsets As Long()) As InvertedIndex
        Dim index As InvertedIndex

        If file.Length = 0 Then
            offsets = Nothing
            index = New InvertedIndex
        Else
            index = ReadIndexInternal(file, offsets)
        End If

        Call file.Close()
        Call file.Dispose()

        Return index
    End Function

    Private Function ReadIndexInternal(file As Stream, <Out> ByRef offsets As Long()) As InvertedIndex
        Dim reader As New BinaryDataReader(file, Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}
        Dim nsize As Integer = reader.ReadInt32
        Dim lastId As Integer = reader.ReadInt32
        Dim ids As New Dictionary(Of String, List(Of Integer))
        Dim token As String

        offsets = reader.ReadInt64s(reader.ReadInt32)

        For i As Integer = 0 To nsize - 1
            Dim byteSize As Integer = reader.ReadInt32
            Dim buf As Byte() = reader.ReadBytes(byteSize)
            Dim idx As Index = Index.Parse(buf)

            token = idx.name
            ids(token) = New List(Of Integer)(idx.id)
        Next

        Return New InvertedIndex(ids, lastId:=lastId)
    End Function

    Private Class Index

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

        Public Shared Function Parse(buff As Byte()) As Index
            Dim rd As New BinaryDataReader(New MemoryStream(buff), Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}
            Dim nameSize As Integer = rd.ReadInt32
            Dim nameBuf As Byte() = rd.ReadBytes(nameSize)
            Dim name As String = Encoding.UTF8.GetString(nameBuf)
            Dim size As Integer = rd.ReadInt32
            Dim id As Integer() = rd.ReadInt32s(size)

            Return New Index With {
                .name = name,
                .n = size,
                .id = id
            }
        End Function

    End Class

    <Extension>
    Public Sub WriteIndex(index As InvertedIndex, offsets As Long(), file As Stream)
        Dim bin As New BinaryDataWriter(file, Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}

        ' last id is not equals to the offset length
        ' due to the reason of empty doc may change the id un-expected?
        Call bin.Write(index.size)
        Call bin.Write(index.lastId)
        Call bin.Write(offsets.Length)
        Call bin.Write(offsets)

        For Each token As NamedCollection(Of Integer) In index.AsEnumerable
            Dim idx As New Index With {
                .name = Strings.Trim(token.name),
                .n = token.Length,
                .id = token.value
            }
            Dim bytes As Byte() = idx.GetBytes

            Call bin.Write(bytes.Length)
            Call bin.Write(bytes)
        Next

        Call bin.Flush()
        Call bin.Close()
        Call bin.Dispose()
    End Sub
End Module
