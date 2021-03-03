Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' RPC message
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class rpc_msg
        ''' <summary>
        ''' transaction identifier
        ''' </summary>
        <Order(0)>
        Public xid As UInteger
        ''' <summary>
        ''' message body
        ''' </summary>
        <Order(1)>
        Public body As body
    End Class
End Namespace
