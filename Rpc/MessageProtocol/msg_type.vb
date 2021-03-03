
Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' message type
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum msg_type As Integer
        ''' <summary>
        ''' call message
        ''' </summary>
        [CALL] = 0
        ''' <summary>
        ''' reply message
        ''' </summary>
        REPLY = 1
    End Enum
End Namespace
