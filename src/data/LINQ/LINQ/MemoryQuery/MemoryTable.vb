#Region "Microsoft.VisualBasic::40aa8873a54b8ff0ed23a46bdd763d2b, src\data\LINQ\LINQ\MemoryQuery\MemoryTable.vb"

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

Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Scripting.Runtime

''' <summary>
''' an in-memory data table with search index supports
''' </summary>
Public Class MemoryTable : Inherits MemoryIndex

    ReadOnly df As DataFrame

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

End Class
