#Region "Microsoft.VisualBasic::ebf14ac242dfc3181792bbf3e458c397, src\networking\TcpSocket\ClientMetadata.vb"

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

    '   Total Lines: 98
    '    Code Lines: 74 (75.51%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 24 (24.49%)
    '     File Size: 2.67 KB


    '     Class ClientMetadata
    ' 
    '         Properties: Client, IpPort, NetworkStream, SslStream, Token
    '                     TokenSource
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: Dispose
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Threading

Namespace TcpSocket

    Friend Class ClientMetadata : Implements IDisposable
#Region "Public-Members"

        Friend ReadOnly Property Client As TcpClient
            Get
                Return _tcpClient
            End Get
        End Property

        Friend ReadOnly Property NetworkStream As NetworkStream
            Get
                Return _networkStream
            End Get
        End Property

        Friend Property SslStream As SslStream
            Get
                Return _sslStream
            End Get
            Set(value As SslStream)
                _sslStream = value
            End Set
        End Property

        Friend ReadOnly Property IpPort As String
            Get
                Return _ipPort
            End Get
        End Property

        Friend SendLock As SemaphoreSlim = New SemaphoreSlim(1, 1)
        Friend ReceiveLock As SemaphoreSlim = New SemaphoreSlim(1, 1)

        Friend Property TokenSource As CancellationTokenSource

        Friend Property Token As CancellationToken

#End Region

#Region "Private-Members"

        Private _tcpClient As TcpClient = Nothing
        Private _networkStream As NetworkStream = Nothing
        Private _sslStream As SslStream = Nothing
        Private _ipPort As String = Nothing

#End Region

#Region "Constructors-and-Factories"

        Friend Sub New(tcp As TcpClient)
            If tcp Is Nothing Then Throw New ArgumentNullException(NameOf(tcp))

            _tcpClient = tcp
            _networkStream = tcp.GetStream()
            _ipPort = tcp.Client.RemoteEndPoint.ToString()
            TokenSource = New CancellationTokenSource()
            Token = TokenSource.Token
        End Sub

#End Region

#Region "Public-Methods"

        Public Sub Dispose() Implements IDisposable.Dispose
            If TokenSource IsNot Nothing Then
                If Not TokenSource.IsCancellationRequested Then
                    TokenSource.Cancel()
                    TokenSource.Dispose()
                End If
            End If

            If _sslStream IsNot Nothing Then
                _sslStream.Close()
            End If

            If _networkStream IsNot Nothing Then
                _networkStream.Close()
            End If

            If _tcpClient IsNot Nothing Then
                _tcpClient.Close()
                _tcpClient.Dispose()
            End If

            SendLock.Dispose()
            ReceiveLock.Dispose()
        End Sub

#End Region
    End Class
End Namespace
