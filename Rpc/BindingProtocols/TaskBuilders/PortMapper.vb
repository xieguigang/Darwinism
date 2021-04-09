#Region "Microsoft.VisualBasic::b9ee3eee7ba86ac3d3dc7da89443fc04, Rpc\BindingProtocols\TaskBuilders\PortMapper.vb"

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

    '     Class PortMapper
    ' 
    '         Properties: Version
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: [Set], CallIt, Dump, GetPort, Null
    '                   UnSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections.Generic
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.Data.IO

Namespace Rpc.BindingProtocols.TaskBuilders
    ''' <summary>
    ''' The portmapper program currently supports two protocols (UDP and TCP).  The portmapper is contacted by talking to it on assigned port
    ''' number 111 (SUNRPC) on either of these protocols.
    ''' http://tools.ietf.org/html/rfc1833#section-3.2
    ''' </summary>
    Public NotInheritable Class PortMapper
        Inherits BaseTaskBuilder
        ''' <summary>
        ''' The portmapper program currently supports two protocols (UDP and TCP).  The portmapper is contacted by talking to it on assigned port
        ''' number 111 (SUNRPC) on either of these protocols.
        ''' http://tools.ietf.org/html/rfc1833#section-3.2
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
                Return 2UI
            End Get
        End Property

        ''' <summary>
        ''' This procedure does no work.  By convention, procedure zero of any protocol takes no parameters and returns no results.
        ''' </summary>
        Public Function Null() As Task(Of Xdr.Void)
            Return CreateTask(Of Xdr.Void, Xdr.Void)(0UI, New Xdr.Void())
        End Function

        ''' <summary>
        ''' When a program first becomes available on a machine, it registers itself with the port mapper program on the same machine.  The program
        ''' passes its program number "prog", version number "vers", transport protocol number "prot", and the port "port" on which it awaits
        ''' service request.
        ''' The procedure returns a boolean reply whose value is "TRUE" if the procedure successfully established the mapping and
        ''' "FALSE" otherwise.  The procedure refuses to establish a mapping if one already exists for the tuple "(prog, vers, prot)".
        ''' </summary>
        Public Function [Set](ByVal args As mapping) As Task(Of Boolean)
            Return CreateTask(Of mapping, Boolean)(1UI, args)
        End Function

        ''' <summary>
        ''' When a program becomes unavailable, it should unregister itself with the port mapper program on the same machine.  The parameters and
        ''' results have meanings identical to those of "PMAPPROC_SET".  The protocol and port number fields of the argument are ignored.
        ''' </summary>
        Public Function UnSet(ByVal args As mapping) As Task(Of Boolean)
            Return CreateTask(Of mapping, Boolean)(2UI, args)
        End Function

        ''' <summary>
        ''' Given a program number "prog", version number "vers", and transport protocol number "prot", this procedure returns the port number on
        ''' which the program is awaiting call requests.  A port value of zeros means the program has not been registered.  The "port" field of the
        ''' argument is ignored.
        ''' </summary>
        Public Function GetPort(ByVal args As mapping) As Task(Of UInteger)
            Return CreateTask(Of mapping, UInteger)(3UI, args)
        End Function

        ''' <summary>
        ''' This procedure enumerates all entries in the port mapper's database.
        ''' The procedure takes no parameters and returns a list of program, version, protocol, and port values.
        ''' </summary>
        Public Function Dump() As Task(Of List(Of mapping))
            Return CreateTask(Of Xdr.Void, List(Of mapping))(4UI, New Xdr.Void())
        End Function

        ''' <summary>
        ''' This procedure allows a client to call another remote procedure on the same machine without knowing the remote procedure's port number.
        ''' It is intended for supporting broadcasts to arbitrary remote programs via the well-known port mapper's port.  The parameters "prog",
        ''' "vers", "proc", and the bytes of "args" are the program number, version number, procedure number, and parameters of the remote
        ''' procedure.
        ''' Note:
        ''' (1) This procedure only sends a reply if the procedure was successfully executed and is silent (no reply) otherwise.
        ''' (2) The port mapper communicates with the remote program using UDP only.
        ''' 
        ''' The procedure returns the remote program's port number, and the reply is the reply of the remote procedure.
        ''' </summary>
        Public Function CallIt(ByVal args As call_args) As Task(Of call_result)
            Return CreateTask(Of call_args, call_result)(5UI, args)
        End Function
    End Class
End Namespace
