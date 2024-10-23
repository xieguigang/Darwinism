Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson(simpleDict:=True), $"{root}/documentMaps.json")

        Using buf As Stream = s.OpenFile($"{root}/hashMaps.bson", FileMode.Create, FileAccess.Write)
            Dim bytes As MemoryStream = BSONFormat.SafeGetBuffer(index.GetHashIndex.CreateJSONElement)

            Call bytes.Seek(Scan0, SeekOrigin.Begin)
            Call bytes.Flush()
            Call bytes.CopyTo(buf)
        End Using
    End Sub

    Public Shared Function LoadIndex(s As StreamPack, root As String) As TermHashIndex
        Dim json_str As String = s.ReadText($"{root}/documentMaps.json")
        Dim documentMaps As Dictionary(Of Integer, Integer) = json_str.LoadJSON(Of Dictionary(Of Integer, Integer))
        Dim buf As Stream = s.OpenFile($"{root}/hashMaps.bson", FileMode.Open, FileAccess.Read)
        Dim hashMaps As JsonObject = BSONFormat.Load(buf, leaveOpen:=True)
        Dim hashMapping As Dictionary(Of String, Integer()) = hashMaps.CreateObject(Of Dictionary(Of String, Integer()))

        Return New TermHashIndex(New InMemoryDocuments, documentMaps, hashMapping)
    End Function

End Class
