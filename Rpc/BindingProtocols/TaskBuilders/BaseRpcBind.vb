#Region "Microsoft.VisualBasic::f1b3521b15d77b06df943a11d2d828cc, Rpc\BindingProtocols\TaskBuilders\BaseRpcBind.vb"

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

'     Class BaseRpcBind
' 
'         Constructor: (+1 Overloads) Sub New
'         Function: [Set], Dump, GetAddr, GetTime, TAddr2Uaddr
'                   UAddr2TAddr, UnSet
' 
' 
' /********************************************************************************/

#End Region

Imports System.Collections.Generic
Imports System.IO.XDR
Imports System.Threading
Imports System.Threading.Tasks

Namespace Rpc.BindingProtocols.TaskBuilders
    ''' <summary>
    ''' RPCBIND is contacted by way of an assigned address specific to the transport being used.
    ''' For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
    ''' http://tools.ietf.org/html/rfc1833#section-2.2.1
    ''' </summary>
    Public MustInherit Class BaseRpcBind
        Inherits BaseTaskBuilder

        Friend Sub New(conn As IRpcClient, token As CancellationToken, attachedToParent As Boolean)
            MyBase.New(conn, token, attachedToParent)
        End Sub

        ''' <summary>
        ''' When a program first becomes available on a machine, it registers itself with RPCBIND running on the same machine.
        ''' The program passes its program number "r_prog", version number "r_vers", network identifier "r_netid", universal address "r_addr",
        ''' and the owner of the service "r_owner".
        ''' The procedure returns a boolean response whose value is TRUE if the procedure successfully established the mapping and FALSE otherwise.
        ''' The procedure refuses to establish a mapping if one already exists for the ordered set ("r_prog", "r_vers", "r_netid").
        ''' Note that neither "r_netid" nor "r_addr" can be NULL, and that "r_netid" should be a valid network identifier on the machine making the call.
        ''' </summary>
        Public Function [Set](arg As rpcb) As Task(Of Boolean)
            Return CreateTask(Of rpcb, Boolean)(1UI, arg)
        End Function

        ''' <summary>
        ''' When a program becomes unavailable, it should unregister itself with the RPCBIND program on the same machine.
        ''' The parameters and results have meanings identical to those of RPCBPROC_SET.
        ''' The mapping of the ("r_prog", "r_vers", "r_netid") tuple with "r_addr" is deleted.
        ''' If "r_netid" is NULL, all mappings specified by the ordered set ("r_prog", "r_vers", *) and the corresponding universal addresses are deleted.
        ''' Only the owner of the service or the super-user is allowed to unset a service
        ''' </summary>
        Public Function UnSet(arg As rpcb) As Task(Of Boolean)
            Return CreateTask(Of rpcb, Boolean)(2UI, arg)
        End Function

        ''' <summary>
        ''' Given a program number "r_prog", version number "r_vers", and network identifier  "r_netid", this procedure returns the universal address
        ''' on which the program is awaiting call requests.  The "r_netid" field of the argument is ignored and the "r_netid" is inferred from the
        ''' network identifier of the transport on which the request came in.
        ''' </summary>
        Public Function GetAddr(arg As rpcb) As Task(Of String)
            Return CreateTask(Of rpcb, String)(3UI, arg)
        End Function

        ''' <summary>
        ''' This procedure lists all entries in RPCBIND's database.
        ''' The procedure takes no parameters and returns a list of program, version, network identifier, and universal addresses.
        ''' </summary>
        Public Function Dump() As Task(Of List(Of rpcb))
            Return CreateTask(Of Void, List(Of rpcb))(4UI, New Void())
        End Function

        ''' <summary>
        ''' This procedure returns the local time on its own machine in seconds
        ''' since the midnight of the First day of January, 1970. 
        ''' </summary>
        Public Function GetTime() As Task(Of UInteger)
            Return CreateTask(Of Void, UInteger)(6UI, New Void())
        End Function

        ''' <summary>
        ''' This procedure converts universal addresses to transport specific addresses.
        ''' </summary>
        Public Function UAddr2TAddr(arg As String) As Task(Of netbuf)
            Return CreateTask(Of String, netbuf)(7UI, arg)
        End Function

        ''' <summary>
        ''' This procedure converts transport specific addresses to universal addresses.
        ''' </summary>
        Public Function TAddr2Uaddr(arg As netbuf) As Task(Of String)
            Return CreateTask(Of netbuf, String)(8UI, arg)
        End Function
    End Class
End Namespace
