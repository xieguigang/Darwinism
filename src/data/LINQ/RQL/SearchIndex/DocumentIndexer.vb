Imports System.IO
Imports System.Runtime.CompilerServices
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.Serialization.JSON

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
            Call index.WriteText(stats.GetJson(simpleDict:=True), "/stats.json")
            ' save offsets
            Call index.WriteText(offsets.GetJson(simpleDict:=True), "/offsets.json")

            ' save hashindex
            For Each field As String In hashIndex.Keys
                Call HashIndexFile.WriteIndex(hashIndex(field), root:=$"/hash/{field}", s:=index)
            Next
        End Using
    End Sub

    Public Shared Function LoadDocument(documentfile As String) As DocumentIndex
        Dim indexfile As String = documentfile.ChangeSuffix("rqi")
        Dim s As Stream = indexfile.Open(FileMode.Open, doClear:=False, [readOnly]:=True)
        Dim index As New StreamPack(s, [readonly]:=True)
        Dim offsets As Dictionary(Of UInteger, Long) = index.ReadText("/offsets.json").LoadJSON(Of Dictionary(Of UInteger, Long))
        Dim document As Stream = documentfile.Open(FileMode.Open, doClear:=False, [readOnly]:=True)

        Call s.Dispose()

        Return New DocumentIndex(document, offsets)
    End Function

End Class
