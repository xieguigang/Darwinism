#Region "Microsoft.VisualBasic::e275edbbf77b46f0016f053c29c90fb7, src\message\Google.Protobuf\Stream\ByteString.vb"

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


    ' Code Statistics:

    '   Total Lines: 324
    '    Code Lines: 128
    ' Comment Lines: 158
    '   Blank Lines: 38
    '     File Size: 13.79 KB


    '     Class ByteString
    ' 
    '         Properties: Empty, IsEmpty, Length
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: AttachBytes, (+3 Overloads) CopyFrom, CopyFromUtf8, CreateCodedInput, (+2 Overloads) Equals
    '                   FromBase64, GetEnumerator, GetEnumerator1, GetHashCode, ToBase64
    '                   ToByteArray, ToString, ToStringUtf8
    ' 
    '         Sub: CopyTo, WriteRawBytesTo, WriteTo
    ' 
    '         Operators: <>, =
    '         Class Unsafe
    ' 
    '             Function: FromBytes, GetBuffer
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace Google.Protobuf
    ''' <summary>
    ''' Immutable array of bytes.
    ''' </summary>
    Public NotInheritable Class ByteString
        Implements IEnumerable(Of Byte), IEquatable(Of ByteString)

        Private Shared ReadOnly emptyField As ByteString = New ByteString(New Byte(-1) {})
        Private ReadOnly bytes As Byte()

        ''' <summary>
        ''' Unsafe operations that can cause IO Failure and/or other catestrophic side-effects.
        ''' </summary>
        Friend NotInheritable Class Unsafe
            ''' <summary>
            ''' Constructs a new ByteString from the given byte array. The array is
            ''' *not* copied, and must not be modified after this constructor is called.
            ''' </summary>
            Friend Shared Function FromBytes(bytes As Byte()) As ByteString
                Return New ByteString(bytes)
            End Function

            ''' <summary>
            ''' Provides direct, unrestricted access to the bytes contained in this instance.
            ''' You must not modify or resize the byte array returned by this method.
            ''' </summary>
            Friend Shared Function GetBuffer(bytes As ByteString) As Byte()
                Return bytes.bytes
            End Function
        End Class

        ''' <summary>
        ''' Internal use only.  Ensure that the provided array is not mutated and belongs to this instance.
        ''' </summary>
        Friend Shared Function AttachBytes(bytes As Byte()) As ByteString
            Return New ByteString(bytes)
        End Function

        ''' <summary>
        ''' Constructs a new ByteString from the given byte array. The array is
        ''' *not* copied, and must not be modified after this constructor is called.
        ''' </summary>
        Private Sub New(bytes As Byte())
            Me.bytes = bytes
        End Sub

        ''' <summary>
        ''' Returns an empty ByteString.
        ''' </summary>
        Public Shared ReadOnly Property Empty As ByteString
            Get
                Return emptyField
            End Get
        End Property

        ''' <summary>
        ''' Returns the length of this ByteString in bytes.
        ''' </summary>
        Public ReadOnly Property Length As Integer
            Get
                Return bytes.Length
            End Get
        End Property

        ''' <summary>
        ''' Returns <c>true</c> if this byte string is empty, <c>false</c> otherwise.
        ''' </summary>
        Public ReadOnly Property IsEmpty As Boolean
            Get
                Return Length = 0
            End Get
        End Property

        ''' <summary>
        ''' Converts this <see cref="ByteString"/> into a byte array.
        ''' </summary>
        ''' <remarks>The data is copied - changes to the returned array will not be reflected in this <c>ByteString</c>.</remarks>
        ''' <returns>A byte array with the same data as this <c>ByteString</c>.</returns>
        Public Function ToByteArray() As Byte()
            Return CType(bytes.Clone(), Byte())
        End Function

        ''' <summary>
        ''' Converts this <see cref="ByteString"/> into a standard base64 representation.
        ''' </summary>
        ''' <returns>A base64 representation of this <c>ByteString</c>.</returns>
        Public Function ToBase64() As String
            Return Convert.ToBase64String(bytes)
        End Function

        ''' <summary>
        ''' Constructs a <see cref="ByteString"/> from the Base64 Encoded String.
        ''' </summary>
        Public Shared Function FromBase64(bytes As String) As ByteString
            ' By handling the empty string explicitly, we not only optimize but we fix a
            ' problem on CF 2.0. See issue 61 for details.
            Return If(Equals(bytes, ""), Empty, New ByteString(Convert.FromBase64String(bytes)))
        End Function

        ''' <summary>
        ''' Constructs a <see cref="ByteString"/> from the given array. The contents
        ''' are copied, so further modifications to the array will not
        ''' be reflected in the returned ByteString.
        ''' This method can also be invoked in <c>ByteString.CopyFrom(0xaa, 0xbb, ...)</c> form
        ''' which is primarily useful for testing.
        ''' </summary>
        Public Shared Function CopyFrom(ParamArray bytes As Byte()) As ByteString
            Return New ByteString(CType(bytes.Clone(), Byte()))
        End Function

        ''' <summary>
        ''' Constructs a <see cref="ByteString"/> from a portion of a byte array.
        ''' </summary>
        Public Shared Function CopyFrom(bytes As Byte(), offset As Integer, count As Integer) As ByteString
            Dim portion = New Byte(count - 1) {}
            Copy(bytes, offset, portion, 0, count)
            Return New ByteString(portion)
        End Function

        ''' <summary>
        ''' Creates a new <see cref="ByteString"/> by encoding the specified text with
        ''' the given encoding.
        ''' </summary>
        Public Shared Function CopyFrom(text As String, encoding As Encoding) As ByteString
            Return New ByteString(encoding.GetBytes(text))
        End Function

        ''' <summary>
        ''' Creates a new <see cref="ByteString"/> by encoding the specified text in UTF-8.
        ''' </summary>
        Public Shared Function CopyFromUtf8(text As String) As ByteString
            Return CopyFrom(text, Encoding.UTF8)
        End Function

        ''' <summary>
        ''' Retuns the byte at the given index.
        ''' </summary>
        Default Public ReadOnly Property Item(index As Integer) As Byte
            Get
                Return bytes(index)
            End Get
        End Property

        ''' <summary>
        ''' Converts this <see cref="ByteString"/> into a string by applying the given encoding.
        ''' </summary>
        ''' <remarks>
        ''' This method should only be used to convert binary data which was the result of encoding
        ''' text with the given encoding.
        ''' </remarks>
        ''' <param name="encoding">The encoding to use to decode the binary data into text.</param>
        ''' <returns>The result of decoding the binary data with the given decoding.</returns>
        Public Overloads Function ToString(encoding As Encoding) As String
            Return encoding.GetString(bytes, 0, bytes.Length)
        End Function

        ''' <summary>
        ''' Converts this <see cref="ByteString"/> into a string by applying the UTF-8 encoding.
        ''' </summary>
        ''' <remarks>
        ''' This method should only be used to convert binary data which was the result of encoding
        ''' text with UTF-8.
        ''' </remarks>
        ''' <returns>The result of decoding the binary data with the given decoding.</returns>
        Public Function ToStringUtf8() As String
            Return ToString(Encoding.UTF8)
        End Function

        ''' <summary>
        ''' Returns an iterator over the bytes in this <see cref="ByteString"/>.
        ''' </summary>
        ''' <returns>An iterator over the bytes in this object.</returns>
        Public Function GetEnumerator() As IEnumerator(Of Byte) Implements IEnumerable(Of Byte).GetEnumerator
            Return CType(bytes, IEnumerable(Of Byte)).GetEnumerator()
        End Function

        ''' <summary>
        ''' Returns an iterator over the bytes in this <see cref="ByteString"/>.
        ''' </summary>
        ''' <returns>An iterator over the bytes in this object.</returns>
        Private Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function

        ''' <summary>
        ''' Creates a CodedInputStream from this ByteString's data.
        ''' </summary>
        Public Function CreateCodedInput() As CodedInputStream
            ' We trust CodedInputStream not to reveal the provided byte array or modify it
            Return New CodedInputStream(bytes)
        End Function

        ''' <summary>
        ''' Compares two byte strings for equality.
        ''' </summary>
        ''' <param name="lhs">The first byte string to compare.</param>
        ''' <param name="rhs">The second byte string to compare.</param>
        ''' <returns><c>true</c> if the byte strings are equal; false otherwise.</returns>
        Public Shared Operator =(lhs As ByteString, rhs As ByteString) As Boolean
            If ReferenceEquals(lhs, rhs) Then
                Return True
            End If

            If ReferenceEquals(lhs, Nothing) OrElse ReferenceEquals(rhs, Nothing) Then
                Return False
            End If

            If lhs.bytes.Length <> rhs.bytes.Length Then
                Return False
            End If

            For i = 0 To lhs.Length - 1

                If rhs.bytes(i) <> lhs.bytes(i) Then
                    Return False
                End If
            Next

            Return True
        End Operator

        ''' <summary>
        ''' Compares two byte strings for inequality.
        ''' </summary>
        ''' <param name="lhs">The first byte string to compare.</param>
        ''' <param name="rhs">The second byte string to compare.</param>
        ''' <returns><c>false</c> if the byte strings are equal; true otherwise.</returns>
        Public Shared Operator <>(lhs As ByteString, rhs As ByteString) As Boolean
            Return Not lhs Is rhs
        End Operator

        ''' <summary>
        ''' Compares this byte string with another object.
        ''' </summary>
        ''' <param name="obj">The object to compare this with.</param>
        ''' <returns><c>true</c> if <paramrefname="obj"/> refers to an equal <see cref="ByteString"/>; <c>false</c> otherwise.</returns>
        Public Overrides Function Equals(obj As Object) As Boolean
            Return Me Is TryCast(obj, ByteString)
        End Function

        ''' <summary>
        ''' Returns a hash code for this object. Two equal byte strings
        ''' will return the same hash code.
        ''' </summary>
        ''' <returns>A hash code for this object.</returns>
        Public Overrides Function GetHashCode() As Integer
            Dim ret = 23

            For Each b In bytes
                ret = ret << 8 Or b
            Next

            Return ret
        End Function

        ''' <summary>
        ''' Compares this byte string with another.
        ''' </summary>
        ''' <param name="other">The <see cref="ByteString"/> to compare this with.</param>
        ''' <returns><c>true</c> if <paramrefname="other"/> refers to an equal byte string; <c>false</c> otherwise.</returns>
        Public Overloads Function Equals(other As ByteString) As Boolean Implements IEquatable(Of ByteString).Equals
            Return Me Is other
        End Function

        ''' <summary>
        ''' Used internally by CodedOutputStream to avoid creating a copy for the write
        ''' </summary>
        Friend Sub WriteRawBytesTo(outputStream As CodedOutputStream)
            outputStream.WriteRawBytes(bytes, 0, bytes.Length)
        End Sub

        ''' <summary>
        ''' Copies the entire byte array to the destination array provided at the offset specified.
        ''' </summary>
        Public Sub CopyTo(array As Byte(), position As Integer)
            Copy(bytes, 0, array, position, bytes.Length)
        End Sub

        ''' <summary>
        ''' Writes the entire byte array to the provided stream
        ''' </summary>
        Public Sub WriteTo(outputStream As Stream)
            outputStream.Write(bytes, 0, bytes.Length)
        End Sub
    End Class
End Namespace
