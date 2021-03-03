Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Arguments of remote calls
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_rmtcallargs
        ''' <summary>
        ''' program number
        ''' </summary>
        <Order(0)>
        Public prog As ULong
        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(1)>
        Public vers As ULong
        ''' <summary>
        ''' procedure number
        ''' </summary>
        <Order(2)>
        Public proc As ULong
        ''' <summary>
        ''' argument
        ''' </summary>
        <Order(3), Var>
        Public args As Byte()
    End Class
End Namespace
