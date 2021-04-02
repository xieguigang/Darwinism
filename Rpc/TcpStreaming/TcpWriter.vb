#Region "Microsoft.VisualBasic::f46feb0257e6cbe942519e39c1c57801, Rpc\TcpStreaming\TcpWriter.vb"

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

    '     Class TcpWriter
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: Build
    ' 
    '         Sub: CreateNextBlock, SetLastBlock, SetLenth, (+2 Overloads) Write
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports System.Collections.Generic
Imports Xdr

Namespace Rpc.TcpStreaming
    ''' <summary>
    ''' generator TCP messages with record mark
    ''' http://tools.ietf.org/html/rfc5531#section-11
    ''' </summary>
    Public Class TcpWriter
        Implements IByteWriter

        Private ReadOnly _maxBlock As Integer
        Private _pos As Long
        Private _currentBlock As Byte()
        Private _blocks As LinkedList(Of Byte())

        ''' <summary>
        ''' generator TCP messages with record mark
        ''' </summary>
        ''' <paramname="maxBlock">maximum block size in the TCP message</param>
        Public Sub New(ByVal maxBlock As Integer)
            _maxBlock = maxBlock
            _pos = 4
            _currentBlock = New Byte(_maxBlock - 1) {}
            _blocks = New LinkedList(Of Byte())()
        End Sub

        ''' <summary>
        ''' write array of bytes
        ''' </summary>
        ''' <paramname="buffer"></param>
        Public Sub Write(ByVal buffer As Byte()) Implements IByteWriter.Write
            Dim offset As Long = 0

            While True
                Dim len = buffer.LongLength - offset

                If len <= _maxBlock - _pos Then
                    Array.Copy(buffer, offset, _currentBlock, _pos, len)
                    _pos += len
                    If _pos >= _maxBlock Then CreateNextBlock()
                    Return
                End If

                Array.Copy(buffer, offset, _currentBlock, _pos, _maxBlock - _pos)
                offset += _maxBlock - _pos
                CreateNextBlock()
            End While
        End Sub

        ''' <summary>
        ''' write byte
        ''' </summary>
        ''' <paramname="b"></param>
        Public Sub Write(ByVal b As Byte) Implements IByteWriter.Write
            _currentBlock(_pos) = b
            _pos += 1
            If _pos >= _maxBlock Then CreateNextBlock()
        End Sub

        Private Sub CreateNextBlock()
            SetLenth(_currentBlock)
            _blocks.AddLast(_currentBlock)
            _currentBlock = New Byte(_maxBlock - 1) {}
            _pos = 4
        End Sub

        Private Sub SetLastBlock()
            Dim last = _blocks.Last
            If last Is Nothing Then Return
            Dim block = last.Value
            block(0) = CByte(block(0) Or &H80)
        End Sub

        Private Sub SetLenth(ByVal block As Byte())
            Dim len = block.Length - 4
            block(0) = CByte(len >> &H18 And &HfF)
            block(1) = CByte(len >> &H10 And &HfF)
            block(2) = CByte(len >> 8 And &HfF)
            block(3) = CByte(len And &HfF)
        End Sub

        ''' <summary>
        ''' create the TCP message (the original object is destroyed)
        ''' </summary>
        ''' <returns>blocks of TCP message</returns>
        Public Function Build() As LinkedList(Of Byte())
            If _pos <> 4 Then ' _currentBlock is not empty
                Dim shortBlock = New Byte(_pos - 1) {}
                Array.Copy(_currentBlock, shortBlock, _pos)
                SetLenth(shortBlock)
                _blocks.AddLast(shortBlock)
                _pos = 4
            End If

            SetLastBlock()
            Dim result = _blocks
            _currentBlock = Nothing
            _blocks = Nothing
            Return result
        End Function
    End Class
End Namespace

