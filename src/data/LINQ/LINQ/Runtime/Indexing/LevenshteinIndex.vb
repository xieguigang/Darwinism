#Region "Microsoft.VisualBasic::cb740f094a969107acc7210216634886, src\data\LINQ\LINQ\Runtime\Indexing\LevenshteinIndex.vb"

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

    '   Total Lines: 28
    '    Code Lines: 20 (71.43%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (28.57%)
    '     File Size: 786 B


    ' Class LevenshteinIndex
    ' 
    '     Constructor: (+1 Overloads) Sub New
    ' 
    '     Function: Search
    ' 
    '     Sub: (+2 Overloads) Indexing, IndexingOneDocument
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Text.Levenshtein

Public Class LevenshteinIndex : Inherits SearchIndex

    ReadOnly index As LevenshteinTreeIndex

    Public Sub New(documents As DocumentPool)
        MyBase.New(documents)

        index = New LevenshteinTreeIndex
    End Sub

    Public Overrides Sub Indexing(doc As String)
        Call index.AddString(doc, documents.Save(doc))
    End Sub

    Public Overrides Sub Indexing(doc As String, id As Integer)
        Call index.AddString(doc, id)
    End Sub

    Protected Overrides Sub IndexingOneDocument(data() As String)
        Call Indexing(data.JoinBy(" "))
    End Sub

    Public Function Search(text As String) As IEnumerable(Of Integer)
        Return index.Query(text)
    End Function
End Class

