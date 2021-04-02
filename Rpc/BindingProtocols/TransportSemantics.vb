#Region "Microsoft.VisualBasic::f74e13a226a5afa9b873abd8be02b82d, Rpc\BindingProtocols\TransportSemantics.vb"

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

    '     Module TransportSemantics
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region


Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Transport semantics (rpcb_entry.r_nc_semantics): This represents the type of transport.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Module TransportSemantics
        ''' <summary>
        ''' Connectionless
        ''' </summary>
        Public Const NC_TPI_CLTS As ULong = 1
        ''' <summary>
        ''' Connection oriented
        ''' </summary>
        Public Const NC_TPI_COTS As ULong = 2
        ''' <summary>
        ''' Connection oriented with graceful close
        ''' </summary>
        Public Const NC_TPI_COTS_ORD As ULong = 3
        ''' <summary>
        ''' Raw transport
        ''' </summary>
        Public Const NC_TPI_RAW As ULong = 4
    End Module
End Namespace

