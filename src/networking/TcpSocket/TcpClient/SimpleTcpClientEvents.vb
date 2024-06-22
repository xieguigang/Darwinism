#Region "Microsoft.VisualBasic::281f6e82866d571ce94ab443233da2c4, src\networking\TcpSocket\TcpClient\SimpleTcpClientEvents.vb"

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

    '   Total Lines: 61
    '    Code Lines: 28 (45.90%)
    ' Comment Lines: 18 (29.51%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 15 (24.59%)
    '     File Size: 1.81 KB


    '     Class SimpleTcpClientEvents
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: HandleClientDisconnected, HandleConnected, HandleDataReceived, HandleDataSent
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket
    ''' <summary>
    ''' SimpleTcp client events.
    ''' </summary>
    Public Class SimpleTcpClientEvents
#Region "Public-Members"

        ''' <summary>
        ''' Event to call when the connection is established.
        ''' </summary>
        Public Event Connected As EventHandler(Of ConnectionEventArgs)

        ''' <summary>
        ''' Event to call when the connection is destroyed.
        ''' </summary>
        Public Event Disconnected As EventHandler(Of ConnectionEventArgs)

        ''' <summary>
        ''' Event to call when byte data has become available from the server.
        ''' </summary>
        Public Event DataReceived As EventHandler(Of DataReceivedEventArgs)

        ''' <summary>
        ''' Event to call when byte data has been sent to the server.
        ''' </summary>
        Public Event DataSent As EventHandler(Of DataSentEventArgs)

#End Region

#Region "Constructors-and-Factories"

        ''' <summary>
        ''' Instantiate the object.
        ''' </summary>
        Public Sub New()

        End Sub

#End Region

#Region "Public-Methods"

        Friend Sub HandleConnected(sender As Object, args As ConnectionEventArgs)
            RaiseEvent Connected(sender, args)
        End Sub

        Friend Sub HandleClientDisconnected(sender As Object, args As ConnectionEventArgs)
            RaiseEvent Disconnected(sender, args)
        End Sub

        Friend Sub HandleDataReceived(sender As Object, args As DataReceivedEventArgs)
            RaiseEvent DataReceived(sender, args)
        End Sub

        Friend Sub HandleDataSent(sender As Object, args As DataSentEventArgs)
            RaiseEvent DataSent(sender, args)
        End Sub

#End Region
    End Class
End Namespace

