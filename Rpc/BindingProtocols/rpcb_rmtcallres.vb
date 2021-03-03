Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Results of the remote call
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_rmtcallres
        ''' <summary>
        ''' remote universal address
        ''' </summary>
        <Order(0), Var>
        Public addr As String
        ''' <summary>
        ''' result
        ''' </summary>
        <Order(1), Var>
        Public results As Byte()
    End Class
End Namespace
