﻿#Region "Microsoft.VisualBasic::c45b7b8efa66bdc94277dc10e6efce06, Rpc\BindingProtocols\ProtocolFamily.vb"

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

    '     Module ProtocolFamily
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Protocol family (rpcb_entry.r_nc_protofmly): This identifies the family to which the protocol belongs.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Module ProtocolFamily
        ''' <summary>
        ''' no protocol family (-)
        ''' NC_NOPROTOFMLY
        ''' </summary>
        Public Const NC_NOPROTOFMLY As String = "-"
        ''' <summary>
        ''' loopback
        ''' </summary>
        Public Const NC_LOOPBACK As String = "loopback"
        ''' <summary>
        ''' inet
        ''' </summary>
        Public Const NC_INET As String = "inet"
        ''' <summary>
        ''' implink
        ''' </summary>
        Public Const NC_IMPLINK As String = "implink"
        ''' <summary>
        ''' pup
        ''' </summary>
        Public Const NC_PUP As String = "pup"
        ''' <summary>
        ''' chaos
        ''' </summary>
        Public Const NC_CHAOS As String = "chaos"
        ''' <summary>
        ''' ns
        ''' </summary>
        Public Const NC_NS As String = "ns"
        ''' <summary>
        ''' nbs
        ''' </summary>
        Public Const NC_NBS As String = "nbs"
        ''' <summary>
        ''' ecma
        ''' </summary>
        Public Const NC_ECMA As String = "ecma"
        ''' <summary>
        ''' datakit
        ''' </summary>
        Public Const NC_DATAKIT As String = "datakit"
        ''' <summary>
        ''' ccitt
        ''' </summary>
        Public Const NC_CCITT As String = "ccitt"
        ''' <summary>
        ''' sna
        ''' </summary>
        Public Const NC_SNA As String = "sna"
        ''' <summary>
        ''' decnet
        ''' </summary>
        Public Const NC_DECNET As String = "decnet"
        ''' <summary>
        ''' dli
        ''' </summary>
        Public Const NC_DLI As String = "dli"
        ''' <summary>
        ''' lat
        ''' </summary>
        Public Const NC_LAT As String = "lat"
        ''' <summary>
        ''' hylink
        ''' </summary>
        Public Const NC_HYLINK As String = "hylink"
        ''' <summary>
        ''' appletalk
        ''' </summary>
        Public Const NC_APPLETALK As String = "appletalk"
        ''' <summary>
        ''' nit
        ''' </summary>
        Public Const NC_NIT As String = "nit"
        ''' <summary>
        ''' ieee802
        ''' </summary>
        Public Const NC_IEEE802 As String = "ieee802"
        ''' <summary>
        ''' osi
        ''' </summary>
        Public Const NC_OSI As String = "osi"
        ''' <summary>
        ''' x25
        ''' </summary>
        Public Const NC_X25 As String = "x25"
        ''' <summary>
        ''' osinet
        ''' </summary>
        Public Const NC_OSINET As String = "osinet"
        ''' <summary>
        ''' gosip
        ''' </summary>
        Public Const NC_GOSIP As String = "gosip"
    End Module
End Namespace
