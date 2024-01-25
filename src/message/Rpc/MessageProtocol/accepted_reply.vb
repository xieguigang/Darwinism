#Region "Microsoft.VisualBasic::e01a4d61633bd5d6b1e005f89c152c05, Rpc\MessageProtocol\accepted_reply.vb"

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

'     Class accepted_reply
' 
' 
'         Class reply_data_union
' 
' 
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Reply to an RPC call that was accepted by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class accepted_reply
        ''' <summary>
        ''' authentication verifier that the server generates in order to validate itself to the client
        ''' </summary>
        <Order(0)>
        Public verf As opaque_auth

        ''' <summary>
        ''' the reply data.
        ''' </summary>
        <Order(1)>
        Public reply_data As reply_data_union

        ''' <summary>
        ''' the reply data
        ''' </summary>
        Public Class reply_data_union
            ''' <summary>
            ''' accept state
            ''' </summary>
            <Switch>
            <[Case](accept_stat.SUCCESS)> ' opaque results[0]; -  procedure-specific results start here
            <[Case](accept_stat.PROG_UNAVAIL), [Case](accept_stat.PROC_UNAVAIL), [Case](accept_stat.GARBAGE_ARGS), [Case](accept_stat.SYSTEM_ERR)> ' void
            Public stat As accept_stat

            ''' <summary>
            ''' the lowest and highest version numbers of the remote program supported by the server
            ''' </summary>
            <[Case](accept_stat.PROG_MISMATCH)>
            Public mismatch_info As mismatch_info
        End Class
    End Class
End Namespace
