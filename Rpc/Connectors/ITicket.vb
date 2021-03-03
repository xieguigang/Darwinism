Imports System
Imports Rpc.MessageProtocol
Imports Xdr

Namespace Rpc.Connectors
    Friend Interface ITicket
        Property Xid As UInteger
        Sub ReadResult(ByVal mr As IMsgReader, ByVal r As Reader, ByVal respMsg As rpc_msg)
        Sub Except(ByVal ex As Exception)
        Sub BuildRpcMessage(ByVal bw As IByteWriter)
    End Interface
End Namespace
