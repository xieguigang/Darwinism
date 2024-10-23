Imports System.IO
Imports System.Runtime.CompilerServices
Imports LINQ

''' <summary>
''' Helper object for create document file search index
''' </summary>
Public MustInherit Class DocumentIndexer

    Protected ReadOnly offsets As New Dictionary(Of UInteger, Long)
    Protected ReadOnly hashIndex As Dictionary(Of String, TermHashIndex)

    Protected hashFields As New List(Of String)

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub AddHashIndex(field As String)
        Call hashFields.Add(field)
    End Sub

    Public MustOverride Sub CreateDocumentIndex(document As Stream)

End Class
