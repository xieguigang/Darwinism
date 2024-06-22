Public Class InMemoryDocuments : Inherits DocumentPool

    ReadOnly documents As New List(Of String)

    Public Overrides Function Save(doc As String) As Integer
        Dim id As Integer = documents.Count
        Call documents.Add(doc)
        Return id
    End Function

    Public Overrides Sub WriteIndex(index As InvertedIndex)
        ' do nothing
    End Sub

    Public Overrides Sub Dispose()
        Call documents.Clear()
    End Sub

    Public Overrides Function GetIndex() As InvertedIndex
        Return New InvertedIndex
    End Function

    Public Overrides Function GetDocument(id As Integer) As String
        Return documents(id)
    End Function

    Public Shared Function CreateEngine() As FTSEngine
        Return New FTSEngine(New InMemoryDocuments)
    End Function
End Class
