Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' the stat about getport and getaddr
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcbs_addr
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(0)>
        Public prog As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1)>
        Public vers As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(2)>
        Public success As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(3)>
        Public failure As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(4), Var>
        Public netid As String
    End Class
End Namespace
