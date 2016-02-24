Imports System.IO
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace FileSystem.Protocols

    Public Class FileStreamInfo : Inherits FileHandle

        Public Property CanRead As Boolean
        Public Property CanSeek As Boolean
        Public Property CanWrite As Boolean
        Public Property FileHandle As IntPtr
        Public Property IsAsync As Boolean

        Sub New()

        End Sub

        Public Shared Function GetInfo(file As FileStream) As FileStreamInfo
#Disable Warning
            Dim handle As New FileStreamInfo With {
                .HashCode = file.GetHashCode,
                .CanRead = file.CanRead,
                .CanSeek = file.CanSeek,
                .CanWrite = file.CanWrite,
                .FileHandle = file.Handle,
                .IsAsync = file.IsAsync
            }  ' 可能会出现重复的文件名，所以使用这个句柄对象来进行唯一标示
#Enable Warning
            Return handle
        End Function
    End Class

    Public Class SeekArgs : Inherits FileHandle

        Public Property offset As Long
        Public Property origin As Integer

        Sub New(handle As FileHandle)
            Call MyBase.New(handle)
        End Sub

        Sub New()

        End Sub

        Public Function Seek(stream As System.IO.FileStream) As Long
            Dim ori As SeekOrigin = CType(origin, SeekOrigin)
            Return stream.Seek(offset, ori)
        End Function
    End Class

    Public Class LockArgs : Inherits FileHandle

        Public Property Lock As Boolean
        Public Property position As Long
        Public Property length As Long

        Sub New(handle As FileHandle)
            Call MyBase.New(handle)
        End Sub

        Sub New()

        End Sub

        Public Sub LockOrNot(stream As System.IO.FileStream)
            If Lock Then
                Call stream.Lock(position, length)
            Else
                Call stream.Unlock(position, length)
            End If
        End Sub
    End Class
End Namespace