Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' message body
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class body
        ''' <summary>
        ''' message type
        ''' </summary>
        <Switch>
        Public mtype As msg_type
        ''' <summary>
        ''' call body
        ''' </summary>
        <[Case](msg_type.CALL)>
        Public cbody As call_body
        ''' <summary>
        ''' reply body
        ''' </summary>
        <[Case](msg_type.REPLY)>
        Public rbody As reply_body
    End Class
End Namespace
