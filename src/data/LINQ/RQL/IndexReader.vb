#Region "Microsoft.VisualBasic::d736f16451466f2bb07c4d45bf685e5c, G:/GCModeller/src/runtime/Darwinism/src/data/LINQ/RQL//IndexReader.vb"

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

    '   Total Lines: 50
    '    Code Lines: 39
    ' Comment Lines: 0
    '   Blank Lines: 11
    '     File Size: 1.50 KB


    ' Class IndexReader
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: Parse, Read
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Data.IO

Public Class IndexReader

    Dim file As BinaryDataReader

    Sub New(file As Stream)
        Me.file = New BinaryDataReader(file)
    End Sub

    Public Function Read() As Trie(Of NodeMap)
        If file.EndOfStream Then
            Dim tree As New Trie(Of NodeMap)
            tree.Root.data = New NodeMap With {.resources = New List(Of String)}
            Return tree
        Else
            Return New Trie(Of NodeMap)(Parse)
        End If
    End Function

    Private Function Parse() As CharacterNode(Of NodeMap)
        Dim node As New CharacterNode(Of NodeMap)(file.ReadChar)
        Dim maps As New List(Of String)
        Dim n As Integer
        Dim child As CharacterNode(Of NodeMap)

        node.Ends = file.ReadInt32
        n = file.ReadInt32

        For i As Integer = 0 To n - 1
            Call maps.Add(file.ReadString(BinaryStringFormat.ZeroTerminated))
        Next

        node.ID = file.ReadInt32
        node.label = file.ReadString(BinaryStringFormat.ZeroTerminated)
        node.Childs = New Dictionary(Of Char, CharacterNode(Of NodeMap))
        node.data = New NodeMap With {.resources = maps}

        n = file.ReadInt32

        For i As Integer = 0 To n - 1
            child = Parse()
            node.Childs.Add(child.Character, child)
        Next

        Return node
    End Function
End Class

