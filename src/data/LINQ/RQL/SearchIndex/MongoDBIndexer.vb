Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript

Public Class MongoDBIndexer : Inherits DocumentIndexer

    Dim mongoDB As Decoder
    Dim offset As Long

    Public Overrides Sub CreateDocumentIndex(document As Stream)
        Dim i As Integer = 0

        mongoDB = New Decoder(document)

        For Each json As JsonObject In TqdmWrapper.WrapStreamReader(document.Length, AddressOf requestJSON)
            ' make hash index


            i += 1
        Next
    End Sub

    Private Function requestJSON(ByRef getOffset As Long, bar As ProgressBar) As JsonObject
        getOffset = mongoDB.getDocumentOffset
        offset = getOffset
        Return mongoDB.decodeDocument
    End Function
End Class
