#Region "Microsoft.VisualBasic::1b374a55b04402085678d834c3d04fce, src\networking\TcpSocket\DisconnectReason.vb"

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


    ' Code Statistics:

    '   Total Lines: 24
    '    Code Lines: 8 (33.33%)
    ' Comment Lines: 15 (62.50%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 1 (4.17%)
    '     File Size: 712 B


    '     Enum DisconnectReason
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket

    ''' <summary>
    ''' Reason why a client disconnected.
    ''' </summary>
    Public Enum DisconnectReason
        ''' <summary>
        ''' Normal disconnection.
        ''' </summary>
        Normal = 0
        ''' <summary>
        ''' Client connection was intentionally terminated programmatically or by the server.
        ''' </summary>
        Kicked = 1
        ''' <summary>
        ''' Client connection timed out; server did not receive data within the timeout window.
        ''' </summary>
        Timeout = 2
        ''' <summary>
        ''' The connection was not disconnected.
        ''' </summary>
        None = 3
    End Enum
End Namespace

