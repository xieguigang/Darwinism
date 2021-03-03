Imports System
Imports System.Collections.Generic
Imports Xdr

Namespace Rpc.TcpStreaming
    ''' <summary>
    ''' parser of RPC message received from TCP protocol
    ''' </summary>
    Public Class TcpReader
        Implements IByteReader
        Implements IMsgReader

        Private _blocks As LinkedList(Of Byte()) = New LinkedList(Of Byte())()
        Private _pos As Long = 0
        Private _currentBlock As Byte() = Nothing
        Private _currentBlockSize As Long = 0

        ''' <summary>
        ''' parser of RPC message received from TCP protocol
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' append the block of RPC message received from TCP protocol
        ''' </summary>
        Public Sub AppendBlock(ByVal block As Byte())
            If _currentBlock Is Nothing Then
                _currentBlock = block
                _currentBlockSize = _currentBlock.LongLength
            Else
                _blocks.AddLast(block)
            End If
        End Sub

        Private Sub NextBlock()
            _pos = 0

            If _blocks.First Is Nothing Then
                _currentBlock = Nothing
                _currentBlockSize = 0
            Else
                _currentBlock = _blocks.First.Value
                _currentBlockSize = _currentBlock.LongLength
                _blocks.RemoveFirst()
            End If
        End Sub

        ''' <summary>
        ''' read an array of length 'count' bytes
        ''' </summary>
        ''' <paramname="count">required bytes</param>
        ''' <returns></returns>
        Public Function Read(ByVal count As UInteger) As Byte() Implements IByteReader.Read
            Dim buffer = New Byte(count - 1) {}
            Dim offset As Long = 0

            While True
                If _currentBlock Is Nothing Then Throw UnexpectedEnd()
                Dim len = count - offset

                If len <= _currentBlockSize - _pos Then
                    Array.Copy(_currentBlock, _pos, buffer, offset, len)
                    _pos += len
                    If _pos >= _currentBlockSize Then NextBlock()
                    Return buffer
                End If

                Array.Copy(_currentBlock, _pos, buffer, offset, _currentBlockSize - _pos)
                offset += _currentBlockSize - _pos
                NextBlock()
            End While

            Throw New Exception("This exception will never happends!")
        End Function

        ''' <summary>
        ''' read one byte
        ''' </summary>
        Public Function Read() As Byte Implements IByteReader.Read
            If _currentBlock Is Nothing Then Throw UnexpectedEnd()
            Dim result = _currentBlock(_pos)
            _pos += 1
            If _pos >= _currentBlockSize Then NextBlock()
            Return result
        End Function

        Private Shared Function UnexpectedEnd() As Exception
            Return New RpcException("unexpected end of RPC message")
        End Function

        ''' <summary>
        ''' check the completeness of parsing
        ''' </summary>
        Public Sub CheckEmpty() Implements IMsgReader.CheckEmpty
            If _currentBlock IsNot Nothing Then
                Throw New RpcException("RPC message parsed not completely")
            End If
        End Sub
    End Class
End Namespace
