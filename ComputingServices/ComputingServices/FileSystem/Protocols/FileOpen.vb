Imports System.IO
Imports Microsoft.VisualBasic.Net.Protocol.Reflection

Namespace FileSystem.Protocols

    ''' <summary>
    ''' Initializes a new instance of the System.IO.FileStream class with the specified
    ''' path and creation mode.
    ''' </summary>
    Public Class FileOpen : Inherits FileHandle

        ''' <summary>
        ''' Specifies how the operating system should open a file.
        ''' </summary>
        ''' <returns></returns>
        Public Property Mode As Integer

        Public Overrides Function ToString() As String
            Return $"[{DirectCast(Mode, FileMode).ToString }] " & FileName.ToFileURL
        End Function

        ''' <summary>
        ''' Initializes a new instance of the System.IO.FileStream class with the specified
        ''' path and creation mode.
        ''' </summary>
        Public Function OpenHandle() As FileStream
            Dim mode As FileMode = DirectCast(Me.Mode, FileMode)
            Return New FileStream(FileName, mode)
        End Function
    End Class

    ''' <summary>
    ''' 在远端服务器上面的文件句柄对象
    ''' </summary>
    Public Class FileHandle
        Public Property FileName As String
        Public Property HashCode As Integer

        Public Overrides Function ToString() As String
            Return $"{HashCode}+{FileName.ToFileURL}"
        End Function
    End Class
End Namespace