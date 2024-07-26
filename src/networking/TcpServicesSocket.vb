#Region "Microsoft.VisualBasic::1d84f61678b8aeb08f16d19345f36b12, src\networking\TcpServicesSocket.vb"

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

    '   Total Lines: 335
    '    Code Lines: 181 (54.03%)
    ' Comment Lines: 101 (30.15%)
    '    - Xml Docs: 78.22%
    ' 
    '   Blank Lines: 53 (15.82%)
    '     File Size: 14.52 KB


    '     Class TcpServicesSocket
    ' 
    '         Properties: IsShutdown, KeepsAlive, LastError, LocalPort, ResponseHandler
    '                     Running
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: BeginListen, IsServerInternalException, LoopbackEndPoint, (+2 Overloads) Run, ToString
    ' 
    '         Sub: AcceptWorker, DataReceived, DataSent, Disconnect, (+2 Overloads) Dispose
    '              HandleRequest, Send
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Darwinism.IPC.Networking.HTTP
Imports Darwinism.IPC.Networking.TcpSocket
Imports Microsoft.VisualBasic.ApplicationServices.Debugging
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Unit
Imports Microsoft.VisualBasic.Language.Default
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Abstract
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Serialization.JSON
Imports TcpEndPoint = System.Net.IPEndPoint

Namespace Tcp

    ''' <summary>
    ''' Socket listening object which is running at the server side asynchronous able multiple threading.
    ''' a wrapper of the <see cref="SimpleTcpServer"/>
    ''' </summary>
    ''' <remarks>
    ''' (运行于服务器端上面的Socket监听对象，多线程模型)
    ''' </remarks>
    Public Class TcpServicesSocket
        Implements IDisposable
        Implements ITaskDriver
        Implements IServicesSocket

        Dim _exceptionHandle As ExceptionHandler
        Dim _maxAccepts As Integer = 4
        Dim _debugMode As Boolean = False
        Dim _socket As SimpleTcpServer

        ''' <summary>
        ''' The server services listening on this local port.(当前的这个服务器对象实例所监听的本地端口号)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LocalPort As Integer Implements IServicesSocket.LocalPort

        ''' <summary>
        ''' This function pointer using for the data request handling of the data request from the client socket.   
        ''' [Public Delegate Function DataResponseHandler(str As <see cref="String"/>, RemoteAddress As <see cref="TcpEndPoint"/>) As <see cref="String"/>]
        ''' (这个函数指针用于处理来自于客户端的请求)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ResponseHandler As DataRequestHandler Implements IServicesSocket.ResponseHandler
        Public Property KeepsAlive As Boolean = False
        Public Property Verbose As Boolean = False
        Public ReadOnly Property Running As Boolean = False Implements IServicesSocket.IsRunning

        Public ReadOnly Property IsShutdown As Boolean Implements IServicesSocket.IsShutdown
            Get
                Return disposedValue
            End Get
        End Property

        Shared ReadOnly defaultHandler As New [Default](Of ExceptionHandler)(AddressOf VBDebugger.PrintException)

        Public ReadOnly Property LastError As String

        ''' <summary>
        ''' 消息处理的方法接口： Public Delegate Function DataResponseHandler(str As String, RemotePort As Integer) As String
        ''' </summary>
        ''' <param name="localPort">监听的本地端口号，假若需要进行端口映射的话，则可以在<see cref="Run"></see>方法之中设置映射的端口号</param>
        ''' <remarks></remarks>
        Sub New(Optional localPort As Integer = 11000,
                Optional exceptionHandler As ExceptionHandler = Nothing,
                Optional debug As Boolean = False)

            Me._LocalPort = localPort
            Me._exceptionHandle = exceptionHandler Or defaultHandler
            Me._debugMode = debug
        End Sub

        ''' <summary>
        ''' 短连接socket服务端
        ''' </summary>
        ''' <param name="requestEventHandler"></param>
        ''' <param name="localPort"></param>
        ''' <param name="exceptionHandler"></param>
        Sub New(requestEventHandler As DataRequestHandler, localPort%,
                Optional exceptionHandler As ExceptionHandler = Nothing,
                Optional debug As Boolean = False)

            Me._ResponseHandler = requestEventHandler
            Me._exceptionHandle = exceptionHandler Or defaultHandler
            Me._LocalPort = localPort
            Me._debugMode = debug
        End Sub

        ''' <summary>
        ''' 函数返回Socket的注销方法
        ''' </summary>
        ''' <param name="requestEventHandler">Public Delegate Function DataResponseHandler(str As String, RemotePort As Integer) As String</param>
        ''' <param name="localPort"></param>
        ''' <param name="exceptionHandler"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BeginListen(requestEventHandler As DataRequestHandler,
                                           Optional localPort As Integer = 11000,
                                           Optional keepsAlive As Boolean = True,
                                           Optional exceptionHandler As ExceptionHandler = Nothing) As Action

            With New TcpServicesSocket(requestEventHandler, localPort, exceptionHandler) With {
                .KeepsAlive = keepsAlive
            }
                Call New Action(AddressOf .Run).BeginInvoke(Nothing, Nothing)
                Return AddressOf .Dispose
            End With
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function LoopbackEndPoint(Port As Integer) As TcpEndPoint
            Return New TcpEndPoint(System.Net.IPAddress.Loopback, Port)
        End Function

        Public Overrides Function ToString() As String
            Return $"{GetIPAddress()}:{LocalPort}"
        End Function

        ''' <summary>
        ''' This server waits for a connection and then uses  asychronous operations to
        ''' accept the connection, get data from the connected client,
        ''' echo that data back to the connected client.
        ''' It then disconnects from the client and waits for another client.(请注意，当服务器的代码运行到这里之后，代码将被阻塞在这里)
        ''' </summary>
        ''' <remarks></remarks>
        Public Function Run() As Integer Implements ITaskDriver.Run, IServicesSocket.Run
            ' Establish the local endpoint for the socket.
            Dim localEndPoint As New TcpEndPoint(System.Net.IPAddress.Any, _LocalPort)
            Return Run(localEndPoint)
        End Function

        ''' <summary>
        ''' This server waits for a connection and then uses  asychronous operations to
        ''' accept the connection, get data from the connected client,
        ''' echo that data back to the connected client.
        ''' It then disconnects from the client and waits for another client.
        ''' </summary>
        ''' <remarks>(请注意，当服务器的代码运行到这里之后，代码将被阻塞在这里)</remarks>
        Public Function Run(localEndPoint As TcpEndPoint) As Integer Implements IServicesSocket.Run
            Dim exitCode As Integer = 0

            If _debugMode Then
                Call VBDebugger.EchoLine($"Start run socket... {localEndPoint.ToString}")
            End If

            _socket = New SimpleTcpServer("*", localEndPoint.Port)

            AddHandler _socket.Events.ClientConnected, AddressOf AcceptWorker
            AddHandler _socket.Events.ClientDisconnected, AddressOf Disconnect
            AddHandler _socket.Events.DataReceived, AddressOf DataReceived
            AddHandler _socket.Events.DataSent, AddressOf DataSent

            If Verbose Then
                _socket.Debugger = AddressOf VBDebugger.EchoLine
            Else
                _socket.Debugger = Nothing
            End If

            _socket.Keepalive.EnableTcpKeepAlives = True
            _socket.Settings.IdleClientTimeoutMs = 0
            _socket.Settings.MutuallyAuthenticate = False
            _socket.Settings.AcceptInvalidCertificates = True
            _socket.Settings.NoDelay = True
            _socket.Settings.StreamBufferSize = App.BufferSize
            _socket.Start()

            Call VBDebugger.EchoLine(_socket.Settings.GetJson)
            Call VBDebugger.EchoLine($"set tcp server message buffer size: {_socket.Settings.StreamBufferSize / ByteSize.KB}KB.")

            _Running = True

            While Not Me.disposedValue AndAlso Running
                Call Thread.Sleep(1)
            End While

            If _debugMode Then
                Call Console.WriteLine("Exit socket loop...")
                Call Console.WriteLine($"status code = {exitCode}")
            End If

            _Running = False

            If _debugMode Then
                Call Console.WriteLine("Release socket done!")
            End If

            Return exitCode
        End Function

        Private Sub AcceptWorker(sender As Object, e As ConnectionEventArgs)
        End Sub

        Private Sub Disconnect(sender As Object, e As ConnectionEventArgs)
        End Sub

        Private Sub DataReceived(sender As Object, e As DataReceivedEventArgs)
            Dim request As New RequestStream(e.Data.Array)
            Dim remote As TcpEndPoint = New IPEndPoint(e.IpPort)

            Using payload As New MemoryStream
                ' received data, then processing the request 
                ' finally send the response data to clinet
                Call HandleRequest(remote, payload, request)
                Call payload.Seek(Scan0, SeekOrigin.Begin)
                Call _socket.Send(e.IpPort, payload.Length, payload)
            End Using

            If Not KeepsAlive Then
                ' finnaly, disconnect client if no needs keeps alive of the
                ' socket connection
                Call _socket.DisconnectClient(e.IpPort)
            End If
        End Sub

        Private Sub DataSent(sender As Object, e As DataSentEventArgs)
        End Sub

        ''' <summary>
        ''' All the data has been read from the client. Display it on the console.
        ''' Echo the data back to the client.
        ''' </summary>
        ''' <param name="remote"></param>
        ''' <param name="requestData"></param>
        Private Sub HandleRequest(remote As TcpEndPoint, response As Stream, requestData As RequestStream)
            Try
                Dim result As BufferPipe

                If ResponseHandler Is Nothing OrElse requestData.IsPing Then
                    result = New DataPipe(NetResponse.RFC_OK)
                Else
                    result = Me.ResponseHandler()(requestData, remote)
                End If

                Call Send(response, result)
            Catch ex As Exception
                Call _exceptionHandle(ex)
                ' 错误可能是内部处理请求的时候出错了，则将SERVER_INTERNAL_EXCEPTION结果返回给客户端
                Try
                    Call Send(response, New DataPipe(NetResponse.RFC_INTERNAL_SERVER_ERROR))
                Catch ex2 As Exception
                    ' 这里处理的是可能是强制断开连接的错误
                    Call _exceptionHandle(ex2)
                End Try
            End Try
        End Sub

        ''' <summary>
        ''' Server reply the processing result of the request from the client.
        ''' </summary>
        ''' <param name="data"></param>
        ''' <remarks></remarks>
        Private Sub Send(response As Stream, data As BufferPipe)
            Call VBDebugger.EchoLine($"send stream: {data}")

            ' Convert the string data to byte data using ASCII encoding.
            For Each byteData As Byte() In data.GetBlocks
                For Each block As Byte() In byteData.Split(4096)
                    Call response.Write(block, Scan0, block.Length)
                    Call response.Flush()
                Next
            Next

            Call response.Flush()

            ' release data
            If TypeOf data Is DataPipe Then
                Call DirectCast(data, DataPipe).Dispose()
            ElseIf TypeOf data Is StreamPipe Then
                Call DirectCast(data, StreamPipe).Dispose()
            End If
        End Sub

        ''' <summary>
        ''' SERVER_INTERNAL_EXCEPTION，Server encounter an internal exception during processing
        ''' the data request from the remote device.
        ''' (判断是否服务器在处理客户端的请求的时候，发生了内部错误)
        ''' </summary>
        ''' <param name="replyData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function IsServerInternalException(replyData As String) As Boolean
            Return String.Equals(replyData, NetResponse.RFC_INTERNAL_SERVER_ERROR.GetUTF8String)
        End Function

#Region "IDisposable Support"

        ''' <summary>
        ''' 退出监听线程所需要的
        ''' </summary>
        ''' <remarks></remarks>
        Private disposedValue As Boolean = False  ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    _Running = False

                    ' TODO: dispose managed state (managed objects).
                    Try
                        Call _socket.Stop()
                    Catch ex As Exception

                    End Try
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

        ''' <summary>
        ''' Stop the server socket listening threads.(终止服务器Socket监听线程)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
