#Region "Microsoft.VisualBasic::0f50fb6c9812d218fb61a417db9f1a23, Rpc\MessageProtocol\msg_type.vb"

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

    '     Enum msg_type
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
    ''' message type
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum msg_type As Integer
        ''' <summary>
        ''' call message
        ''' </summary>
        [CALL] = 0
        ''' <summary>
        ''' reply message
        ''' </summary>
        REPLY = 1
    End Enum
End Namespace
