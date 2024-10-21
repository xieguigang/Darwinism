Public Class LevenshteinIndex : Inherits SearchIndex



    Public Sub New(documents As DocumentPool)
        MyBase.New(documents)
    End Sub

    Public Overrides Sub Indexing(doc As String)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Indexing(doc As String, id As Integer)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub IndexingOneDocument(data() As String)
        Throw New NotImplementedException()
    End Sub
End Class
