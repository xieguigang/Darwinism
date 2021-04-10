#Region "Microsoft.VisualBasic::3fd0c1078a5c1616f65bdb2d1c57de97, Rpc\BindingProtocols\Protocol.vb"

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

    '     Enum Protocol
    ' 
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' constants of port mapper protocol
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Enum Protocol As Integer
        ''' <summary>
        ''' protocol number for TCP/IP
        ''' </summary>
        TCP = 6
        ''' <summary>
        ''' protocol number for UDP/IP
        ''' </summary>
        UDP = 17
    End Enum
End Namespace
