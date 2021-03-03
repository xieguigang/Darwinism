Imports System
Imports Xdr

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
        ''' <paramname="bytes">UDP datagram</param>
        Public Sub New(ByVal bytes As Byte())
            _pos = 0
            _leftToRead = bytes.Length
            _bytes = bytes
        End Sub

        ''' <summary>
        ''' read an array of length 'count' bytes
        ''' </summary>
        ''' <paramname="count">required bytes</param>
        ''' <returns></returns>
        Public Function Read(ByVal count As UInteger) As Byte() Implements IByteReader.Read
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
