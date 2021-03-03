
Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Protocol name (rpcb_entry.r_nc_proto): This identifies a protocol within a family.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Module ProtocolName
        ''' <summary>
        ''' no protocol name (-)
        ''' </summary>
        Public Const NC_NOPROTO As String = "-"
        ''' <summary>
        ''' tcp
        ''' </summary>
        Public Const NC_TCP As String = "tcp"
        ''' <summary>
        ''' udp
        ''' </summary>
        Public Const NC_UDP As String = "udp"
        ''' <summary>
        ''' icmp
        ''' </summary>
        Public Const NC_ICMP As String = "icmp"
    End Module
End Namespace
