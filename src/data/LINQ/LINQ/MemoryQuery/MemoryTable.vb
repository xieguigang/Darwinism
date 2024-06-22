#Region "Microsoft.VisualBasic::8e4b4408b5e685d0dd988a0267beae15, src\data\LINQ\LINQ\MemoryQuery\MemoryTable.vb"

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

    '   Total Lines: 52
    '    Code Lines: 38 (73.08%)
    ' Comment Lines: 3 (5.77%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 11 (21.15%)
    '     File Size: 1.74 KB


    ' Class MemoryTable
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: FullText, HashIndex, ValueRange
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Scripting.Runtime

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

End Class

