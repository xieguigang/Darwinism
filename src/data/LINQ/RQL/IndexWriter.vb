#Region "Microsoft.VisualBasic::f7b112059ce7b4663aa42600daf37d00, G:/GCModeller/src/runtime/Darwinism/src/data/LINQ/RQL//IndexWriter.vb"

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

    '   Total Lines: 40
    '    Code Lines: 29
    ' Comment Lines: 0
    '   Blank Lines: 11
    '     File Size: 1.10 KB


    ' Class IndexWriter
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Sub: (+2 Overloads) Write
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Data.IO

Public Class IndexWriter

    Dim file As BinaryDataWriter

    Sub New(file As Stream)
        Me.file = New BinaryDataWriter(file)
    End Sub

    Public Sub Write(tree As Trie(Of NodeMap))
        Call Write(tree.Root)
    End Sub

    Public Sub Write(node As CharacterNode(Of NodeMap))
        If node.data Is Nothing Then
            node.data = New NodeMap With {.resources = New List(Of String)}
        End If

        Call file.Write(node.Character)
        Call file.Write(node.Ends)
        Call file.Write(node.data.size)

        For Each si As String In node.data.resources
            Call file.Write(si, BinaryStringFormat.ZeroTerminated)
        Next

        Call file.Write(node.ID)
        Call file.Write(node.label, BinaryStringFormat.ZeroTerminated)

        Call file.Write(node.Childs.Count)

        For Each val As CharacterNode(Of NodeMap) In node.Childs.Values
            Call Write(val)
        Next
    End Sub

End Class

