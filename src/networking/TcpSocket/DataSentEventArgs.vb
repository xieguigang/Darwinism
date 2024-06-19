Namespace TcpSocket

    ''' <summary>
    ''' Arguments for data sent to a connected endpoint.
    ''' </summary>
    Public Class DataSentEventArgs : Inherits EventArgs

        ''' <summary>
        ''' The IP address and port number of the connected endpoint.
        ''' </summary>
        Public ReadOnly Property IpPort As String

        ''' <summary>
        ''' The number of bytes sent.
        ''' </summary>
        Public ReadOnly Property BytesSent As Long

        Sub New(ipPort As String, bytesSent As Long)
            Me.IpPort = ipPort
            Me.BytesSent = bytesSent
        End Sub
    End Class
End Namespace
