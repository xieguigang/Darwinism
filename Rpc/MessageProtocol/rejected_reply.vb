Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Reply to an RPC call that was rejected by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class rejected_reply
        ''' <summary>
        ''' Reasons why a call message was rejected
        ''' </summary>
        <Switch>
        Public rstat As reject_stat

        ''' <summary>
        ''' the lowest and highest version numbers of the remote program supported by the server
        ''' </summary>
        <[Case](reject_stat.RPC_MISMATCH)>
        Public mismatch_info As mismatch_info

        ''' <summary>
        ''' the server rejects the identity of the caller
        ''' </summary>
        <[Case](reject_stat.AUTH_ERROR)>
        Public astat As auth_stat
    End Class
End Namespace
