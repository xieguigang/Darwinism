Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' the lowest and highest version numbers of the remote program supported by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class mismatch_info
        ''' <summary>
        ''' lowest version number
        ''' </summary>
        <Order(0)>
        Public low As UInteger

        ''' <summary>
        ''' highest version number
        ''' </summary>
        <Order(1)>
        Public high As UInteger
    End Class
End Namespace
