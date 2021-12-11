#Region "Microsoft.VisualBasic::4fbae78d156727bb507c8f4bd3319cb3, Rpc\MessageProtocol\reply_body.vb"

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

'     Class reply_body
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Body of a reply to an RPC call
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class reply_body
        ''' <summary>
        ''' A reply to a call message can take on two forms: the message was either accepted or rejected.
        ''' </summary>
        <Switch>
        Public stat As reply_stat

        ''' <summary>
        ''' Reply to an RPC call that was accepted by the server
        ''' </summary>
        <[Case](reply_stat.MSG_ACCEPTED)>
        Public areply As accepted_reply

        ''' <summary>
        ''' Reply to an RPC call that was rejected by the server
        ''' </summary>
        <[Case](reply_stat.MSG_DENIED)>
        Public rreply As rejected_reply
    End Class
End Namespace
