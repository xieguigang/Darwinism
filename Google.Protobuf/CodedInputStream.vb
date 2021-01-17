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
Imports System.Collections.Generic
Imports System.IO

Namespace Google.Protobuf
    ''' <summary>
    ''' Reads and decodes protocol message fields.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' This class is generally used by generated code to read appropriate
    ''' primitives from the stream. It effectively encapsulates the lowest
    ''' levels of protocol buffer format.
    ''' </para>
    ''' <para>
    ''' Repeated fields and map fields are not handled by this class; use <see cref="RepeatedField(Of T)"/>
    ''' and <see cref="MapField(Of TKey,TValue)"/> to serialize such fields.
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class CodedInputStream
        Implements IDisposable
        ''' <summary>
        ''' Whether to leave the underlying stream open when disposing of this stream.
        ''' This is always true when there's no stream.
        ''' </summary>
        Private ReadOnly leaveOpen As Boolean

        ''' <summary>
        ''' Buffer of data read from the stream or provided at construction time.
        ''' </summary>
        Private ReadOnly buffer As Byte()

        ''' <summary>
        ''' The index of the buffer at which we need to refill from the stream (if there is one).
        ''' </summary>
        Private bufferSizeField As Integer
        Private bufferSizeAfterLimit As Integer = 0
        ''' <summary>
        ''' The position within the current buffer (i.e. the next byte to read)
        ''' </summary>
        Private bufferPos As Integer = 0

        ''' <summary>
        ''' The stream to read further input from, or null if the byte array buffer was provided
        ''' directly on construction, with no further data available.
        ''' </summary>
        Private ReadOnly input As Stream

        ''' <summary>
        ''' The last tag we read. 0 indicates we've read to the end of the stream
        ''' (or haven't read anything yet).
        ''' </summary>
        Private lastTagField As UInteger = 0

        ''' <summary>
        ''' The next tag, used to store the value read by PeekTag.
        ''' </summary>
        Private nextTag As UInteger = 0
        Private hasNextTag As Boolean = False
        Friend Const DefaultRecursionLimit As Integer = 64
        Friend Const DefaultSizeLimit As Integer = 64 << 20 ' 64MB
        Friend Const BufferSize As Integer = 4096

        ''' <summary>
        ''' The total number of bytes read before the current buffer. The
        ''' total bytes read up to the current position can be computed as
        ''' totalBytesRetired + bufferPos.
        ''' </summary>
        Private totalBytesRetired As Integer = 0

        ''' <summary>
        ''' The absolute position of the end of the current message.
        ''' </summary> 
        Private currentLimit As Integer = Integer.MaxValue
        Private recursionDepth As Integer = 0
        Private ReadOnly recursionLimitField As Integer
        Private ReadOnly sizeLimitField As Integer

#Region "Construction"
        ' Note that the checks are performed such that we don't end up checking obviously-valid things
        ' like non-null references for arrays we've just created.

        ''' <summary>
        ''' Creates a new CodedInputStream reading data from the given byte array.
        ''' </summary>
        Public Sub New(buffer As Byte())
            Me.New(Nothing, CheckNotNull(buffer, "buffer"), 0, buffer.Length)
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="CodedInputStream"/> that reads from the given byte array slice.
        ''' </summary>
        Public Sub New(buffer As Byte(), offset As Integer, length As Integer)
            Me.New(Nothing, CheckNotNull(buffer, "buffer"), offset, offset + length)

            If offset < 0 OrElse offset > buffer.Length Then
                Throw New ArgumentOutOfRangeException("offset", "Offset must be within the buffer")
            End If

            If length < 0 OrElse offset + length > buffer.Length Then
                Throw New ArgumentOutOfRangeException("length", "Length must be non-negative and within the buffer")
            End If
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="CodedInputStream"/> reading data from the given stream, which will be disposed
        ''' when the returned object is disposed.
        ''' </summary>
        ''' <param name="input">The stream to read from.</param>
        Public Sub New(input As Stream)
            Me.New(input, False)
        End Sub

        ''' <summary>
        ''' Creates a new <see cref="CodedInputStream"/> reading data from the given stream.
        ''' </summary>
        ''' <param name="input">The stream to read from.</param>
        ''' <param name="leaveOpen"><c>true</c> to leave <paramrefname="input"/> open when the returned
        ''' <ccref="CodedInputStream"/> is disposed; <c>false</c> to dispose of the given stream when the
        ''' returned object is disposed.</param>
        Public Sub New(input As Stream, leaveOpen As Boolean)
            Me.New(CheckNotNull(input, "input"), New Byte(4095) {}, 0, 0)
            Me.leaveOpen = leaveOpen
        End Sub

        ''' <summary>
        ''' Creates a new CodedInputStream reading data from the given
        ''' stream and buffer, using the default limits.
        ''' </summary>
        Friend Sub New(input As Stream, buffer As Byte(), bufferPos As Integer, bufferSize As Integer)
            Me.input = input
            Me.buffer = buffer
            Me.bufferPos = bufferPos
            bufferSizeField = bufferSize
            sizeLimitField = DefaultSizeLimit
            recursionLimitField = DefaultRecursionLimit
        End Sub

        ''' <summary>
        ''' Creates a new CodedInputStream reading data from the given
        ''' stream and buffer, using the specified limits.
        ''' </summary>
        ''' <remarks>
        ''' This chains to the version with the default limits instead of vice versa to avoid
        ''' having to check that the default values are valid every time.
        ''' </remarks>
        Friend Sub New(input As Stream, buffer As Byte(), bufferPos As Integer, bufferSize As Integer, sizeLimit As Integer, recursionLimit As Integer)
            Me.New(input, buffer, bufferPos, bufferSize)

            If sizeLimit <= 0 Then
                Throw New ArgumentOutOfRangeException("sizeLimit", "Size limit must be positive")
            End If

            If recursionLimit <= 0 Then
                Throw New ArgumentOutOfRangeException("recursionLimit!", "Recursion limit must be positive")
            End If

            sizeLimitField = sizeLimit
            recursionLimitField = recursionLimit
        End Sub
#End Region

        ''' <summary>
        ''' Creates a <see cref="CodedInputStream"/> with the specified size and recursion limits, reading
        ''' from an input stream.
        ''' </summary>
        ''' <remarks>
        ''' This method exists separately from the constructor to reduce the number of constructor overloads.
        ''' It is likely to be used considerably less frequently than the constructors, as the default limits
        ''' are suitable for most use cases.
        ''' </remarks>
        ''' <param name="input">The input stream to read from</param>
        ''' <param name="sizeLimit">The total limit of data to read from the stream.</param>
        ''' <param name="recursionLimit">The maximum recursion depth to allow while reading.</param>
        ''' <returns>A <c>CodedInputStream</c> reading from <paramrefname="input"/> with the specified size
        ''' and recursion limits.</returns>
        Public Shared Function CreateWithLimits(input As Stream, sizeLimit As Integer, recursionLimit As Integer) As CodedInputStream
            Return New CodedInputStream(input, New Byte(4095) {}, 0, 0, sizeLimit, recursionLimit)
        End Function

        ''' <summary>
        ''' Returns the current position in the input stream, or the position in the input buffer
        ''' </summary>
        Public ReadOnly Property Position As Long
            Get

                If input IsNot Nothing Then
                    Return input.Position - (bufferSizeField + bufferSizeAfterLimit - bufferPos)
                End If

                Return bufferPos
            End Get
        End Property

        ''' <summary>
        ''' Returns the last tag read, or 0 if no tags have been read or we've read beyond
        ''' the end of the stream.
        ''' </summary>
        Friend ReadOnly Property LastTag As UInteger
            Get
                Return lastTagField
            End Get
        End Property

        ''' <summary>
        ''' Returns the size limit for this stream.
        ''' </summary>
        ''' <remarks>
        ''' This limit is applied when reading from the underlying stream, as a sanity check. It is
        ''' not applied when reading from a byte array data source without an underlying stream.
        ''' The default value is 64MB.
        ''' </remarks>
        ''' <value>
        ''' The size limit.
        ''' </value>
        Public ReadOnly Property SizeLimit As Integer
            Get
                Return sizeLimitField
            End Get
        End Property

        ''' <summary>
        ''' Returns the recursion limit for this stream. This limit is applied whilst reading messages,
        ''' to avoid maliciously-recursive data.
        ''' </summary>
        ''' <remarks>
        ''' The default limit is 64.
        ''' </remarks>
        ''' <value>
        ''' The recursion limit for this stream.
        ''' </value>
        Public ReadOnly Property RecursionLimit As Integer
            Get
                Return recursionLimitField
            End Get
        End Property

        ''' <summary>
        ''' Disposes of this instance, potentially closing any underlying stream.
        ''' </summary>
        ''' <remarks>
        ''' As there is no flushing to perform here, disposing of a <see cref="CodedInputStream"/> which
        ''' was constructed with the <c>leaveOpen</c> option parameter set to <c>true</c> (or one which
        ''' was constructed to read from a byte array) has no effect.
        ''' </remarks>
        Public Sub Dispose() Implements IDisposable.Dispose
            If Not leaveOpen Then
                input.Dispose()
            End If
        End Sub

#Region "Validation"
        ''' <summary>
        ''' Verifies that the last call to ReadTag() returned tag 0 - in other words,
        ''' we've reached the end of the stream when we expected to.
        ''' </summary>
        ''' <exception cref="InvalidProtocolBufferException">The 
        ''' tag read was not the one specified</exception>
        Friend Sub CheckReadEndOfStreamTag()
            If lastTagField <> 0 Then
                Throw InvalidProtocolBufferException.MoreDataAvailable()
            End If
        End Sub
#End Region

#Region "Reading of tags etc"

        ''' <summary>
        ''' Peeks at the next field tag. This is like calling <see cref="ReadTag"/>, but the
        ''' tag is not consumed. (So a subsequent call to <see cref="ReadTag"/> will return the
        ''' same value.)
        ''' </summary>
        Public Function PeekTag() As UInteger
            If hasNextTag Then
                Return nextTag
            End If

            Dim savedLast = lastTagField
            nextTag = ReadTag()
            hasNextTag = True
            lastTagField = savedLast ' Undo the side effect of ReadTag
            Return nextTag
        End Function

        ''' <summary>
        ''' Reads a field tag, returning the tag of 0 for "end of stream".
        ''' </summary>
        ''' <remarks>
        ''' If this method returns 0, it doesn't necessarily mean the end of all
        ''' the data in this CodedInputStream; it may be the end of the logical stream
        ''' for an embedded message, for example.
        ''' </remarks>
        ''' <returns>The next field tag, or 0 for end of stream. (0 is never a valid tag.)</returns>
        Public Function ReadTag() As UInteger
            If hasNextTag Then
                lastTagField = nextTag
                hasNextTag = False
                Return lastTagField
            End If

            ' Optimize for the incredibly common case of having at least two bytes left in the buffer,
            ' and those two bytes being enough to get the tag. This will be true for fields up to 4095.
            If bufferPos + 2 <= bufferSizeField Then
                Dim tmp As Integer = buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))

                If tmp < 128 Then
                    lastTagField = CUInt(tmp)
                Else
                    Dim result = tmp And &H7F

                    If CSharpImpl.__Assign(tmp, buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))) < 128 Then
                        result = result Or tmp << 7
                        lastTagField = CUInt(result)
                    Else
                        ' Nope, rewind and go the potentially slow route.
                        bufferPos -= 2
                        lastTagField = ReadRawVarint32()
                    End If
                End If
            Else

                If IsAtEnd Then
                    lastTagField = 0
                    Return 0 ' This is the only case in which we return 0.
                End If

                lastTagField = ReadRawVarint32()
            End If

            If lastTagField = 0 Then
                ' If we actually read zero, that's not a valid tag.
                Throw InvalidProtocolBufferException.InvalidTag()
            End If

            Return lastTagField
        End Function

        ''' <summary>
        ''' Skips the data for the field with the tag we've just read.
        ''' This should be called directly after <see cref="ReadTag"/>, when
        ''' the caller wishes to skip an unknown field.
        ''' </summary>
        ''' <remarks>
        ''' This method throws <see cref="InvalidProtocolBufferException"/> if the last-read tag was an end-group tag.
        ''' If a caller wishes to skip a group, they should skip the whole group, by calling this method after reading the
        ''' start-group tag. This behavior allows callers to call this method on any field they don't understand, correctly
        ''' resulting in an error if an end-group tag has not been paired with an earlier start-group tag.
        ''' </remarks>
        ''' <exception cref="InvalidProtocolBufferException">The last tag was an end-group tag</exception>
        ''' <exception cref="InvalidOperationException">The last read operation read to the end of the logical stream</exception>
        Public Sub SkipLastField()
            If lastTagField = 0 Then
                Throw New InvalidOperationException("SkipLastField cannot be called at the end of a stream")
            End If

            Select Case GetTagWireType(lastTagField)
                Case WireType.StartGroup
                    SkipGroup(lastTagField)
                Case WireType.EndGroup
                    Throw New InvalidProtocolBufferException("SkipLastField called on an end-group tag, indicating that the corresponding start-group was missing")
                Case WireType.Fixed32
                    ReadFixed32()
                Case WireType.Fixed64
                    ReadFixed64()
                Case WireType.LengthDelimited
                    Dim length = ReadLength()
                    SkipRawBytes(length)
                Case WireType.Varint
                    ReadRawVarint32()
            End Select
        End Sub

        Private Sub SkipGroup(startGroupTag As UInteger)
            ' Note: Currently we expect this to be the way that groups are read. We could put the recursion
            ' depth changes into the ReadTag method instead, potentially...
            recursionDepth += 1

            If recursionDepth >= recursionLimitField Then
                Throw InvalidProtocolBufferException.RecursionLimitExceeded()
            End If

            Dim tag As UInteger

            While True
                tag = ReadTag()

                If tag = 0 Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                End If
                ' Can't call SkipLastField for this case- that would throw.
                If GetTagWireType(tag) = WireType.EndGroup Then
                    Exit While
                End If
                ' This recursion will allow us to handle nested groups.
                SkipLastField()
            End While

            Dim startField = GetTagFieldNumber(startGroupTag)
            Dim endField = GetTagFieldNumber(tag)

            If startField <> endField Then
                Throw New InvalidProtocolBufferException($"Mismatched end-group tag. Started with field {startField}; ended with field {endField}")
            End If

            recursionDepth -= 1
        End Sub

        ''' <summary>
        ''' Reads a double field from the stream.
        ''' </summary>
        Public Function ReadDouble() As Double
            Return BitConverter.Int64BitsToDouble(CLng(ReadRawLittleEndian64()))
        End Function

        ''' <summary>
        ''' Reads a float field from the stream.
        ''' </summary>
        Public Function ReadFloat() As Single
            If BitConverter.IsLittleEndian AndAlso 4 <= bufferSizeField - bufferPos Then
                Dim ret = BitConverter.ToSingle(buffer, bufferPos)
                bufferPos += 4
                Return ret
            Else
                Dim rawBytes = ReadRawBytes(4)

                If Not BitConverter.IsLittleEndian Then
                    Reverse(rawBytes)
                End If

                Return BitConverter.ToSingle(rawBytes, 0)
            End If
        End Function

        ''' <summary>
        ''' Reads a uint64 field from the stream.
        ''' </summary>
        Public Function ReadUInt64() As ULong
            Return ReadRawVarint64()
        End Function

        ''' <summary>
        ''' Reads an int64 field from the stream.
        ''' </summary>
        Public Function ReadInt64() As Long
            Return CLng(ReadRawVarint64())
        End Function

        ''' <summary>
        ''' Reads an int32 field from the stream.
        ''' </summary>
        Public Function ReadInt32() As Integer
            Return CInt(ReadRawVarint32())
        End Function

        ''' <summary>
        ''' Reads a fixed64 field from the stream.
        ''' </summary>
        Public Function ReadFixed64() As ULong
            Return ReadRawLittleEndian64()
        End Function

        ''' <summary>
        ''' Reads a fixed32 field from the stream.
        ''' </summary>
        Public Function ReadFixed32() As UInteger
            Return ReadRawLittleEndian32()
        End Function

        ''' <summary>
        ''' Reads a bool field from the stream.
        ''' </summary>
        Public Function ReadBool() As Boolean
            Return ReadRawVarint32() <> 0
        End Function

        ''' <summary>
        ''' Reads a string field from the stream.
        ''' </summary>
        Public Function ReadString() As String
            Dim length As Integer = ReadLength()
            ' No need to read any data for an empty string.
            If length = 0 Then
                Return ""
            End If

            If length <= bufferSizeField - bufferPos Then
                ' Fast path:  We already have the bytes in a contiguous buffer, so
                '   just copy directly from it.
                Dim result = CodedOutputStream.Utf8Encoding.GetString(buffer, bufferPos, length)
                bufferPos += length
                Return result
            End If
            ' Slow path: Build a byte array first then copy it.
            Return CodedOutputStream.Utf8Encoding.GetString(ReadRawBytes(length), 0, length)
        End Function

        ''' <summary>
        ''' Reads an embedded message field value from the stream.
        ''' </summary>   
        Public Sub ReadMessage(builder As IMessage)
            Dim length As Integer = ReadLength()

            If recursionDepth >= recursionLimitField Then
                Throw InvalidProtocolBufferException.RecursionLimitExceeded()
            End If

            Dim oldLimit = PushLimit(length)
            Threading.Interlocked.Increment(recursionDepth)
            builder.MergeFrom(Me)
            CheckReadEndOfStreamTag()
            ' Check that we've read exactly as much data as expected.
            If Not ReachedLimit Then
                Throw InvalidProtocolBufferException.TruncatedMessage()
            End If

            Threading.Interlocked.Decrement(recursionDepth)
            PopLimit(oldLimit)
        End Sub

        ''' <summary>
        ''' Reads a bytes field value from the stream.
        ''' </summary>   
        Public Function ReadBytes() As ByteString
            Dim length As Integer = ReadLength()

            If length <= bufferSizeField - bufferPos AndAlso length > 0 Then
                ' Fast path:  We already have the bytes in a contiguous buffer, so
                '   just copy directly from it.
                Dim result = ByteString.CopyFrom(buffer, bufferPos, length)
                bufferPos += length
                Return result
            Else
                ' Slow path:  Build a byte array and attach it to a new ByteString.
                Return ByteString.AttachBytes(ReadRawBytes(length))
            End If
        End Function

        ''' <summary>
        ''' Reads a uint32 field value from the stream.
        ''' </summary>   
        Public Function ReadUInt32() As UInteger
            Return ReadRawVarint32()
        End Function

        ''' <summary>
        ''' Reads an enum field value from the stream.
        ''' </summary>   
        Public Function ReadEnum() As Integer
            ' Currently just a pass-through, but it's nice to separate it logically from WriteInt32.
            Return CInt(ReadRawVarint32())
        End Function

        ''' <summary>
        ''' Reads an sfixed32 field value from the stream.
        ''' </summary>   
        Public Function ReadSFixed32() As Integer
            Return CInt(ReadRawLittleEndian32())
        End Function

        ''' <summary>
        ''' Reads an sfixed64 field value from the stream.
        ''' </summary>   
        Public Function ReadSFixed64() As Long
            Return CLng(ReadRawLittleEndian64())
        End Function

        ''' <summary>
        ''' Reads an sint32 field value from the stream.
        ''' </summary>   
        Public Function ReadSInt32() As Integer
            Return DecodeZigZag32(ReadRawVarint32())
        End Function

        ''' <summary>
        ''' Reads an sint64 field value from the stream.
        ''' </summary>   
        Public Function ReadSInt64() As Long
            Return DecodeZigZag64(ReadRawVarint64())
        End Function

        ''' <summary>
        ''' Reads a length for length-delimited data.
        ''' </summary>
        ''' <remarks>
        ''' This is internally just reading a varint, but this method exists
        ''' to make the calling code clearer.
        ''' </remarks>
        Public Function ReadLength() As Integer
            Return CInt(ReadRawVarint32())
        End Function

        ''' <summary>
        ''' Peeks at the next tag in the stream. If it matches <paramrefname="tag"/>,
        ''' the tag is consumed and the method returns <c>true</c>; otherwise, the
        ''' stream is left in the original position and the method returns <c>false</c>.
        ''' </summary>
        Public Function MaybeConsumeTag(tag As UInteger) As Boolean
            If PeekTag() = tag Then
                hasNextTag = False
                Return True
            End If

            Return False
        End Function

#End Region

#Region "Underlying reading primitives"

        ''' <summary>
        ''' Same code as ReadRawVarint32, but read each byte individually, checking for
        ''' buffer overflow.
        ''' </summary>
        Private Function SlowReadRawVarint32() As UInteger
            Dim tmp As Integer = ReadRawByte()

            If tmp < 128 Then
                Return tmp
            End If

            Dim result = tmp And &H7F

            If (CSharpImpl.__Assign(tmp, ReadRawByte())) < 128 Then
                result = result Or tmp << 7
            Else
                result = result Or (tmp And &H7F) << 7

                If (CSharpImpl.__Assign(tmp, ReadRawByte())) < 128 Then
                    result = result Or tmp << 14
                Else
                    result = result Or (tmp And &H7F) << 14

                    If (CSharpImpl.__Assign(tmp, ReadRawByte())) < 128 Then
                        result = result Or tmp << 21
                    Else
                        result = result Or (tmp And &H7F) << 21
                        result = result Or (CSharpImpl.__Assign(tmp, ReadRawByte())) << 28

                        If tmp >= 128 Then
                            ' Discard upper 32 bits.
                            For i = 0 To 5 - 1

                                If ReadRawByte() < 128 Then
                                    Return result
                                End If
                            Next

                            Throw InvalidProtocolBufferException.MalformedVarint()
                        End If
                    End If
                End If
            End If

            Return result
        End Function

        ''' <summary>
        ''' Reads a raw Varint from the stream.  If larger than 32 bits, discard the upper bits.
        ''' This method is optimised for the case where we've got lots of data in the buffer.
        ''' That means we can check the size just once, then just read directly from the buffer
        ''' without constant rechecking of the buffer length.
        ''' </summary>
        Friend Function ReadRawVarint32() As UInteger
            If bufferPos + 5 > bufferSizeField Then
                Return SlowReadRawVarint32()
            End If

            Dim tmp As Integer = buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))

            If tmp < 128 Then
                Return tmp
            End If

            Dim result = tmp And &H7F

            If CSharpImpl.__Assign(tmp, buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))) < 128 Then
                result = result Or tmp << 7
            Else
                result = result Or (tmp And &H7F) << 7

                If CSharpImpl.__Assign(tmp, buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))) < 128 Then
                    result = result Or tmp << 14
                Else
                    result = result Or (tmp And &H7F) << 14

                    If CSharpImpl.__Assign(tmp, buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))) < 128 Then
                        result = result Or tmp << 21
                    Else
                        result = result Or (tmp And &H7F) << 21
                        result = result Or CSharpImpl.__Assign(tmp, buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))) << 28

                        If tmp >= 128 Then
                            ' Discard upper 32 bits.
                            ' Note that this has to use ReadRawByte() as we only ensure we've
                            ' got at least 5 bytes at the start of the method. This lets us
                            ' use the fast path in more cases, and we rarely hit this section of code.
                            For i = 0 To 5 - 1

                                If ReadRawByte() < 128 Then
                                    Return result
                                End If
                            Next

                            Throw InvalidProtocolBufferException.MalformedVarint()
                        End If
                    End If
                End If
            End If

            Return result
        End Function

        ''' <summary>
        ''' Reads a varint from the input one byte at a time, so that it does not
        ''' read any bytes after the end of the varint. If you simply wrapped the
        ''' stream in a CodedInputStream and used ReadRawVarint32(Stream)
        ''' then you would probably end up reading past the end of the varint since
        ''' CodedInputStream buffers its input.
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns></returns>
        Friend Shared Function ReadRawVarint32(input As Stream) As UInteger
            Dim result = 0
            Dim offset = 0

            While offset < 32
                Dim b As Integer = input.ReadByte()

                If b = -1 Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                End If

                result = result Or (b And &H7F) << offset

                If (b And &H80) = 0 Then
                    Return result
                End If

                offset += 7
            End While
            ' Keep reading up to 64 bits.
            While offset < 64
                Dim b As Integer = input.ReadByte()

                If b = -1 Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                End If

                If (b And &H80) = 0 Then
                    Return result
                End If

                offset += 7
            End While

            Throw InvalidProtocolBufferException.MalformedVarint()
        End Function

        ''' <summary>
        ''' Reads a raw varint from the stream.
        ''' </summary>
        Friend Function ReadRawVarint64() As ULong
            Dim shift = 0
            Dim result As ULong = 0

            While shift < 64
                Dim b As Byte = ReadRawByte()
                result = result Or CULng(b And &H7F) << shift

                If (b And &H80) = 0 Then
                    Return result
                End If

                shift += 7
            End While

            Throw InvalidProtocolBufferException.MalformedVarint()
        End Function

        ''' <summary>
        ''' Reads a 32-bit little-endian integer from the stream.
        ''' </summary>
        Friend Function ReadRawLittleEndian32() As UInteger
            Dim b1 As UInteger = ReadRawByte()
            Dim b2 As UInteger = ReadRawByte()
            Dim b3 As UInteger = ReadRawByte()
            Dim b4 As UInteger = ReadRawByte()
            Return b1 Or b2 << 8 Or b3 << 16 Or b4 << 24
        End Function

        ''' <summary>
        ''' Reads a 64-bit little-endian integer from the stream.
        ''' </summary>
        Friend Function ReadRawLittleEndian64() As ULong
            Dim b1 As ULong = ReadRawByte()
            Dim b2 As ULong = ReadRawByte()
            Dim b3 As ULong = ReadRawByte()
            Dim b4 As ULong = ReadRawByte()
            Dim b5 As ULong = ReadRawByte()
            Dim b6 As ULong = ReadRawByte()
            Dim b7 As ULong = ReadRawByte()
            Dim b8 As ULong = ReadRawByte()
            Return b1 Or b2 << 8 Or b3 << 16 Or b4 << 24 Or b5 << 32 Or b6 << 40 Or b7 << 48 Or b8 << 56
        End Function

        ''' <summary>
        ''' Decode a 32-bit value with ZigZag encoding.
        ''' </summary>
        ''' <remarks>
        ''' ZigZag encodes signed integers into values that can be efficiently
        ''' encoded with varint.  (Otherwise, negative values must be 
        ''' sign-extended to 64 bits to be varint encoded, thus always taking
        ''' 10 bytes on the wire.)
        ''' </remarks>
        Friend Shared Function DecodeZigZag32(n As UInteger) As Integer
            Return CInt(n >> 1) Xor -CInt(n And 1)
        End Function

        ''' <summary>
        ''' Decode a 32-bit value with ZigZag encoding.
        ''' </summary>
        ''' <remarks>
        ''' ZigZag encodes signed integers into values that can be efficiently
        ''' encoded with varint.  (Otherwise, negative values must be 
        ''' sign-extended to 64 bits to be varint encoded, thus always taking
        ''' 10 bytes on the wire.)
        ''' </remarks>
        Friend Shared Function DecodeZigZag64(n As ULong) As Long
            Return n >> 1 Xor -(n And 1)
        End Function
#End Region

#Region "Internal reading and buffer management"

        ''' <summary>
        ''' Sets currentLimit to (current position) + byteLimit. This is called
        ''' when descending into a length-delimited embedded message. The previous
        ''' limit is returned.
        ''' </summary>
        ''' <returns>The old limit.</returns>
        Friend Function PushLimit(byteLimit As Integer) As Integer
            If byteLimit < 0 Then
                Throw InvalidProtocolBufferException.NegativeSize()
            End If

            byteLimit += totalBytesRetired + bufferPos
            Dim oldLimit = currentLimit

            If byteLimit > oldLimit Then
                Throw InvalidProtocolBufferException.TruncatedMessage()
            End If

            currentLimit = byteLimit
            RecomputeBufferSizeAfterLimit()
            Return oldLimit
        End Function

        Private Sub RecomputeBufferSizeAfterLimit()
            bufferSizeField += bufferSizeAfterLimit
            Dim bufferEnd = totalBytesRetired + bufferSizeField

            If bufferEnd > currentLimit Then
                ' Limit is in current buffer.
                bufferSizeAfterLimit = bufferEnd - currentLimit
                bufferSizeField -= bufferSizeAfterLimit
            Else
                bufferSizeAfterLimit = 0
            End If
        End Sub

        ''' <summary>
        ''' Discards the current limit, returning the previous limit.
        ''' </summary>
        Friend Sub PopLimit(oldLimit As Integer)
            currentLimit = oldLimit
            RecomputeBufferSizeAfterLimit()
        End Sub

        ''' <summary>
        ''' Returns whether or not all the data before the limit has been read.
        ''' </summary>
        ''' <returns></returns>
        Friend ReadOnly Property ReachedLimit As Boolean
            Get

                If currentLimit = Integer.MaxValue Then
                    Return False
                End If

                Dim currentAbsolutePosition = totalBytesRetired + bufferPos
                Return currentAbsolutePosition >= currentLimit
            End Get
        End Property

        ''' <summary>
        ''' Returns true if the stream has reached the end of the input. This is the
        ''' case if either the end of the underlying input source has been reached or
        ''' the stream has reached a limit created using PushLimit.
        ''' </summary>
        Public ReadOnly Property IsAtEnd As Boolean
            Get
                Return bufferPos = bufferSizeField AndAlso Not RefillBuffer(False)
            End Get
        End Property

        ''' <summary>
        ''' Called when buffer is empty to read more bytes from the
        ''' input.  If <paramrefname="mustSucceed"/> is true, RefillBuffer() gurantees that
        ''' either there will be at least one byte in the buffer when it returns
        ''' or it will throw an exception.  If <paramrefname="mustSucceed"/> is false,
        ''' RefillBuffer() returns false if no more bytes were available.
        ''' </summary>
        ''' <param name="mustSucceed"></param>
        ''' <returns></returns>
        Private Function RefillBuffer(mustSucceed As Boolean) As Boolean
            If bufferPos < bufferSizeField Then
                Throw New InvalidOperationException("RefillBuffer() called when buffer wasn't empty.")
            End If

            If totalBytesRetired + bufferSizeField = currentLimit Then
                ' Oops, we hit a limit.
                If mustSucceed Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                Else
                    Return False
                End If
            End If

            totalBytesRetired += bufferSizeField
            bufferPos = 0
            bufferSizeField = If(input Is Nothing, 0, input.Read(buffer, 0, buffer.Length))

            If bufferSizeField < 0 Then
                Throw New InvalidOperationException("Stream.Read returned a negative count")
            End If

            If bufferSizeField = 0 Then
                If mustSucceed Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                Else
                    Return False
                End If
            Else
                RecomputeBufferSizeAfterLimit()
                Dim totalBytesRead = totalBytesRetired + bufferSizeField + bufferSizeAfterLimit

                If totalBytesRead > sizeLimitField OrElse totalBytesRead < 0 Then
                    Throw InvalidProtocolBufferException.SizeLimitExceeded()
                End If

                Return True
            End If
        End Function

        ''' <summary>
        ''' Read one byte from the input.
        ''' </summary>
        ''' <exception cref="InvalidProtocolBufferException">
        ''' the end of the stream or the current limit was reached
        ''' </exception>
        Friend Function ReadRawByte() As Byte
            If bufferPos = bufferSizeField Then
                RefillBuffer(True)
            End If

            Return buffer(Math.Min(Threading.Interlocked.Increment(bufferPos), bufferPos - 1))
        End Function

        ''' <summary>
        ''' Reads a fixed size of bytes from the input.
        ''' </summary>
        ''' <exception cref="InvalidProtocolBufferException">
        ''' the end of the stream or the current limit was reached
        ''' </exception>
        Friend Function ReadRawBytes(size As Integer) As Byte()
            If size < 0 Then
                Throw InvalidProtocolBufferException.NegativeSize()
            End If

            If totalBytesRetired + bufferPos + size > currentLimit Then
                ' Read to the end of the stream (up to the current limit) anyway.
                SkipRawBytes(currentLimit - totalBytesRetired - bufferPos)
                ' Then fail.
                Throw InvalidProtocolBufferException.TruncatedMessage()
            End If

            If size <= bufferSizeField - bufferPos Then
                ' We have all the bytes we need already.
                Dim bytes = New Byte(size - 1) {}
                Copy(buffer, bufferPos, bytes, 0, size)
                bufferPos += size
                Return bytes
            ElseIf size < buffer.Length Then
                ' Reading more bytes than are in the buffer, but not an excessive number
                ' of bytes.  We can safely allocate the resulting array ahead of time.

                ' First copy what we have.
                Dim bytes = New Byte(size - 1) {}
                Dim pos = bufferSizeField - bufferPos
                Copy(buffer, bufferPos, bytes, 0, pos)
                bufferPos = bufferSizeField

                ' We want to use RefillBuffer() and then copy from the buffer into our
                ' byte array rather than reading directly into our byte array because
                ' the input may be unbuffered.
                RefillBuffer(True)

                While size - pos > bufferSizeField
                    System.Buffer.BlockCopy(buffer, 0, bytes, pos, bufferSizeField)
                    pos += bufferSizeField
                    bufferPos = bufferSizeField
                    RefillBuffer(True)
                End While

                Copy(buffer, 0, bytes, pos, size - pos)
                bufferPos = size - pos
                Return bytes
            Else
                ' The size is very large.  For security reasons, we can't allocate the
                ' entire byte array yet.  The size comes directly from the input, so a
                ' maliciously-crafted message could provide a bogus very large size in
                ' order to trick the app into allocating a lot of memory.  We avoid this
                ' by allocating and reading only a small chunk at a time, so that the
                ' malicious message must actually *be* extremely large to cause
                ' problems.  Meanwhile, we limit the allowed size of a message elsewhere.

                ' Remember the buffer markers since we'll have to copy the bytes out of
                ' it later.
                Dim originalBufferPos = bufferPos
                Dim originalBufferSize = bufferSizeField

                ' Mark the current buffer consumed.
                totalBytesRetired += bufferSizeField
                bufferPos = 0
                bufferSizeField = 0

                ' Read all the rest of the bytes we need.
                Dim sizeLeft = size - (originalBufferSize - originalBufferPos)
                Dim chunks As List(Of Byte()) = New List(Of Byte())()

                While sizeLeft > 0
                    Dim chunk = New Byte(Math.Min(sizeLeft, buffer.Length) - 1) {}
                    Dim pos = 0

                    While pos < chunk.Length
                        Dim n = If(input Is Nothing, -1, input.Read(chunk, pos, chunk.Length - pos))

                        If n <= 0 Then
                            Throw InvalidProtocolBufferException.TruncatedMessage()
                        End If

                        totalBytesRetired += n
                        pos += n
                    End While

                    sizeLeft -= chunk.Length
                    chunks.Add(chunk)
                End While

                ' OK, got everything.  Now concatenate it all into one buffer.
                Dim bytes = New Byte(size - 1) {}

                ' Start by copying the leftover bytes from this.buffer.
                Dim newPos = originalBufferSize - originalBufferPos
                Copy(buffer, originalBufferPos, bytes, 0, newPos)

                ' And now all the chunks.
                For Each chunk In chunks
                    System.Buffer.BlockCopy(chunk, 0, bytes, newPos, chunk.Length)
                    newPos += chunk.Length
                Next

                ' Done.
                Return bytes
            End If
        End Function

        ''' <summary>
        ''' Reads and discards <paramrefname="size"/> bytes.
        ''' </summary>
        ''' <exception cref="InvalidProtocolBufferException">the end of the stream
        ''' or the current limit was reached</exception>
        Private Sub SkipRawBytes(size As Integer)
            If size < 0 Then
                Throw InvalidProtocolBufferException.NegativeSize()
            End If

            If totalBytesRetired + bufferPos + size > currentLimit Then
                ' Read to the end of the stream anyway.
                SkipRawBytes(currentLimit - totalBytesRetired - bufferPos)
                ' Then fail.
                Throw InvalidProtocolBufferException.TruncatedMessage()
            End If

            If size <= bufferSizeField - bufferPos Then
                ' We have all the bytes we need already.
                bufferPos += size
            Else
                ' Skipping more bytes than are in the buffer.  First skip what we have.
                Dim pos = bufferSizeField - bufferPos

                ' ROK 5/7/2013 Issue #54: should retire all bytes in buffer (bufferSize)
                ' totalBytesRetired += pos;
                totalBytesRetired += bufferSizeField
                bufferPos = 0
                bufferSizeField = 0

                ' Then skip directly from the InputStream for the rest.
                If pos < size Then
                    If input Is Nothing Then
                        Throw InvalidProtocolBufferException.TruncatedMessage()
                    End If

                    SkipImpl(size - pos)
                    totalBytesRetired += size - pos
                End If
            End If
        End Sub

        ''' <summary>
        ''' Abstraction of skipping to cope with streams which can't really skip.
        ''' </summary>
        Private Sub SkipImpl(amountToSkip As Integer)
            If input.CanSeek Then
                Dim previousPosition = input.Position
                input.Position += amountToSkip

                If input.Position <> previousPosition + amountToSkip Then
                    Throw InvalidProtocolBufferException.TruncatedMessage()
                End If
            Else
                Dim skipBuffer = New Byte(Math.Min(1024, amountToSkip) - 1) {}

                While amountToSkip > 0
                    Dim bytesRead = input.Read(skipBuffer, 0, Math.Min(skipBuffer.Length, amountToSkip))

                    If bytesRead <= 0 Then
                        Throw InvalidProtocolBufferException.TruncatedMessage()
                    End If

                    amountToSkip -= bytesRead
                End While
            End If
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class

#End Region
    End Class
End Namespace
