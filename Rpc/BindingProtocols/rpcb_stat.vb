Imports Xdr
Imports System.Collections.Generic

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' rpcbind statistics
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_stat
        ''' <summary>
        ''' # of procs in rpcbind V4 plus one
        ''' </summary>
        Public Const RPCBSTAT_HIGHPROC As UInteger = 13

        ''' <summary>
        ''' number of procedure calls by numbers
        ''' </summary>
        <Order(0), Fix(RPCBSTAT_HIGHPROC)>
        Public info As Integer()
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1)>
        Public setinfo As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(2)>
        Public unsetinfo As Integer
        ''' <summary>
        ''' list of all the stats about getport and getaddr
        ''' </summary>
        <Order(3)>
        Public addrinfo As List(Of rpcbs_addr)
        ''' <summary>
        ''' list of all the stats about rmtcall
        ''' </summary>
        <Order(4)>
        Public rmtinfo As List(Of rpcbs_rmtcall)
    End Class
End Namespace
