Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class ObjectStream : Inherits RawStream
    Implements IDisposable

    Dim disposedValue As Boolean

    Public Property method As StreamMethods
    Public Property stream As Byte()
    Public Property type As TypeInfo

    Sub New(type As TypeInfo, method As StreamMethods, stream As Stream)
        Me.method = method
        Me.stream = New StreamPipe(stream).Read
        Me.type = type

        Call stream.Close()
        Call stream.Dispose()
    End Sub

    Sub New(raw As Byte())
        Using read As New BinaryReader(New MemoryStream(raw))
            Dim methodi As Integer = read.ReadInt32
            Dim size As Integer = read.ReadInt32
            Dim chunk As Byte() = read.ReadBytes(size)
            Dim typeJson As String = chunk.UTF8String

            size = read.ReadInt32
            type = typeJson.LoadJSON(Of TypeInfo)
            method = CType(methodi, StreamMethods)
            stream = read.ReadBytes(size)
        End Using
    End Sub

    Public Overrides Function Serialize() As Byte()
        Using ms As New MemoryStream
            Dim json As Byte() = Encoding.UTF8.GetBytes(type.GetJson)

            Call ms.Write(BitConverter.GetBytes(method), Scan0, RawStream.INT32)
            Call ms.Write(BitConverter.GetBytes(json.Length), Scan0, RawStream.INT32)
            Call ms.Write(json, Scan0, json.Length)
            Call ms.Write(BitConverter.GetBytes(stream.Length), Scan0, RawStream.INT32)
            Call ms.Write(stream, Scan0, stream.Length)

            Return ms.ToArray
        End Using
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Erase stream
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并替代终结器
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
