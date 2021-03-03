
Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Transport semantics (rpcb_entry.r_nc_semantics): This represents the type of transport.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Module TransportSemantics
        ''' <summary>
        ''' Connectionless
        ''' </summary>
        Public Const NC_TPI_CLTS As ULong = 1
        ''' <summary>
        ''' Connection oriented
        ''' </summary>
        Public Const NC_TPI_COTS As ULong = 2
        ''' <summary>
        ''' Connection oriented with graceful close
        ''' </summary>
        Public Const NC_TPI_COTS_ORD As ULong = 3
        ''' <summary>
        ''' Raw transport
        ''' </summary>
        Public Const NC_TPI_RAW As ULong = 4
    End Module
End Namespace
