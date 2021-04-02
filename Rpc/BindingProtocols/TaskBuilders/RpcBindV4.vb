#Region "Microsoft.VisualBasic::8a0a38428b215a5424c73b8470a8281d, Rpc\BindingProtocols\TaskBuilders\RpcBindV4.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class RpcBindV4
    ' 
    '         Properties: Version
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: BCast, GetAddrList, GetStat, GetVersAddr, Indirect
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections.Generic
Imports System.Threading
Imports System.Threading.Tasks

Namespace Rpc.BindingProtocols.TaskBuilders
    ''' <summary>
    ''' RPCBIND Version 4
    ''' 
    ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
    ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
    ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
    ''' </summary>
    Public NotInheritable Class RpcBindV4
        Inherits BaseRpcBind
        ''' <summary>
        ''' RPCBIND Version 4
        ''' 
        ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
        ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
        ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
        ''' </summary>
        ''' <paramname="conn">instance of connector</param>
        ''' <paramname="token">cancellation token</param>
        ''' <paramname="attachedToParent">attache created task to parent task</param>
        Public Sub New(ByVal conn As IRpcClient, ByVal token As CancellationToken, ByVal attachedToParent As Boolean)
            MyBase.New(conn, token, attachedToParent)
        End Sub

        ''' <summary>
        ''' Gets the version of protocol
        ''' </summary>
        Protected Overrides ReadOnly Property Version As UInteger
            Get
                Return 4UI
            End Get
        End Property

        ''' <summary>
        ''' This procedure is identical to the version 3 RPCBPROC_CALLIT procedure.  The new name indicates that the procedure should be used
        ''' for broadcast RPCs only.  RPCBPROC_INDIRECT, defined below, should be used for indirect RPC calls.
        ''' </summary>
        Public Function BCast(ByVal arg As rpcb_rmtcallargs) As Task(Of rpcb_rmtcallres)
            Return CreateTask(Of rpcb_rmtcallargs, rpcb_rmtcallres)(5UI, arg)
        End Function

        ''' <summary>
        ''' This procedure is similar to RPCBPROC_GETADDR. The difference is the "r_vers" field of the rpcb structure can be used to specify the
        ''' version of interest.  If that version is not registered, no address is returned.
        ''' </summary>
        Public Function GetVersAddr(ByVal arg As rpcb) As Task(Of String)
            Return CreateTask(Of rpcb, String)(9UI, arg)
        End Function

        ''' <summary>
        ''' Similar to RPCBPROC_CALLIT. Instead of being silent about errors (such as the program not being registered on the system), this
        ''' procedure returns an indication of the error.  This procedure should not be used for broadcast RPC. It is intended to be used with
        ''' indirect RPC calls only.
        ''' </summary>
        Public Function Indirect(ByVal arg As rpcb_rmtcallargs) As Task(Of rpcb_rmtcallres)
            Return CreateTask(Of rpcb_rmtcallargs, rpcb_rmtcallres)(10UI, arg)
        End Function

        ''' <summary>
        ''' This procedure returns a list of addresses for the given rpcb entry.
        ''' The client may be able use the results to determine alternate transports that it can use to communicate with the server.
        ''' </summary>
        Public Function GetAddrList(ByVal arg As rpcb) As Task(Of List(Of rpcb_entry))
            Return CreateTask(Of rpcb, List(Of rpcb_entry))(11UI, arg)
        End Function

        ''' <summary>
        ''' This procedure returns statistics on the activity of the RPCBIND server.  The information lists the number and kind of requests the
        ''' server has received.
        ''' 
        ''' Note - All procedures except RPCBPROC_SET and RPCBPROC_UNSET can be called by clients running on a machine other than a machine on which
        ''' RPCBIND is running.  RPCBIND only accepts RPCBPROC_SET and RPCBPROC_UNSET requests by clients running on the same machine as the
        ''' RPCBIND program.
        ''' </summary>
        Public Function GetStat() As Task(Of rpcb_stat_byvers)
            Return CreateTask(Of Xdr.Void, rpcb_stat_byvers)(12UI, New Xdr.Void())
        End Function
    End Class
End Namespace

