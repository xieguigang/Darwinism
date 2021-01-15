Imports System.IO
Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.BSON

''' <summary>
''' 只能够在进程之间映射一个不大于2GB的对象
''' </summary>
Public Class MapObject : Implements IDisposable

    Private disposedValue As Boolean

    Dim hMem As String
    Dim size As Integer

    Private Sub New()
    End Sub

    Public Function GetObject(type As Type) As Object
        Dim obj As Object
        Dim memory As MemoryMappedFile = MemoryMappedFile.OpenExisting(hMem)
        Dim view As MemoryMappedViewStream = memory.CreateViewStream

        obj = BSONFormat.Load(view).CreateObject(type)

        Return obj
    End Function

    Public Shared Function FromPointer(mem As UnmanageMemoryRegion) As MapObject
        Return New MapObject With {
            .hMem = mem.memoryFile,
            .size = mem.size
        }
    End Function

    Public Shared Function FromObject(obj As Object) As MapObject
        Dim type As Type = obj.GetType
        Dim element = type.GetJsonElement(obj, New JSONSerializerOptions)
        Dim bufferSize As Integer
        Dim hMem As String = App.GetNextUniqueName($"mem_{type.Name}_")

        Static mapFiles As New Dictionary(Of String, MemoryMappedFile)

        Using ms As MemoryStream = BSONFormat.SafeGetBuffer(element)
            Dim buffer As Byte() = ms.ToArray

            bufferSize = buffer.Length

            ' 20210115 MemoryMappedFile.CreateNew not working on unix .net 5
            Dim hMemFile As MemoryMappedFile = MemoryMappedFile.CreateNew(hMem, bufferSize)
            Dim view As MemoryMappedViewStream = hMemFile.CreateViewStream

            Call mapFiles.Add(hMem, hMemFile)
            Call view.Write(buffer, Scan0, bufferSize)

            Erase buffer
        End Using

        Return New MapObject With {
            .hMem = hMem,
            .size = bufferSize
        }
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Marshal.FreeHGlobal(hMem)
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

    Public Shared Narrowing Operator CType(map As MapObject) As UnmanageMemoryRegion
        Return New UnmanageMemoryRegion With {
            .memoryFile = map.hMem,
            .size = map.size
        }
    End Operator
End Class
