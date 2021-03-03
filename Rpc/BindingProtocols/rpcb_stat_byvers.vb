Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' One rpcb_stat structure is returned for each version of rpcbind being monitored.
    ''' Provide only for rpcbind V2, V3 and V4.
    ''' typedef rpcb_stat rpcb_stat_byvers[RPCBVERS_STAT];
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_stat_byvers
        ''' <summary>
        ''' rpcbind V2 statistics
        ''' </summary>
        <Order(0)>
        Public V2 As rpcb_stat
        ''' <summary>
        ''' rpcbind V3 statistics
        ''' </summary>
        <Order(1)>
        Public V3 As rpcb_stat
        ''' <summary>
        ''' rpcbind V4 statistics
        ''' </summary>
        <Order(2)>
        Public V4 As rpcb_stat
    End Class
End Namespace
