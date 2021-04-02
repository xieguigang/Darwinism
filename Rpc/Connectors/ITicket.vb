#Region "Microsoft.VisualBasic::16498068f3f11e71d3e79060915c9c87, Rpc\Connectors\ITicket.vb"

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

    '     Interface ITicket
    ' 
    '         Properties: Xid
    ' 
    '         Sub: BuildRpcMessage, Except, ReadResult
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports Rpc.MessageProtocol
Imports Xdr

Namespace Rpc.Connectors
    Friend Interface ITicket
        Property Xid As UInteger
        Sub ReadResult(ByVal mr As IMsgReader, ByVal r As Reader, ByVal respMsg As rpc_msg)
        Sub Except(ByVal ex As Exception)
        Sub BuildRpcMessage(ByVal bw As IByteWriter)
    End Interface
End Namespace

