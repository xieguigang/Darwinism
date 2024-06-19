Public MustInherit Class DocumentPool

    Public MustOverride Sub Save(doc As String)
    Public MustOverride Function GetIndex() As InvertedIndex
    Public MustOverride Function GetDocument(id As Integer) As String
    Public MustOverride Sub WriteIndex(index As InvertedIndex)

    Public MustOverride Sub Dispose()

End Class

