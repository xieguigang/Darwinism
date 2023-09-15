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
