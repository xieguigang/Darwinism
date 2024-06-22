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
End Class
