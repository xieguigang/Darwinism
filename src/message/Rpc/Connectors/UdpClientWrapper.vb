#Region "Microsoft.VisualBasic::0e46a4a5d2e6d4c63440cb8ae8c4dd62, Rpc\Connectors\UdpClientWrapper.vb"

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

'     Class UdpClientWrapper
' 
'         Constructor: (+1 Overloads) Sub New
'         Sub: AsyncRead, AsyncWrite, Close, EndRead, EndWrite
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Rpc.UdpDatagrams

Namespace Rpc.Connectors
    Friend Class UdpClientWrapper
        Private Shared Log As LogFile = Microsoft.VisualBasic.My.FrameworkInternal.getLogger(GetType(UdpClientWrapper).FullName)
        Private ReadOnly _ep As IPEndPoint
        Private _sync As Object = New Object()
        Private _disposed As Boolean = False
        Private _client As UdpClient

        Public Sub New(ep As IPEndPoint)
            _ep = ep
            _client = New UdpClient()
            _client.Connect(_ep)
        End Sub

        Private _readCompleted As Action(Of Exception, UdpReader) = Nothing

        Public Sub AsyncRead(completed As Action(Of Exception, UdpReader))
            _readCompleted = completed

            SyncLock _sync

                Try
                    If _disposed Then Throw New ObjectDisposedException(GetType(UdpClient).FullName)
                    _client.BeginReceive(New AsyncCallback(AddressOf EndRead), Nothing)
                Catch ex As Exception
                    _readCompleted = Nothing
                    ThreadPool.QueueUserWorkItem(Sub(state) completed(ex, Nothing))
                End Try
            End SyncLock
        End Sub

        Private Sub EndRead(ar As IAsyncResult)
            Dim reader As UdpReader = Nothing
            Dim err As Exception = Nothing
            Dim copy = _readCompleted
            _readCompleted = Nothing

            SyncLock _sync

                Try
                    If _disposed Then Throw New ObjectDisposedException(GetType(UdpClient).FullName)
                    Dim ep As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
                    Dim datagram = _client.EndReceive(ar, ep)
                    Call Log.Trace(New Func(Of String, Byte(), String)(AddressOf DumpToLog), "received datagram: {0}", datagram)
                    reader = New UdpReader(datagram)
                Catch ex As Exception
                    err = ex
                End Try
            End SyncLock

            copy(err, reader)
        End Sub

        Private _writeCompleted As Action(Of Exception) = Nothing

        Public Sub AsyncWrite(datagram As Byte(), completed As Action(Of Exception))
            Call Log.Trace(New Func(Of String, Byte(), String)(AddressOf DumpToLog), "sending datagram: {0}", datagram)
            _writeCompleted = completed

            SyncLock _sync

                Try
                    If _disposed Then Throw New ObjectDisposedException(GetType(UdpClient).FullName)
                    _client.BeginSend(datagram, datagram.Length, New AsyncCallback(AddressOf EndWrite), Nothing)
                Catch ex As Exception
                    _writeCompleted = Nothing
                    ThreadPool.QueueUserWorkItem(Sub(state) completed(ex))
                End Try
            End SyncLock
        End Sub

        Private Sub EndWrite(ar As IAsyncResult)
            Dim err As Exception = Nothing
            Dim copy = _writeCompleted
            _writeCompleted = Nothing

            SyncLock _sync

                Try
                    If _disposed Then Throw New ObjectDisposedException(GetType(UdpClient).FullName)
                    _client.EndSend(ar)
                Catch ex As Exception
                    err = ex
                End Try
            End SyncLock

            copy(err)
        End Sub

        Public Sub Close()
            SyncLock _sync
                If _disposed Then Return
                _disposed = True
                _client.Close()
            End SyncLock
        End Sub
    End Class
End Namespace
