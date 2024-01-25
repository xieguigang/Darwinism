﻿#Region "Microsoft.VisualBasic::56816b31e3ebe28240fdc458329a2a7c, Rpc\Connectors\TcpClientWrapper.vb"

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

'     Class TcpClientWrapper
' 
'         Properties: Connected
' 
'         Constructor: (+1 Overloads) Sub New
'         Sub: AsyncConnect, AsyncRead, AsyncWrite, BeginReadRecordMark, Close
'              EndReadBody, EndReadRecordMark, EndWrite, ExtractRecordMark, OnConnected
'              SafeBeginRead, SafeBeginWrite
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Debugging.Logging
Imports Rpc.TcpStreaming

Namespace Rpc.Connectors
    Friend Class TcpClientWrapper
        Private Shared Log As LogFile = Microsoft.VisualBasic.My.FrameworkInternal.getLogger(GetType(TcpClientWrapper).FullName)
        Private ReadOnly _ep As IPEndPoint
        Private _connected As Boolean = False
        Private _sync As Object = New Object()
        Private _disposed As Boolean = False
        Private _client As TcpClient
        Private _stream As NetworkStream

        Public Sub New(ep As IPEndPoint)
            _ep = ep
            _client = New TcpClient(_ep.AddressFamily)
        End Sub

        Public ReadOnly Property Connected As Boolean
            Get
                Return _connected
            End Get
        End Property

        Private _connectCompleted As Action(Of Exception) = Nothing

        Public Sub AsyncConnect(completed As Action(Of Exception))
            Try

                SyncLock _sync
                    If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                    If _connected OrElse _connectCompleted IsNot Nothing Then Throw New InvalidOperationException("already connecting")
                    _connected = True
                    _connectCompleted = completed
                    _client.BeginConnect(_ep.Address, _ep.Port, New AsyncCallback(AddressOf OnConnected), Nothing)
                End SyncLock

            Catch ex As Exception
                Log.Debug("Unable to connected to {0}. Reason: {1}", _ep, ex)
                _connectCompleted = Nothing
                ThreadPool.QueueUserWorkItem(Sub(__) completed(ex))
            End Try
        End Sub

        Private Sub OnConnected(ar As IAsyncResult)
            Dim copy = _connectCompleted
            _connectCompleted = Nothing

            Try

                SyncLock _sync
                    If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                    _client.EndConnect(ar)
                    _stream = _client.GetStream()
                End SyncLock

            Catch ex As Exception
                Log.Debug("Unable to connected to {0}. Reason: {1}", _ep, ex)
                copy(ex)
            End Try

            Log.Debug("Sucess connect to {0}", _ep)
            copy(Nothing)
        End Sub

#Region "async read"

        Public Sub AsyncRead(completed As Action(Of Exception, TcpReader))
            'HACK: here you need to implement a timeout interrupt
            Try
                If _readCompleted IsNot Nothing Then Throw New InvalidOperationException("already reading")
                _tcpReader = New TcpReader()
                _readCompleted = completed
                BeginReadRecordMark()
            Catch ex As Exception
                _tcpReader = Nothing
                _readCompleted = Nothing
                ThreadPool.QueueUserWorkItem(Sub(state) completed(ex, Nothing))
            End Try
        End Sub

        Private _tcpReader As TcpReader
        Private _readCompleted As Action(Of Exception, TcpReader)
        Private _readBuf As Byte()
        Private _readPos As Integer
        Private _leftToRead As Integer
        Private _lastReadBlock As Boolean

        Private Sub BeginReadRecordMark()
            _readBuf = New Byte(3) {}
            _readPos = 0
            _leftToRead = 4
            SafeBeginRead(New AsyncCallback(AddressOf EndReadRecordMark))
        End Sub

        Private Sub EndReadRecordMark(ar As IAsyncResult)
            Try
                Dim read As Integer

                SyncLock _sync
                    If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                    read = _stream.EndRead(ar)
                End SyncLock

                Log.Debug("read {0} bytes of Record Mark", read)
                If read <= 0 Then Throw New EndOfStreamException()
                _leftToRead -= read
                _readPos += read

                If _leftToRead <= 0 Then
                    ExtractRecordMark()
                    SafeBeginRead(New AsyncCallback(AddressOf EndReadBody))
                Else
                    SafeBeginRead(New AsyncCallback(AddressOf EndReadRecordMark))
                End If

            Catch ex As Exception
                _readCompleted(ex, Nothing)
                _tcpReader = Nothing
                _readCompleted = Nothing
                _readBuf = Nothing
            End Try
        End Sub

        Private Sub SafeBeginRead(callback As AsyncCallback)
            SyncLock _sync
                If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                _stream.BeginRead(_readBuf, _readPos, _leftToRead, callback, Nothing)
            End SyncLock
        End Sub

        Private Sub EndReadBody(ar As IAsyncResult)
            Dim [error] As Exception = Nothing

            Try
                Dim read As Integer

                SyncLock _sync
                    If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                    read = _stream.EndRead(ar)
                End SyncLock

                Log.Debug("read {0} bytes of body", read)
                If read <= 0 Then Throw New EndOfStreamException()
                _leftToRead -= read
                _readPos += read

                If _leftToRead <= 0 Then
                    Call Log.Trace(New Func(Of String, Byte(), String)(AddressOf DumpToLog), "received byte dump: {0}", _readBuf)
                    _tcpReader.AppendBlock(_readBuf)

                    If Not _lastReadBlock Then
                        Log.Debug("body readed", read)
                        BeginReadRecordMark()
                        Return
                    End If
                Else
                    SafeBeginRead(New AsyncCallback(AddressOf EndReadBody))
                    Return
                End If

            Catch ex As Exception
                [error] = ex
            End Try

            If [error] IsNot Nothing Then _tcpReader = Nothing
            _readCompleted([error], _tcpReader)
            _tcpReader = Nothing
            _readCompleted = Nothing
            _readBuf = Nothing
        End Sub

        Private Sub ExtractRecordMark()
            _leftToRead = (_readBuf(0) And &H7F) << &H18 Or _readBuf(1) << &H10 Or _readBuf(2) << &H08 Or _readBuf(3)
            _readBuf = New Byte(_leftToRead - 1) {}
            _readPos = 0
            _lastReadBlock = (_readBuf(0) And &H80) = 0
            Log.Debug("read Record Mark lenght: {0} is last: {1}", _leftToRead, _lastReadBlock)
        End Sub

#End Region

#Region "async write"

        Public Sub AsyncWrite(blocks As LinkedList(Of Byte()), completed As Action(Of Exception))
            'HACK: here you need to implement a timeout interrupt
            Try
                If _writeCompleted IsNot Nothing Then Throw New InvalidOperationException("already writing")
                _writeBlocks = blocks
                _writeCompleted = completed
                If _writeBlocks.First Is Nothing Then Throw New ArgumentException("blocks is empty")
                Dim block = _writeBlocks.First.Value
                _byteSending = block.Length
                _writeBlocks.RemoveFirst()
                Call Log.Trace(New Func(Of String, Byte(), String)(AddressOf DumpToLog), "sending byte dump: {0}", block)
                SafeBeginWrite(block)
            Catch ex As Exception
                _writeCompleted = Nothing
                _writeBlocks = Nothing
                ThreadPool.QueueUserWorkItem(Sub(state) completed(ex))
            End Try
        End Sub

        Private _writeBlocks As LinkedList(Of Byte())
        Private _writeCompleted As Action(Of Exception)
        Private _byteSending As Integer = 0

        Private Sub EndWrite(ar As IAsyncResult)
            Dim [error] As Exception = Nothing

            Try

                SyncLock _sync
                    If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                    _stream.EndWrite(ar)
                End SyncLock

                Log.Debug("sended {0} bytes", _byteSending)

                If _writeBlocks.First IsNot Nothing Then
                    Dim block = _writeBlocks.First.Value
                    _byteSending = block.Length
                    _writeBlocks.RemoveFirst()
                    SafeBeginWrite(block)
                    Return
                End If

            Catch ex As Exception
                [error] = ex
            End Try

            _writeBlocks = Nothing
            _writeCompleted([error])
            _writeCompleted = Nothing
        End Sub

        Private Sub SafeBeginWrite(block As Byte())
            SyncLock _sync
                If _disposed Then Throw New ObjectDisposedException(GetType(TcpClient).FullName)
                _stream.BeginWrite(block, 0, block.Length, New AsyncCallback(AddressOf EndWrite), Nothing)
            End SyncLock
        End Sub

#End Region

        Public Sub Close()
            SyncLock _sync
                If _disposed Then Return
                _disposed = True
                _client.Close()
            End SyncLock
        End Sub
    End Class
End Namespace
