﻿#Region "Microsoft.VisualBasic::abab52ca566b17042b38277ed00d5b40, src\data\LINQ\LINQ\Runtime\Indexing\TermHashIndex.vb"

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

    '   Total Lines: 96
    '    Code Lines: 52 (54.17%)
    ' Comment Lines: 27 (28.12%)
    '    - Xml Docs: 88.89%
    ' 
    '   Blank Lines: 17 (17.71%)
    '     File Size: 3.37 KB


    ' Class TermHashIndex
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    '     Function: GetDocumentMaps, GetHashIndex, Query
    ' 
    '     Sub: (+2 Overloads) Indexing, IndexingOneDocument
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' use the clr dictionary hash index for make the term string indexed
''' </summary>
Public Class TermHashIndex : Inherits SearchIndex

    ''' <summary>
    ''' term mapping to the document id set
    ''' </summary>
    ReadOnly hashIndex As New Dictionary(Of String, List(Of Integer))
    ''' <summary>
    ''' index mapping for resolve mapping the external index with the document index
    ''' </summary>
    ''' <remarks>
    ''' a mapping of document id to the query id
    ''' </remarks>
    ReadOnly documentMaps As New Dictionary(Of Integer, Integer)

    Dim queryId As i32 = 0

    Public Sub New(documents As DocumentPool)
        MyBase.New(documents)
    End Sub

    Sub New(documents As DocumentPool, documentMaps As Dictionary(Of Integer, Integer), hashMaps As Dictionary(Of String, Integer()))
        MyBase.New(documents)

        Me.documentMaps = New Dictionary(Of Integer, Integer)(documentMaps)
        Me.hashIndex = hashMaps _
            .ToDictionary(Function(a) a.Key,
                          Function(a)
                              Return New List(Of Integer)(a.Value)
                          End Function)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetDocumentMaps() As Dictionary(Of Integer, Integer)
        Return New Dictionary(Of Integer, Integer)(documentMaps)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetHashIndex() As Dictionary(Of String, Integer())
        Return hashIndex.ToDictionary(Function(a) a.Key, Function(a) a.Value.ToArray)
    End Function

    ''' <summary>
    ''' index for the scalar data field
    ''' </summary>
    ''' <param name="doc"></param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Overrides Sub Indexing(doc As String)
        Call Indexing(doc, ++queryId)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="doc">the document content data for make hash index</param>
    ''' <param name="id">the document id that associated with the given document content data</param>
    Public Overrides Sub Indexing(doc As String, id As Integer)
        Dim doc_key = doc.ToLower

        If Not hashIndex.ContainsKey(doc_key) Then
            Call hashIndex.Add(doc_key, New List(Of Integer))
        End If

        Dim documentId As Integer = documents.Save(doc)

        Call hashIndex(doc_key).Add(documentId)
        Call documentMaps.Add(documentId, id)
    End Sub

    ''' <summary>
    ''' helper function for make index for the non-scalar data field
    ''' </summary>
    ''' <param name="data"></param>
    Protected Overrides Sub IndexingOneDocument(data() As String)
        Dim id As Integer = ++queryId

        For Each doc As String In data.SafeQuery
            Call Indexing(doc, id)
        Next
    End Sub

    Public Function Query(term As String) As IEnumerable(Of Integer)
        Dim docsId = hashIndex.TryGetValue(Strings.LCase(term)).SafeQuery
        ' mapping to the query id
        Dim index = docsId.Select(Function(docId) documentMaps(docId))

        Return index
    End Function
End Class
