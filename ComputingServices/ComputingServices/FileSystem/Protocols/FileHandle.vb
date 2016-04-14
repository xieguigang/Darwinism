Namespace FileSystem.Protocols

    ''' <summary>
    ''' The file handle object on the remote server machine.
    ''' (在远端服务器上面的文件句柄对象)
    ''' </summary>
    Public Class FileHandle

        ''' <summary>
        ''' The file location on the remote file system.
        ''' </summary>
        ''' <returns></returns>
        Public Property FileName As String
        ''' <summary>
        ''' The hash code value on the remote services program.
        ''' </summary>
        ''' <returns></returns>
        Public Property HashCode As Integer

        Sub New()
        End Sub

        Sub New(handle As FileHandle)
            Me.FileName = handle.FileName
            Me.HashCode = handle.HashCode
        End Sub

        Public Overrides Function ToString() As String
            Return Handle
        End Function

        ''' <summary>
        ''' 远程机器上面唯一标示的文件句柄值
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Handle As String
            Get
                Return $"{HashCode}+{FileName.ToFileURL}"
            End Get
        End Property
    End Class
End Namespace