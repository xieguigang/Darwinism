
Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Given that a call message was accepted, the following is the status of an attempt to call a remote procedure.
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum accept_stat As Integer
        ''' <summary>
        ''' RPC executed successfully
        ''' </summary>
        SUCCESS = 0

        ''' <summary>
        ''' remote hasn't exported program
        ''' </summary>
        PROG_UNAVAIL = 1

        ''' <summary>
        ''' remote can't support version # 
        ''' </summary>
        PROG_MISMATCH = 2

        ''' <summary>
        ''' program can't support procedure
        ''' </summary>
        PROC_UNAVAIL = 3

        ''' <summary>
        ''' procedure can't decode params
        ''' </summary>
        GARBAGE_ARGS = 4

        ''' <summary>
        ''' e.g. memory allocation failure
        ''' </summary>
        SYSTEM_ERR = 5
    End Enum
End Namespace
