#Region "Microsoft.VisualBasic::4e39ee11460e241eb7ab8e479c0ba7a1, src\data\LINQ\LINQ\MemoryQuery\MemoryIndex.vb"

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

'   Total Lines: 228
'    Code Lines: 156 (68.42%)
' Comment Lines: 30 (13.16%)
'    - Xml Docs: 96.67%
' 
'   Blank Lines: 42 (18.42%)
'     File Size: 8.98 KB


' Class MemoryIndex
' 
'     Function: FullText, GetIndex, HashIndex, ValueRange
' 
'     Sub: FullTextSearch, HashSearch, ValueMatchSearch, ValueRangeSearch
' 
' /********************************************************************************/

#End Region

Imports System.Data
Imports System.IO

Public MustInherit Class MemoryIndex : Inherits MemoryQuery

    ''' <summary>
    ''' a proxy reader function for read data from the internal data resource set.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="field">the filed name/property name.</param>
    ''' <returns></returns>
    Protected MustOverride Function GetData(Of T)(field As String) As T()

    ''' <summary>
    ''' check of the data field its element type is scalar type or not
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns>
    ''' data frame object field should always be scalar, and the clr object its property field may be an array
    ''' </returns>
    Protected MustOverride Function CheckScalar(field As String) As Boolean

    ''' <summary>
    ''' make full text search index on a specific field
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns></returns>
    Public Function FullText(field As String) As MemoryIndex
        Dim fts As FTSEngine = InMemoryDocuments.CreateFullTextSearch

        If CheckScalar(field) Then
            Call fts.Indexing(GetData(Of String)(field))
        Else
            Call fts.Indexing(GetData(Of String())(field).Select(Function(bstr) bstr.JoinBy(" ")))
        End If

        m_fulltext(field) = fts

        Return Me
    End Function

    Public Function LevenshteinTree(field As String) As MemoryIndex
        Dim levenshtein = InMemoryDocuments.CreateLevenshteinIndex

        If CheckScalar(field) Then
            Call levenshtein.Indexing(GetData(Of String)(field))
        Else
            Throw New InvalidDataException("the character vector is not supported in levenshtein text similarity index!")
        End If

        m_levenshtein(field) = levenshtein

        Return Me
    End Function

    Public Function HashIndex(field As String) As MemoryIndex
        If Not CheckScalar(field) Then
            Throw New InvalidDataException("the character vector is not supported in term hash index!")
        End If

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
End Class
