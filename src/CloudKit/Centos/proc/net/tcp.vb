Imports System.IO
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Tcp

Namespace proc.net

    ' In Linux, the /proc filesystem is a virtual filesystem that provides an interface to
    ' kernel data structures. It is not stored on disk, but is created dynamically when the
    ' system boots up and is removed when the system shuts down. The /proc filesystem
    ' contains a wealth of information about the system, including process information,
    ' memory usage, hardware configuration, and network status.

    ' The /proc/net/tcp and /proc/net/udp files are part of the /proc filesystem and provide
    ' detailed information about the TCP and UDP sockets that are currently in use by the
    ' Linux kernel.

    ''' <summary>
    ''' /proc/net/tcp
    ''' </summary>
    ''' <remarks>
    ''' This file contains information about all TCP sockets that are currently open on 
    ''' the system. Each line in the file represents a TCP socket, and the fields in 
    ''' each line provide details about the socket’s state, the local and remote addresses 
    ''' and ports, and other related information.
    ''' </remarks>
    Public Class tcp

        ''' <summary>
        ''' The socket’s slot (index) in the kernel’s socket table.
        ''' </summary>
        ''' <returns></returns>
        Public Property sl As Integer
        ''' <summary>
        ''' The local IP address and port number (in hexadecimal).
        ''' </summary>
        ''' <returns></returns>
        Public Property local_address As String
        ''' <summary>
        ''' The remote IP address and port number (in hexadecimal), or 00000000:0000 if not connected.
        ''' </summary>
        ''' <returns></returns>
        Public Property rem_address As String
        ''' <summary>
        ''' The socket’s state (e.g., 0A for listening, 01 for established).
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Property st As String
        ''' <summary>
        ''' The amount of data in the transmit queue.
        ''' </summary>
        ''' <returns></returns>
        Public Property tx_queue As String
        ''' <summary>
        ''' The amount of data in the receive queue.
        ''' </summary>
        ''' <returns></returns>
        Public Property rx_queue As String
        ''' <summary>
        ''' Timing and retransmission information.
        ''' </summary>
        ''' <returns></returns>
        Public Property tr As String
        ''' <summary>
        ''' Timing and retransmission information.
        ''' </summary>
        ''' <returns></returns>
        <Column("tm->when")>
        Public Property tm_when As String
        ''' <summary>
        ''' Timing and retransmission information.
        ''' </summary>
        ''' <returns></returns>
        Public Property retrnsmt As String
        ''' <summary>
        ''' The user ID of the process that owns the socket.
        ''' </summary>
        ''' <returns></returns>
        Public Property uid As Integer
        ''' <summary>
        ''' The socket’s timeout value.
        ''' </summary>
        ''' <returns></returns>
        Public Property timeout As Integer
        ''' <summary>
        ''' The inode number of the socket. This can be used to find the process using 
        ''' the socket by looking at ``/proc/[pid]/fdinfo/`` for the inode.
        ''' </summary>
        ''' <returns></returns>
        Public Property inode As String()

        Public Overrides Function ToString() As String
            Return GetLocalAddress.ToString
        End Function

        Public Function GetLocalAddress() As IPEndPoint
            Dim ipaddr = local_address.Split(":"c)
            Dim ip As String = IPv4.NumericIpToSymbolic(hexIp:=ipaddr(0))
            Dim port As Integer = Convert.ToInt32(ipaddr(1), 16)

            Return New IPEndPoint(ip, port)
        End Function

        Public Shared Iterator Function Parse(file As Stream) As IEnumerable(Of tcp)
            Dim str As New StreamReader(file)
            Dim line As Value(Of String) = str.ReadLine()

            Do While (line = str.ReadLine) IsNot Nothing
                Dim cols As String() = line.Trim.StringSplit("\s+")
                Dim queue As String() = cols(4).Split(":"c)
                Dim trtm As String() = cols(5).Split(":"c)
                Dim port As New tcp With {
                    .sl = Val(cols(0)),
                    .local_address = cols(1),
                    .rem_address = cols(2),
                    .st = cols(3),
                    .tx_queue = queue(0),
                    .rx_queue = queue(1),
                    .tr = trtm(0),
                    .tm_when = trtm(1),
                    .retrnsmt = cols(6),
                    .uid = Integer.Parse(cols(7)),
                    .timeout = cols(8),
                    .inode = cols.Skip(9).ToArray
                }

                Yield port
            Loop
        End Function

    End Class
End Namespace