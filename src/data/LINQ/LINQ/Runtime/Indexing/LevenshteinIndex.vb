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
End Class
