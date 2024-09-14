#Region "Microsoft.VisualBasic::e3e8c735447db39701d879b7cfe0c49a, src\data\LINQ\LINQ\Runtime\Indexing\SearchIndex.vb"

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

    '   Total Lines: 65
    '    Code Lines: 19 (29.23%)
    ' Comment Lines: 38 (58.46%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 8 (12.31%)
    '     File Size: 2.21 KB


    ' Class SearchIndex
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Sub: (+2 Overloads) Indexing
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' a general search index model
''' </summary>
Public MustInherit Class SearchIndex

    ''' <summary>
    ''' document data pool
    ''' </summary>
    Protected ReadOnly documents As DocumentPool

    Protected Sub New(documents As DocumentPool)
        Me.documents = documents
    End Sub

    ''' <summary>
    ''' make index of the given document set
    ''' </summary>
    ''' <param name="doc">
    ''' each element insdie the input doc collection is a document.
    ''' </param>
    ''' <remarks>
    ''' usually be apply for processing index for the dataframe field or clr object array with scalar property value
    ''' </remarks>
    Public Sub Indexing(doc As IEnumerable(Of String))
        For Each par As String In doc
            Call Indexing(par)
        Next
    End Sub

    ''' <summary>
    ''' make index of the given document set
    ''' </summary>
    ''' <param name="doc">
    ''' each element inside the input doc collection is consist with multiple document contents.
    ''' each value in one collection element share the same index offset value. due to the reason of 
    ''' each value in one collection element comes from the same object property field. 
    ''' </param>
    ''' <remarks>
    ''' usually be apply for processing index for the clr object array with string array property value.
    ''' </remarks>
    Public Sub Indexing(doc As IEnumerable(Of String()))
        For Each par As String() In doc
            Call IndexingOneDocument(par)
        Next
    End Sub

    Protected MustOverride Sub IndexingOneDocument(data As String())

    ''' <summary>
    ''' make data index with auto index offset incremental
    ''' </summary>
    ''' <param name="doc">the doc content data for make index</param>
    Public MustOverride Sub Indexing(doc As String)

    ''' <summary>
    ''' index helper for non-scalar data field
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="id"></param>
    ''' <remarks>
    ''' multiple document content data share the same document id
    ''' </remarks>
    Public MustOverride Sub Indexing(doc As String, id As Integer)

End Class
