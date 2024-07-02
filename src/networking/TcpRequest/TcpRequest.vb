#Region "Microsoft.VisualBasic::2e320fcfa281a42ebd255f032bcf25dc, src\networking\TcpRequest\TcpRequest.vb"

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

    '   Total Lines: 249
    '    Code Lines: 135 (54.22%)
    ' Comment Lines: 72 (28.92%)
    '    - Xml Docs: 79.17%
    ' 
    '   Blank Lines: 42 (16.87%)
    '     File Size: 10.02 KB


    '     Class TcpRequest
    ' 
    '         Constructor: (+5 Overloads) Sub New
    ' 
    '         Function: LocalConnection, (+4 Overloads) SendMessage, SetTimeOut, ToString
    ' 
    '         Sub: Connected, DataSent, Disconnected, (+2 Overloads) Dispose, RequestToStream
    '         Class DataReceived
    ' 
    '             Sub: HandleEvent
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Net
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports Darwinism.IPC.Networking.TcpSocket
Imports Microsoft.VisualBasic.ApplicationServices.Debugging
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Parallel
Imports IPEndPoint = Microsoft.VisualBasic.Net.IPEndPoint
Imports TcpEndPoint = System.Net.IPEndPoint

Namespace Tcp

    ''' <summary>
    ''' The server socket should returns some data string to this client or this client 
    ''' will stuck at the <see cref="SendMessage"></see> function.
    ''' (服务器端``TcpServicesSocket``必须要返回数据， 
    ''' 否则本客户端会在<see cref="SendMessage"></see>函数位置一直处于等待的状态)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TcpRequest : Implements IDisposable

        ''' <summary>
        ''' The port number for the remote device.
        ''' </summary>
        ''' <remarks></remarks>
        Dim port As Integer
        Dim exceptionHandler As ExceptionHandler
        Dim remoteHost As String

        ''' <summary>
        ''' A System.TimeSpan that represents the number of milliseconds to wait, or a System.TimeSpan
        ''' that represents -1 milliseconds to wait indefinitely.
        ''' </summary>
        Dim timeout As TimeSpan = TimeSpan.FromSeconds(60)

        ''' <summary>
        ''' Remote End Point
        ''' </summary>
        ''' <remarks></remarks>
        Protected ReadOnly remoteEP As TcpEndPoint

        Public Function SetTimeOut(timespan As TimeSpan) As TcpRequest
            Me.timeout = timespan
            Return Me
        End Function

        Public Overrides Function ToString() As String
            Return $"Remote_connection={remoteHost}:{port},  local_host={LocalIPAddress}"
        End Function

        Sub New(localPort As Integer)
            Call Me.New(New IPEndPoint("127.0.0.1", localPort))
        End Sub

        Sub New(remoteDevice As TcpEndPoint, Optional exceptionHandler As ExceptionHandler = Nothing)
            Call Me.New(remoteDevice.Address.ToString, remoteDevice.Port, exceptionHandler)
        End Sub

        Sub New(remoteDevice As IPEndPoint, Optional exceptionHandler As ExceptionHandler = Nothing)
            Call Me.New(remoteDevice.ipAddress, remoteDevice.port, exceptionHandler)
        End Sub

        Shared ReadOnly defaultHandler As New [Default](Of ExceptionHandler)(AddressOf VBDebugger.PrintException)

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="client">
        ''' Copy the TCP client connection profile data from this object.
        ''' (从本客户端对象之中复制出连接配置参数以进行初始化操作)
        ''' </param>
        ''' <param name="exceptionHandler"></param>
        ''' <remarks></remarks>
        Sub New(client As TcpRequest, Optional exceptionHandler As ExceptionHandler = Nothing)
            Me.remoteHost = client.remoteHost
            Me.port = client.port
            Me.exceptionHandler = exceptionHandler Or defaultHandler
            Me.remoteEP = New TcpEndPoint(IPAddress.Parse(remoteHost), port)
        End Sub

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="remotePort"></param>
        ''' <param name="exceptionHandler">
        ''' Public <see cref="System.Delegate"/> Sub ExceptionHandler(ex As <see cref="Exception"/>)
        ''' </param>
        ''' <remarks></remarks>
        Sub New(hostName$, remotePort%, Optional exceptionHandler As ExceptionHandler = Nothing)
            remoteHost = hostName

            If String.Equals(remoteHost, "localhost", StringComparison.OrdinalIgnoreCase) Then
                remoteHost = "127.0.0.1" ' LocalIPAddress
            End If

            Me.port = remotePort
            Me.exceptionHandler = exceptionHandler Or defaultHandler
            Me.remoteEP = New TcpEndPoint(IPAddress.Parse(remoteHost), port)
        End Sub

        ''' <summary>
        ''' 初始化一个在本机进行进程间通信的Socket对象
        ''' </summary>
        ''' <param name="localPort"></param>
        ''' <param name="exceptionHandler"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function LocalConnection(localPort%, Optional exceptionHandler As ExceptionHandler = Nothing) As TcpRequest
            Return New TcpRequest(LocalIPAddress, localPort, exceptionHandler)
        End Function

        Public Function SendMessage(Message As String, Callback As Action(Of String)) As IAsyncResult
            Dim SendMessageClient As New TcpRequest(Me, exceptionHandler:=Me.exceptionHandler)
            Return (Sub() Call Callback(SendMessageClient.SendMessage(Message))).BeginInvoke(Nothing, Nothing)
        End Function

        ''' <summary>
        ''' This function returns the server reply for this request <paramref name="Message"></paramref>.
        ''' </summary>
        ''' <param name="Message">The client request to the server.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SendMessage(Message As String) As String
            Dim byteData As Byte() = Encoding.UTF8.GetBytes(Message)
            byteData = SendMessage(byteData)
            Dim response As String = New RequestStream(byteData).GetUTF8String
            Return response
        End Function

        ''' <summary>
        ''' Send a request message to the remote server.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <returns></returns>
        Public Function SendMessage(message As RequestStream) As RequestStream
            Dim byteData As Byte() = SendMessage(message.Serialize)
            Dim stream As RequestStream

            If RequestStream.IsAvaliableStream(byteData) Then
                stream = New RequestStream(byteData)
            Else
                stream = New RequestStream(0, 0, byteData)
            End If

            If ZipDataPipe.TestBufferMagic(stream.ChunkBuffer) Then
                stream.ChunkBuffer = ZipDataPipe.UncompressBuffer(stream.ChunkBuffer)
            End If

            Return stream
        End Function

        Public Sub RequestToStream(message As RequestStream, stream As Stream)
            Dim buffer = SendMessage(message.Serialize)

            Call stream.Write(buffer, Scan0, buffer.Length)
            Call stream.Flush()
        End Sub

        ''' <summary>
        ''' 最底层的消息发送函数
        ''' </summary>
        ''' <param name="message"></param>
        ''' <returns></returns>
        Public Function SendMessage(message As Byte()) As Byte()
            Dim socket As New SimpleTcpClient(remoteHost, port)
            Dim buffer As New DataReceived

            AddHandler socket.Events.Connected, AddressOf Connected
            AddHandler socket.Events.Disconnected, AddressOf Disconnected
            AddHandler socket.Events.DataReceived, AddressOf buffer.HandleEvent
            AddHandler socket.Events.DataSent, AddressOf DataSent

            socket.Keepalive.EnableTcpKeepAlives = True
            socket.Settings.MutuallyAuthenticate = False
            socket.Settings.AcceptInvalidCertificates = True
            socket.Settings.ConnectTimeoutMs = 5000
            socket.Settings.NoDelay = True
            socket.Settings.StreamBufferSize = 64 * 1024 * 1024
            socket.ConnectWithRetries(5000)
            socket.Logger = AddressOf VBDebugger.EchoLine
            socket.Send(message)

            Do While Not buffer.triggered
                Call Thread.Sleep(1)
            Loop

            socket.Disconnect()
            socket.Dispose()

            Return buffer.cache_buffer
        End Function

        Private Sub Connected(sender As Object, e As ConnectionEventArgs)
        End Sub

        Private Class DataReceived

            Public cache_buffer As Byte()
            Public triggered As Boolean = False

            Private Sub Disconnected(sender As Object, e As ConnectionEventArgs)
                ' 20240702 the server socket may send empty package
                ' then handle event will not be triggered
                ' disconnected event from the server will happends
                ' set trigger flag at here
                ' or this client socket will wait for the server data
                ' forever
                If cache_buffer Is Nothing Then
                    cache_buffer = {}
                End If

                triggered = True
            End Sub

            Public Sub HandleEvent(sender As Object, e As DataReceivedEventArgs)
                cache_buffer = e.Data.Array
                triggered = True
            End Sub
        End Class

        Private Sub DataSent(sender As Object, e As DataSentEventArgs)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(      disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(      disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace
