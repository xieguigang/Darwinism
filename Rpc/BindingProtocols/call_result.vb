Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Results of callit
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Class call_result
        ''' <summary>
        ''' port of called program
        ''' </summary>
        <Order(0)>
        Public port As UInteger

        ''' <summary>
        ''' result
        ''' </summary>
        <Order(1), Var>
        Public res As Byte()
    End Class
End Namespace
