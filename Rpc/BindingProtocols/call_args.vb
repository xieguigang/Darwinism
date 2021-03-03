Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' arguments to callit
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Class call_args
        ''' <summary>
        ''' program
        ''' </summary>
        <Order(0)>
        Public prog As UInteger

        ''' <summary>
        ''' version
        ''' </summary>
        <Order(1)>
        Public vers As UInteger

        ''' <summary>
        ''' procedure
        ''' </summary>
        <Order(2)>
        Public proc As UInteger

        ''' <summary>
        ''' arguments
        ''' </summary>
        <Order(3), Var>
        Public args As Byte()
    End Class
End Namespace
