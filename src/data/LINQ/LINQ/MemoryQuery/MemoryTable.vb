﻿#Region "Microsoft.VisualBasic::40aa8873a54b8ff0ed23a46bdd763d2b, src\data\LINQ\LINQ\MemoryQuery\MemoryTable.vb"

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

    '   Total Lines: 209
    '    Code Lines: 157 (75.12%)
    ' Comment Lines: 15 (7.18%)
    '    - Xml Docs: 93.33%
    ' 
    '   Blank Lines: 37 (17.70%)
    '     File Size: 8.17 KB


    ' Class MemoryTable
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: FullText, HashIndex, Query, ValueRange
    ' 
    '     Sub: FullTextSearch, HashSearch, ValueMatchSearch, ValueRangeSearch
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.TagData
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports any = Microsoft.VisualBasic.Scripting

''' <summary>
''' an in-memory data table with search index supports
''' </summary>
Public Class MemoryTable

    ReadOnly df As DataFrame

    ReadOnly m_fulltext As New Dictionary(Of String, FTSEngine)
    ReadOnly m_hashindex As New Dictionary(Of String, TermHashIndex)
    ReadOnly m_valueindex As New Dictionary(Of String, ValueIndex)

    Sub New(df As DataFrame)
        Me.df = df
    End Sub

    Public Function FullText(field As String) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim fts As FTSEngine = InMemoryDocuments.CreateFullTextSearch
        fts.Indexing(col)
        m_fulltext(field) = fts
        Return Me
    End Function

    Public Function HashIndex(field As String) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim hash As TermHashIndex = InMemoryDocuments.CreateHashSearch
        hash.Indexing(col)
        m_hashindex(field) = hash
        Return Me
    End Function

    Public Function ValueRange(field As String, asType As Type) As MemoryTable
        Dim col As String() = df.Column(field)
        Dim index As ValueIndex

        Select Case asType
            Case GetType(Integer) : index = ValueIndex.IntegerIndex.IndexData(col.AsInteger)
            Case GetType(Double) : index = ValueIndex.DoubleIndex.IndexData(col.AsDouble)
            Case GetType(Date) : index = ValueIndex.DateIndex.IndexData(col.AsDateTime)
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
    Public Function Query(filter As IEnumerable(Of Query)) As DataFrame
        Dim index As Integer() = Nothing

        For Each q As Query In filter
            Select Case q.search
                Case LINQ.Query.Type.FullText : Call FullTextSearch(q, index)
                Case LINQ.Query.Type.HashTerm : Call HashSearch(q, index)
                Case LINQ.Query.Type.ValueRange : Call ValueRangeSearch(q, index)
                Case LINQ.Query.Type.ValueMatch, LINQ.Query.Type.ValueRangeGreaterThan, LINQ.Query.Type.ValueRangeLessThan
                    Call ValueMatchSearch(q, index)
                Case Else
                    Throw New NotImplementedException()
            End Select

            If index.Length = 0 Then
                Return Nothing
            End If
        Next

        Return df.Slice(index)
    End Function

    Private Sub ValueMatchSearch(q As Query, ByRef index As Integer())
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
    Private Sub ValueRangeSearch(q As Query, ByRef index As Integer())
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

    Private Sub HashSearch(q As Query, ByRef index As Integer())
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

    Private Sub FullTextSearch(q As Query, ByRef index As Integer())
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
