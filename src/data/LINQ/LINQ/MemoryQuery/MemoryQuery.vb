Imports System.Data
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.Expressions
Imports any = Microsoft.VisualBasic.Scripting

Public MustInherit Class MemoryQuery

    Protected ReadOnly m_fulltext As New Dictionary(Of String, FTSEngine)
    Protected ReadOnly m_hashindex As New Dictionary(Of String, TermHashIndex)
    Protected ReadOnly m_valueindex As New Dictionary(Of String, ValueIndex)
    Protected ReadOnly m_levenshtein As New Dictionary(Of String, LevenshteinIndex)

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
                Case Query.Type.FullText : Call FullTextSearch(q, index)
                Case Query.Type.HashTerm : Call HashSearch(q, index)
                Case Query.Type.Levenshtein : Call LevenshteinSearch(q, index)
                Case Query.Type.ValueRange : Call ValueRangeSearch(q, index)
                Case Query.Type.ValueMatch,
                     Query.Type.ValueRangeGreaterThan,
                     Query.Type.ValueRangeLessThan

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

    Protected Sub LevenshteinSearch(q As Query, ByRef index As Integer())
        Dim text As String = any.ToString(q.value)

        If Not m_fulltext.ContainsKey(q.field) Then
            Throw New MissingPrimaryKeyException($"missing levenshtein text search index on data field '{q.field}'!")
        End If

        Dim offsets = m_levenshtein(q.field).Search(text).ToArray

        If index Is Nothing Then
            index = offsets
        Else
            index = index.Intersect(offsets).ToArray
        End If
    End Sub

End Class
