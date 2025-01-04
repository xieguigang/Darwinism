Namespace proc.net

    ''' <summary>
    ''' /proc/net/udp
    ''' 
    ''' This file is similar to /proc/net/tcp, but it provides information about UDP sockets. 
    ''' Since UDP is connectionless, some of the fields that are relevant to TCP connections
    ''' (like st for socket state) are not applicable or are set to default values.
    ''' </summary>
    Public Class udp : Inherits tcp

        ''' <summary>
        ''' The socket’s state. For UDP, this is typically 07, which represents a socket 
        ''' that is waiting to receive packets.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Property st As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property ref As Integer
        Public Property pointer As String
        ''' <summary>
        ''' The number of packets dropped by the socket.
        ''' </summary>
        ''' <returns></returns>
        Public Property drops As String

    End Class
End Namespace