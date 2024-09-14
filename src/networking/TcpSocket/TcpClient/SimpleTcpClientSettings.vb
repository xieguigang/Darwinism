#Region "Microsoft.VisualBasic::4645c96edcdec57e024cb487bf2bae1b, src\networking\TcpSocket\TcpClient\SimpleTcpClientSettings.vb"

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

    '   Total Lines: 184
    '    Code Lines: 102 (55.43%)
    ' Comment Lines: 55 (29.89%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 27 (14.67%)
    '     File Size: 6.77 KB


    '     Class SimpleTcpClientSettings
    ' 
    '         Properties: ConnectionLostEvaluationIntervalMs, ConnectTimeoutMs, IdleServerEvaluationIntervalMs, IdleServerTimeoutMs, LocalEndpoint
    '                     NoDelay, ReadTimeoutMs, StreamBufferSize, Verbose
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Net
Imports System.Net.Security

Namespace TcpSocket
    ''' <summary>
    ''' SimpleTcp client settings.
    ''' </summary>
    Public Class SimpleTcpClientSettings
#Region "Public-Members"

        ''' <summary>
        ''' The System.Net.IPEndPoint to which you bind the TCP System.Net.Sockets.Socket.
        ''' </summary>
        Public Property LocalEndpoint As IPEndPoint
            Get
                Return _localEndpoint
            End Get
            Set(value As IPEndPoint)
                _localEndpoint = value
            End Set
        End Property

        ''' <summary>
        ''' Nagle's algorithm.
        ''' Gets or sets a value that disables a delay when send or receive buffers are not full.
        ''' true if the delay is disabled; otherwise, false. The default value is false.
        ''' </summary>
        Public Property NoDelay As Boolean
            Get
                Return _noDelay
            End Get
            Set(value As Boolean)
                _noDelay = value
            End Set
        End Property

        ''' <summary>
        ''' Buffer size to use while interacting with streams. 
        ''' </summary>
        Public Property StreamBufferSize As Integer
            Get
                Return _streamBufferSize
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("StreamBufferSize must be one or greater.")
                _streamBufferSize = value
            End Set
        End Property

        ''' <summary>
        ''' The number of milliseconds to wait when attempting to connect.
        ''' </summary>
        Public Property ConnectTimeoutMs As Integer
            Get
                Return _connectTimeoutMs
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("ConnectTimeoutMs must be greater than zero.")
                _connectTimeoutMs = value
            End Set
        End Property

        ''' <summary>
        ''' The number of milliseconds to wait when attempting to read before returning null.
        ''' </summary>
        Public Property ReadTimeoutMs As Integer
            Get
                Return _readTimeoutMs
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("ReadTimeoutMs must be greater than zero.")
                _readTimeoutMs = value
            End Set
        End Property

        ''' <summary>
        ''' Maximum amount of time to wait before considering the server to be idle and disconnecting from it. 
        ''' By default, this value is set to 0, which will never disconnect due to inactivity.
        ''' The timeout is reset any time a message is received from the server.
        ''' For instance, if you set this value to 30000, the client will disconnect if the server has not sent a message to the client within 30 seconds.
        ''' </summary>
        Public Property IdleServerTimeoutMs As Integer
            Get
                Return _idleServerTimeoutMs
            End Get
            Set(value As Integer)
                If value < 0 Then Throw New ArgumentException("IdleClientTimeoutMs must be zero or greater.")
                _idleServerTimeoutMs = value
            End Set
        End Property

        ''' <summary>
        ''' Number of milliseconds to wait between each iteration of evaluating the server connection to see if the configured timeout interval has been exceeded.
        ''' </summary>
        Public Property IdleServerEvaluationIntervalMs As Integer
            Get
                Return _idleServerEvaluationIntervalMs
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentOutOfRangeException("IdleServerEvaluationIntervalMs must be one or greater.")
                _idleServerEvaluationIntervalMs = value
            End Set
        End Property

        ''' <summary>
        ''' Number of milliseconds to wait between each iteration of evaluating the server connection to see if the connection is lost.
        ''' </summary>
        Public Property ConnectionLostEvaluationIntervalMs As Integer
            Get
                Return _connectionLostEvaluationIntervalMs
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentOutOfRangeException("ConnectionLostEvaluationIntervalMs must be one or greater.")
                _connectionLostEvaluationIntervalMs = value
            End Set
        End Property

        ''' <summary>
        ''' Enable or disable acceptance of invalid SSL certificates.
        ''' </summary>
        Public AcceptInvalidCertificates As Boolean = True

        ''' <summary>
        ''' Enable or disable mutual authentication of SSL client and server.
        ''' </summary>
        Public MutuallyAuthenticate As Boolean = True

        ''' <summary>
        ''' Enable or disable whether the data receiver thread fires the DataReceived event from a background task.
        ''' The default is enabled.
        ''' </summary>
        Public UseAsyncDataReceivedEvents As Boolean = True

        ''' <summary>
        ''' Enable or disable checking certificate revocation list during the validation process.
        ''' </summary>
        Public CheckCertificateRevocation As Boolean = True

        ''' <summary>
        ''' Delegate responsible for validating a certificate supplied by a remote party.
        ''' </summary>
        Public CertificateValidationCallback As RemoteCertificateValidationCallback = Nothing

        ''' <summary>
        ''' print and log the verbose debug echo?
        ''' </summary>
        ''' <returns></returns>
        Public Property Verbose As Boolean = False

#End Region

#Region "Private-Members"

        Private _localEndpoint As IPEndPoint = Nothing
        Private _noDelay As Boolean = True
        Private _streamBufferSize As Integer = 65536
        Private _connectTimeoutMs As Integer = 5000
        Private _readTimeoutMs As Integer = 1000
        Private _idleServerTimeoutMs As Integer = 0
        Private _idleServerEvaluationIntervalMs As Integer = 1000
        Private _connectionLostEvaluationIntervalMs As Integer = 200

#End Region

#Region "Constructors-and-Factories"

        ''' <summary>
        ''' Instantiate the object.
        ''' </summary>
        Public Sub New()

        End Sub

#End Region

#Region "Public-Methods"

#End Region

#Region "Private-Methods"

#End Region
    End Class
End Namespace
