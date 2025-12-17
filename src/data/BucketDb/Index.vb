Imports System.IO
Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' data index
''' </summary>
Public Class Index

    ''' <summary>
    ''' 索引项: key: hashcode, value: (offset, size)
    ''' </summary>
    Dim index As Dictionary(Of UInteger, BufferRegion)
    Dim indexFile As String

    Sub New(file As String)
        indexFile = file
    End Sub

    Public Function IndexValue() As Dictionary(Of UInteger, BufferRegion)
        If index Is Nothing Then
            Call LoadIndex(indexFile, index)
        End If

        Return index
    End Function

    ''' <summary>
    ''' 从索引文件加载索引到内存
    ''' </summary>
    Private Shared Sub LoadIndex(indexFilePath As String, ByRef index As Dictionary(Of UInteger, BufferRegion))
        If index Is Nothing Then
            index = New Dictionary(Of UInteger, BufferRegion)
        End If
        If Not File.Exists(indexFilePath) OrElse New FileInfo(indexFilePath).Length = 0 Then
            Return
        End If

        Using indexStream As New FileStream(indexFilePath, FileMode.Open, FileAccess.Read)
            Using indexReader As New BinaryDataReader(indexStream)
                Dim count As Integer = indexReader.ReadInt32()
                For j As Integer = 0 To count - 1
                    Dim hashcode As UInteger = indexReader.ReadUInt32()
                    Dim offset As Long = indexReader.ReadInt64()
                    Dim size As Integer = indexReader.ReadInt32()
                    index(hashcode) = New BufferRegion(offset, size)
                Next
            End Using
        End Using
    End Sub
End Class
