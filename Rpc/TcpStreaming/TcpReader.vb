#Region "Microsoft.VisualBasic::b43a3952755b4a3c1792e6daecb89e40, Rpc\TcpStreaming\TcpReader.vb"

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

'     Class TcpReader
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: (+2 Overloads) Read, UnexpectedEnd
' 
'         Sub: AppendBlock, CheckEmpty, NextBlock
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic
Imports Microsoft.VisualBasic.Data.IO

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

        Public ReadOnly Property EndOfStream As Boolean Implements IByteReader.EndOfStream
            Get
                Return _currentBlock Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' parser of RPC message received from TCP protocol
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' append the block of RPC message received from TCP protocol
        ''' </summary>
        Public Sub AppendBlock(block As Byte())
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
        ''' <param name="count">required bytes</param>
        ''' <returns></returns>
        Public Function Read(count As UInteger) As Byte() Implements IByteReader.Read
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
