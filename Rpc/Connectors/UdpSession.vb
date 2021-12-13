#Region "Microsoft.VisualBasic::14f21439dc2d2a8324d44249cffbd635, Rpc\Connectors\UdpSession.vb"

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

'     Class UdpSession
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: EnqueueTicket
' 
'         Sub: AsyncSend, BeginReceive, BuildMessage, Close, OnDatagramWrited
'              OnException, OnMessageReaded, OnSend, RemoveTicket
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic
Imports System.IO.XDR.Reading
Imports System.Linq
Imports System.Net
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Rpc.MessageProtocol
Imports Rpc.UdpDatagrams

Namespace Rpc.Connectors
    Friend Class UdpSession
        Implements IRpcSession

        Private Shared Log As LogFile = Microsoft.VisualBasic.My.FrameworkInternal.getLogger(GetType(UdpSession).FullName)
        Private ReadOnly _client As UdpClientWrapper
        Private _sendingTicket As ITicket = Nothing
        Private ReadOnly _sync As Object = New Object()
        Private _receivingInProgress As Boolean = False
        Private _handlers As Dictionary(Of UInteger, ITicket) = New Dictionary(Of UInteger, ITicket)()

        Public Sub New(ep As IPEndPoint)
            _client = New UdpClientWrapper(ep)
        End Sub

        Public Sub AsyncSend(ticket As ITicket) Implements IRpcSession.AsyncSend
            If _sendingTicket IsNot Nothing Then Throw New InvalidOperationException("ticket already sending")
            _sendingTicket = ticket

            SyncLock _sync
                _handlers.Add(_sendingTicket.Xid, _sendingTicket)
            End SyncLock

            Call ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf BuildMessage))
        End Sub

        Private Sub BuildMessage(state As Object)
            Dim datagram As Byte()

            Try
                Dim uw As UdpWriter = New UdpWriter()
                _sendingTicket.BuildRpcMessage(uw)
                datagram = uw.Build()
            Catch ex As Exception
                Log.Debug("UDP datagram not builded (xid:{0}) reason: {1}", _sendingTicket.Xid, ex)

                SyncLock _sync
                    _handlers.Remove(_sendingTicket.Xid)
                End SyncLock

                _sendingTicket.Except(ex)
                OnSend()
                Return
            End Try

            Log.Debug("Begin sending UDP datagram (xid:{0})", _sendingTicket.Xid)
            _client.AsyncWrite(datagram, New Action(Of Exception)(AddressOf OnDatagramWrited))
        End Sub

        Private Sub BeginReceive()
            SyncLock _sync
                If _receivingInProgress Then Return

                If _handlers.Count = 0 Then
                    Log.Debug("Receive stop. No handlers.")
                    Return
                End If

                _receivingInProgress = True
            End SyncLock

            Log.Trace("Wait response.")
            _client.AsyncRead(New Action(Of Exception, UdpReader)(AddressOf OnMessageReaded))
        End Sub

        Private Sub OnMessageReaded(err As Exception, udpReader As UdpReader)
            If err IsNot Nothing Then
                Log.Debug("No receiving UDP datagrams. Reason: {0}", err)
                OnException(err)
                Return
            End If

            SyncLock _sync
                _receivingInProgress = False
            End SyncLock

            Dim respMsg As rpc_msg = Nothing
            Dim r As Reader = Nothing

            Try
                r = CreateReader(udpReader)
                respMsg = r.Read(Of rpc_msg)()
            Catch ex As Exception
                Log.info($"Parse exception: {ex.ToString}")
                BeginReceive()
                Return
            End Try

            Log.Trace("Received response xid:{0}", respMsg.xid)
            Dim ticket = EnqueueTicket(respMsg.xid)
            BeginReceive()

            If ticket Is Nothing Then
                Log.Debug("No handler for xid:{0}", respMsg.xid)
            Else
                ticket.ReadResult(udpReader, r, respMsg)
            End If
        End Sub

        Private Function EnqueueTicket(xid As UInteger) As ITicket
            SyncLock _sync
                Dim result As ITicket = Nothing
                If Not _handlers.TryGetValue(xid, result) Then Return Nothing
                _handlers.Remove(xid)
                Return result
            End SyncLock
        End Function

        Private Sub OnDatagramWrited(ex As Exception)
            If ex IsNot Nothing Then
                Log.Debug("UDP datagram not sended (xid:{0}) reason: {1}", _sendingTicket.Xid, ex)
                OnException(ex)
            Else
                Log.Debug("UDP datagram sended (xid:{0})", _sendingTicket.Xid)
                BeginReceive()
                OnSend()
            End If
        End Sub

        Public Sub RemoveTicket(ticket As ITicket) Implements ITicketOwner.RemoveTicket
            SyncLock _sync
                _handlers.Remove(ticket.Xid)
            End SyncLock
        End Sub

        Public Sub Close(ex As Exception) Implements IRpcSession.Close
            Log.Debug("Close session.")
            Dim tickets As ITicket()

            SyncLock _sync
                tickets = _handlers.Values.ToArray()
                _handlers.Clear()
            End SyncLock

            For Each t In tickets
                t.Except(ex)
            Next

            _client.Close()
        End Sub

        Public Event OnExcepted As Action(Of IRpcSession, Exception) Implements IRpcSession.OnExcepted

        Private Sub OnException(ex As Exception)
            Dim copy = OnExceptedEvent
            If copy IsNot Nothing Then copy(Me, ex)
        End Sub

        Public Event OnSended As Action(Of IRpcSession) Implements IRpcSession.OnSended

        Private Sub OnSend()
            _sendingTicket = Nothing
            Dim copy = OnSendedEvent
            If copy IsNot Nothing Then copy(Me)
        End Sub
    End Class
End Namespace
