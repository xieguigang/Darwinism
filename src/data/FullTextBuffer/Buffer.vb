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
            Dim idx As IndexData = IndexData.Parse(buf)

            token = idx.name
            ids(token) = New List(Of Integer)(idx.id)
        Next

        Return New InvertedIndex(ids, lastId:=lastId)
    End Function

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
            Dim idx As New IndexData With {
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
