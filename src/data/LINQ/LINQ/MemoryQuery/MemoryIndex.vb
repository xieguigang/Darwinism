﻿Imports System.Data
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Linq
Imports any = Microsoft.VisualBasic.Scripting

Public MustInherit Class MemoryIndex

    Protected ReadOnly m_fulltext As New Dictionary(Of String, FTSEngine)
    Protected ReadOnly m_hashindex As New Dictionary(Of String, TermHashIndex)
    Protected ReadOnly m_valueindex As New Dictionary(Of String, ValueIndex)

    Protected MustOverride Function GetData(Of T)(field As String) As T()

    Public Function FullText(field As String) As MemoryIndex
        Dim col As String() = GetData(Of String)(field)
        Dim fts As FTSEngine = InMemoryDocuments.CreateFullTextSearch
        fts.Indexing(col)
        m_fulltext(field) = fts
        Return Me
    End Function

    Public Function HashIndex(field As String) As MemoryIndex
        Dim col As String() = GetData(Of String)(field)
        Dim hash As TermHashIndex = InMemoryDocuments.CreateHashSearch
        hash.Indexing(col)
        m_hashindex(field) = hash
        Return Me
    End Function

    Public Function ValueRange(field As String, asType As Type) As MemoryIndex
        Dim index As ValueIndex

        Select Case asType
            Case GetType(Integer) : index = ValueIndex.IntegerIndex.IndexData(GetData(Of Integer)(field))
            Case GetType(Double) : index = ValueIndex.DoubleIndex.IndexData(GetData(Of Double)(field))
            Case GetType(Date) : index = ValueIndex.DateIndex.IndexData(GetData(Of Date)(field))
            Case Else
                Throw New NotImplementedException(asType.FullName)
        End Select

        m_valueindex(field) = index

        Return Me
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' this function will returns nothing if query filter has no result
    ''' </returns>
    Public Function GetIndex(filter As IEnumerable(Of Query)) As Integer()
        Dim index As Integer() = Nothing

        For Each q As Query In filter
            Select Case q.search
                Case LINQ.Query.Type.FullText : Call FullTextSearch(q, index)
                Case LINQ.Query.Type.HashTerm : Call HashSearch(q, index)
                Case LINQ.Query.Type.ValueRange : Call ValueRangeSearch(q, index)
                Case LINQ.Query.Type.ValueMatch,
                     LINQ.Query.Type.ValueRangeGreaterThan,
                     LINQ.Query.Type.ValueRangeLessThan

                    Call ValueMatchSearch(q, index)

                Case Else
                    Throw New NotImplementedException(q.search.Description)
            End Select

            If index.Length = 0 Then
                Return Nothing
            End If
        Next

        Return index
    End Function

    Protected Sub ValueMatchSearch(q As Query, ByRef index As Integer())
        Dim search As ValueIndex = m_valueindex.TryGetValue(q.field)
        Dim offsets As IEnumerable(Of Integer)

        If search Is Nothing Then
            Throw New MissingPrimaryKeyException($"missing value range search index on data field '{q.field}'!")
        End If

        Dim query As IEnumerable(Of IAddressOf)

        Select Case search.UnderlyingType
            Case GetType(Double)
                Dim val As Double = CDbl(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Double)).SearchGreaterThan(val)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Double)).SearchLessThan(val)
                Else
                    query = DirectCast(search, RangeIndex(Of Double)).Search(val)
                End If
            Case GetType(Integer)
                Dim val As Integer = CInt(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Integer)).SearchGreaterThan(val)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Integer)).SearchLessThan(val)
                Else
                    query = DirectCast(search, RangeIndex(Of Integer)).Search(val)
                End If
            Case GetType(Date)
                Dim val As Date = CDate(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Date)).SearchGreaterThan(val)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Date)).SearchLessThan(val)
                Else
                    query = DirectCast(search, RangeIndex(Of Date)).Search(val)
                End If
            Case Else
                Throw New NotImplementedException(search.UnderlyingType.FullName)
        End Select

        offsets = query.Select(Function(a) a.Address)

        If index Is Nothing Then
            index = offsets.ToArray
        Else
            index = index.Intersect(offsets).ToArray
        End If
    End Sub

    ''' <summary>
    ''' implements between ... and ... range search
    ''' </summary>
    ''' <param name="q"></param>
    ''' <param name="index"></param>
    Protected Sub ValueRangeSearch(q As Query, ByRef index As Integer())
        Dim search As ValueIndex = m_valueindex.TryGetValue(q.field)
        Dim offsets As IEnumerable(Of Integer)

        If search Is Nothing Then
            Throw New MissingPrimaryKeyException($"missing value range search index on data field '{q.field}'!")
        End If

        Dim query As IEnumerable(Of IAddressOf)

        Select Case search.UnderlyingType
            Case GetType(Double)
                Dim minmax As Double() = DirectCast(q.value, Double())
                query = DirectCast(search, RangeIndex(Of Double)).Search(minmax(0), minmax(1))
            Case GetType(Integer)
                Dim minmax As Integer() = DirectCast(q.value, Integer())
                query = DirectCast(search, RangeIndex(Of Integer)).Search(minmax(0), minmax(1))
            Case GetType(Date)
                Dim minmax As Date() = DirectCast(q.value, Date())
                query = DirectCast(search, RangeIndex(Of Date)).Search(minmax(0), minmax(1))
            Case Else
                Throw New NotImplementedException(search.UnderlyingType.FullName)
        End Select

        offsets = query.Select(Function(a) a.Address)

        If index Is Nothing Then
            index = offsets.ToArray
        Else
            index = index.Intersect(offsets).ToArray
        End If
    End Sub

    Protected Sub HashSearch(q As Query, ByRef index As Integer())
        Dim text As String = any.ToString(q.value)

        If Not m_hashindex.ContainsKey(q.field) Then
            Throw New MissingPrimaryKeyException($"missing hash term search index on data field '{q.field}'!")
        End If

        Dim offsets = m_hashindex(q.field).Query(text).ToArray

        If index Is Nothing Then
            index = offsets
        Else
            index = index.Intersect(offsets).ToArray
        End If
    End Sub

    Protected Sub FullTextSearch(q As Query, ByRef index As Integer())
        Dim text As String = any.ToString(q.value)

        If Not m_fulltext.ContainsKey(q.field) Then
            Throw New MissingPrimaryKeyException($"missing full text search index on data field '{q.field}'!")
        End If

        Dim offsets = m_fulltext(q.field).Search(text).ToArray

        If index Is Nothing Then
            index = offsets.Select(Function(a) a.i).ToArray
        Else
            index = index.Intersect(offsets.Select(Function(a) a.i)).ToArray
        End If
    End Sub
End Class