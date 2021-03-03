Imports Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' A mapping of (program, version, network ID) to address.
    ''' The network identifier (r_netid): This is a string that represents a local identification for a network.
    ''' This is defined by a system administrator based on local conventions, and cannot be depended on to have
    ''' the same value on every system.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Structure rpcb
        ''' <summary>
        ''' program number
        ''' </summary>
        <Order(0)>
        Public r_prog As UInteger
        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(1)>
        Public r_vers As UInteger
        ''' <summary>
        ''' network id 
        ''' </summary>
        <Order(2), Var>
        Public r_netid As String
        ''' <summary>
        ''' universal address
        ''' </summary>
        <Order(3), Var>
        Public r_addr As String
        ''' <summary>
        ''' owner of this service
        ''' </summary>
        <Order(4), Var>
        Public r_owner As String
    End Structure
End Namespace
