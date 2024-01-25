#Region "Microsoft.VisualBasic::6497e581883477f797cd1dcde4296a11, Rpc\BindingProtocols\rpcb.vb"

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

'     Structure rpcb
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

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
