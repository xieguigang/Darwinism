#Region "Microsoft.VisualBasic::287da013d27904029df82d3c839bc50a, Distribute_computing\ManagerInterface\SocketHost\InterfaceSocket.vb"

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

    ' Class InterfaceSocket
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Sub: InterfaceSocket_onClientBinaryMessage, InterfaceSocket_onClientDisconnect, InterfaceSocket_onClientTextMessage
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Net.Sockets
Imports SMRUCC.WebCloud.HTTPInternal.Core.WebSocket

Public Class InterfaceSocket : Inherits WsProcessor

    Public Sub New(tcp As TcpClient)
        MyBase.New(tcp)
    End Sub

    Private Sub InterfaceSocket_onClientBinaryMessage(sender As WsProcessor, data As MemoryStream, responseStream As NetworkStream) Handles Me.onClientBinaryMessage

    End Sub

    Private Sub InterfaceSocket_onClientTextMessage(sender As WsProcessor, data As String, responseStream As NetworkStream) Handles Me.onClientTextMessage

    End Sub

    Private Sub InterfaceSocket_onClientDisconnect(sender As Object) Handles Me.onClientDisconnect

    End Sub
End Class

