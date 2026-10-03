Imports System.Buffers.Binary
Imports System.IO
Imports Microsoft.VisualBasic.Data.IO

#If NETCOREAPP Then
Imports System.IO.Compression
#End If

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

#If NETCOREAPP Then
        Using file As New FileStream(indexFilePath, FileMode.Open, FileAccess.Read)
            Using compressor As New BrotliStream(file, mode:=CompressionMode.Decompress)
                Using buffer As New MemoryStream
                    Call compressor.CopyTo(buffer)
                    Call buffer.Seek(Scan0, SeekOrigin.Begin)
                    Call ParseIndex(buffer, index)
                End Using
            End Using
        End Using
#Else
        Using indexStream As New FileStream(indexFilePath, FileMode.Open, FileAccess.Read)
            Call ParseIndex(indexStream, index)
        End Using
#End If
    End Sub

    ''' <summary>
    ''' 批量解析索引: 将索引流一次性读入内存缓冲区后解析，
    ''' 取代逐条 <c>ReadUInt32/ReadInt64/ReadInt32</c> 流式读取（百万级条目快一个数量级）。
    ''' </summary>
    Private Shared Sub ParseIndex(indexStream As Stream, ByRef index As Dictionary(Of UInteger, BufferRegion))
        Using buffer As New MemoryStream
            Call indexStream.CopyTo(buffer)
            Call ParseIndexBuffer(buffer.GetBuffer(), CInt(buffer.Length), index)
        End Using
    End Sub

    ''' <summary>
    ''' 解析索引缓冲区: 格式 <c>[count(4)] + count * [hashcode(4)][offset(8)][size(4)]</c>，
    ''' 每条记录 16 字节，使用 <see cref="BinaryPrimitives"/> 批量小端读取。
    ''' </summary>
    Private Shared Sub ParseIndexBuffer(buffer As Byte(), length As Integer, ByRef index As Dictionary(Of UInteger, BufferRegion))
        If length < 4 Then
            Return
        End If

        Dim count As Integer = BinaryPrimitives.ReadInt32LittleEndian(buffer.AsSpan(0, 4))

        If count <= 0 Then
            Return
        End If

        ' 按 count 预设字典容量，避免加载过程中的 rehash 扩容
        index = New Dictionary(Of UInteger, BufferRegion)(count)

        Dim i As Integer = 4

        For j As Integer = 0 To count - 1
            Dim hashcode As UInteger = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan(i, 4))
            Dim offset As Long = BinaryPrimitives.ReadInt64LittleEndian(buffer.AsSpan(i + 4, 8))
            Dim size As Integer = BinaryPrimitives.ReadInt32LittleEndian(buffer.AsSpan(i + 12, 4))
            index(hashcode) = New BufferRegion(offset, size)
            i += 16
        Next
    End Sub
End Class
