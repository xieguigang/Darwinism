Imports System

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Cause of authentication failed
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum auth_stat As Integer
        ''' <summary>
        ''' success
        ''' </summary>
        AUTH_OK = 0

        ''' <summary>
        ''' failed at remote end - bad credential (seal broken)
        ''' </summary>
        AUTH_BADCRED = 1

        ''' <summary>
        ''' failed at remote end - client must begin new session
        ''' </summary>
        AUTH_REJECTEDCRED = 2

        ''' <summary>
        ''' failed at remote end - bad verifier (seal broken)
        ''' </summary>
        AUTH_BADVERF = 3

        ''' <summary>
        ''' failed at remote end - verifier expired or replayed
        ''' </summary>
        AUTH_REJECTEDVERF = 4

        ''' <summary>
        ''' failed at remote end - rejected for security reasons
        ''' </summary>
        AUTH_TOOWEAK = 5

        ''' <summary>
        ''' failed locally - bogus response verifier
        ''' </summary>
        AUTH_INVALIDRESP = 6

        ''' <summary>
        ''' failed locally - reason unknown
        ''' </summary>
        AUTH_FAILED = 7

        ''' <summary>
        ''' kerberos generic error
        ''' </summary>
        <Obsolete("http://tools.ietf.org/html/rfc2695")>
        AUTH_KERB_GENERIC = 8

        ''' <summary>
        ''' time of credential expired
        ''' </summary>
        <Obsolete("http://tools.ietf.org/html/rfc2695")>
        AUTH_TIMEEXPIRE = 9

        ''' <summary>
        ''' problem with ticket file
        ''' </summary>
        <Obsolete("http://tools.ietf.org/html/rfc2695")>
        AUTH_TKT_FILE = 10

        ''' <summary>
        ''' can't decode authenticator
        ''' </summary>
        <Obsolete("http://tools.ietf.org/html/rfc2695")>
        AUTH_DECODE = 11

        ''' <summary>
        ''' wrong net address in ticket
        ''' </summary>
        <Obsolete("http://tools.ietf.org/html/rfc2695")>
        AUTH_NET_ADDR = 12

        ''' <summary>
        ''' GSS related errors - no credentials for user
        ''' </summary>
        RPCSEC_GSS_CREDPROBLEM = 13

        ''' <summary>
        ''' GSS related errors - problem with context
        ''' </summary>
        RPCSEC_GSS_CTXPROBLEM = 14
    End Enum
End Namespace
