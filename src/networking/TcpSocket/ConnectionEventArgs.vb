#Region "Microsoft.VisualBasic::a10cda00b9b1c321ffed056301768d01, src\networking\TcpSocket\ConnectionEventArgs.vb"

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
    '    Code Lines: 10 (41.67%)
    ' Comment Lines: 9 (37.50%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 5 (20.83%)
    '     File Size: 735 B


    '     Class ConnectionEventArgs
    ' 
    '         Properties: IpPort, Reason
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket

    ''' <summary>
    ''' Arguments for connection events.
    ''' </summary>
    Public Class ConnectionEventArgs : Inherits EventArgs

        ''' <summary>
        ''' The IP address and port number of the connected peer socket.
        ''' </summary>
        Public ReadOnly Property IpPort As String

        ''' <summary>
        ''' The reason for the disconnection, if any.
        ''' </summary>
        Public ReadOnly Property Reason As DisconnectReason = DisconnectReason.None

        Sub New(ipPort As String, Optional reason As DisconnectReason = DisconnectReason.None)
            Me.IpPort = ipPort
            Me.Reason = reason
        End Sub

    End Class
End Namespace
