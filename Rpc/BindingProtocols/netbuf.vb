Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' netbuf structure, used to store the transport specific form of a universal transport address.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class netbuf
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(0)>
        Public maxlen As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1), Var>
        Public buf As Byte()
    End Class
End Namespace
