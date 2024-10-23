Imports System.IO
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.MIME.application.json.BSON
Imports Microsoft.VisualBasic.MIME.application.json.Javascript

Public Class DocumentIndex : Implements IDisposable

    ReadOnly offsets As Dictionary(Of UInteger, Long)
    ReadOnly document As Stream
    ReadOnly text As StreamReader

    Private disposedValue As Boolean

    Sub New(s As Stream)
        document = s
        text = New StreamReader(document)
    End Sub

    ''' <summary>
    ''' read text line, usually apply for the csv table file
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function ReadLine(id As UInteger) As String
        Dim offset As Long = offsets(id)
        text.DiscardBufferedData()
        document.Seek(offset, SeekOrigin.Begin)
        Return text.ReadLine
    End Function

    ''' <summary>
    ''' get a substream for the next document, usually apply for the bson list file, example as MongoDB
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
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

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call text.Close()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
