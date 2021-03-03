Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Reply to an RPC call that was accepted by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class accepted_reply
        ''' <summary>
        ''' authentication verifier that the server generates in order to validate itself to the client
        ''' </summary>
        <Order(0)>
        Public verf As opaque_auth

        ''' <summary>
        ''' the reply data.
        ''' </summary>
        <Order(1)>
        Public reply_data As reply_data_union

        ''' <summary>
        ''' the reply data
        ''' </summary>
        Public Class reply_data_union
            ''' <summary>
            ''' accept state
            ''' </summary>
            <Switch>
            <[Case](accept_stat.SUCCESS)> ' opaque results[0]; -  procedure-specific results start here
            <[Case](accept_stat.PROG_UNAVAIL), [Case](accept_stat.PROC_UNAVAIL), [Case](accept_stat.GARBAGE_ARGS), [Case](accept_stat.SYSTEM_ERR)> ' void
            Public stat As accept_stat

            ''' <summary>
            ''' the lowest and highest version numbers of the remote program supported by the server
            ''' </summary>
            <[Case](accept_stat.PROG_MISMATCH)>
            Public mismatch_info As mismatch_info
        End Class
    End Class
End Namespace
