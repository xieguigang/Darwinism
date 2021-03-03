Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports NLog
Imports Rpc.Connectors
Imports Rpc.MessageProtocol

Namespace Rpc
    ''' <summary>
    ''' The RPC client.
    ''' </summary>
    Public Class RpcClient
        Implements IDisposable, IRpcClient, ITicketOwner

        Private Shared Log As Logger = LogManager.GetCurrentClassLogger()
        Private ReadOnly _sessionCreater As Func(Of IRpcSession)
        Private ReadOnly _sync As Object = New Object()
        Private _session As IRpcSession = Nothing
        Private _nextXid As Integer = 0
        Private _sendingInProgress As Boolean = False
        Private _pendingRequests As LinkedList(Of ITicket) = New LinkedList(Of ITicket)()

        Friend Sub New(ByVal sessionCreater As Func(Of IRpcSession))
            _sessionCreater = sessionCreater
            NewSession()
        End Sub
        ''' <summary>
        ''' Create RPC client from TCP protocol.
        ''' </summary>
        ''' <paramname="ep">server address</param>
        ''' <paramname="blockSize">block size</param>
        Public Shared Function FromTcp(ByVal ep As IPEndPoint, ByVal Optional blockSize As Integer = 1024 * 4) As RpcClient
            Log.Debug("Create RPC client for TCP server:{0}", ep)
            Return New RpcClient(Function() New TcpSession(ep, blockSize))
        End Function

        ''' <summary>
        ''' Create RPC client from UDP protocol.
        ''' </summary>
        ''' <paramname="ep">server address</param>
        Public Shared Function FromUdp(ByVal ep As IPEndPoint) As RpcClient
            Log.Debug("Create RPC client for UDP server:{0}", ep)
            Return New RpcClient(Function() New UdpSession(ep))
        End Function

        Private Function NewSession() As IRpcSession
            Dim prevSession = _session
            _session = _sessionCreater()
            AddHandler _session.OnExcepted, AddressOf OnSessionExcepted
            AddHandler _session.OnSended, AddressOf OnSessionMessageSended
            Return prevSession
        End Function

        Private Sub RemoveTicket(ByVal ticket As ITicket) Implements ITicketOwner.RemoveTicket
            Dim sessionCopy As IRpcSession

            SyncLock _sync
                If _pendingRequests.Remove(ticket) Then Return
                sessionCopy = _session
            End SyncLock

            sessionCopy.RemoveTicket(ticket)
        End Sub

        ''' <summary>
        ''' Close this connection and cancel all queued tasks
        ''' </summary>
        Public Sub Close() Implements IRpcClient.Close
            Log.Debug("Close connector.")
            Dim tickets As ITicket()
            Dim prevSession As IRpcSession

            SyncLock _sync
                tickets = _pendingRequests.ToArray()
                _pendingRequests.Clear()
                prevSession = NewSession()
            End SyncLock

            Dim ex = New TaskCanceledException("close connector")

            For Each t In tickets
                t.Except(ex)
            Next

            prevSession.Close(ex)
        End Sub

        ''' <summary>
        ''' creates the task for the control request to the RPC server
        ''' </summary>
        Public Function CreateTask(Of TReq, TResp)(ByVal callBody As call_body, ByVal reqArgs As TReq, ByVal options As TaskCreationOptions, ByVal token As CancellationToken) As Task(Of TResp) Implements IRpcClient.CreateTask
            Dim ticket As Ticket(Of TReq, TResp) = New Ticket(Of TReq, TResp)(Me, callBody, reqArgs, options, token)

            SyncLock _sync
                If token.IsCancellationRequested Then Return ticket.Task
                _pendingRequests.AddLast(ticket)
            End SyncLock

            SendNextQueuedItem()
            Return ticket.Task
        End Function

        Private Sub SendNextQueuedItem()
            Log.Trace("Send next queued item.")
            Dim ticket As ITicket = Nothing
            Dim sessionCopy As IRpcSession = Nothing

            SyncLock _sync

                If _sendingInProgress Then
                    Log.Debug("Already sending.")
                    Return
                End If

                If _pendingRequests.Count = 0 Then
                    Log.Debug("Not pending requests to send.")
                    Return
                End If

                sessionCopy = _session
                _sendingInProgress = True
                ticket = _pendingRequests.First.Value
                _pendingRequests.RemoveFirst()
                ticket.Xid = Math.Min(Interlocked.Increment(_nextXid), _nextXid - 1)
            End SyncLock

            sessionCopy.AsyncSend(ticket)
        End Sub

        Private Sub OnSessionExcepted(ByVal session As IRpcSession, ByVal ex As Exception)
            Dim prevSession As IRpcSession

            SyncLock _sync
                If session IsNot _session Then Return
                _sendingInProgress = False
                prevSession = NewSession()
            End SyncLock

            Log.Debug("Session excepted: {0}", ex)
            SendNextQueuedItem()
            prevSession.Close(ex)
        End Sub

        Private Sub OnSessionMessageSended(ByVal session As IRpcSession)
            SyncLock _sync
                If session IsNot _session Then Return
                _sendingInProgress = False
            End SyncLock

            SendNextQueuedItem()
        End Sub

        ''' <summary>
        ''' dispose this connection
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            Close()
        End Sub
    End Class
End Namespace
