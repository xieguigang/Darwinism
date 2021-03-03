Imports System
Imports System.Collections.Generic
Imports Xdr

Namespace Rpc.UdpDatagrams
    ''' <summary>
    ''' generator of UDP datagram
    ''' </summary>
    Public Class UdpWriter
        Implements IByteWriter

        Private Const _max As Integer = 65535
        Private Const _blockSize As Integer = 1024 * 4 ' 4k
        Private _pos As Integer
        Private _totalSize As Integer
        Private _currentBlock As Byte()
        Private _blocks As LinkedList(Of Byte())

        ''' <summary>
        ''' generator of UDP datagram
        ''' </summary>
        Public Sub New()
            _pos = 0
            _totalSize = 0
            _currentBlock = New Byte(4095) {}
            _blocks = New LinkedList(Of Byte())()
        End Sub

        ''' <summary>
        ''' write array of bytes
        ''' </summary>
        ''' <paramname="buffer"></param>
        Public Sub Write(ByVal buffer As Byte()) Implements IByteWriter.Write
            _totalSize += buffer.Length
            If _totalSize > _max Then Throw SizeIsExceeded()
            Dim offset = 0

            While True
                Dim len = buffer.Length - offset

                If len <= _blockSize - _pos Then
                    Array.Copy(buffer, offset, _currentBlock, _pos, len)
                    _pos += len
                    If _pos >= _blockSize Then CreateNextBlock()
                    Return
                End If

                Array.Copy(buffer, offset, _currentBlock, _pos, _blockSize - _pos)
                offset += _blockSize - _pos
                CreateNextBlock()
            End While
        End Sub

        ''' <summary>
        ''' write byte
        ''' </summary>
        ''' <paramname="b"></param>
        Public Sub Write(ByVal b As Byte) Implements IByteWriter.Write
            _totalSize += 1
            If _totalSize > _max Then Throw SizeIsExceeded()
            _currentBlock(_pos) = b
            _pos += 1
            If _pos >= _blockSize Then CreateNextBlock()
        End Sub

        Private Sub CreateNextBlock()
            _blocks.AddLast(_currentBlock)
            _currentBlock = New Byte(4095) {}
            _pos = 0
        End Sub

        Private Shared Function SizeIsExceeded() As Exception
            Return New RpcException("UDP datagram size is exceeded")
        End Function

        ''' <summary>
        ''' create the UDP datagram (the original object is destroyed)
        ''' </summary>
        ''' <returns>UDP datagram</returns>
        Public Function Build() As Byte()
            Dim result = New Byte(_totalSize - 1) {}
            Dim offset = 0

            For Each block In _blocks
                Array.Copy(block, 0, result, offset, _blockSize)
                offset += _blockSize
            Next

            If _pos <> 0 Then Array.Copy(_currentBlock, 0, result, offset, _pos) ' _currentBlock is not empty
            _currentBlock = Nothing
            _blocks = Nothing
            Return result
        End Function
    End Class
End Namespace
