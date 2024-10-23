Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript

Public Class DocumentIndex

    ReadOnly offsets As Dictionary(Of UInteger, Long)
    ReadOnly document As Stream
    ReadOnly text As StreamReader

    Sub New(s As Stream)

    End Sub

    Public Function ReadLine(id As UInteger) As String
        Dim offset As Long = offsets(id)
        text.DiscardBufferedData()
        document.Seek(offset, SeekOrigin.Begin)
        Return text.ReadLine
    End Function

    Public Function GetSubBuffer(id As UInteger) As Stream
        Dim offset As Long = offsets(id)
        Dim s As New SubStream(document, offset, document.Length - offset)
        Return s
    End Function

    Public Function GetObject(id As UInteger) As JsonObject
        Dim s As Stream = GetSubBuffer(id)
        Dim obj As JsonObject = BSONFormat.Load(s, leaveOpen:=True)
        Return obj
    End Function

End Class
