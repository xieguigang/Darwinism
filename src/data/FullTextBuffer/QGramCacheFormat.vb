Imports System.IO
Imports System.Linq
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' Raised when a QGram cache file has an invalid format or inconsistent state.
''' </summary>
Public Class QGramCacheException : Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class

''' <summary>
''' Binary layout and validation helpers for the QGramFullText cache.
''' </summary>
Friend Module QGramCacheFormat

    Friend ReadOnly Magic As Byte() = Encoding.ASCII.GetBytes("QFTB")
    Friend Const Version As Integer = 1
    Friend Const ChecksumSize As Integer = 4
    Friend Const DefaultTokenizerId As String = "TextSplit.MakeWords"
    Friend Const HeaderMinimumSize As Long = 4L + 4L + 4L + 4L + 4L + 4L + 4L + 4L

    Friend Sub WriteHeader(writer As BinaryDataWriter,
                           q As Integer,
                           tokenizerId As String,
                           docCount As Integer,
                           wordCount As Integer,
                           stringCount As Integer,
                           gramCount As Integer)
        Call writer.Write(Magic)
        Call writer.Write(Version)
        Call writer.Write(q)
        Call WriteString(writer, tokenizerId)
        Call writer.Write(docCount)
        Call writer.Write(wordCount)
        Call writer.Write(stringCount)
        Call writer.Write(gramCount)
    End Sub

    Friend Function ReadHeader(reader As BinaryDataReader, limit As Long) As (q As Integer, tokenizerId As String, docCount As Integer, wordCount As Integer, stringCount As Integer, gramCount As Integer)
        Dim magic As Byte() = reader.ReadBytes(Magic.Length)

        If magic Is Nothing OrElse magic.Length <> Magic.Length OrElse Not magic.SequenceEqual(Magic) Then
            Throw New QGramCacheException("invalid QGram cache magic header")
        End If

        Dim version As Integer = reader.ReadInt32()

        If version <> Version Then
            Throw New QGramCacheException($"unsupported QGram cache version: {version}")
        End If

        Dim q As Integer = reader.ReadInt32()
        Dim tokenizerId As String = ReadString(reader, limit)
        Dim docCount As Integer = ReadCount(reader, limit, "document")
        Dim wordCount As Integer = ReadCount(reader, limit, "word")
        Dim stringCount As Integer = ReadCount(reader, limit, "q-gram string")
        Dim gramCount As Integer = ReadCount(reader, limit, "q-gram")

        Return (q, tokenizerId, docCount, wordCount, stringCount, gramCount)
    End Function

    Friend Sub WriteString(writer As BinaryDataWriter, value As String)
        Dim data As Byte() = If(value Is Nothing, Array.Empty(Of Byte)(), Encoding.UTF8.GetBytes(value))

        Call writer.Write(data.Length)

        If data.Length > 0 Then
            Call writer.Write(data)
        End If
    End Sub

    Friend Function ReadString(reader As BinaryDataReader, limit As Long) As String
        Dim length As Integer = reader.ReadInt32()

        If length < 0 Then
            Throw New QGramCacheException("negative string length in QGram cache")
        End If

        Call EnsureAvailable(reader.BaseStream.Position, length, limit)

        If length = 0 Then
            Return String.Empty
        End If

        Dim data As Byte() = reader.ReadBytes(length)

        If data Is Nothing OrElse data.Length <> length Then
            Throw New QGramCacheException("truncated string in QGram cache")
        End If

        Return Encoding.UTF8.GetString(data)
    End Function

    Friend Function ReadCount(reader As BinaryDataReader, limit As Long, name As String) As Integer
        Dim count As Integer = reader.ReadInt32()

        If count < 0 OrElse CLng(count) > limit Then
            Throw New QGramCacheException($"invalid {name} count in QGram cache: {count}")
        End If

        Return count
    End Function

    Friend Function ReadInt32Array(reader As BinaryDataReader, count As Integer, limit As Long) As Integer()
        If count < 0 Then
            Throw New QGramCacheException("negative integer array length in QGram cache")
        End If

        Call EnsureAvailable(reader.BaseStream.Position, CLng(count) * 4L, limit)

        Return reader.ReadInt32s(count)
    End Function

    Friend Function ReadStringArray(reader As BinaryDataReader, count As Integer, limit As Long) As String()
        If count < 0 Then
            Throw New QGramCacheException("negative string array length in QGram cache")
        End If

        Dim values(count - 1) As String

        For i As Integer = 0 To count - 1
            values(i) = ReadString(reader, limit)
        Next

        Return values
    End Function

    ''' <summary>
    ''' Ensures that the restored objects can satisfy QGramFullText.Search without missing-key errors.
    ''' </summary>
    Friend Sub ValidateState(index As QGramFullText, q As Integer)
        If index Is Nothing Then
            Throw New ArgumentNullException(NameOf(index))
        End If
        If q < 1 Then
            Throw New QGramCacheException("invalid q value in QGram cache")
        End If

        For Each doc In index.docs
            If doc.Key Is Nothing Then
                Throw New QGramCacheException("null document id in QGram cache")
            End If
        Next

        For Each word In index.word2Docs
            If word.Key Is Nothing Then
                Throw New QGramCacheException("null word in QGram cache")
            End If
            If Not index.wordIndex._counts.ContainsKey(word.Key) Then
                Throw New QGramCacheException($"word has no q-gram count in QGram cache: {word.Key}")
            End If

            For Each docId As String In word.Value
                If docId Is Nothing OrElse Not index.docs.ContainsKey(docId) Then
                    Throw New QGramCacheException($"word references an unknown document in QGram cache: {docId}")
                End If
            Next
        Next

        For Each entry In index.wordIndex._strings
            If entry.str Is Nothing Then
                Throw New QGramCacheException("null q-gram string in QGram cache")
            End If
            If Not index.wordIndex._counts.ContainsKey(entry.str) Then
                Throw New QGramCacheException($"q-gram string has no count in QGram cache: {entry.str}")
            End If
        Next

        For Each entry In index.wordIndex._index
            If entry.Key Is Nothing Then
                Throw New QGramCacheException("null q-gram key in QGram cache")
            End If

            For Each position As Integer In entry.Value
                If position < 0 OrElse position >= index.wordIndex._strings.Count Then
                    Throw New QGramCacheException($"q-gram references an invalid string position: {position}")
                End If
            Next
        Next
    End Sub

    Private Sub EnsureAvailable(position As Long, bytes As Long, limit As Long)
        If position < 0 OrElse bytes < 0 OrElse position + bytes > limit Then
            Throw New QGramCacheException("QGram cache record exceeds the data boundary")
        End If
    End Sub

End Module
