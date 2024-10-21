#Region "Microsoft.VisualBasic::e7c0213fc253cea1c1aa41c31a009cdd, src\data\LINQ\LINQ\Runtime\Indexing\FullTextSearch\InMemoryDocuments.vb"

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

    '   Total Lines: 34
    '    Code Lines: 25 (73.53%)
    ' Comment Lines: 1 (2.94%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (23.53%)
    '     File Size: 983 B


    ' Class InMemoryDocuments
    ' 
    '     Function: CreateFullTextSearch, CreateHashSearch, GetDocument, GetIndex, Save
    ' 
    '     Sub: Dispose, WriteIndex
    ' 
    ' /********************************************************************************/

#End Region

Public Class InMemoryDocuments : Inherits DocumentPool

    ReadOnly documents As New List(Of String)

    Public Overrides Function Save(doc As String) As Integer
        Dim id As Integer = documents.Count
        Call documents.Add(doc)
        Return id
    End Function

    Public Overrides Sub WriteIndex(index As InvertedIndex)
        ' do nothing
    End Sub

    Public Overrides Sub Dispose()
        Call documents.Clear()
    End Sub

    Public Overrides Function GetIndex() As InvertedIndex
        Return New InvertedIndex
    End Function

    Public Overrides Function GetDocument(id As Integer) As String
        Return documents(id)
    End Function

    Public Shared Function CreateLevenshteinIndex() As LevenshteinIndex
        Return New LevenshteinIndex(New InMemoryDocuments)
    End Function

    Public Shared Function CreateFullTextSearch() As FTSEngine
        Return New FTSEngine(New InMemoryDocuments)
    End Function

    Public Shared Function CreateHashSearch() As TermHashIndex
        Return New TermHashIndex(New InMemoryDocuments)
    End Function
End Class
