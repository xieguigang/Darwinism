Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson, $"{root}/documentMaps.json")
        Call s.WriteText(index.GetHashIndex.GetJson, $"{root}/hashMaps.json")
    End Sub

End Class
