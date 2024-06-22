#Region "Microsoft.VisualBasic::7a08467f017d8abc52236ecb60026d3b, src\data\LINQ\LINQ\Runtime\Indexing\TermHashIndex.vb"

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

'   Total Lines: 18
'    Code Lines: 13 (72.22%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 5 (27.78%)
'     File Size: 509 B


' Class TermHashIndex
' 
'     Constructor: (+1 Overloads) Sub New
'     Sub: Indexing
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Linq

Public Class TermHashIndex : Inherits SearchIndex

    ReadOnly hashIndex As New Dictionary(Of String, List(Of Integer))

    Public Sub New(documents As DocumentPool)
        MyBase.New(documents)
    End Sub

    Public Overrides Sub Indexing(doc As String)
        Dim doc_key = doc.ToLower

        If Not hashIndex.ContainsKey(doc_key) Then
            hashIndex.Add(doc_key, New List(Of Integer))
        End If

        hashIndex(doc_key).Add(documents.Save(doc))
    End Sub

    Public Function Query(term As String) As IEnumerable(Of Integer)
        Return hashIndex.TryGetValue(Strings.LCase(term)).SafeQuery
    End Function
End Class
