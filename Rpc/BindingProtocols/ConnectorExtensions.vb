Imports System.Threading
Imports Rpc.BindingProtocols.TaskBuilders
Imports System.Runtime.CompilerServices

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Connector extensions.
    ''' </summary>
    Public Module ConnectorExtensions
        ''' <summary>
        ''' The portmapper program currently supports two protocols (UDP and TCP).  The portmapper is contacted by talking to it on assigned port
        ''' number 111 (SUNRPC) on either of these protocols.
        ''' http://tools.ietf.org/html/rfc1833#section-3.2
        ''' </summary>
        <Extension()>
        Public Function PortMapper(ByVal conn As IRpcClient, ByVal token As CancellationToken, ByVal Optional attachedToParent As Boolean = False) As PortMapper
            Return New PortMapper(conn, token, attachedToParent)
        End Function

        ''' <summary>
        ''' The portmapper program currently supports two protocols (UDP and TCP).  The portmapper is contacted by talking to it on assigned port
        ''' number 111 (SUNRPC) on either of these protocols.
        ''' http://tools.ietf.org/html/rfc1833#section-3.2
        ''' </summary>
        <Extension()>
        Public Function PortMapper(ByVal conn As IRpcClient, ByVal Optional attachedToParent As Boolean = False) As PortMapper
            Return New PortMapper(conn, CancellationToken.None, attachedToParent)
        End Function

        ''' <summary>
        ''' RPCBIND Version 3
        ''' 
        ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
        ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
        ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
        ''' </summary>
        <Extension()>
        Public Function RpcBindV3(ByVal conn As IRpcClient, ByVal token As CancellationToken, ByVal Optional attachedToParent As Boolean = False) As RpcBindV3
            Return New RpcBindV3(conn, token, attachedToParent)
        End Function

        ''' <summary>
        ''' RPCBIND Version 3
        ''' 
        ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
        ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
        ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
        ''' </summary>
        <Extension()>
        Public Function RpcBindV3(ByVal conn As IRpcClient, ByVal Optional attachedToParent As Boolean = False) As RpcBindV3
            Return New RpcBindV3(conn, CancellationToken.None, attachedToParent)
        End Function

        ''' <summary>
        ''' RPCBIND Version 4
        ''' 
        ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
        ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
        ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
        ''' </summary>
        <Extension()>
        Public Function RpcBindV4(ByVal conn As IRpcClient, ByVal token As CancellationToken, ByVal Optional attachedToParent As Boolean = False) As RpcBindV4
            Return New RpcBindV4(conn, token, attachedToParent)
        End Function

        ''' <summary>
        ''' RPCBIND Version 4
        ''' 
        ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
        ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
        ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
        ''' </summary>
        <Extension()>
        Public Function RpcBindV4(ByVal conn As IRpcClient, ByVal Optional attachedToParent As Boolean = False) As RpcBindV4
            Return New RpcBindV4(conn, CancellationToken.None, attachedToParent)
        End Function
    End Module
End Namespace
