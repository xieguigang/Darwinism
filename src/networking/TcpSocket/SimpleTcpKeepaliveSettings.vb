#Region "Microsoft.VisualBasic::985801a67d18aa3b8e12e265c5089a78, src\networking\TcpSocket\SimpleTcpKeepaliveSettings.vb"

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

    '   Total Lines: 83
    '    Code Lines: 47 (56.63%)
    ' Comment Lines: 27 (32.53%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 9 (10.84%)
    '     File Size: 3.61 KB


    '     Class SimpleTcpKeepaliveSettings
    ' 
    '         Properties: TcpKeepAliveInterval, TcpKeepAliveIntervalMilliseconds, TcpKeepAliveRetryCount, TcpKeepAliveTime, TcpKeepAliveTimeMilliseconds
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace TcpSocket

    ''' <summary>
    ''' SimpleTcp keepalive settings.
    ''' Keepalive probes are sent after an idle period defined by TcpKeepAliveTime (seconds).
    ''' Should a keepalive response not be received within TcpKeepAliveInterval (seconds), a subsequent keepalive probe will be sent.
    ''' For .NET Framework, should 10 keepalive probes fail, the connection will terminate.
    ''' For .NET Core, should a number of probes fail as specified in TcpKeepAliveRetryCount, the connection will terminate.
    ''' TCP keepalives are not supported in .NET Standard.
    ''' </summary>
    Public Class SimpleTcpKeepaliveSettings

        ''' <summary>
        ''' Enable or disable TCP-based keepalive probes.
        ''' TCP keepalives are only supported in .NET Core and .NET Framework projects.  .NET Standard does not provide facilities to support TCP keepalives.
        ''' </summary>
        Public EnableTcpKeepAlives As Boolean = False

        ''' <summary>
        ''' TCP keepalive interval, i.e. the number of seconds a TCP connection will wait for a keepalive response before sending another keepalive probe.
        ''' Default is 5 seconds.  Value must be greater than zero.
        ''' </summary>
        Public Property TcpKeepAliveInterval As Integer
            Get
                Return _tcpKeepAliveInterval
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("TcpKeepAliveInterval must be greater than zero.")
                _tcpKeepAliveInterval = value
            End Set
        End Property

        ''' <summary>
        ''' TCP keepalive time, i.e. the number of seconds a TCP connection will remain alive/idle before keepalive probes are sent to the remote. 
        ''' Default is 5 seconds.  Value must be greater than zero.
        ''' </summary>
        Public Property TcpKeepAliveTime As Integer
            Get
                Return _tcpKeepAliveTime
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("TcpKeepAliveTime must be greater than zero.")
                _tcpKeepAliveTime = value
            End Set
        End Property

        ''' <summary>
        ''' TCP keepalive retry count, i.e. the number of times a TCP probe will be sent in effort to verify the connection.
        ''' After the specified number of probes fail, the connection will be terminated.
        ''' </summary>
        Public Property TcpKeepAliveRetryCount As Integer
            Get
                Return _tcpKeepAliveRetryCount
            End Get
            Set(value As Integer)
                If value < 1 Then Throw New ArgumentException("TcpKeepAliveRetryCount must be greater than zero.")
                _tcpKeepAliveRetryCount = value
            End Set
        End Property

        Friend ReadOnly Property TcpKeepAliveIntervalMilliseconds As Integer
            Get
                Return TcpKeepAliveInterval * 1000
            End Get
        End Property

        Friend ReadOnly Property TcpKeepAliveTimeMilliseconds As Integer
            Get
                Return TcpKeepAliveTime * 1000
            End Get
        End Property

        Private _tcpKeepAliveInterval As Integer = 2
        Private _tcpKeepAliveTime As Integer = 2
        Private _tcpKeepAliveRetryCount As Integer = 3

        ''' <summary>
        ''' Instantiate the object.
        ''' </summary>
        Public Sub New()
        End Sub
    End Class
End Namespace
