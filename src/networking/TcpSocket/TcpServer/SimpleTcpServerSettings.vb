#Region "Microsoft.VisualBasic::1e9bcaf88d19b49710e7916e5538c57d, src\networking\TcpSocket\TcpServer\SimpleTcpServerSettings.vb"

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

    '   Total Lines: 153
    '    Code Lines: 87 (56.86%)
    ' Comment Lines: 50 (32.68%)
    '    - Xml Docs: 98.00%
    ' 
    '   Blank Lines: 16 (10.46%)
    '     File Size: 6.09 KB


    '     Class SimpleTcpServerSettings
    ' 
    '         Properties: AcceptInvalidCertificates, BlockedIPs, CertificateValidationCallback, CheckCertificateRevocation, IdleClientEvaluationIntervalMs
    '                     IdleClientTimeoutMs, MaxConnections, MutuallyAuthenticate, NoDelay, PermittedIPs
    '                     StreamBufferSize, UseAsyncDataReceivedEvents
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Net.Security

Namespace TcpSocket

    ''' <summary>
    ''' SimpleTcp server settings.
    ''' </summary>
    Public Class SimpleTcpServerSettings

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
                ' If value > 65536 Then Throw New ArgumentException("StreamBufferSize must be less than or equal to 65,536.")
                _streamBufferSize = value
            End Set
        End Property

        ''' <summary>
        ''' Maximum amount of time to wait before considering a client idle and disconnecting them. 
        ''' By default, this value is set to 0, which will never disconnect a client due to inactivity.
        ''' The timeout is reset any time a message is received from a client.
        ''' For instance, if you set this value to 30000, the client will be disconnected if the server has not received a message from the client within 30 seconds.
        ''' </summary>
        Public Property IdleClientTimeoutMs As Integer
            Get
                Return _idleClientTimeoutMs
            End Get
            Set(value As Integer)
                If value < 0 Then Throw New ArgumentException("IdleClientTimeoutMs must be zero or greater.")
                _idleClientTimeoutMs = value
            End Set
        End Property

        ''' <summary>
        ''' Maximum number of connections the server will accept.
        ''' Default is 4096.  Value must be greater than zero.
        ''' </summary>
        Public Property MaxConnections As Integer
            Get
                Return _maxConnections
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("Max connections must be greater than zero.")
                _maxConnections = value
            End Set
        End Property

        ''' <summary>
        ''' Number of milliseconds to wait between each iteration of evaluating connected clients to see if they have exceeded the configured timeout interval.
        ''' </summary>
        Public Property IdleClientEvaluationIntervalMs As Integer
            Get
                Return _idleClientEvaluationIntervalMs
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentOutOfRangeException("IdleClientEvaluationIntervalMs must be one or greater.")
                _idleClientEvaluationIntervalMs = value
            End Set
        End Property

        ''' <summary>
        ''' Enable or disable acceptance of invalid SSL certificates.
        ''' </summary>
        Public Property AcceptInvalidCertificates As Boolean = True

        ''' <summary>
        ''' Enable or disable mutual authentication of SSL client and server.
        ''' </summary>
        Public Property MutuallyAuthenticate As Boolean = True

        ''' <summary>
        ''' Enable or disable whether the data receiver thread fires the DataReceived event from a background task.
        ''' The default is enabled.
        ''' </summary>
        Public Property UseAsyncDataReceivedEvents As Boolean = True

        ''' <summary>
        ''' Enable or disable checking certificate revocation list during the validation process.
        ''' </summary>
        Public Property CheckCertificateRevocation As Boolean = True

        ''' <summary>
        ''' Delegate responsible for validating a certificate supplied by a remote party.
        ''' </summary>
        Public Property CertificateValidationCallback As RemoteCertificateValidationCallback = Nothing

        ''' <summary>
        ''' The list of permitted IP addresses from which connections can be received.
        ''' </summary>
        Public Property PermittedIPs As List(Of String)
            Get
                Return _permittedIPs
            End Get
            Set(value As List(Of String))
                If value Is Nothing Then
                    _permittedIPs = New List(Of String)()
                Else
                    _permittedIPs = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' The list of blocked IP addresses from which connections will be declined.
        ''' </summary>
        Public Property BlockedIPs As List(Of String)
            Get
                Return _blockedIPs
            End Get
            Set(value As List(Of String))
                If value Is Nothing Then
                    _blockedIPs = New List(Of String)()
                Else
                    _blockedIPs = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' print and log the verbose debug echo?
        ''' </summary>
        ''' <returns></returns>
        Public Property Verbose As Boolean = False

        Private _noDelay As Boolean = True
        Private _streamBufferSize As Integer = 65536
        Private _maxConnections As Integer = 4096
        Private _idleClientTimeoutMs As Integer = 0
        Private _idleClientEvaluationIntervalMs As Integer = 5000
        Private _permittedIPs As List(Of String) = New List(Of String)()
        Private _blockedIPs As List(Of String) = New List(Of String)()

        ''' <summary>
        ''' Instantiate the object.
        ''' </summary>
        Public Sub New()
        End Sub

    End Class
End Namespace
