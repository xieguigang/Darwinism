Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' contains a merged address of a service on a particular transport, plus associated netconfig information.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_entry
        ''' <summary>
        ''' merged address of service
        ''' </summary>
        <Order(0), Var>
        Public r_maddr As String
        ''' <summary>
        ''' The network identifier: This is a string that represents a local identification for a network.
        ''' This is defined by a system administrator based on local conventions, and cannot be depended on to have the same value on every system.
        ''' </summary>
        <Order(1), Var>
        Public r_nc_netid As String
        ''' <summary>
        ''' semantics of transport (see conctants of <seecref="Rpc.BindingProtocols.TransportSemantics"/>)
        ''' </summary>
        <Order(2)>
        Public r_nc_semantics As ULong
        ''' <summary>
        ''' protocol family (see conctants of <seecref="Rpc.BindingProtocols.ProtocolFamily"/>)
        ''' </summary>
        <Order(3), Var>
        Public r_nc_protofmly As String
        ''' <summary>
        ''' protocol name (see conctants of <seecref="Rpc.BindingProtocols.ProtocolName"/>)
        ''' </summary>
        <Order(4), Var>
        Public r_nc_proto As String
    End Class
End Namespace
