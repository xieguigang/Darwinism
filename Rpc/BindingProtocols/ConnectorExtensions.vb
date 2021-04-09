#Region "Microsoft.VisualBasic::e5588b71b9715e18e2f44f39c77e042d, Rpc\BindingProtocols\ConnectorExtensions.vb"

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

    '     Module ConnectorExtensions
    ' 
    '         Function: (+2 Overloads) PortMapper, (+2 Overloads) RpcBindV3, (+2 Overloads) RpcBindV4
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
