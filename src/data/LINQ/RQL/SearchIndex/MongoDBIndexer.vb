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
                If Not json.HasObjectKey(field) Then
                    Continue For
                End If

                Dim val As JsonElement = json(field)
                Dim index As TermHashIndex = hashIndex(field)

                If TypeOf val Is JsonValue Then
                    ' is scalar string 
                    Dim str As String = DirectCast(val, JsonValue).GetStripString(decodeMetachar:=False, null:="")
                    Call index.Indexing(str, id)
                ElseIf TypeOf val Is JsonArray Then
                    ' is string array?
                    Dim strs As String() = DirectCast(val, JsonArray) _
                        .Select(Function(si) DirectCast(si, JsonValue) _
                        .GetStripString(decodeMetachar:=False, null:="")) _
                        .ToArray

                    For Each str As String In strs
                        Call index.Indexing(str, id)
                    Next
                Else
                    ' do nothing
                End If
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
