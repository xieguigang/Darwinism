#Region "Microsoft.VisualBasic::1417e64a905bc81363ca8d3c4a998ef6, Rpc\MessageProtocol\rpc_msg.vb"

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

    '     Class rpc_msg
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' RPC message
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class rpc_msg
        ''' <summary>
        ''' transaction identifier
        ''' </summary>
        <Order(0)>
        Public xid As UInteger
        ''' <summary>
        ''' message body
        ''' </summary>
        <Order(1)>
        Public body As body
    End Class
End Namespace

