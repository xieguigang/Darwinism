Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class HashIndexFile

    Public Shared Sub WriteIndex(index As TermHashIndex, root As String, s As StreamPack)
        Call s.WriteText(index.GetDocumentMaps.GetJson, $"{root}/documentMaps.json")

        For Each term In index.GetHashIndex
            Call s.WriteText(term.Value.JoinBy(","), $"{root}/term/{term.Key}.txt")
        Next
    End Sub

End Class
