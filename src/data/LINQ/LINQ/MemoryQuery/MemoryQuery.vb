#Region "Microsoft.VisualBasic::8d0a9cc9b8ae145a61530e8be569eaa1, src\data\LINQ\LINQ\MemoryQuery\MemoryQuery.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 188
    '    Code Lines: 140 (74.47%)
    ' Comment Lines: 12 (6.38%)
    '    - Xml Docs: 91.67%
    ' 
    '   Blank Lines: 36 (19.15%)
    '     File Size: 7.47 KB


    ' Class MemoryQuery
    ' 
    '     Function: GetIndex
    ' 
    '     Sub: FullTextSearch, HashSearch, LevenshteinSearch, ValueMatchSearch, ValueRangeSearch
    ' 
    ' /********************************************************************************/

#End Region

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
        Dim result As IndexQueryResult = TryGetIndex(filter)

        If result Is Nothing Then
            Return Nothing
        Else
            Return result.Addresses
        End If
    End Function

    ''' <summary>
    ''' 尝试使用当前对象之中已经建立好的索引来执行条件查询，返回满足条件的候选数据行偏移。
    ''' </summary>
    ''' <param name="filter"></param>
    ''' <returns>
    ''' 当任意一个查询条件在当前对象之中没有可用的索引的时候，函数返回<c>Nothing</c>，
    ''' 调用方应当回退为全表扫描以保证查询结果的正确性。<see cref="IndexQueryResult.Used"/>
    ''' 为<c>True</c>并且<see cref="IndexQueryResult.Addresses"/>为空数组的时候则表示索引判定为无命中。
    ''' </returns>
    Public Function TryGetIndex(filter As IEnumerable(Of Query)) As IndexQueryResult
        Dim index As Integer() = Nothing
        Dim used As Boolean = False

        For Each q As Query In filter
            Dim offsets As Integer() = SearchByIndex(q)

            If offsets Is Nothing Then
                ' 存在无法使用索引的查询条件
                ' 整体回退为全表扫描
                Return Nothing
            End If

            used = True

            If index Is Nothing Then
                index = offsets
            Else
                index = index.Intersect(offsets).ToArray
            End If

            If index.Length = 0 Then
                Exit For
            End If
        Next

        If Not used Then
            Return Nothing
        End If

        ' 索引检索出来的偏移顺序是索引键值的顺序
        ' 这里重新按照数据行的原始顺序进行排列，与全表扫描路径保持一致的记录顺序
        index = index.OrderBy(Function(i) i).ToArray

        Return New IndexQueryResult With {.Used = True, .Addresses = index}
    End Function

    ''' <summary>
    ''' 按照指定的查询条件搜索索引，返回候选数据行偏移。<c>Nothing</c>则表示当前查询条件无法使用索引。
    ''' </summary>
    ''' <param name="q"></param>
    ''' <returns></returns>
    Public Function SearchByIndex(q As Query) As Integer()
        Select Case q.search
            Case Query.Type.ValueMatch
                ' 等值匹配：优先使用数值索引，其次使用词项哈希索引
                ' 全文索引以及编辑距离索引也可以用于缩小候选集
                If m_valueindex.ContainsKey(q.field) Then
                    Return ValueMatchSearch(q)
                ElseIf m_hashindex.ContainsKey(q.field) Then
                    Return HashSearch(q)
                ElseIf m_fulltext.ContainsKey(q.field) Then
                    Return FullTextSearch(q)
                ElseIf m_levenshtein.ContainsKey(q.field) Then
                    Return LevenshteinSearch(q)
                Else
                    Return Nothing
                End If
            Case Query.Type.ValueRangeGreaterThan, Query.Type.ValueRangeLessThan
                If m_valueindex.ContainsKey(q.field) Then
                    Return ValueMatchSearch(q)
                Else
                    Return Nothing
                End If
            Case Query.Type.ValueRange
                Return ValueRangeSearch(q)
            Case Query.Type.HashTerm
                Return HashSearch(q)
            Case Query.Type.FullText
                Return FullTextSearch(q)
            Case Query.Type.Levenshtein
                Return LevenshteinSearch(q)
            Case Else
                Return Nothing
        End Select
    End Function

    Protected Function ValueMatchSearch(q As Query) As Integer()
        Dim search As ValueIndex = m_valueindex.TryGetValue(q.field)

        If search Is Nothing Then
            Return Nothing
        End If

        Dim query As IEnumerable(Of IAddressOf)

        Select Case search.UnderlyingType
            Case GetType(Double)
                Dim val As Double = CDbl(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Double)).SearchGreaterThan(val, strict:=q.strict)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Double)).SearchLessThan(val, strict:=q.strict)
                Else
                    query = DirectCast(search, RangeIndex(Of Double)).Search(val)
                End If
            Case GetType(Integer)
                Dim val As Integer = CInt(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Integer)).SearchGreaterThan(val, strict:=q.strict)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Integer)).SearchLessThan(val, strict:=q.strict)
                Else
                    query = DirectCast(search, RangeIndex(Of Integer)).Search(val)
                End If
            Case GetType(Date)
                Dim val As Date = CDate(q.value)

                If q.search = LINQ.Query.Type.ValueRangeGreaterThan Then
                    query = DirectCast(search, RangeIndex(Of Date)).SearchGreaterThan(val, strict:=q.strict)
                ElseIf q.search = LINQ.Query.Type.ValueRangeLessThan Then
                    query = DirectCast(search, RangeIndex(Of Date)).SearchLessThan(val, strict:=q.strict)
                Else
                    query = DirectCast(search, RangeIndex(Of Date)).Search(val)
                End If
            Case Else
                Throw New NotImplementedException(search.UnderlyingType.FullName)
        End Select

        Return query.Select(Function(a) a.Address).ToArray
    End Function

    ''' <summary>
    ''' implements between ... and ... range search
    ''' </summary>
    ''' <param name="q"></param>
    ''' <returns></returns>
    Protected Function ValueRangeSearch(q As Query) As Integer()
        Dim search As ValueIndex = m_valueindex.TryGetValue(q.field)

        If search Is Nothing Then
            Return Nothing
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

        Return query.Select(Function(a) a.Address).ToArray
    End Function

    Protected Function HashSearch(q As Query) As Integer()
        If Not m_hashindex.ContainsKey(q.field) Then
            Return Nothing
        End If

        Return m_hashindex(q.field).Query(any.ToString(q.value)).ToArray
    End Function

    Protected Function FullTextSearch(q As Query) As Integer()
        If Not m_fulltext.ContainsKey(q.field) Then
            Return Nothing
        End If

        Return m_fulltext(q.field).Search(any.ToString(q.value)).Select(Function(a) a.i).ToArray
    End Function

    Protected Function LevenshteinSearch(q As Query) As Integer()
        If Not m_levenshtein.ContainsKey(q.field) Then
            Return Nothing
        End If

        Return m_levenshtein(q.field).Search(any.ToString(q.value)).ToArray
    End Function

End Class
