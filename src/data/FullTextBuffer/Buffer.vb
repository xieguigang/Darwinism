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
        Dim idsize As Integer

        offsets = reader.ReadInt64s(reader.ReadInt32)

        For i As Integer = 0 To nsize - 1
            token = reader.ReadString(BinaryStringFormat.ByteLengthPrefix)
            idsize = reader.ReadInt32
            ids.Add(token, New List(Of Integer)(reader.ReadInt32s(idsize)))
        Next

        Return New InvertedIndex(ids, lastId:=lastId)
    End Function

    <Extension>
    Public Sub WriteIndex(index As InvertedIndex, offsets As Long(), file As Stream)
        Dim bin As New BinaryDataWriter(file, Encoding.UTF8) With {.ByteOrder = ByteOrder.BigEndian}

        ' last id is not equals to the offset length
        ' due to the reason of empty doc may change the id un-expected?
        bin.Write(index.size)
        bin.Write(index.lastId)
        bin.Write(offsets.Length)
        bin.Write(offsets)

        For Each token As NamedCollection(Of Integer) In index.AsEnumerable
            Call bin.Write(token.name, BinaryStringFormat.ByteLengthPrefix)
            Call bin.Write(token.Length)
            Call bin.Write(token.value)
        Next

        Call bin.Flush()
        Call bin.Close()
        Call bin.Dispose()
    End Sub
End Module
