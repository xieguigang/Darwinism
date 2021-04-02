#Region "Microsoft.VisualBasic::91fb29d3a9316ef2d51063e5cdcf81b7, Rpc\MessageProtocol\rejected_reply.vb"

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

    '     Class rejected_reply
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Reply to an RPC call that was rejected by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class rejected_reply
        ''' <summary>
        ''' Reasons why a call message was rejected
        ''' </summary>
        <Switch>
        Public rstat As reject_stat

        ''' <summary>
        ''' the lowest and highest version numbers of the remote program supported by the server
        ''' </summary>
        <[Case](reject_stat.RPC_MISMATCH)>
        Public mismatch_info As mismatch_info

        ''' <summary>
        ''' the server rejects the identity of the caller
        ''' </summary>
        <[Case](reject_stat.AUTH_ERROR)>
        Public astat As auth_stat
    End Class
End Namespace

