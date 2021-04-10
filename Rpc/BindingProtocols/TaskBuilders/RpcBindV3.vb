#Region "Microsoft.VisualBasic::260d607f3869f6875a6192de18fed731, Rpc\BindingProtocols\TaskBuilders\RpcBindV3.vb"

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

    '     Class RpcBindV3
    ' 
    '         Properties: Version
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CallIt
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Threading
Imports System.Threading.Tasks

Namespace Rpc.BindingProtocols.TaskBuilders
    ''' <summary>
    ''' RPCBIND Version 3
    ''' 
    ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
    ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
    ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
    ''' </summary>
    Public NotInheritable Class RpcBindV3
        Inherits BaseRpcBind
        ''' <summary>
        ''' RPCBIND Version 3
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
                Return 3UI
            End Get
        End Property

        ''' <summary>
        ''' This procedure allows a caller to call another remote procedure on the same machine without knowing the remote procedure's universal
        ''' address.  It is intended for supporting broadcasts to arbitrary remote programs via RPCBIND's universal address.  The parameters
        ''' "prog", "vers", "proc", and args are the program number, version number, procedure number, and parameters of the remote procedure.
        ''' Note - This procedure only sends a response if the procedure was successfully executed and is silent (no response) otherwise.
        ''' The procedure returns the remote program's universal address, and the results of the remote procedure.
        ''' </summary>
        Public Function CallIt(ByVal arg As rpcb_rmtcallargs) As Task(Of rpcb_rmtcallres)
            Return CreateTask(Of rpcb_rmtcallargs, rpcb_rmtcallres)(5UI, arg)
        End Function
    End Class
End Namespace
