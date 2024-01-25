#Region "Microsoft.VisualBasic::cb4b4952e0eafdafc056b31e09294801, Rpc\MessageProtocol\reject_stat.vb"

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

    '     Enum reject_stat
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
    ''' Reasons why a call message was rejected
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Enum reject_stat As Integer
        ''' <summary>
        ''' RPC version number != 2
        ''' </summary>
        RPC_MISMATCH = 0
        ''' <summary>
        ''' remote can't authenticate caller
        ''' </summary>
        AUTH_ERROR = 1
    End Enum
End Namespace
