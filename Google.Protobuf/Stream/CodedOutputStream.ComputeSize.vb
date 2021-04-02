#Region "Microsoft.VisualBasic::a6ed0d89f4e88852b226d065eedfc94d, Google.Protobuf\Stream\CodedOutputStream.ComputeSize.vb"

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

    '     Class CodedOutputStream
    ' 
    '         Function: ComputeBoolSize, ComputeBytesSize, ComputeDoubleSize, ComputeEnumSize, ComputeFixed32Size
    '                   ComputeFixed64Size, ComputeFloatSize, ComputeGroupSize, ComputeInt32Size, ComputeInt64Size
    '                   ComputeLengthSize, ComputeMessageSize, ComputeRawVarint32Size, ComputeRawVarint64Size, ComputeSFixed32Size
    '                   ComputeSFixed64Size, ComputeSInt32Size, ComputeSInt64Size, ComputeStringSize, ComputeTagSize
    '                   ComputeUInt32Size, ComputeUInt64Size
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


Namespace Google.Protobuf
    ' This part of CodedOutputStream provides all the static entry points that are used
    ' by generated code and internally to compute the size of messages prior to being
    ' written to an instance of CodedOutputStream.
    Public NotInheritable Partial Class CodedOutputStream
        Private Const LittleEndian64Size As Integer = 8
        Private Const LittleEndian32Size As Integer = 4

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' double field, including the tag.
        ''' </summary>
        Public Shared Function ComputeDoubleSize(value As Double) As Integer
            Return LittleEndian64Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' float field, including the tag.
        ''' </summary>
        Public Shared Function ComputeFloatSize(value As Single) As Integer
            Return LittleEndian32Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' uint64 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeUInt64Size(value As ULong) As Integer
            Return ComputeRawVarint64Size(value)
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' int64 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeInt64Size(value As Long) As Integer
            Return ComputeRawVarint64Size(value)
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' int32 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeInt32Size(value As Integer) As Integer
            If value >= 0 Then
                Return ComputeRawVarint32Size(value)
            Else
                ' Must sign-extend.
                Return 10
            End If
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' fixed64 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeFixed64Size(value As ULong) As Integer
            Return LittleEndian64Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' fixed32 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeFixed32Size(value As UInteger) As Integer
            Return LittleEndian32Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' bool field, including the tag.
        ''' </summary>
        Public Shared Function ComputeBoolSize(value As Boolean) As Integer
            Return 1
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' string field, including the tag.
        ''' </summary>
        Public Shared Function ComputeStringSize(value As String) As Integer
            Dim byteArraySize = Utf8Encoding.GetByteCount(value)
            Return ComputeLengthSize(byteArraySize) + byteArraySize
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' group field, including the tag.
        ''' </summary>
        Public Shared Function ComputeGroupSize(value As IMessage) As Integer
            Return value.CalculateSize()
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' embedded message field, including the tag.
        ''' </summary>
        Public Shared Function ComputeMessageSize(value As IMessage) As Integer
            Dim size As Integer = value.CalculateSize()
            Return ComputeLengthSize(size) + size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' bytes field, including the tag.
        ''' </summary>
        Public Shared Function ComputeBytesSize(value As ByteString) As Integer
            Return ComputeLengthSize(value.Length) + value.Length
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' uint32 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeUInt32Size(value As UInteger) As Integer
            Return ComputeRawVarint32Size(value)
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a
        ''' enum field, including the tag. The caller is responsible for
        ''' converting the enum value to its numeric value.
        ''' </summary>
        Public Shared Function ComputeEnumSize(value As Integer) As Integer
            ' Currently just a pass-through, but it's nice to separate it logically.
            Return ComputeInt32Size(value)
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' sfixed32 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeSFixed32Size(value As Integer) As Integer
            Return LittleEndian32Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' sfixed64 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeSFixed64Size(value As Long) As Integer
            Return LittleEndian64Size
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' sint32 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeSInt32Size(value As Integer) As Integer
            Return ComputeRawVarint32Size(EncodeZigZag32(value))
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode an
        ''' sint64 field, including the tag.
        ''' </summary>
        Public Shared Function ComputeSInt64Size(value As Long) As Integer
            Return ComputeRawVarint64Size(EncodeZigZag64(value))
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a length,
        ''' as written by <see cref="WriteLength"/>.
        ''' </summary>
        Public Shared Function ComputeLengthSize(length As Integer) As Integer
            Return ComputeRawVarint32Size(length)
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a varint.
        ''' </summary>
        Public Shared Function ComputeRawVarint32Size(value As UInteger) As Integer
            If (value And &HfffffffF << 7) = 0 Then
                Return 1
            End If

            If (value And &HfffffffF << 14) = 0 Then
                Return 2
            End If

            If (value And &HfffffffF << 21) = 0 Then
                Return 3
            End If

            If (value And &HfffffffF << 28) = 0 Then
                Return 4
            End If

            Return 5
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a varint.
        ''' </summary>
        Public Shared Function ComputeRawVarint64Size(value As ULong) As Integer
            If (value And &HffffffffffffffffL << 7) = 0 Then
                Return 1
            End If

            If (value And &HffffffffffffffffL << 14) = 0 Then
                Return 2
            End If

            If (value And &HffffffffffffffffL << 21) = 0 Then
                Return 3
            End If

            If (value And &HffffffffffffffffL << 28) = 0 Then
                Return 4
            End If

            If (value And &HffffffffffffffffL << 35) = 0 Then
                Return 5
            End If

            If (value And &HffffffffffffffffL << 42) = 0 Then
                Return 6
            End If

            If (value And &HffffffffffffffffL << 49) = 0 Then
                Return 7
            End If

            If (value And &HffffffffffffffffL << 56) = 0 Then
                Return 8
            End If

            If (value And &HffffffffffffffffL << 63) = 0 Then
                Return 9
            End If

            Return 10
        End Function

        ''' <summary>
        ''' Computes the number of bytes that would be needed to encode a tag.
        ''' </summary>
        Public Shared Function ComputeTagSize(fieldNumber As Integer) As Integer
            Return ComputeRawVarint32Size(MakeTag(fieldNumber, 0))
        End Function
    End Class
End Namespace
