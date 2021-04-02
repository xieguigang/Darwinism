#Region "Microsoft.VisualBasic::370b5a447c484484e508d32e2bf75164, Google.Protobuf\FieldCodec.vb"

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

    '     Module FieldCodec
    ' 
    '         Function: ForBool, ForBytes, ForClassWrapper, ForDouble, ForEnum
    '                   ForFixed32, ForFixed64, ForFloat, ForInt32, ForInt64
    '                   ForMessage, ForSFixed32, ForSFixed64, ForSInt32, ForSInt64
    '                   ForString, ForStructWrapper, ForUInt32, ForUInt64
    '         Class WrapperCodecs
    ' 
    '             Function: CalculateSize, GetCodec, Read
    ' 
    '             Sub: Write
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2015 Google Inc.  All rights reserved.
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

Imports Google.Protobuf.WellKnownTypes
Imports Microsoft.VisualBasic.Language
Imports System
Imports System.Collections.Generic

Namespace Google.Protobuf
    ''' <summary>
    ''' Factory methods for <see cref="FieldCodecType(OfT)"/>.
    ''' </summary>
    Public Module FieldCodec
        ' TODO: Avoid the "dual hit" of lambda expressions: create open delegates instead. (At least test...)

        ''' <summary>
        ''' Retrieves a codec suitable for a string field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForString(tag As UInteger) As FieldCodecType(Of String)
            Return New FieldCodecType(Of String)(Function(input) input.ReadString(), Sub(output, value) output.WriteString(value), AddressOf CodedOutputStream.ComputeStringSize, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a bytes field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForBytes(tag As UInteger) As FieldCodecType(Of ByteString)
            Return New FieldCodecType(Of ByteString)(Function(input) input.ReadBytes(), Sub(output, value) output.WriteBytes(value), AddressOf CodedOutputStream.ComputeBytesSize, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a bool field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForBool(tag As UInteger) As FieldCodecType(Of Boolean)
            Return New FieldCodecType(Of Boolean)(Function(input) input.ReadBool(), Sub(output, value) output.WriteBool(value), AddressOf CodedOutputStream.ComputeBoolSize, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an int32 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForInt32(tag As UInteger) As FieldCodecType(Of Integer)
            Return New FieldCodecType(Of Integer)(Function(input) input.ReadInt32(), Sub(output, value) output.WriteInt32(value), AddressOf CodedOutputStream.ComputeInt32Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an sint32 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForSInt32(tag As UInteger) As FieldCodecType(Of Integer)
            Return New FieldCodecType(Of Integer)(Function(input) input.ReadSInt32(), Sub(output, value) output.WriteSInt32(value), AddressOf CodedOutputStream.ComputeSInt32Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a fixed32 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForFixed32(tag As UInteger) As FieldCodecType(Of UInteger)
            Return New FieldCodecType(Of UInteger)(Function(input) input.ReadFixed32(), Sub(output, value) output.WriteFixed32(value), 4, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an sfixed32 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForSFixed32(tag As UInteger) As FieldCodecType(Of Integer)
            Return New FieldCodecType(Of Integer)(Function(input) input.ReadSFixed32(), Sub(output, value) output.WriteSFixed32(value), 4, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a uint32 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForUInt32(tag As UInteger) As FieldCodecType(Of UInteger)
            Return New FieldCodecType(Of UInteger)(Function(input) input.ReadUInt32(), Sub(output, value) output.WriteUInt32(value), AddressOf CodedOutputStream.ComputeUInt32Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an int64 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForInt64(tag As UInteger) As FieldCodecType(Of Long)
            Return New FieldCodecType(Of Long)(Function(input) input.ReadInt64(), Sub(output, value) output.WriteInt64(value), AddressOf CodedOutputStream.ComputeInt64Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an sint64 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForSInt64(tag As UInteger) As FieldCodecType(Of Long)
            Return New FieldCodecType(Of Long)(Function(input) input.ReadSInt64(), Sub(output, value) output.WriteSInt64(value), AddressOf CodedOutputStream.ComputeSInt64Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a fixed64 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForFixed64(tag As UInteger) As FieldCodecType(Of ULong)
            Return New FieldCodecType(Of ULong)(Function(input) input.ReadFixed64(), Sub(output, value) output.WriteFixed64(value), 8, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for an sfixed64 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForSFixed64(tag As UInteger) As FieldCodecType(Of Long)
            Return New FieldCodecType(Of Long)(Function(input) input.ReadSFixed64(), Sub(output, value) output.WriteSFixed64(value), 8, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a uint64 field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForUInt64(tag As UInteger) As FieldCodecType(Of ULong)
            Return New FieldCodecType(Of ULong)(Function(input) input.ReadUInt64(), Sub(output, value) output.WriteUInt64(value), AddressOf CodedOutputStream.ComputeUInt64Size, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a float field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForFloat(tag As UInteger) As FieldCodecType(Of Single)
            Return New FieldCodecType(Of Single)(Function(input) input.ReadFloat(), Sub(output, value) output.WriteFloat(value), AddressOf CodedOutputStream.ComputeFloatSize, tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a double field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForDouble(tag As UInteger) As FieldCodecType(Of Double)
            Return New FieldCodecType(Of Double)(Function(input) input.ReadDouble(), Sub(output, value) output.WriteDouble(value), AddressOf CodedOutputStream.ComputeDoubleSize, tag)
        End Function

        ' Enums are tricky. We can probably use expression trees to build these delegates automatically,
        ' but it's easy to generate the code for it.

        ''' <summary>
        ''' Retrieves a codec suitable for an enum field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <param name="toInt32">A conversion function from <see cref="Int32"/> to the enum type.</param>
        ''' <param name="fromInt32">A conversion function from the enum type to <see cref="Int32"/>.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForEnum(Of T)(tag As UInteger, toInt32 As Func(Of T, Integer), fromInt32 As Func(Of Integer, T)) As FieldCodecType(Of T)
            Return New FieldCodecType(Of T)(Function(input) fromInt32(input.ReadEnum()), Sub(output, value) output.WriteEnum(toInt32(value)), Function(value) CodedOutputStream.ComputeEnumSize(toInt32(value)), tag)
        End Function

        ''' <summary>
        ''' Retrieves a codec suitable for a message field with the given tag.
        ''' </summary>
        ''' <param name="tag">The tag.</param>
        ''' <param name="parser">A parser to use for the message type.</param>
        ''' <returns>A codec for the given tag.</returns>
        Public Function ForMessage(Of T As IMessageType(Of T))(tag As UInteger, parser As MessageParserType(Of T)) As FieldCodecType(Of T)
            Return New FieldCodecType(Of T)(Function(input)
                                                Dim message As T = parser.CreateTemplate()
                                                input.ReadMessage(message)
                                                Return message
                                            End Function, Sub(output, value) output.WriteMessage(value), Function(message) CodedOutputStream.ComputeMessageSize(message), tag)
        End Function

        ''' <summary>
        ''' Creates a codec for a wrapper type of a class - which must be string or ByteString.
        ''' </summary>
        Public Function ForClassWrapper(Of T As Class)(tag As UInteger) As FieldCodecType(Of T)
            Dim nestedCodec = WrapperCodecs.GetCodec(Of T)()
            Return New FieldCodecType(Of T)(Function(input) WrapperCodecs.Read(input, nestedCodec), Sub(output, value) WrapperCodecs.Write(output, value, nestedCodec), Function(value) WrapperCodecs.CalculateSize(value, nestedCodec), tag, Nothing) ' Default value for the wrapper
        End Function

        ''' <summary>
        ''' Creates a codec for a wrapper type of a struct - which must be Int32, Int64, UInt32, UInt64,
        ''' Bool, Single or Double.
        ''' </summary>
        Public Function ForStructWrapper(Of T As Structure)(tag As UInteger) As FieldCodecType(Of T?)
            Dim nestedCodec = WrapperCodecs.GetCodec(Of T)()
            Return New FieldCodecType(Of T?)(Function(input) WrapperCodecs.Read(input, nestedCodec), Sub(output, value) WrapperCodecs.Write(output, value.Value, nestedCodec), Function(value) If(value Is Nothing, 0, WrapperCodecs.CalculateSize(value.Value, nestedCodec)), tag, Nothing) ' Default value for the wrapper
        End Function

        ''' <summary>
        ''' Helper code to create codecs for wrapper types.
        ''' </summary>
        ''' <remarks>
        ''' Somewhat ugly with all the static methods, but the conversions involved to/from nullable types make it
        ''' slightly tricky to improve. So long as we keep the public API (ForClassWrapper, ForStructWrapper) in place,
        ''' we can refactor later if we come up with something cleaner.
        ''' </remarks>
        Private NotInheritable Class WrapperCodecs
            Private Shared ReadOnly Codecs As Dictionary(Of System.Type, Object) = New Dictionary(Of System.Type, Object) From {
                {GetType(Boolean), ForBool(MakeTag(WrapperValueFieldNumber, WireType.Varint))},
                {GetType(Integer), ForInt32(MakeTag(WrapperValueFieldNumber, WireType.Varint))},
                {GetType(Long), ForInt64(MakeTag(WrapperValueFieldNumber, WireType.Varint))},
                {GetType(UInteger), ForUInt32(MakeTag(WrapperValueFieldNumber, WireType.Varint))},
                {GetType(ULong), ForUInt64(MakeTag(WrapperValueFieldNumber, WireType.Varint))},
                {GetType(Single), ForFloat(MakeTag(WrapperValueFieldNumber, WireType.Fixed32))},
                {GetType(Double), ForDouble(MakeTag(WrapperValueFieldNumber, WireType.Fixed64))},
                {GetType(String), ForString(MakeTag(WrapperValueFieldNumber, WireType.LengthDelimited))},
                {GetType(ByteString), ForBytes(MakeTag(WrapperValueFieldNumber, WireType.LengthDelimited))}
            }

            ''' <summary>
            ''' Returns a field codec which effectively wraps a value of type T in a message.
            ''' 
            ''' </summary>
            Friend Shared Function GetCodec(Of T)() As FieldCodecType(Of T)
                Dim value As Object

                If Not Codecs.TryGetValue(GetType(T), value) Then
                    Throw New InvalidOperationException("Invalid type argument requested for wrapper codec: " & GetType(T).ToString)
                End If

                Return CType(value, FieldCodecType(Of T))
            End Function

            Friend Shared Function Read(Of T)(input As CodedInputStream, codec As FieldCodecType(Of T)) As T
                Dim length As Integer = input.ReadLength()
                Dim oldLimit = input.PushLimit(length)
                Dim tag As New Value(Of UInteger)
                Dim value = codec.DefaultValue

                While (tag = input.ReadTag()) <> 0

                    If tag.Value = codec.Tag Then
                        value = codec.Read(input)
                    Else
                        input.SkipLastField()
                    End If
                End While

                input.CheckReadEndOfStreamTag()
                input.PopLimit(oldLimit)
                Return value
            End Function

            Friend Shared Sub Write(Of T)(output As CodedOutputStream, value As T, codec As FieldCodecType(Of T))
                output.WriteLength(codec.CalculateSizeWithTag(value))
                codec.WriteTagAndValue(output, value)
            End Sub

            Friend Shared Function CalculateSize(Of T)(value As T, codec As FieldCodecType(Of T)) As Integer
                Dim fieldLength = codec.CalculateSizeWithTag(value)
                Return CodedOutputStream.ComputeLengthSize(fieldLength) + fieldLength
            End Function
        End Class
    End Module
End Namespace
