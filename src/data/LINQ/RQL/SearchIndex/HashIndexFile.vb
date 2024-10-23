Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson, $"{root}/documentMaps.json")

        Using buf As Stream = s.OpenFile($"{root}/hashMaps.bson", FileMode.Create, FileAccess.Write)
            Dim bytes As MemoryStream = BSONFormat.SafeGetBuffer(index.GetHashIndex.CreateJSONElement)

            Call bytes.Seek(Scan0, SeekOrigin.Begin)
            Call bytes.Flush()
            Call bytes.CopyTo(buf)
        End Using
    End Sub

End Class
