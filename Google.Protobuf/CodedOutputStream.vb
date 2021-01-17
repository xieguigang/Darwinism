#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2008 Google Inc.  All rights reserved.
' https://developers.google.com/protocol-buffers/
'
' Redistribution and use in source and binary forms, with or without
' modification, are permitted provided that the following conditions are
' met:
'
'     * Redistributions of source code must retain the above copyright
' notice, this list of conditions and the following disclaimer.
'     * Redistributions in binary form must reproduce the above
' copyright notice, this list of conditions and the following disclaimer
' in the documentation and/or other materials provided with the
' distribution.
'     * Neither the name of Google Inc. nor the names of its
' contributors may be used to endorse or promote products derived from
' this software without specific prior written permission.
'
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#End Region

Imports System
Imports System.IO
Imports System.Text

Namespace Google.Protobuf
    ''' <summary>
    ''' Encodes and writes protocol message fields.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' This class is generally used by generated code to write appropriate
    ''' primitives to the stream. It effectively encapsulates the lowest
    ''' levels of protocol buffer format. Unlike some other implementations,
    ''' this does not include combined "write tag and value" methods. Generated
    ''' code knows the exact byte representations of the tags they're going to write,
    ''' so there's no need to re-encode them each time. Manually-written code calling
    ''' this class should just call one of the <c>WriteTag</c> overloads before each value.
    ''' </para>
    ''' <para>
    ''' Repeated fields and map fields are not handled by this class; use <c>RepeatedField&lt;T&gt;</c>
    ''' and <c>MapField&lt;TKey, TValue&gt;</c> to serialize such fields.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Partial Class CodedOutputStream
        Implements IDisposable
        ' "Local" copy of Encoding.UTF8, for efficiency. (Yes, it makes a difference.)
        Friend Shared ReadOnly Utf8Encoding As Encoding = Encoding.UTF8

        ''' <summary>
        ''' The buffer size used by CreateInstance(Stream).
        ''' </summary>
        Public Shared ReadOnly DefaultBufferSize As Integer = 4096
        Private ReadOnly leaveOpen As Boolean
        Private ReadOnly buffer As Byte()
        Private ReadOnly limit As Integer
        Private positionField As Integer
        Private ReadOnly output As Stream

#Region "Construction"
        ''' <summary>
        ''' Creates a new CodedOutputStream that writes directly to the given
        ''' byte array. If more bytes are written than fit in the array,
        ''' OutOfSpaceException will be thrown.
        ''' </summary>
        Public Sub New(flatArray As Byte())
            Me.New(flatArray, 0, flatArray.Length)
        End Sub

        ''' <summary>
        ''' Creates a new CodedOutputStream that writes directly to the given
        ''' byte array slice. If more bytes are written than fit in the array,
        ''' OutOfSpaceException will be thrown.
        ''' </summary>
        Private Sub New(buffer As Byte(), offset As Integer, length As Integer)
            output = Nothing
            Me.buffer = buffer
            positionField = offset
            limit = offset + length
            leaveOpen = True ' Simple way of avoiding trying to dispose of a null reference
        End Sub

        Private Sub New(output As Stream, buffer As Byte(), leaveOpen As Boolean)
            Me.output = CheckNotNull(output, NameOf(output))
            Me.buffer = buffer
            positionField = 0
            limit = buffer.Length
            Me.leaveOpen = leaveOpen
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="CodedOutputStream"/> which write to the given stream, and disposes of that
        ''' stream when the returned <c>CodedOutputStream</c> is disposed.
        ''' </summary>
        ''' <param name="output">The stream to write to. It will be disposed when the returned <c>CodedOutputStream is disposed.</c></param>
        Public Sub New(output As Stream)
            Me.New(output, DefaultBufferSize, False)
        End Sub

        ''' <summary>
        ''' Creates a new CodedOutputStream which write to the given stream and uses
        ''' the specified buffer size.
        ''' </summary>
        ''' <param name="output">The stream to write to. It will be disposed when the returned <c>CodedOutputStream is disposed.</c></param>
        ''' <param name="bufferSize">The size of buffer to use internally.</param>
        Public Sub New(output As Stream, bufferSize As Integer)
            Me.New(output, New Byte(bufferSize - 1) {}, False)
        End Sub

        ''' <summary>
        ''' Creates a new CodedOutputStream which write to the given stream.
        ''' </summary>
        ''' <param name="output">The stream to write to.</param>
        ''' <param name="leaveOpen">If <c>true</c>, <paramrefname="output"/> is left open when the returned <c>CodedOutputStream</c> is disposed;
        ''' if <c>false</c>, the provided stream is disposed as well.</param>
        Public Sub New(output As Stream, leaveOpen As Boolean)
            Me.New(output, DefaultBufferSize, leaveOpen)
        End Sub

        ''' <summary>
        ''' Creates a new CodedOutputStream which write to the given stream and uses
        ''' the specified buffer size.
        ''' </summary>
        ''' <param name="output">The stream to write to.</param>
        ''' <param name="bufferSize">The size of buffer to use internally.</param>
        ''' <param name="leaveOpen">If <c>true</c>, <paramrefname="output"/> is left open when the returned <c>CodedOutputStream</c> is disposed;
        ''' if <c>false</c>, the provided stream is disposed as well.</param>
        Public Sub New(output As Stream, bufferSize As Integer, leaveOpen As Boolean)
            Me.New(output, New Byte(bufferSize - 1) {}, leaveOpen)
        End Sub
#End Region

        ''' <summary>
        ''' Returns the current position in the stream, or the position in the output buffer
        ''' </summary>
        Public ReadOnly Property Position As Long
            Get

                If output IsNot Nothing Then
                    Return output.Position + positionField
                End If

                Return positionField
            End Get
        End Property

#Region "Writing of values (not including tags)"

        ''' <summary>
        ''' Writes a double field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteDouble(value As Double)
            WriteRawLittleEndian64(BitConverter.DoubleToInt64Bits(value))
        End Sub

        ''' <summary>
        ''' Writes a float field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteFloat(value As Single)
            Dim rawBytes = BitConverter.GetBytes(value)

            If Not BitConverter.IsLittleEndian Then
                Reverse(rawBytes)
            End If

            If limit - positionField >= 4 Then
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = rawBytes(0)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = rawBytes(1)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = rawBytes(2)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = rawBytes(3)
            Else
                WriteRawBytes(rawBytes, 0, 4)
            End If
        End Sub

        ''' <summary>
        ''' Writes a uint64 field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteUInt64(value As ULong)
            WriteRawVarint64(value)
        End Sub

        ''' <summary>
        ''' Writes an int64 field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteInt64(value As Long)
            WriteRawVarint64(value)
        End Sub

        ''' <summary>
        ''' Writes an int32 field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteInt32(value As Integer)
            If value >= 0 Then
                WriteRawVarint32(value)
            Else
                ' Must sign-extend.
                WriteRawVarint64(value)
            End If
        End Sub

        ''' <summary>
        ''' Writes a fixed64 field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteFixed64(value As ULong)
            WriteRawLittleEndian64(value)
        End Sub

        ''' <summary>
        ''' Writes a fixed32 field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteFixed32(value As UInteger)
            WriteRawLittleEndian32(value)
        End Sub

        ''' <summary>
        ''' Writes a bool field value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteBool(value As Boolean)
            WriteRawByte(If(value, CByte(1), CByte(0)))
        End Sub

        ''' <summary>
        ''' Writes a string field value, without a tag, to the stream.
        ''' The data is length-prefixed.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteString(value As String)
            ' Optimise the case where we have enough space to write
            ' the string directly to the buffer, which should be common.
            Dim length = Utf8Encoding.GetByteCount(value)
            WriteLength(length)

            If limit - positionField >= length Then
                If length = value.Length Then ' Must be all ASCII...
                    For i = 0 To length - 1
                        buffer(positionField + i) = Microsoft.VisualBasic.AscW(value(i))
                    Next
                Else
                    Utf8Encoding.GetBytes(value, 0, value.Length, buffer, positionField)
                End If

                positionField += length
            Else
                Dim bytes = Utf8Encoding.GetBytes(value)
                WriteRawBytes(bytes)
            End If
        End Sub

        ''' <summary>
        ''' Writes a message, without a tag, to the stream.
        ''' The data is length-prefixed.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteMessage(value As IMessage)
            WriteLength(value.CalculateSize())
            value.WriteTo(Me)
        End Sub

        ''' <summary>
        ''' Write a byte string, without a tag, to the stream.
        ''' The data is length-prefixed.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteBytes(value As ByteString)
            WriteLength(value.Length)
            value.WriteRawBytesTo(Me)
        End Sub

        ''' <summary>
        ''' Writes a uint32 value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteUInt32(value As UInteger)
            WriteRawVarint32(value)
        End Sub

        ''' <summary>
        ''' Writes an enum value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteEnum(value As Integer)
            WriteInt32(value)
        End Sub

        ''' <summary>
        ''' Writes an sfixed32 value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write.</param>
        Public Sub WriteSFixed32(value As Integer)
            WriteRawLittleEndian32(value)
        End Sub

        ''' <summary>
        ''' Writes an sfixed64 value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteSFixed64(value As Long)
            WriteRawLittleEndian64(value)
        End Sub

        ''' <summary>
        ''' Writes an sint32 value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteSInt32(value As Integer)
            WriteRawVarint32(EncodeZigZag32(value))
        End Sub

        ''' <summary>
        ''' Writes an sint64 value, without a tag, to the stream.
        ''' </summary>
        ''' <param name="value">The value to write</param>
        Public Sub WriteSInt64(value As Long)
            WriteRawVarint64(EncodeZigZag64(value))
        End Sub

        ''' <summary>
        ''' Writes a length (in bytes) for length-delimited data.
        ''' </summary>
        ''' <remarks>
        ''' This method simply writes a rawint, but exists for clarity in calling code.
        ''' </remarks>
        ''' <param name="length">Length value, in bytes.</param>
        Public Sub WriteLength(length As Integer)
            WriteRawVarint32(length)
        End Sub

#End Region

#Region "Raw tag writing"
        ''' <summary>
        ''' Encodes and writes a tag.
        ''' </summary>
        ''' <param name="fieldNumber">The number of the field to write the tag for</param>
        ''' <param name="type">The wire format type of the tag to write</param>
        Public Sub WriteTag(fieldNumber As Integer, type As WireType)
            WriteRawVarint32(MakeTag(fieldNumber, type))
        End Sub

        ''' <summary>
        ''' Writes an already-encoded tag.
        ''' </summary>
        ''' <param name="tag">The encoded tag</param>
        Public Sub WriteTag(tag As UInteger)
            WriteRawVarint32(tag)
        End Sub

        ''' <summary>
        ''' Writes the given single-byte tag directly to the stream.
        ''' </summary>
        ''' <param name="b1">The encoded tag</param>
        Public Sub WriteRawTag(b1 As Byte)
            WriteRawByte(b1)
        End Sub

        ''' <summary>
        ''' Writes the given two-byte tag directly to the stream.
        ''' </summary>
        ''' <param name="b1">The first byte of the encoded tag</param>
        ''' <param name="b2">The second byte of the encoded tag</param>
        Public Sub WriteRawTag(b1 As Byte, b2 As Byte)
            WriteRawByte(b1)
            WriteRawByte(b2)
        End Sub

        ''' <summary>
        ''' Writes the given three-byte tag directly to the stream.
        ''' </summary>
        ''' <param name="b1">The first byte of the encoded tag</param>
        ''' <param name="b2">The second byte of the encoded tag</param>
        ''' <param name="b3">The third byte of the encoded tag</param>
        Public Sub WriteRawTag(b1 As Byte, b2 As Byte, b3 As Byte)
            WriteRawByte(b1)
            WriteRawByte(b2)
            WriteRawByte(b3)
        End Sub

        ''' <summary>
        ''' Writes the given four-byte tag directly to the stream.
        ''' </summary>
        ''' <param name="b1">The first byte of the encoded tag</param>
        ''' <param name="b2">The second byte of the encoded tag</param>
        ''' <param name="b3">The third byte of the encoded tag</param>
        ''' <param name="b4">The fourth byte of the encoded tag</param>
        Public Sub WriteRawTag(b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte)
            WriteRawByte(b1)
            WriteRawByte(b2)
            WriteRawByte(b3)
            WriteRawByte(b4)
        End Sub

        ''' <summary>
        ''' Writes the given five-byte tag directly to the stream.
        ''' </summary>
        ''' <param name="b1">The first byte of the encoded tag</param>
        ''' <param name="b2">The second byte of the encoded tag</param>
        ''' <param name="b3">The third byte of the encoded tag</param>
        ''' <param name="b4">The fourth byte of the encoded tag</param>
        ''' <param name="b5">The fifth byte of the encoded tag</param>
        Public Sub WriteRawTag(b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte, b5 As Byte)
            WriteRawByte(b1)
            WriteRawByte(b2)
            WriteRawByte(b3)
            WriteRawByte(b4)
            WriteRawByte(b5)
        End Sub
#End Region

#Region "Underlying writing primitives"
        ''' <summary>
        ''' Writes a 32 bit value as a varint. The fast route is taken when
        ''' there's enough buffer space left to whizz through without checking
        ''' for each byte; otherwise, we resort to calling WriteRawByte each time.
        ''' </summary>
        Friend Sub WriteRawVarint32(value As UInteger)
            ' Optimize for the common case of a single byte value
            If value < 128 AndAlso positionField < limit Then
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value)
                Return
            End If

            While value > 127 AndAlso positionField < limit
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value And &H7F Or &H80)
                value >>= 7
            End While

            While value > 127
                WriteRawByte(CByte(value And &H7F Or &H80))
                value >>= 7
            End While

            If positionField < limit Then
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value)
            Else
                WriteRawByte(CByte(value))
            End If
        End Sub

        Friend Sub WriteRawVarint64(value As ULong)
            While value > 127 AndAlso positionField < limit
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value And &H7F Or &H80)
                value >>= 7
            End While

            While value > 127
                WriteRawByte(CByte(value And &H7F Or &H80))
                value >>= 7
            End While

            If positionField < limit Then
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value)
            Else
                WriteRawByte(CByte(value))
            End If
        End Sub

        Friend Sub WriteRawLittleEndian32(value As UInteger)
            If positionField + 4 > limit Then
                WriteRawByte(CByte(value))
                WriteRawByte(CByte(value >> 8))
                WriteRawByte(CByte(value >> 16))
                WriteRawByte(CByte(value >> 24))
            Else
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 8)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 16)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 24)
            End If
        End Sub

        Friend Sub WriteRawLittleEndian64(value As ULong)
            If positionField + 8 > limit Then
                WriteRawByte(CByte(value))
                WriteRawByte(CByte(value >> 8))
                WriteRawByte(CByte(value >> 16))
                WriteRawByte(CByte(value >> 24))
                WriteRawByte(CByte(value >> 32))
                WriteRawByte(CByte(value >> 40))
                WriteRawByte(CByte(value >> 48))
                WriteRawByte(CByte(value >> 56))
            Else
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 8)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 16)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 24)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 32)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 40)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 48)
                buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = CByte(value >> 56)
            End If
        End Sub

        Friend Sub WriteRawByte(value As Byte)
            If positionField = limit Then
                RefreshBuffer()
            End If

            buffer(Math.Min(Threading.Interlocked.Increment(positionField), positionField - 1)) = value
        End Sub

        Friend Sub WriteRawByte(value As UInteger)
            WriteRawByte(CByte(value))
        End Sub

        ''' <summary>
        ''' Writes out an array of bytes.
        ''' </summary>
        Friend Sub WriteRawBytes(value As Byte())
            WriteRawBytes(value, 0, value.Length)
        End Sub

        ''' <summary>
        ''' Writes out part of an array of bytes.
        ''' </summary>
        Friend Sub WriteRawBytes(value As Byte(), offset As Integer, length As Integer)
            If limit - positionField >= length Then
                Copy(value, offset, buffer, positionField, length)
                ' We have room in the current buffer.
                positionField += length
            Else
                ' Write extends past current buffer.  Fill the rest of this buffer and
                ' flush.
                Dim bytesWritten = limit - positionField
                Copy(value, offset, buffer, positionField, bytesWritten)
                offset += bytesWritten
                length -= bytesWritten
                positionField = limit
                RefreshBuffer()

                ' Now deal with the rest.
                ' Since we have an output stream, this is our buffer
                ' and buffer offset == 0
                If length <= limit Then
                    ' Fits in new buffer.
                    Copy(value, offset, buffer, 0, length)
                    positionField = length
                Else
                    ' Write is very big.  Let's do it all at once.
                    output.Write(value, offset, length)
                End If
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Encode a 32-bit value with ZigZag encoding.
        ''' </summary>
        ''' <remarks>
        ''' ZigZag encodes signed integers into values that can be efficiently
        ''' encoded with varint.  (Otherwise, negative values must be 
        ''' sign-extended to 64 bits to be varint encoded, thus always taking
        ''' 10 bytes on the wire.)
        ''' </remarks>
        Friend Shared Function EncodeZigZag32(n As Integer) As UInteger
            ' Note:  the right-shift must be arithmetic
            Return n << 1 Xor n >> 31
        End Function

        ''' <summary>
        ''' Encode a 64-bit value with ZigZag encoding.
        ''' </summary>
        ''' <remarks>
        ''' ZigZag encodes signed integers into values that can be efficiently
        ''' encoded with varint.  (Otherwise, negative values must be 
        ''' sign-extended to 64 bits to be varint encoded, thus always taking
        ''' 10 bytes on the wire.)
        ''' </remarks>
        Friend Shared Function EncodeZigZag64(n As Long) As ULong
            Return n << 1 Xor n >> 63
        End Function

        Private Sub RefreshBuffer()
            If output Is Nothing Then
                ' We're writing to a single buffer.
                Throw New OutOfSpaceException()
            End If

            ' Since we have an output stream, this is our buffer
            ' and buffer offset == 0
            output.Write(buffer, 0, positionField)
            positionField = 0
        End Sub

        ''' <summary>
        ''' Indicates that a CodedOutputStream wrapping a flat byte array
        ''' ran out of space.
        ''' </summary>
        Public NotInheritable Class OutOfSpaceException
            Inherits IOException

            Friend Sub New()
                MyBase.New("CodedOutputStream was writing to a flat byte array and ran out of space.")
            End Sub
        End Class

        ''' <summary>
        ''' Flushes any buffered data and optionally closes the underlying stream, if any.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' By default, any underlying stream is closed by this method. To configure this behaviour,
        ''' use a constructor overload with a <c>leaveOpen</c> parameter. If this instance does not
        ''' have an underlying stream, this method does nothing.
        ''' </para>
        ''' <para>
        ''' For the sake of efficiency, calling this method does not prevent future write calls - but
        ''' if a later write ends up writing to a stream which has been disposed, that is likely to
        ''' fail. It is recommend that you not call any other methods after this.
        ''' </para>
        ''' </remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            Flush()

            If Not leaveOpen Then
                output.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' Flushes any buffered data to the underlying stream (if there is one).
        ''' </summary>
        Public Sub Flush()
            If output IsNot Nothing Then
                RefreshBuffer()
            End If
        End Sub

        ''' <summary>
        ''' Verifies that SpaceLeft returns zero. It's common to create a byte array
        ''' that is exactly big enough to hold a message, then write to it with
        ''' a CodedOutputStream. Calling CheckNoSpaceLeft after writing verifies that
        ''' the message was actually as big as expected, which can help bugs.
        ''' </summary>
        Public Sub CheckNoSpaceLeft()
            If SpaceLeft <> 0 Then
                Throw New InvalidOperationException("Did not write as much data as expected.")
            End If
        End Sub

        ''' <summary>
        ''' If writing to a flat array, returns the space left in the array. Otherwise,
        ''' throws an InvalidOperationException.
        ''' </summary>
        Public ReadOnly Property SpaceLeft As Integer
            Get

                If output Is Nothing Then
                    Return limit - positionField
                Else
                    Throw New InvalidOperationException("SpaceLeft can only be called on CodedOutputStreams that are " & "writing to a flat array.")
                End If
            End Get
        End Property
    End Class
End Namespace
