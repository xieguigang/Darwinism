Imports System.IO
Imports LINQ
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem

Public Class QueryIndex : Inherits MemoryQuery

    Public ReadOnly Property hashKeys As String()
        Get
            Return m_hashindex.Keys.ToArray
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rqi">
    ''' the data stream to the resource query index file
    ''' </param>
    Sub New(rqi As Stream)
        Call loadIndex(New StreamPack(rqi, [readonly]:=True))
    End Sub

    Private Sub loadIndex(rqi As StreamPack)
        Dim hash As StreamGroup = rqi.GetObject("/hash/")

        If Not hash Is Nothing Then
            For Each field As StreamGroup In hash.files.OfType(Of StreamGroup)
                Dim dir As String = $"/hash/{field.fileName}/"
                Dim index As TermHashIndex = HashIndexFile.LoadIndex(rqi, dir)

                m_hashindex(field.fileName) = index
            Next
        End If
    End Sub
End Class
