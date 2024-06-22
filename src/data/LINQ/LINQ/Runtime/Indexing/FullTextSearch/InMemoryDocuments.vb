Public Class InMemoryDocuments : Inherits DocumentPool

    ReadOnly documents As New List(Of String)

    Public Overrides Sub Save(doc As String)
        Call documents.Add(doc)
    End Sub

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
