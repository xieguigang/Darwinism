Imports System.IO
Imports System.Runtime.CompilerServices
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json

''' <summary>
''' Helper object for create document file search index
''' </summary>
Public MustInherit Class DocumentIndexer

    ''' <summary>
    ''' the binary data offsets for read document block
    ''' </summary>
    Protected ReadOnly offsets As New Dictionary(Of UInteger, Long)
    Protected ReadOnly hashIndex As New Dictionary(Of String, TermHashIndex)

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub AddHashIndex(field As String)
        Call hashIndex.Add(field, InMemoryDocuments.CreateHashSearch)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="document">
    ''' the target document file
    ''' </param>
    Public MustOverride Sub CreateDocumentIndex(document As Stream)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s">the index file</param>
    Public Sub Save(s As Stream)
        Using index As New StreamPack(s)
            Dim stats As New Dictionary(Of String, Integer) From {
                {"blocks", offsets.Count},
                {"hash_index", hashIndex.Count}
            }

            ' save index stats counter
            Call index.WriteText(stats.GetJson, "/stats.json")
            ' save offsets
            Call index.WriteText(offsets.GetJson, "/offsets.json")

            ' save hashindex

        End Using
    End Sub

End Class
