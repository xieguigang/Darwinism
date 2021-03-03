Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Body of an RPC call
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class call_body
        ''' <summary>
        ''' MUST be equal to 2
        ''' </summary>
        <Order(0)>
        Public rpcvers As UInteger

        ''' <summary>
        ''' the remote program
        ''' </summary>
        <Order(1)>
        Public prog As UInteger

        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(2)>
        Public vers As UInteger

        ''' <summary>
        ''' the procedure within the remote program to be called
        ''' </summary>
        <Order(3)>
        Public proc As UInteger

        ''' <summary>
        ''' authentication credential
        ''' </summary>
        <Order(4)>
        Public cred As opaque_auth

        ''' <summary>
        ''' authentication verifier
        ''' </summary>
        <Order(5)>
        Public verf As opaque_auth
    End Class
End Namespace
