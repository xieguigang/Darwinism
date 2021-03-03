
Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Reasons why a call message was rejected
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum reject_stat As Integer
        ''' <summary>
        ''' RPC version number != 2
        ''' </summary>
        RPC_MISMATCH = 0
        ''' <summary>
        ''' remote can't authenticate caller
        ''' </summary>
        AUTH_ERROR = 1
    End Enum
End Namespace
