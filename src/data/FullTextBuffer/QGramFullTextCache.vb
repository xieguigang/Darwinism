Imports System.Buffers.Binary
Imports System.IO
Imports System.Linq
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Data.Trinity.NLP

''' <summary>
''' Saves and restores <see cref="QGramFullText"/> index state to/from a compact binary cache file.
''' </summary>
Public Module QGramFullTextCache

    ''' <summary>
    ''' Writes the complete index state to a cache file.
    ''' </summary>
    Public Function Save(index As QGramFullText, file As String, Optional tokenizerId As String = Nothing) As Boolean
        If String.IsNullOrWhiteSpace(file) Then
            Throw New ArgumentException("cache file path cannot be empty", NameOf(file))
        End If

        Dim fullPath As String = System.IO.Path.GetFullPath(file)
        Dim parentDirectory As String = System.IO.Path.GetDirectoryName(fullPath)

        If Not String.IsNullOrEmpty(parentDirectory) Then
            Call System.IO.Directory.CreateDirectory(parentDirectory)
        End If

        Dim temporary As String = fullPath & ".tmp"

        Try
            Using stream As New FileStream(temporary, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
                Call Save(index, DirectCast(stream, Stream), tokenizerId)
            End Using

            Call System.IO.File.Move(temporary, fullPath, overwrite:=True)

            Return True
        Catch
            If System.IO.File.Exists(temporary) Then
                Try
                    Call System.IO.File.Delete(temporary)
                Catch
                    ' keep the original exception from the write path
                End Try
            End If

            Throw
        End Try
    End Function

    ''' <summary>
    ''' Writes the complete index state starting at the current position of a seekable stream.
    ''' </summary>
    Public Function Save(index As QGramFullText, stream As Stream, Optional tokenizerId As String = Nothing) As Boolean
        If index Is Nothing Then
            Throw New ArgumentNullException(NameOf(index))
        End If
        If stream Is Nothing Then
            Throw New ArgumentNullException(NameOf(stream))
        End If
        If Not stream.CanSeek OrElse Not stream.CanRead Then
            Throw New ArgumentException("cache stream must be seekable and readable", NameOf(stream))
        End If

        Dim start As Long = stream.Position
        Dim cacheTokenizerId As String = If(String.IsNullOrEmpty(tokenizerId), QGramCacheFormat.DefaultTokenizerId, tokenizerId)

        Call WriteCacheData(index, stream, cacheTokenizerId)

        Dim dataEnd As Long = stream.Position
        stream.Position = start

        Dim checksum As UInteger = QGramCacheChecksum.Compute(stream, dataEnd - start)

        stream.Position = dataEnd
        Call WriteFooter(stream, checksum)

        Return True
    End Function

    ''' <summary>
    ''' Loads a cache file and rebuilds a directly searchable index.
    ''' </summary>
    Public Function Load(file As String, Optional tokenlicer As Func(Of String, IEnumerable(Of String)) = Nothing) As QGramFullText
        If String.IsNullOrWhiteSpace(file) Then
            Throw New ArgumentException("cache file path cannot be empty", NameOf(file))
        End If

        Using stream As New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)
            Return Load(stream, tokenlicer)
        End Using
    End Function

    ''' <summary>
    ''' Loads a cache from the current position of a seekable stream.
    ''' </summary>
    Public Function Load(stream As Stream, Optional tokenlicer As Func(Of String, IEnumerable(Of String)) = Nothing) As QGramFullText
        If stream Is Nothing Then
            Throw New ArgumentNullException(NameOf(stream))
        End If
        If Not stream.CanSeek Then
            Throw New ArgumentException("cache stream must be seekable", NameOf(stream))
        End If

        Dim start As Long = stream.Position
        Dim available As Long = stream.Length - start

        If available < QGramCacheFormat.HeaderMinimumSize + QGramCacheFormat.ChecksumSize Then
            Throw New QGramCacheException("QGram cache file is too short")
        End If

        Dim dataLength As Long = available - QGramCacheFormat.ChecksumSize
        Dim limit As Long = start + dataLength

        stream.Position = start
        Dim expected As UInteger = QGramCacheChecksum.Compute(stream, dataLength)

        Dim footer(QGramCacheFormat.ChecksumSize - 1) As Byte

        stream.Position = limit

        If stream.Read(footer, 0, footer.Length) <> footer.Length Then
            Throw New QGramCacheException("truncated QGram cache checksum")
        End If

        Dim actual As UInteger = BinaryPrimitives.ReadUInt32BigEndian(footer)

        If expected <> actual Then
            Throw New QGramCacheException("QGram cache checksum mismatch")
        End If

        stream.Position = start

        Dim reader As New BinaryDataReader(stream, Encoding.UTF8, leaveOpen:=True) With {
            .ByteOrder = ByteOrder.BigEndian
        }
        Dim header = QGramCacheFormat.ReadHeader(reader, limit)

        If header.q < 1 Then
            Throw New QGramCacheException("invalid q value in QGram cache")
        End If
        If Not String.Equals(header.tokenizerId, QGramCacheFormat.DefaultTokenizerId, StringComparison.Ordinal) AndAlso tokenlicer Is Nothing Then
            Throw New QGramCacheException($"the cache requires tokenizer '{header.tokenizerId}', but no tokenizer was supplied")
        End If

        Dim effectiveTokenizer As Func(Of String, IEnumerable(Of String)) = If(tokenlicer, AddressOf TextSplit.MakeWords)

        Dim docs As New Dictionary(Of String, String)

        For i As Integer = 1 To header.docCount
            Dim docId As String = QGramCacheFormat.ReadString(reader, limit)
            Dim text As String = QGramCacheFormat.ReadString(reader, limit)

            If Not docs.TryAdd(docId, text) Then
                Throw New QGramCacheException($"duplicated document id in QGram cache: {docId}")
            End If
        Next

        Dim word2Docs As New Dictionary(Of String, HashSet(Of String))

        For i As Integer = 1 To header.wordCount
            Dim word As String = QGramCacheFormat.ReadString(reader, limit)
            Dim count As Integer = QGramCacheFormat.ReadCount(reader, limit, "document reference")
            Dim docIds As String() = QGramCacheFormat.ReadStringArray(reader, count, limit)

            If Not word2Docs.TryAdd(word, New HashSet(Of String)(docIds)) Then
                Throw New QGramCacheException($"duplicated word in QGram cache: {word}")
            End If
        Next

        Dim strings As New List(Of (str As String, id As Integer))(header.stringCount)
        Dim counts As New Dictionary(Of String, Integer)

        For i As Integer = 1 To header.stringCount
            Dim position As Integer = reader.ReadInt32()

            If position <> i - 1 Then
                Throw New QGramCacheException("non-contiguous q-gram string position in cache")
            End If

            Dim text As String = QGramCacheFormat.ReadString(reader, limit)
            Dim id As Integer = reader.ReadInt32()
            Dim gramCount As Integer = QGramCacheFormat.ReadCount(reader, limit, "q-gram count")

            If Not counts.TryAdd(text, gramCount) Then
                Throw New QGramCacheException($"duplicated q-gram string in cache: {text}")
            End If

            strings.Add((text, id))
        Next

        Dim gramIndex As New Dictionary(Of String, HashSet(Of Integer))(StringComparer.OrdinalIgnoreCase)

        For i As Integer = 1 To header.gramCount
            Dim gram As String = QGramCacheFormat.ReadString(reader, limit)
            Dim count As Integer = QGramCacheFormat.ReadCount(reader, limit, "q-gram postings")
            Dim positions As Integer() = QGramCacheFormat.ReadInt32Array(reader, count, limit)

            If Not gramIndex.TryAdd(gram, New HashSet(Of Integer)(positions)) Then
                Throw New QGramCacheException($"duplicated q-gram in cache: {gram}")
            End If
        Next

        If stream.Position <> limit Then
            Throw New QGramCacheException("QGram cache contains unexpected trailing data")
        End If

        Dim wordIndex As New QGramIndex(header.q, strings, counts, gramIndex)
        Dim index As New QGramFullText(docs, word2Docs, wordIndex, effectiveTokenizer)

        Call QGramCacheFormat.ValidateState(index, header.q)

        Return index
    End Function

    ''' <summary>
    ''' Attempts to load a cache file without throwing for expected cache-format failures.
    ''' </summary>
    Public Function TryLoad(file As String,
                            ByRef index As QGramFullText,
                            Optional tokenlicer As Func(Of String, IEnumerable(Of String)) = Nothing) As Boolean
        index = Nothing

        Try
            Using stream As New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)
                Return TryLoad(stream, index, tokenlicer)
            End Using
        Catch ex As Exception
            If IsCacheFailure(ex) Then
                Return False
            End If

            Throw
        End Try
    End Function

    ''' <summary>
    ''' Attempts to load a cache from the current position of a seekable stream.
    ''' </summary>
    Public Function TryLoad(stream As Stream,
                            ByRef index As QGramFullText,
                            Optional tokenlicer As Func(Of String, IEnumerable(Of String)) = Nothing) As Boolean
        index = Nothing

        Try
            index = Load(stream, tokenlicer)

            Return True
        Catch ex As Exception
            If IsCacheFailure(ex) Then
                Return False
            End If

            Throw
        End Try
    End Function

    Private Sub WriteCacheData(index As QGramFullText, stream As Stream, tokenizerId As String)
        Call QGramCacheFormat.ValidateState(index, index.wordIndex._q)

        Dim writer As New BinaryDataWriter(stream, Encoding.UTF8, leaveOpen:=True) With {
            .ByteOrder = ByteOrder.BigEndian
        }

        Call QGramCacheFormat.WriteHeader(writer,
                                          index.wordIndex._q,
                                          tokenizerId,
                                          index.docs.Count,
                                          index.word2Docs.Count,
                                          index.wordIndex._strings.Count,
                                          index.wordIndex._index.Count)

        For Each doc In index.docs.OrderBy(Function(item) item.Key, StringComparer.Ordinal)
            Call QGramCacheFormat.WriteString(writer, doc.Key)
            Call QGramCacheFormat.WriteString(writer, doc.Value)
        Next

        For Each word In index.word2Docs.OrderBy(Function(item) item.Key, StringComparer.Ordinal)
            Call QGramCacheFormat.WriteString(writer, word.Key)

            Dim docIds As String() = word.Value.OrderBy(Function(id) id, StringComparer.Ordinal).ToArray()

            Call writer.Write(docIds.Length)

            For Each docId As String In docIds
                Call QGramCacheFormat.WriteString(writer, docId)
            Next
        Next

        For i As Integer = 0 To index.wordIndex._strings.Count - 1
            Dim entry = index.wordIndex._strings(i)

            Call writer.Write(i)
            Call QGramCacheFormat.WriteString(writer, entry.str)
            Call writer.Write(entry.id)
            Call writer.Write(index.wordIndex._counts(entry.str))
        Next

        For Each gram In index.wordIndex._index.OrderBy(Function(item) item.Key, StringComparer.Ordinal)
            Call QGramCacheFormat.WriteString(writer, gram.Key)

            Dim positions As Integer() = gram.Value.OrderBy(Function(id) id).ToArray()

            Call writer.Write(positions.Length)
            Call writer.Write(positions)
        Next

        Call writer.Flush()
    End Sub

    Private Sub WriteFooter(stream As Stream, checksum As UInteger)
        Dim footer(QGramCacheFormat.ChecksumSize - 1) As Byte

        Call BinaryPrimitives.WriteUInt32BigEndian(footer, checksum)
        Call stream.Write(footer, 0, footer.Length)
        Call stream.Flush()
    End Sub

    Private Function IsCacheFailure(ex As Exception) As Boolean
        Return TypeOf ex Is QGramCacheException OrElse
               TypeOf ex Is IOException OrElse
               TypeOf ex Is UnauthorizedAccessException OrElse
               TypeOf ex Is ArgumentException OrElse
               TypeOf ex Is FormatException OrElse
               TypeOf ex Is OverflowException
    End Function

End Module
