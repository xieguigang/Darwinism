Public MustInherit Class SearchIndex

    Protected ReadOnly documents As DocumentPool

    Protected Sub New(documents As DocumentPool)
        Me.documents = documents
    End Sub

End Class
