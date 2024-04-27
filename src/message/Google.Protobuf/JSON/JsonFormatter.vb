#Region "Microsoft.VisualBasic::822ce310a13c2d350e6b19e5f24521a6, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//JSON/JsonFormatter.vb"

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

    '   Total Lines: 800
    '    Code Lines: 495
    ' Comment Lines: 194
    '   Blank Lines: 111
    '     File Size: 38.35 KB


    '     Class JsonFormatter
    ' 
    '         Properties: [Default], DiagnosticOnly
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: Format, IsDefaultValue, ToCamelCase, ToCamelCaseForFieldMask, ToDiagnosticString
    '                   WriteMessageFields
    ' 
    '         Sub: Format, HexEncodeUtf16CodeUnit, WriteAny, WriteDiagnosticOnlyAny, WriteDictionary
    '              WriteDuration, WriteFieldMask, WriteList, WriteMessage, WriteNull
    '              WriteString, WriteStruct, WriteStructFieldValue, WriteTimestamp, WriteValue
    '              WriteWellKnownTypeValue
    '         Class Settings
    ' 
    '             Properties: [Default], FormatDefaultValues, TypeRegistry
    ' 
    '             Constructor: (+3 Overloads) Sub New
    ' 
    '         Class OriginalEnumValueHelper
    ' 
    '             Function: GetNameMapping, GetOriginalName
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

Imports System
Imports System.Collections
Imports System.Globalization
Imports System.Text
Imports Google.Protobuf.Reflection
Imports Google.Protobuf.WellKnownTypes
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Reflection
Imports Microsoft.VisualBasic.Language

Namespace Google.Protobuf
    ''' <summary>
    ''' Reflection-based converter from messages to JSON.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Instances of this class are thread-safe, with no mutable state.
    ''' </para>
    ''' <para>
    ''' This is a simple start to get JSON formatting working. As it's reflection-based,
    ''' it's not as quick as baking calls into generated messages - but is a simpler implementation.
    ''' (This code is generally not heavily optimized.)
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class JsonFormatter
        Friend Const AnyTypeUrlField As String = "@type"
        Friend Const AnyDiagnosticValueField As String = "@value"
        Friend Const AnyWellKnownTypeValueField As String = "value"
        Private Const TypeUrlPrefix As String = "type.googleapis.com"
        Private Const NameValueSeparator As String = ": "
        Private Const PropertySeparator As String = ", "

        ''' <summary>
        ''' Returns a formatter using the default settings.
        ''' </summary>
        Public Shared ReadOnly Property [Default] As JsonFormatter = New JsonFormatter(Settings.Default)

        ' A JSON formatter which *only* exists 
        Private Shared ReadOnly diagnosticFormatter As JsonFormatter = New JsonFormatter(Settings.Default)

        ''' <summary>
        ''' The JSON representation of the first 160 characters of Unicode.
        ''' Empty strings are replaced by the static constructor.
        ''' </summary>
        ' C0 (ASCII and derivatives) control characters
        ' Escaping of " and \ are required by www.json.org string definition.
        ' Escaping of < and > are required for HTML security.
        ' C1 (ISO 8859 and Unicode) extended control characters
        Private Shared ReadOnly CommonRepresentations As String() = {"\u0000", "\u0001", "\u0002", "\u0003", "\u0004", "\u0005", "\u0006", "\u0007", "\b", "\t", "\n", "\u000b", "\f", "\r", "\u000e", "\u000f", "\u0010", "\u0011", "\u0012", "\u0013", "\u0014", "\u0015", "\u0016", "\u0017", "\u0018", "\u0019", "\u001a", "\u001b", "\u001c", "\u001d", "\u001e", "\u001f", "", "", "\""", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "\u003c", "", "\u003e", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "\\", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "\u007f", "\u0080", "\u0081", "\u0082", "\u0083", "\u0084", "\u0085", "\u0086", "\u0087", "\u0088", "\u0089", "\u008a", "\u008b", "\u008c", "\u008d", "\u008e", "\u008f", "\u0090", "\u0091", "\u0092", "\u0093", "\u0094", "\u0095", "\u0096", "\u0097", "\u0098", "\u0099", "\u009a", "\u009b", "\u009c", "\u009d", "\u009e", "\u009f"}  ' 0x00
        ' 0x10
        ' 0x20
        ' 0x30
        ' 0x40
        ' 0x50
        ' 0x60
        ' 0x70
        ' 0x80
        ' 0x90

        Shared Sub New()
            For i = 0 To CommonRepresentations.Length - 1

                If Equals(CommonRepresentations(i), "") Then
                    CommonRepresentations(i) = Microsoft.VisualBasic.ChrW(i).ToString()
                End If
            Next
        End Sub

        Private ReadOnly settingsField As Settings

        Private ReadOnly Property DiagnosticOnly As Boolean
            Get
                Return ReferenceEquals(Me, diagnosticFormatter)
            End Get
        End Property

        ''' <summary>
        ''' Creates a new formatted with the given settings.
        ''' </summary>
        ''' <param name="settings">The settings.</param>
        Public Sub New(settings As Settings)
            settingsField = settings
        End Sub

        ''' <summary>
        ''' Formats the specified message as JSON.
        ''' </summary>
        ''' <param name="message">The message to format.</param>
        ''' <returns>The formatted message.</returns>
        Public Function Format(message As IMessage) As String
            Dim writer = New StringWriter()
            Format(message, writer)
            Return writer.ToString()
        End Function

        ''' <summary>
        ''' Formats the specified message as JSON.
        ''' </summary>
        ''' <param name="message">The message to format.</param>
        ''' <param name="writer">The TextWriter to write the formatted message to.</param>
        ''' <returns>The formatted message.</returns>
        Public Sub Format(message As IMessage, writer As TextWriter)
            CheckNotNull(message, NameOf(message))
            CheckNotNull(writer, NameOf(writer))

            If message.Descriptor.IsWellKnownType Then
                WriteWellKnownTypeValue(writer, message.Descriptor, message)
            Else
                WriteMessage(writer, message)
            End If
        End Sub

        ''' <summary>
        ''' Converts a message to JSON for diagnostic purposes with no extra context.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' This differs from calling <see cref="Format(IMessage)"/> on the default JSON
        ''' formatter in its handling of <see cref="Any"/>. As no type registry is available
        ''' in <see cref="Object.ToString"/> calls, the normal way of resolving the type of
        ''' an <c>Any</c> message cannot be applied. Instead, a JSON property named <c>@value</c>
        ''' is included with the base64 data from the <see cref="Any.Value"/> property of the message.
        ''' </para>
        ''' <para>The value returned by this method is only designed to be used for diagnostic
        ''' purposes. It may not be parsable by <see cref="JsonParser"/>, and may not be parsable
        ''' by other Protocol Buffer implementations.</para>
        ''' </remarks>
        ''' <param name="message">The message to format for diagnostic purposes.</param>
        ''' <returns>The diagnostic-only JSON representation of the message</returns>
        Public Shared Function ToDiagnosticString(message As IMessage) As String
            CheckNotNull(message, NameOf(message))
            Return diagnosticFormatter.Format(message)
        End Function

        Private Sub WriteMessage(writer As TextWriter, message As IMessage)
            If message Is Nothing Then
                WriteNull(writer)
                Return
            End If

            If DiagnosticOnly Then
                Dim customDiagnosticMessage As ICustomDiagnosticMessage = TryCast(message, ICustomDiagnosticMessage)

                If customDiagnosticMessage IsNot Nothing Then
                    writer.Write(customDiagnosticMessage.ToDiagnosticString())
                    Return
                End If
            End If

            writer.Write("{ ")
            Dim writtenFields = WriteMessageFields(writer, message, False)
            writer.Write(If(writtenFields, " }", "}"))
        End Sub

        Private Function WriteMessageFields(writer As TextWriter, message As IMessage, assumeFirstFieldWritten As Boolean) As Boolean
            Dim fields = message.Descriptor.Fields
            Dim first = Not assumeFirstFieldWritten
            ' First non-oneof fields
            For Each field In fields.InFieldNumberOrder()
                Dim accessor = field.Accessor

                If field.ContainingOneof IsNot Nothing AndAlso field.ContainingOneof.Accessor.GetCaseFieldDescriptor(message) IsNot field Then
                    Continue For
                End If
                ' Omit default values unless we're asked to format them, or they're oneofs (where the default
                ' value is still formatted regardless, because that's how we preserve the oneof case).
                Dim value = accessor.GetValue(message)

                If field.ContainingOneof Is Nothing AndAlso Not settingsField.FormatDefaultValues AndAlso IsDefaultValue(accessor, value) Then
                    Continue For
                End If

                ' Okay, all tests complete: let's write the field value...
                If Not first Then
                    writer.Write(PropertySeparator)
                End If

                WriteString(writer, accessor.Descriptor.JsonName)
                writer.Write(NameValueSeparator)
                WriteValue(writer, value)
                first = False
            Next

            Return Not first
        End Function

        ''' <summary>
        ''' Camel-case converter with added strictness for field mask formatting.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The field mask is invalid for JSON representation</exception>
        Private Shared Function ToCamelCaseForFieldMask(input As String) As String
            For i = 0 To input.Length - 1
                Dim c = input(i)

                If c >= "A"c AndAlso c <= "Z"c Then
                    Throw New InvalidOperationException($"Invalid field mask to be converted to JSON: {input}")
                End If

                If c = "_"c AndAlso i < input.Length - 1 Then
                    Dim [next] = input(i + 1)

                    If [next] < "a"c OrElse [next] > "z"c Then
                        Throw New InvalidOperationException($"Invalid field mask to be converted to JSON: {input}")
                    End If
                End If
            Next

            Return ToCamelCase(input)
        End Function

        ' Converted from src/google/protobuf/util/internal/utility.cc ToCamelCase
        ' TODO: Use the new field in FieldDescriptor.
        Friend Shared Function ToCamelCase(input As String) As String
            Dim capitalizeNext = False
            Dim wasCap = True
            Dim isCap = False
            Dim firstWord = True
            Dim result As StringBuilder = New StringBuilder(input.Length)
            Dim i = 0

            While i < input.Length
                isCap = Char.IsUpper(input(i))

                If input(i) = "_"c Then
                    capitalizeNext = True

                    If result.Length <> 0 Then
                        firstWord = False
                    End If

                    Continue While
                ElseIf firstWord Then
                    ' Consider when the current character B is capitalized,
                    ' first word ends when:
                    ' 1) following a lowercase:   "...aB..."
                    ' 2) followed by a lowercase: "...ABc..."
                    If result.Length <> 0 AndAlso isCap AndAlso (Not wasCap OrElse i + 1 < input.Length AndAlso Char.IsLower(input(i + 1))) Then
                        firstWord = False
                    Else
                        result.Append(Char.ToLowerInvariant(input(i)))
                        Continue While
                    End If
                ElseIf capitalizeNext Then
                    capitalizeNext = False

                    If Char.IsLower(input(i)) Then
                        result.Append(Char.ToUpperInvariant(input(i)))
                        Continue While
                    End If
                End If

                result.Append(input(i))
                i += 1
                wasCap = isCap
            End While

            Return result.ToString()
        End Function

        Private Shared Sub WriteNull(writer As TextWriter)
            writer.Write("null")
        End Sub

        Private Shared Function IsDefaultValue(accessor As IFieldAccessor, value As Object) As Boolean
            If accessor.Descriptor.IsMap Then
                Dim dictionary = CType(value, IDictionary)
                Return dictionary.Count = 0
            End If

            If accessor.Descriptor.IsRepeated Then
                Dim list = CType(value, IList)
                Return list.Count = 0
            End If

            Select Case accessor.Descriptor.FieldType
                Case FieldType.Bool
                    Return CBool(value) = False
                Case FieldType.Bytes
                    Return CType(value, ByteString) Is ByteString.Empty
                Case FieldType.String
                    Return Equals(CStr(value), "")
                Case FieldType.Double
                    Return CDbl(value) = 0.0
                Case FieldType.SInt32, FieldType.Int32, FieldType.SFixed32, FieldType.Enum
                    Return CInt(value) = 0
                Case FieldType.Fixed32, FieldType.UInt32
                    Return CUInt(value) = 0
                Case FieldType.Fixed64, FieldType.UInt64
                    Return CULng(value) = 0
                Case FieldType.SFixed64, FieldType.Int64, FieldType.SInt64
                    Return CLng(value) = 0
                Case FieldType.Float
                    Return CSng(value) = 0F
                Case FieldType.Message, FieldType.Group ' Never expect to get this, but...
                    Return value Is Nothing
                Case Else
                    Throw New ArgumentException("Invalid field type")
            End Select
        End Function

        ''' <summary>
        ''' Writes a single value to the given writer as JSON. Only types understood by
        ''' Protocol Buffers can be written in this way. This method is only exposed for
        ''' advanced use cases; most users should be using <see cref="Format(IMessage)"/>
        ''' or <see cref="Format(IMessage,TextWriter)"/>.
        ''' </summary>
        ''' <param name="writer">The writer to write the value to. Must not be null.</param>
        ''' <param name="value">The value to write. May be null.</param>
        Public Sub WriteValue(writer As TextWriter, value As Object)
            If value Is Nothing Then
                WriteNull(writer)
            ElseIf TypeOf value Is Boolean Then
                writer.Write(If(value, "true", "false"))
            ElseIf TypeOf value Is ByteString Then
                ' Nothing in Base64 needs escaping
                writer.Write(""""c)
                writer.Write(CType(value, ByteString).ToBase64())
                writer.Write(""""c)
            ElseIf TypeOf value Is String Then
                WriteString(writer, CStr(value))
            ElseIf TypeOf value Is IDictionary Then
                WriteDictionary(writer, CType(value, IDictionary))
            ElseIf TypeOf value Is IList Then
                WriteList(writer, CType(value, IList))
            ElseIf TypeOf value Is Integer OrElse TypeOf value Is UInteger Then
                Dim formattable = CType(value, IFormattable)
                writer.Write(formattable.ToString("d", CultureInfo.InvariantCulture))
            ElseIf TypeOf value Is Long OrElse TypeOf value Is ULong Then
                writer.Write(""""c)
                Dim formattable = CType(value, IFormattable)
                writer.Write(formattable.ToString("d", CultureInfo.InvariantCulture))
                writer.Write(""""c)
            ElseIf TypeOf value Is System.Enum Then
                Dim name = OriginalEnumValueHelper.GetOriginalName(value)

                If Not Equals(name, Nothing) Then
                    WriteString(writer, name)
                Else
                    WriteValue(writer, CInt(value))
                End If
            ElseIf TypeOf value Is Single OrElse TypeOf value Is Double Then
                Dim text = CType(value, IFormattable).ToString("r", CultureInfo.InvariantCulture)

                If Equals(text, "NaN") OrElse Equals(text, "Infinity") OrElse Equals(text, "-Infinity") Then
                    writer.Write(""""c)
                    writer.Write(text)
                    writer.Write(""""c)
                Else
                    writer.Write(text)
                End If
            ElseIf TypeOf value Is IMessage Then
                Format(CType(value, IMessage), writer)
            Else
                Throw New ArgumentException("Unable to format value of type " & value.GetType().ToString)
            End If
        End Sub

        ''' <summary>
        ''' Central interception point for well-known type formatting. Any well-known types which
        ''' don't need special handling can fall back to WriteMessage. We avoid assuming that the
        ''' values are using the embedded well-known types, in order to allow for dynamic messages
        ''' in the future.
        ''' </summary>
        Private Sub WriteWellKnownTypeValue(writer As TextWriter, descriptor As MessageDescriptor, value As Object)
            ' Currently, we can never actually get here, because null values are always handled by the caller. But if we *could*,
            ' this would do the right thing.
            If value Is Nothing Then
                WriteNull(writer)
                Return
            End If
            ' For wrapper types, the value will either be the (possibly boxed) "native" value,
            ' or the message itself if we're formatting it at the top level (e.g. just calling ToString on the object itself).
            ' If it's the message form, we can extract the value first, which *will* be the (possibly boxed) native value,
            ' and then proceed, writing it as if we were definitely in a field. (We never need to wrap it in an extra string...
            ' WriteValue will do the right thing.)
            If descriptor.IsWrapperType Then
                If TypeOf value Is IMessage Then
                    Dim message = CType(value, IMessage)
                    value = message.Descriptor.Fields(WrapperValueFieldNumber).Accessor.GetValue(message)
                End If

                WriteValue(writer, value)
                Return
            End If

            If Equals(descriptor.FullName, Timestamp.DescriptorProp.FullName) Then
                WriteTimestamp(writer, CType(value, IMessage))
                Return
            End If

            If Equals(descriptor.FullName, Duration.DescriptorProp.FullName) Then
                WriteDuration(writer, CType(value, IMessage))
                Return
            End If

            If Equals(descriptor.FullName, FieldMask.DescriptorProp.FullName) Then
                WriteFieldMask(writer, CType(value, IMessage))
                Return
            End If

            If Equals(descriptor.FullName, Struct.DescriptorProp.FullName) Then
                WriteStruct(writer, CType(value, IMessage))
                Return
            End If

            If Equals(descriptor.FullName, ListValue.DescriptorProp.FullName) Then
                Dim fieldAccessor = descriptor.Fields(ListValue.ValuesFieldNumber).Accessor
                WriteList(writer, CType(fieldAccessor.GetValue(CType(value, IMessage)), IList))
                Return
            End If

            If Equals(descriptor.FullName, WellKnownTypes.Value.DescriptorProp.FullName) Then
                WriteStructFieldValue(writer, CType(value, IMessage))
                Return
            End If

            If Equals(descriptor.FullName, Any.DescriptorProp.FullName) Then
                WriteAny(writer, CType(value, IMessage))
                Return
            End If

            WriteMessage(writer, CType(value, IMessage))
        End Sub

        Private Sub WriteTimestamp(writer As TextWriter, value As IMessage)
            ' TODO: In the common case where this *is* using the built-in Timestamp type, we could
            ' avoid all the reflection at this point, by casting to Timestamp. In the interests of
            ' avoiding subtle bugs, don't do that until we've implemented DynamicMessage so that we can prove
            ' it still works in that case.
            Dim nanos As Integer = value.Descriptor.Fields(Timestamp.NanosFieldNumber).Accessor.GetValue(value)
            Dim seconds As Long = value.Descriptor.Fields(Timestamp.SecondsFieldNumber).Accessor.GetValue(value)
            writer.Write(Timestamp.ToJson(seconds, nanos, DiagnosticOnly))
        End Sub

        Private Sub WriteDuration(writer As TextWriter, value As IMessage)
            ' TODO: Same as for WriteTimestamp
            Dim nanos As Integer = value.Descriptor.Fields(Duration.NanosFieldNumber).Accessor.GetValue(value)
            Dim seconds As Long = value.Descriptor.Fields(Duration.SecondsFieldNumber).Accessor.GetValue(value)
            writer.Write(Duration.ToJson(seconds, nanos, DiagnosticOnly))
        End Sub

        Private Sub WriteFieldMask(writer As TextWriter, value As IMessage)
            Dim paths = CType(value.Descriptor.Fields(FieldMask.PathsFieldNumber).Accessor.GetValue(value), IList(Of String))
            writer.Write(FieldMask.ToJson(paths, DiagnosticOnly))
        End Sub

        Private Sub WriteAny(writer As TextWriter, value As IMessage)
            If DiagnosticOnly Then
                WriteDiagnosticOnlyAny(writer, value)
                Return
            End If

            Dim typeUrl = CStr(value.Descriptor.Fields(Any.TypeUrlFieldNumber).Accessor.GetValue(value))
            Dim data = CType(value.Descriptor.Fields(Any.ValueFieldNumber).Accessor.GetValue(value), ByteString)
            Dim typeName = Any.GetTypeName(typeUrl)
            Dim descriptor = settingsField.TypeRegistry.Find(typeName)

            If descriptor Is Nothing Then
                Throw New InvalidOperationException($"Type registry has no descriptor for type name '{typeName}'")
            End If

            Dim message = descriptor.Parser.ParseFrom(data)
            writer.Write("{ ")
            WriteString(writer, AnyTypeUrlField)
            writer.Write(NameValueSeparator)
            WriteString(writer, typeUrl)

            If descriptor.IsWellKnownType Then
                writer.Write(PropertySeparator)
                WriteString(writer, AnyWellKnownTypeValueField)
                writer.Write(NameValueSeparator)
                WriteWellKnownTypeValue(writer, descriptor, message)
            Else
                WriteMessageFields(writer, message, True)
            End If

            writer.Write(" }")
        End Sub

        Private Sub WriteDiagnosticOnlyAny(writer As TextWriter, value As IMessage)
            Dim typeUrl = CStr(value.Descriptor.Fields(Any.TypeUrlFieldNumber).Accessor.GetValue(value))
            Dim data = CType(value.Descriptor.Fields(Any.ValueFieldNumber).Accessor.GetValue(value), ByteString)
            writer.Write("{ ")
            WriteString(writer, AnyTypeUrlField)
            writer.Write(NameValueSeparator)
            WriteString(writer, typeUrl)
            writer.Write(PropertySeparator)
            WriteString(writer, AnyDiagnosticValueField)
            writer.Write(NameValueSeparator)
            writer.Write(""""c)
            writer.Write(data.ToBase64())
            writer.Write(""""c)
            writer.Write(" }")
        End Sub

        Private Sub WriteStruct(writer As TextWriter, message As IMessage)
            writer.Write("{ ")
            Dim fields = CType(message.Descriptor.Fields(Struct.FieldsFieldNumber).Accessor.GetValue(message), IDictionary)
            Dim first = True

            For Each entry As DictionaryEntry In fields
                Dim key = CStr(entry.Key)
                Dim value = CType(entry.Value, IMessage)

                If String.IsNullOrEmpty(key) OrElse value Is Nothing Then
                    Throw New InvalidOperationException("Struct fields cannot have an empty key or a null value.")
                End If

                If Not first Then
                    writer.Write(PropertySeparator)
                End If

                WriteString(writer, key)
                writer.Write(NameValueSeparator)
                WriteStructFieldValue(writer, value)
                first = False
            Next

            writer.Write(If(first, "}", " }"))
        End Sub

        Private Sub WriteStructFieldValue(writer As TextWriter, message As IMessage)
            Dim specifiedField = message.Descriptor.Oneofs(0).Accessor.GetCaseFieldDescriptor(message)

            If specifiedField Is Nothing Then
                Throw New InvalidOperationException("Value message must contain a value for the oneof.")
            End If

            Dim value = specifiedField.Accessor.GetValue(message)

            Select Case specifiedField.FieldNumber
                Case WellKnownTypes.Value.BoolValueFieldNumber, WellKnownTypes.Value.StringValueFieldNumber, WellKnownTypes.Value.NumberValueFieldNumber
                    WriteValue(writer, value)
                    Return
                Case WellKnownTypes.Value.StructValueFieldNumber, WellKnownTypes.Value.ListValueFieldNumber
                    ' Structs and ListValues are nested messages, and already well-known types.
                    Dim nestedMessage = CType(specifiedField.Accessor.GetValue(message), IMessage)
                    WriteWellKnownTypeValue(writer, nestedMessage.Descriptor, nestedMessage)
                    Return
                Case WellKnownTypes.Value.NullValueFieldNumber
                    WriteNull(writer)
                    Return
                Case Else
                    Throw New InvalidOperationException("Unexpected case in struct field: " & specifiedField.FieldNumber)
            End Select
        End Sub

        Friend Sub WriteList(writer As TextWriter, list As IList)
            writer.Write("[ ")
            Dim first = True

            For Each value In list

                If Not first Then
                    writer.Write(PropertySeparator)
                End If

                WriteValue(writer, value)
                first = False
            Next

            writer.Write(If(first, "]", " ]"))
        End Sub

        Friend Sub WriteDictionary(writer As TextWriter, dictionary As IDictionary)
            writer.Write("{ ")
            Dim first = True
            ' This will box each pair. Could use IDictionaryEnumerator, but that's ugly in terms of disposal.
            For Each pair As DictionaryEntry In dictionary

                If Not first Then
                    writer.Write(PropertySeparator)
                End If

                Dim keyText As String

                If TypeOf pair.Key Is String Then
                    keyText = CStr(pair.Key)
                ElseIf TypeOf pair.Key Is Boolean Then
                    keyText = If(pair.Key, "true", "false")
                ElseIf TypeOf pair.Key Is Integer OrElse TypeOf pair.Key Is UInteger Or TypeOf pair.Key Is Long OrElse TypeOf pair.Key Is ULong Then
                    keyText = CType(pair.Key, IFormattable).ToString("d", CultureInfo.InvariantCulture)
                Else

                    If pair.Key Is Nothing Then
                        Throw New ArgumentException("Dictionary has entry with null key")
                    End If

                    Throw New ArgumentException("Unhandled dictionary key type: " & pair.Key.GetType().ToString)
                End If

                WriteString(writer, keyText)
                writer.Write(NameValueSeparator)
                WriteValue(writer, pair.Value)
                first = False
            Next

            writer.Write(If(first, "}", " }"))
        End Sub

        ''' <summary>
        ''' Writes a string (including leading and trailing double quotes) to a builder, escaping as required.
        ''' </summary>
        ''' <remarks>
        ''' Other than surrogate pair handling, this code is mostly taken from src/google/protobuf/util/internal/json_escaping.cc.
        ''' </remarks>
        Friend Shared Sub WriteString(writer As TextWriter, text As String)
            writer.Write(""""c)

            For i = 0 To text.Length - 1
                Dim c As chr = text(i)

                If c < &HA0 Then
                    writer.Write(CommonRepresentations(c))
                    Continue For
                End If

                If Char.IsHighSurrogate(c) Then
                    ' Encountered first part of a surrogate pair.
                    ' Check that we have the whole pair, and encode both parts as hex.
                    i += 1

                    If i = text.Length OrElse Not Char.IsLowSurrogate(text(i)) Then
                        Throw New ArgumentException("String contains low surrogate not followed by high surrogate")
                    End If

                    HexEncodeUtf16CodeUnit(writer, c)
                    HexEncodeUtf16CodeUnit(writer, text(i))
                    Continue For
                ElseIf Char.IsLowSurrogate(c) Then
                    Throw New ArgumentException("String contains high surrogate not preceded by low surrogate")
                End If

                Select Case CInt(c)
                    ' These are not required by json spec
                    ' but used to prevent security bugs in javascript.
                    Case &HFEFF, &HFFF9, &HFFFA, &HFFFB, &HAD, &H6DD, &H70F, &H17B4, &H17B5  ' Zero width no-break space
                        ' Interlinear annotation anchor
                        ' Interlinear annotation separator
                        ' Interlinear annotation terminator
                        ' Soft-hyphen
                        ' Arabic end of ayah
                        ' Syriac abbreviation mark
                        ' Khmer vowel inherent Aq
                        ' Khmer vowel inherent Aa
                        HexEncodeUtf16CodeUnit(writer, c)
                    Case Else

                        If c >= &H600 AndAlso c <= &H603 OrElse c >= &H200B AndAlso c <= &H200F OrElse c >= &H2028 AndAlso c <= &H202E OrElse c >= &H2060 AndAlso c <= &H2064 OrElse c >= &H206A AndAlso c <= &H206F Then  ' Arabic signs
                            ' Zero width etc.
                            ' Separators etc.
                            ' Invisible etc.
                            HexEncodeUtf16CodeUnit(writer, c)
                        Else
                            ' No handling of surrogates here - that's done earlier
                            writer.Write(c)
                        End If
                End Select
            Next

            writer.Write(""""c)
        End Sub

        Private Const Hex As String = "0123456789abcdef"

        Private Shared Sub HexEncodeUtf16CodeUnit(writer As TextWriter, c As chr)
            writer.Write("\u")
            writer.Write(Hex(c >> 12 And &HF))
            writer.Write(Hex(c >> 8 And &HF))
            writer.Write(Hex(c >> 4 And &HF))
            writer.Write(Hex(c >> 0 And &HF))
        End Sub

        ''' <summary>
        ''' Settings controlling JSON formatting.
        ''' </summary>
        Public NotInheritable Class Settings
            ''' <summary>
            ''' Default settings, as used by <see cref="JsonFormatter.Default"/>
            ''' </summary>
            Public Shared ReadOnly Property [Default] As Settings

            ' Workaround for the Mono compiler complaining about XML comments not being on
            ' valid language elements.
            Shared Sub New()
                [Default] = New Settings(False)
            End Sub

            ''' <summary>
            ''' Whether fields whose values are the default for the field type (e.g. 0 for integers)
            ''' should be formatted (true) or omitted (false).
            ''' </summary>
            Public ReadOnly Property FormatDefaultValues As Boolean

            ''' <summary>
            ''' The type registry used to format <see cref="Any"/> messages.
            ''' </summary>
            Public ReadOnly Property TypeRegistry As TypeRegistry

            ' TODO: Work out how we're going to scale this to multiple settings. "WithXyz" methods?

            ''' <summary>
            ''' Creates a new <see cref="Settings"/> object with the specified formatting of default values
            ''' and an empty type registry.
            ''' </summary>
            ''' <param name="formatDefaultValues"><c>true</c> if default values (0, empty strings etc) should be formatted; <c>false</c> otherwise.</param>
            Public Sub New(formatDefaultValues As Boolean)
                Me.New(formatDefaultValues, TypeRegistry.Empty)
            End Sub

            ''' <summary>
            ''' Creates a new <see cref="Settings"/> object with the specified formatting of default values
            ''' and type registry.
            ''' </summary>
            ''' <param name="formatDefaultValues"><c>true</c> if default values (0, empty strings etc) should be formatted; <c>false</c> otherwise.</param>
            ''' <param name="typeRegistry">The <see cref="TypeRegistry"/> to use when formatting <see cref="Any"/> messages.</param>
            Public Sub New(formatDefaultValues As Boolean, typeRegistry As TypeRegistry)
                Me.FormatDefaultValues = formatDefaultValues
                Me.TypeRegistry = CheckNotNull(typeRegistry, NameOf(typeRegistry))
            End Sub
        End Class

        ' Effectively a cache of mapping from enum values to the original name as specified in the proto file,
        ' fetched by reflection.
        ' The need for this is unfortunate, as is its unbounded size, but realistically it shouldn't cause issues.
        Private NotInheritable Class OriginalEnumValueHelper
            ' TODO: In the future we might want to use ConcurrentDictionary, at the point where all
            ' the platforms we target have it.
            Private Shared ReadOnly dictionaries As Dictionary(Of System.Type, Dictionary(Of Object, String)) = New Dictionary(Of System.Type, Dictionary(Of Object, String))()

            Friend Shared Function GetOriginalName(value As Object) As String
                Dim enumType = value.GetType()
                Dim nameMapping As Dictionary(Of Object, String)

                SyncLock dictionaries

                    If Not dictionaries.TryGetValue(enumType, nameMapping) Then
                        nameMapping = GetNameMapping(enumType)
                        dictionaries(enumType) = nameMapping
                    End If
                End SyncLock

                Dim originalName As String
                ' If this returns false, originalName will be null, which is what we want.
                nameMapping.TryGetValue(value, originalName)
                Return originalName
            End Function

#If DOTNET35
            // TODO: Consider adding functionality to TypeExtensions to avoid this difference.
            private static Dictionary<object, string> GetNameMapping(System.Type enumType) =>
                enumType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                    .ToDictionary(f => f.GetValue(null),
                                  f => (f.GetCustomAttributes(typeof(OriginalNameAttribute), false)
                                        .FirstOrDefault() as OriginalNameAttribute)
                                        // If the attribute hasn't been applied, fall back to the name of the field.
                                        ?.Name ?? f.Name);
#Else
            Private Shared Function GetNameMapping(enumType As System.Type) As Dictionary(Of Object, String)
                ' If the attribute hasn't been applied, fall back to the name of the field.
                Return enumType.GetTypeInfo().DeclaredFields.Where(Function(f) f.IsStatic).ToDictionary(Function(f) f.GetValue(Nothing), Function(f) If(f.GetCustomAttributes(Of OriginalNameAttribute)().FirstOrDefault()?.Name, f.Name))
            End Function
#End If
        End Class
    End Class
End Namespace
