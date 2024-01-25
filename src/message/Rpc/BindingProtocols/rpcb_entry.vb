#Region "Microsoft.VisualBasic::c8445eafe71d16b729cff7dc680113ac, Rpc\BindingProtocols\rpcb_entry.vb"

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

'     Class rpcb_entry
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

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
        ''' semantics of transport (see conctants of <see cref="Rpc.BindingProtocols.TransportSemantics"/>)
        ''' </summary>
        <Order(2)>
        Public r_nc_semantics As ULong
        ''' <summary>
        ''' protocol family (see conctants of <see cref="Rpc.BindingProtocols.ProtocolFamily"/>)
        ''' </summary>
        <Order(3), Var>
        Public r_nc_protofmly As String
        ''' <summary>
        ''' protocol name (see conctants of <see cref="Rpc.BindingProtocols.ProtocolName"/>)
        ''' </summary>
        <Order(4), Var>
        Public r_nc_proto As String
    End Class
End Namespace
