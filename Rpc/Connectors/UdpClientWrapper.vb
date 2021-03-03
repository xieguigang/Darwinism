Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports NLog
Imports Rpc.UdpDatagrams

Namespace Rpc.Connectors
    Friend Class UdpClientWrapper
        Private Shared Log As Logger = LogManager.GetCurrentClassLogger()
        Private ReadOnly _ep As IPEndPoint
        Private _sync As Object = New Object()
        Private _disposed As Boolean = False
        Private _client As UdpClient

        Public Sub New(ByVal ep As IPEndPoint)
            _ep = ep
            _client = New UdpClient()
            _client.Connect(_ep)
        End Sub

        Private _readCompleted As Action(Of Exception, UdpReader) = Nothing

        Public Sub AsyncRead(ByVal completed As Action(Of Exception, UdpReader))
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

        Private Sub EndRead(ByVal ar As IAsyncResult)
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

        Public Sub AsyncWrite(ByVal datagram As Byte(), ByVal completed As Action(Of Exception))
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

        Private Sub EndWrite(ByVal ar As IAsyncResult)
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
