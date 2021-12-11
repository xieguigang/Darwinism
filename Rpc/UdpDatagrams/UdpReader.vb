#Region "Microsoft.VisualBasic::fefc384bbb2422b477c4270de9234f57, Rpc\UdpDatagrams\UdpReader.vb"

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

    '     Class UdpReader
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: (+2 Overloads) Read, UnexpectedEnd
    ' 
    '         Sub: CheckEmpty
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System
Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.UdpDatagrams
    ''' <summary>
    ''' parser of RPC message received from UDP protocol
    ''' </summary>
    Public Class UdpReader
        Implements IByteReader
        Implements IMsgReader

        Private _pos As Integer = 0
        Private _leftToRead As Integer
        Private _bytes As Byte() = Nothing

        ''' <summary>
        ''' parser of RPC message received from UDP protocol
        ''' </summary>
        ''' <param name="bytes">UDP datagram</param>
        Public Sub New(bytes As Byte())
            _pos = 0
            _leftToRead = bytes.Length
            _bytes = bytes
        End Sub

        ''' <summary>
        ''' read an array of length 'count' bytes
        ''' </summary>
        ''' <param name="count">required bytes</param>
        ''' <returns></returns>
        Public Function Read(count As UInteger) As Byte() Implements IByteReader.Read
            If _leftToRead < count Then Throw UnexpectedEnd()
            Dim icount As Integer = count
            Dim result = New Byte(count - 1) {}
            Array.Copy(_bytes, _pos, result, 0, count)
            _pos += icount
            _leftToRead -= icount
            Return result
        End Function

        ''' <summary>
        ''' read one byte
        ''' </summary>
        ''' <returns></returns>
        Public Function Read() As Byte Implements IByteReader.Read
            If _leftToRead < 1 Then Throw UnexpectedEnd()
            Dim result = _bytes(_pos)
            _pos += 1
            _leftToRead -= 1
            Return result
        End Function

        Private Shared Function UnexpectedEnd() As Exception
            Return New RpcException("unexpected end of RPC message")
        End Function

        ''' <summary>
        ''' check the completeness of parsing
        ''' </summary>
        Public Sub CheckEmpty() Implements IMsgReader.CheckEmpty
            If _leftToRead > 0 Then Throw New RpcException("RPC message parsed not completely")
        End Sub
    End Class
End Namespace
