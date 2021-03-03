
Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Authentication flavor
    ''' http://tools.ietf.org/html/rfc5531#section-8.2
    ''' </summary>
    Public Enum auth_flavor As Integer
        ''' <summary>
        '''
        ''' </summary>
        AUTH_NONE = 0

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_SYS = 1

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_SHORT = 2

        ''' <summary>
        ''' 
        ''' </summary>
        AUTH_DH = 3

        ''' <summary>
        ''' 
        ''' </summary>
        RPCSEC_GSS = 6
    End Enum
End Namespace
