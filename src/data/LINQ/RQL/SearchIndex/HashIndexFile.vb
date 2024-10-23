Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson, $"{root}/documentMaps.json")
        Call s.WriteText(BSONFormat.SafeGetBuffer(index.GetHashIndex.CreateJSONElement), $"{root}/hashMaps.bson")
    End Sub

End Class
