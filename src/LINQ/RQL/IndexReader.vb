Imports System.IO
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Data.IO

Public Class IndexReader

    Dim file As BinaryDataReader

    Sub New(file As Stream)
        Me.file = New BinaryDataReader(file)
    End Sub

    Public Function Read() As Trie(Of NodeMap)
        Return New Trie(Of NodeMap)(Parse)
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

        n = file.ReadInt32

        For i As Integer = 0 To n - 1
            child = Parse()
            node.Childs.Add(child.Character, child)
        Next

        Return node
    End Function
End Class
