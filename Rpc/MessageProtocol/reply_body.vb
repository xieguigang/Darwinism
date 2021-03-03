Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Body of a reply to an RPC call
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class reply_body
        ''' <summary>
        ''' A reply to a call message can take on two forms: the message was either accepted or rejected.
        ''' </summary>
        <Switch>
        Public stat As reply_stat

        ''' <summary>
        ''' Reply to an RPC call that was accepted by the server
        ''' </summary>
        <[Case](reply_stat.MSG_ACCEPTED)>
        Public areply As accepted_reply

        ''' <summary>
        ''' Reply to an RPC call that was rejected by the server
        ''' </summary>
        <[Case](reply_stat.MSG_DENIED)>
        Public rreply As rejected_reply
    End Class
End Namespace
