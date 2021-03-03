Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' 
    ''' http://tools.ietf.org/html/rfc5531#section-8.2
    ''' </summary>
    Public Structure opaque_auth
        ''' <summary>
        ''' Null Authentication
        ''' http://tools.ietf.org/html/rfc5531#section-10.1
        ''' </summary>
        Public Shared ReadOnly None As opaque_auth = New opaque_auth() With {
            .flavor = auth_flavor.AUTH_NONE,
            .body = New Byte(-1) {}
        }

        ''' <summary>
        ''' Authentication flavor
        ''' </summary>
        <Order(0)>
        Public flavor As auth_flavor

        ''' <summary>
        ''' The interpretation and semantics of the data contained within the 
        ''' authentication fields are specified by individual, independent
        ''' authentication protocol specifications.
        ''' </summary>
        <Order(1), Var(400)>
        Public body As Byte()
    End Structure
End Namespace
