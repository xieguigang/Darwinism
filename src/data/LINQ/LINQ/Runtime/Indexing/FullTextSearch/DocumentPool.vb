Public MustInherit Class DocumentPool

    ''' <summary>
    ''' save the document text to file
    ''' </summary>
    ''' <param name="doc"></param>
    Public MustOverride Function Save(doc As String) As Integer

    ''' <summary>
    ''' usually used for load index from file
    ''' </summary>
    ''' <returns></returns>
    Public MustOverride Function GetIndex() As InvertedIndex
    ''' <summary>
    ''' read document text from file via a given index
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public MustOverride Function GetDocument(id As Integer) As String
    ''' <summary>
    ''' apply for save index to file
    ''' </summary>
    ''' <param name="index"></param>
    Public MustOverride Sub WriteIndex(index As InvertedIndex)

    Public MustOverride Sub Dispose()

End Class

