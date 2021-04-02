#Region "Microsoft.VisualBasic::27db7c8e9e48f67ab1be006a2cf35a0d, Rpc\MessageProtocol\reply_stat.vb"

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

    '     Enum reply_stat
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region


Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' A reply to a call message can take on two forms: the message was either accepted or rejected.
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum reply_stat As Integer
        ''' <summary>
        ''' the message was accepted
        ''' </summary>
        MSG_ACCEPTED = 0
        ''' <summary>
        ''' the message was rejected
        ''' </summary>
        MSG_DENIED = 1
    End Enum
End Namespace

