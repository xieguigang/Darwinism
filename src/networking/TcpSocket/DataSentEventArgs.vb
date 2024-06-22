#Region "Microsoft.VisualBasic::610fc855a7d3549b693a5362a02b024b, src\networking\TcpSocket\DataSentEventArgs.vb"

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

    '   Total Lines: 23
    '    Code Lines: 10 (43.48%)
    ' Comment Lines: 9 (39.13%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (17.39%)
    '     File Size: 661 B


    '     Class DataSentEventArgs
    ' 
    '         Properties: BytesSent, IpPort
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket

    ''' <summary>
    ''' Arguments for data sent to a connected endpoint.
    ''' </summary>
    Public Class DataSentEventArgs : Inherits EventArgs

        ''' <summary>
        ''' The IP address and port number of the connected endpoint.
        ''' </summary>
        Public ReadOnly Property IpPort As String

        ''' <summary>
        ''' The number of bytes sent.
        ''' </summary>
        Public ReadOnly Property BytesSent As Long

        Sub New(ipPort As String, bytesSent As Long)
            Me.IpPort = ipPort
            Me.BytesSent = bytesSent
        End Sub
    End Class
End Namespace

