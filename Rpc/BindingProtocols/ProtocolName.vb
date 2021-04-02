#Region "Microsoft.VisualBasic::4194e470c8ec3f98103175893a3bcfc4, Rpc\BindingProtocols\ProtocolName.vb"

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

    '     Module ProtocolName
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region


Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Protocol name (rpcb_entry.r_nc_proto): This identifies a protocol within a family.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Module ProtocolName
        ''' <summary>
        ''' no protocol name (-)
        ''' </summary>
        Public Const NC_NOPROTO As String = "-"
        ''' <summary>
        ''' tcp
        ''' </summary>
        Public Const NC_TCP As String = "tcp"
        ''' <summary>
        ''' udp
        ''' </summary>
        Public Const NC_UDP As String = "udp"
        ''' <summary>
        ''' icmp
        ''' </summary>
        Public Const NC_ICMP As String = "icmp"
    End Module
End Namespace

