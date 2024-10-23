Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript

Public Class MongoDBIndexer : Inherits DocumentIndexer

    Dim mongoDB As Decoder
    Dim offset As Long

    Public Overrides Sub CreateDocumentIndex(document As Stream)
        Dim id As Integer = 0

        mongoDB = New Decoder(document)

        For Each json As JsonObject In TqdmWrapper.WrapStreamReader(document.Length, AddressOf requestJSON)
            ' make hash index
            For Each field As String In hashIndex.Keys
                Dim str As String = DirectCast(json(field), JsonValue).GetStripString(decodeMetachar:=False)
                Dim index As TermHashIndex = hashIndex(field)

                Call index.Indexing(str, id)
            Next

            Call offsets.Add(id, offset)

            id += 1
        Next
    End Sub

    Private Function requestJSON(ByRef getOffset As Long, bar As ProgressBar) As JsonObject
        getOffset = mongoDB.getDocumentOffset
        offset = getOffset
        Return mongoDB.decodeDocument
    End Function
End Class
