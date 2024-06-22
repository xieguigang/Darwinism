Public MustInherit Class SearchIndex

    Protected ReadOnly documents As DocumentPool

    Protected Sub New(documents As DocumentPool)
        Me.documents = documents
    End Sub

    Public Sub Indexing(doc As IEnumerable(Of String))
        For Each par As String In doc
            Call Indexing(par)
        Next
    End Sub

    Public MustOverride Sub Indexing(doc As String)

End Class
