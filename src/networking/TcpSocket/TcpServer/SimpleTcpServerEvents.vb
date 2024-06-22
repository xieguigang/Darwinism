#Region "Microsoft.VisualBasic::b17c982e850a2e68c84572de71a53773, src\networking\TcpSocket\TcpServer\SimpleTcpServerEvents.vb"

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
    '     File Size: 1.82 KB


    '     Class SimpleTcpServerEvents
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: HandleClientConnected, HandleClientDisconnected, HandleDataReceived, HandleDataSent
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket
    ''' <summary>
    ''' SimpleTcp server events.
    ''' </summary>
    Public Class SimpleTcpServerEvents
#Region "Public-Members"

        ''' <summary>
        ''' Event to call when a client connects.
        ''' </summary>
        Public Event ClientConnected As EventHandler(Of ConnectionEventArgs)

        ''' <summary>
        ''' Event to call when a client disconnects.
        ''' </summary>
        Public Event ClientDisconnected As EventHandler(Of ConnectionEventArgs)

        ''' <summary>
        ''' Event to call when byte data has become available from the client.
        ''' </summary>
        Public Event DataReceived As EventHandler(Of DataReceivedEventArgs)

        ''' <summary>
        ''' Event to call when byte data has been sent to a client.
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

        Friend Sub HandleClientConnected(sender As Object, args As ConnectionEventArgs)
            RaiseEvent ClientConnected(sender, args)
        End Sub

        Friend Sub HandleClientDisconnected(sender As Object, args As ConnectionEventArgs)
            RaiseEvent ClientDisconnected(sender, args)
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
