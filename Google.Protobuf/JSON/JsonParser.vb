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

Imports Google.Protobuf.Reflection
Imports Google.Protobuf.WellKnownTypes
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Google.Protobuf
    ''' <summary>
    ''' Reflection-based converter from JSON to messages.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' Instances of this class are thread-safe, with no mutable state.
    ''' </para>
    ''' <para>
    ''' This is a simple start to get JSON parsing working. As it's reflection-based,
    ''' it's not as quick as baking calls into generated messages - but is a simpler implementation.
    ''' (This code is generally not heavily optimized.)
    ''' </para>
    ''' </remarks>
    Public NotInheritable Class JsonParser
        ' Note: using 0-9 instead of \d to ensure no non-ASCII digits.
        ' This regex isn't a complete validator, but will remove *most* invalid input. We rely on parsing to do the rest.
        Private Shared ReadOnly TimestampRegex As Regex = New Regex("^(?<datetime>[0-9]{4}-[01][0-9]-[0-3][0-9]T[012][0-9]:[0-5][0-9]:[0-5][0-9])(?<subseconds>\.[0-9]{1,9})?(?<offset>(Z|[+-][0-1][0-9]:[0-5][0-9]))$", CompiledRegexWhereAvailable)
        Private Shared ReadOnly DurationRegex As Regex = New Regex("^(?<sign>-)?(?<int>[0-9]{1,12})(?<subseconds>\.[0-9]{1,9})?s$", CompiledRegexWhereAvailable)
        Private Shared ReadOnly SubsecondScalingFactors As Integer() = {0, 100000000, 100000000, 10000000, 1000000, 100000, 10000, 1000, 100, 10, 1}
        Private Shared ReadOnly FieldMaskPathSeparators As Char() = {","c}
        Private Shared ReadOnly defaultInstance As JsonParser = New JsonParser(Settings.Default)

        ' TODO: Consider introducing a class containing parse state of the parser, tokenizer and depth. That would simplify these handlers
        ' and the signatures of various methods.
        Private Shared ReadOnly WellKnownTypeHandlers As Dictionary(Of String, Action(Of JsonParser, IMessage, JsonTokenizer)) = New Dictionary(Of String, Action(Of JsonParser, IMessage, JsonTokenizer)) From {
            {Timestamp.DescriptorProp.FullName, Sub(parser, message, tokenizer) MergeTimestamp(message, tokenizer.Next())},
            {Duration.DescriptorProp.FullName, Sub(parser, message, tokenizer) MergeDuration(message, tokenizer.Next())},
            {Value.DescriptorProp.FullName, Sub(parser, message, tokenizer) parser.MergeStructValue(message, tokenizer)},
            {ListValue.DescriptorProp.FullName, Sub(parser, message, tokenizer) parser.MergeRepeatedField(message, message.Descriptor.Fields(ListValue.ValuesFieldNumber), tokenizer)},
            {Struct.DescriptorProp.FullName, Sub(parser, message, tokenizer) parser.MergeStruct(message, tokenizer)},
            {Any.DescriptorProp.FullName, Sub(parser, message, tokenizer) parser.MergeAny(message, tokenizer)},
            {FieldMask.DescriptorProp.FullName, Sub(parser, message, tokenizer) MergeFieldMask(message, tokenizer.Next())},
            {Int32Value.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {Int64Value.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {UInt32Value.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {UInt64Value.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {FloatValue.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {DoubleValue.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {BytesValue.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {StringValue.DescriptorProp.FullName, AddressOf MergeWrapperField},
            {BoolValue.DescriptorProp.FullName, AddressOf MergeWrapperField}
        }

        ' Convenience method to avoid having to repeat the same code multiple times in the above
        ' dictionary initialization.
        Private Shared Sub MergeWrapperField(parser As JsonParser, message As IMessage, tokenizer As JsonTokenizer)
            parser.MergeField(message, message.Descriptor.Fields(WrapperValueFieldNumber), tokenizer)
        End Sub

        ''' <summary>
        ''' Returns a formatter using the default settings.
        ''' </summary>
        Public Shared ReadOnly Property [Default] As JsonParser
            Get
                Return defaultInstance
            End Get
        End Property

        Private ReadOnly settingsField As Settings

        ''' <summary>
        ''' Creates a new formatted with the given settings.
        ''' </summary>
        ''' <param name="settings">The settings.</param>
        Public Sub New(settings As Settings)
            settingsField = settings
        End Sub

        ''' <summary>
        ''' Parses <paramrefname="json"/> and merges the information into the given message.
        ''' </summary>
        ''' <param name="message">The message to merge the JSON information into.</param>
        ''' <param name="json">The JSON to parse.</param>
        Friend Sub Merge(message As IMessage, json As String)
            Merge(message, New StringReader(json))
        End Sub

        ''' <summary>
        ''' Parses JSON read from <paramrefname="jsonReader"/> and merges the information into the given message.
        ''' </summary>
        ''' <param name="message">The message to merge the JSON information into.</param>
        ''' <param name="jsonReader">Reader providing the JSON to parse.</param>
        Friend Sub Merge(message As IMessage, jsonReader As TextReader)
            Dim tokenizer = JsonTokenizer.FromTextReader(jsonReader)
            Merge(message, tokenizer)
            Dim lastToken = tokenizer.Next()

            If lastToken IsNot JsonToken.EndDocument Then
                Throw New InvalidProtocolBufferException("Expected end of JSON after object")
            End If
        End Sub

        ''' <summary>
        ''' Merges the given message using data from the given tokenizer. In most cases, the next
        ''' token should be a "start object" token, but wrapper types and nullity can invalidate
        ''' that assumption. This is implemented as an LL(1) recursive descent parser over the stream
        ''' of tokens provided by the tokenizer. This token stream is assumed to be valid JSON, with the
        ''' tokenizer performing that validation - but not every token stream is valid "protobuf JSON".
        ''' </summary>
        Private Sub Merge(message As IMessage, tokenizer As JsonTokenizer)
            If tokenizer.ObjectDepth > settingsField.RecursionLimit Then
                Throw InvalidProtocolBufferException.JsonRecursionLimitExceeded()
            End If

            If message.Descriptor.IsWellKnownType Then
                Dim handler As Action(Of JsonParser, IMessage, JsonTokenizer)

                If WellKnownTypeHandlers.TryGetValue(message.Descriptor.FullName, handler) Then
                    handler(Me, message, tokenizer)
                    Return
                End If
                ' Well-known types with no special handling continue in the normal way.
            End If

            Dim token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StartObject Then
                Throw New InvalidProtocolBufferException("Expected an object")
            End If

            Dim descriptor = message.Descriptor
            Dim jsonFieldMap = descriptor.Fields.ByJsonName()
            ' All the oneof fields we've already accounted for - we can only see each of them once.
            ' The set is created lazily to avoid the overhead of creating a set for every message
            ' we parsed, when oneofs are relatively rare.
            Dim seenOneofs As HashSet(Of OneofDescriptor) = Nothing

            While True
                token = tokenizer.Next()

                If token.Type = JsonToken.TokenType.EndObject Then
                    Return
                End If

                If token.Type <> JsonToken.TokenType.Name Then
                    Throw New InvalidOperationException("Unexpected token type " & token.Type)
                End If

                Dim name = token.StringValue
                Dim field As FieldDescriptor

                If jsonFieldMap.TryGetValue(name, field) Then
                    If field.ContainingOneof IsNot Nothing Then
                        If seenOneofs Is Nothing Then
                            seenOneofs = New HashSet(Of OneofDescriptor)()
                        End If

                        If Not seenOneofs.Add(field.ContainingOneof) Then
                            Throw New InvalidProtocolBufferException($"Multiple values specified for oneof {field.ContainingOneof.Name}")
                        End If
                    End If

                    MergeField(message, field, tokenizer)
                Else
                    ' TODO: Is this what we want to do? If not, we'll need to skip the value,
                    ' which may be an object or array. (We might want to put code in the tokenizer
                    ' to do that.)
                    Throw New InvalidProtocolBufferException("Unknown field: " & name)
                End If
            End While
        End Sub

        Private Sub MergeField(message As IMessage, field As FieldDescriptor, tokenizer As JsonTokenizer)
            Dim token = tokenizer.Next()

            If token.Type = JsonToken.TokenType.Null Then
                ' Clear the field if we see a null token, unless it's for a singular field of type
                ' google.protobuf.Value.
                ' Note: different from Java API, which just ignores it.
                ' TODO: Bring it more in line? Discuss...
                If field.IsMap OrElse field.IsRepeated OrElse Not IsGoogleProtobufValueField(field) Then
                    field.Accessor.Clear(message)
                    Return
                End If
            End If

            tokenizer.PushBack(token)

            If field.IsMap Then
                MergeMapField(message, field, tokenizer)
            ElseIf field.IsRepeated Then
                MergeRepeatedField(message, field, tokenizer)
            Else
                Dim value = ParseSingleValue(field, tokenizer)
                field.Accessor.SetValue(message, value)
            End If
        End Sub

        Private Sub MergeRepeatedField(message As IMessage, field As FieldDescriptor, tokenizer As JsonTokenizer)
            Dim token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StartArray Then
                Throw New InvalidProtocolBufferException("Repeated field value was not an array. Token type: " & token.Type)
            End If

            Dim list = CType(field.Accessor.GetValue(message), IList)

            While True
                token = tokenizer.Next()

                If token.Type = JsonToken.TokenType.EndArray Then
                    Return
                End If

                tokenizer.PushBack(token)

                If token.Type = JsonToken.TokenType.Null Then
                    Throw New InvalidProtocolBufferException("Repeated field elements cannot be null")
                End If

                list.Add(ParseSingleValue(field, tokenizer))
            End While
        End Sub

        Private Sub MergeMapField(message As IMessage, field As FieldDescriptor, tokenizer As JsonTokenizer)
            ' Map fields are always objects, even if the values are well-known types: ParseSingleValue handles those.
            Dim token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StartObject Then
                Throw New InvalidProtocolBufferException("Expected an object to populate a map")
            End If

            Dim type = field.MessageType
            Dim keyField = type.FindFieldByNumber(1)
            Dim valueField = type.FindFieldByNumber(2)

            If keyField Is Nothing OrElse valueField Is Nothing Then
                Throw New InvalidProtocolBufferException("Invalid map field: " & field.FullName)
            End If

            Dim dictionary = CType(field.Accessor.GetValue(message), IDictionary)

            While True
                token = tokenizer.Next()

                If token.Type = JsonToken.TokenType.EndObject Then
                    Return
                End If

                Dim key = ParseMapKey(keyField, token.StringValue)
                Dim value = ParseSingleValue(valueField, tokenizer)

                If value Is Nothing Then
                    Throw New InvalidProtocolBufferException("Map values must not be null")
                End If

                dictionary(key) = value
            End While
        End Sub

        Private Shared Function IsGoogleProtobufValueField(field As FieldDescriptor) As Boolean
            Return field.FieldType = FieldType.Message AndAlso Equals(field.MessageType.FullName, Value.DescriptorProp.FullName)
        End Function

        Private Function ParseSingleValue(field As FieldDescriptor, tokenizer As JsonTokenizer) As Object
            Dim token = tokenizer.Next()

            If token.Type = JsonToken.TokenType.Null Then
                ' TODO: In order to support dynamic messages, we should really build this up
                ' dynamically.
                If IsGoogleProtobufValueField(field) Then
                    Return Value.ForNull()
                End If

                Return Nothing
            End If

            Dim fieldType = field.FieldType

            If fieldType = FieldType.Message Then
                ' Parse wrapper types as their constituent types.
                ' TODO: What does this mean for null?
                If field.MessageType.IsWrapperType Then
                    field = field.MessageType.Fields(WrapperValueFieldNumber)
                    fieldType = field.FieldType
                Else
                    ' TODO: Merge the current value in message? (Public API currently doesn't make this relevant as we don't expose merging.)
                    tokenizer.PushBack(token)
                    Dim subMessage = NewMessageForField(field)
                    Merge(subMessage, tokenizer)
                    Return subMessage
                End If
            End If

            Select Case token.Type
                Case JsonToken.TokenType.True, JsonToken.TokenType.False

                    If fieldType = FieldType.Bool Then
                        Return token.Type = JsonToken.TokenType.True
                    End If
                    ' Fall through to "we don't support this type for this case"; could duplicate the behaviour of the default
                    ' case instead, but this way we'd only need to change one place.
                    GoTo _Select0_CaseDefault
                Case JsonToken.TokenType.StringValue
                    Return ParseSingleStringValue(field, token.StringValue)
                ' Note: not passing the number value itself here, as we may end up storing the string value in the token too.
                Case JsonToken.TokenType.Number
                    Return ParseSingleNumberValue(field, token)
                Case JsonToken.TokenType.Null
                    Throw New NotImplementedException("Haven't worked out what to do for null yet")
                Case Else
_Select0_CaseDefault:
                    Throw New InvalidProtocolBufferException("Unsupported JSON token type " & token.Type & " for field type " & fieldType)
            End Select
        End Function

        ''' <summary>
        ''' Parses <paramrefname="json"/> into a new message.
        ''' </summary>
        ''' <typeparam name="T">The type of message to create.</typeparam>
        ''' <param name="json">The JSON to parse.</param>
        ''' <exception cref="InvalidJsonException">The JSON does not comply with RFC 7159</exception>
        ''' <exception cref="InvalidProtocolBufferException">The JSON does not represent a Protocol Buffers message correctly</exception>
        Public Function Parse(Of T As {IMessage, New})(json As String) As T
            CheckNotNull(json, NameOf(json))
            Return Parse(Of T)(New StringReader(json))
        End Function

        ''' <summary>
        ''' Parses JSON read from <paramrefname="jsonReader"/> into a new message.
        ''' </summary>
        ''' <typeparam name="T">The type of message to create.</typeparam>
        ''' <param name="jsonReader">Reader providing the JSON to parse.</param>
        ''' <exception cref="InvalidJsonException">The JSON does not comply with RFC 7159</exception>
        ''' <exception cref="InvalidProtocolBufferException">The JSON does not represent a Protocol Buffers message correctly</exception>
        Public Function Parse(Of T As {IMessage, New})(jsonReader As TextReader) As T
            CheckNotNull(jsonReader, NameOf(jsonReader))
            Dim message As T = New T()
            Merge(message, jsonReader)
            Return message
        End Function

        ''' <summary>
        ''' Parses <paramrefname="json"/> into a new message.
        ''' </summary>
        ''' <param name="json">The JSON to parse.</param>
        ''' <param name="descriptor">Descriptor of message type to parse.</param>
        ''' <exception cref="InvalidJsonException">The JSON does not comply with RFC 7159</exception>
        ''' <exception cref="InvalidProtocolBufferException">The JSON does not represent a Protocol Buffers message correctly</exception>
        Public Function Parse(json As String, descriptor As MessageDescriptor) As IMessage
            CheckNotNull(json, NameOf(json))
            CheckNotNull(descriptor, NameOf(descriptor))
            Return Parse(New StringReader(json), descriptor)
        End Function

        ''' <summary>
        ''' Parses JSON read from <paramrefname="jsonReader"/> into a new message.
        ''' </summary>
        ''' <param name="jsonReader">Reader providing the JSON to parse.</param>
        ''' <param name="descriptor">Descriptor of message type to parse.</param>
        ''' <exception cref="InvalidJsonException">The JSON does not comply with RFC 7159</exception>
        ''' <exception cref="InvalidProtocolBufferException">The JSON does not represent a Protocol Buffers message correctly</exception>
        Public Function Parse(jsonReader As TextReader, descriptor As MessageDescriptor) As IMessage
            CheckNotNull(jsonReader, NameOf(jsonReader))
            CheckNotNull(descriptor, NameOf(descriptor))
            Dim message As IMessage = descriptor.Parser.CreateTemplate()
            Merge(message, jsonReader)
            Return message
        End Function

        Private Sub MergeStructValue(message As IMessage, tokenizer As JsonTokenizer)
            Dim firstToken = tokenizer.Next()
            Dim fields = message.Descriptor.Fields

            Select Case firstToken.Type
                Case JsonToken.TokenType.Null
                    fields(Value.NullValueFieldNumber).Accessor.SetValue(message, 0)
                    Return
                Case JsonToken.TokenType.StringValue
                    fields(Value.StringValueFieldNumber).Accessor.SetValue(message, firstToken.StringValue)
                    Return
                Case JsonToken.TokenType.Number
                    fields(Value.NumberValueFieldNumber).Accessor.SetValue(message, firstToken.NumberValue)
                    Return
                Case JsonToken.TokenType.False, JsonToken.TokenType.True
                    fields(Value.BoolValueFieldNumber).Accessor.SetValue(message, firstToken.Type = JsonToken.TokenType.True)
                    Return
                Case JsonToken.TokenType.StartObject
                    Dim field = fields(Value.StructValueFieldNumber)
                    Dim structMessage = NewMessageForField(field)
                    tokenizer.PushBack(firstToken)
                    Merge(structMessage, tokenizer)
                    field.Accessor.SetValue(message, structMessage)
                    Return
                Case JsonToken.TokenType.StartArray
                    Dim field = fields(Value.ListValueFieldNumber)
                    Dim list = NewMessageForField(field)
                    tokenizer.PushBack(firstToken)
                    Merge(list, tokenizer)
                    field.Accessor.SetValue(message, list)
                    Return
                Case Else
                    Throw New InvalidOperationException("Unexpected token type: " & firstToken.Type)
            End Select
        End Sub

        Private Sub MergeStruct(message As IMessage, tokenizer As JsonTokenizer)
            Dim token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StartObject Then
                Throw New InvalidProtocolBufferException("Expected object value for Struct")
            End If

            tokenizer.PushBack(token)
            Dim field = message.Descriptor.Fields(Struct.FieldsFieldNumber)
            MergeMapField(message, field, tokenizer)
        End Sub

        Private Sub MergeAny(message As IMessage, tokenizer As JsonTokenizer)
            ' Record the token stream until we see the @type property. At that point, we can take the value, consult
            ' the type registry for the relevant message, and replay the stream, omitting the @type property.
            Dim tokens = New List(Of JsonToken)()
            Dim token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StartObject Then
                Throw New InvalidProtocolBufferException("Expected object value for Any")
            End If

            Dim typeUrlObjectDepth = tokenizer.ObjectDepth

            ' The check for the property depth protects us from nested Any values which occur before the type URL
            ' for *this* Any.
            While token.Type <> JsonToken.TokenType.Name OrElse Not Equals(token.StringValue, JsonFormatter.AnyTypeUrlField) OrElse tokenizer.ObjectDepth <> typeUrlObjectDepth
                tokens.Add(token)
                token = tokenizer.Next()

                If tokenizer.ObjectDepth < typeUrlObjectDepth Then
                    Throw New InvalidProtocolBufferException("Any message with no @type")
                End If
            End While

            ' Don't add the @type property or its value to the recorded token list
            token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.StringValue Then
                Throw New InvalidProtocolBufferException("Expected string value for Any.@type")
            End If

            Dim typeUrl = token.StringValue
            Dim typeName = Any.GetTypeName(typeUrl)
            Dim descriptor = settingsField.TypeRegistry.Find(typeName)

            If descriptor Is Nothing Then
                Throw New InvalidOperationException($"Type registry has no descriptor for type name '{typeName}'")
            End If

            ' Now replay the token stream we've already read and anything that remains of the object, just parsing it
            ' as normal. Our original tokenizer should end up at the end of the object.
            Dim replay = JsonTokenizer.FromReplayedTokens(tokens, tokenizer)
            Dim body = descriptor.Parser.CreateTemplate()

            If descriptor.IsWellKnownType Then
                MergeWellKnownTypeAnyBody(body, replay)
            Else
                Merge(body, replay)
            End If

            Dim data = body.ToByteString()

            ' Now that we have the message data, we can pack it into an Any (the message received as a parameter).
            message.Descriptor.Fields(Any.TypeUrlFieldNumber).Accessor.SetValue(message, typeUrl)
            message.Descriptor.Fields(Any.ValueFieldNumber).Accessor.SetValue(message, data)
        End Sub

        ' Well-known types end up in a property called "value" in the JSON. As there's no longer a @type property
        ' in the given JSON token stream, we should *only* have tokens of start-object, name("value"), the value
        ' itself, and then end-object.
        Private Sub MergeWellKnownTypeAnyBody(body As IMessage, tokenizer As JsonTokenizer)
            Dim token = tokenizer.Next() ' Definitely start-object; checked in previous method
            token = tokenizer.Next()
            ' TODO: What about an absent Int32Value, for example?
            If token.Type <> JsonToken.TokenType.Name OrElse Not Equals(token.StringValue, JsonFormatter.AnyWellKnownTypeValueField) Then
                Throw New InvalidProtocolBufferException($"Expected '{JsonFormatter.AnyWellKnownTypeValueField}' property for well-known type Any body")
            End If

            Merge(body, tokenizer)
            token = tokenizer.Next()

            If token.Type <> JsonToken.TokenType.EndObject Then
                Throw New InvalidProtocolBufferException($"Expected end-object token after @type/value for well-known type")
            End If
        End Sub

#Region "Utility methods which don't depend on the state (or settings) of the parser."
        Private Shared Function ParseMapKey(field As FieldDescriptor, keyText As String) As Object
            Select Case field.FieldType
                Case FieldType.Bool

                    If Equals(keyText, "true") Then
                        Return True
                    End If

                    If Equals(keyText, "false") Then
                        Return False
                    End If

                    Throw New InvalidProtocolBufferException("Invalid string for bool map key: " & keyText)
                Case FieldType.String
                    Return keyText
                Case FieldType.Int32, FieldType.SInt32, FieldType.SFixed32
                    Return ParseNumericString(keyText, New Func(Of String, NumberStyles, IFormatProvider, Integer)(AddressOf Integer.Parse))
                Case FieldType.UInt32, FieldType.Fixed32
                    Return ParseNumericString(keyText, New Func(Of String, NumberStyles, IFormatProvider, UInteger)(AddressOf UInteger.Parse))
                Case FieldType.Int64, FieldType.SInt64, FieldType.SFixed64
                    Return ParseNumericString(keyText, New Func(Of String, NumberStyles, IFormatProvider, Long)(AddressOf Long.Parse))
                Case FieldType.UInt64, FieldType.Fixed64
                    Return ParseNumericString(keyText, New Func(Of String, NumberStyles, IFormatProvider, ULong)(AddressOf ULong.Parse))
                Case Else
                    Throw New InvalidProtocolBufferException("Invalid field type for map: " & field.FieldType)
            End Select
        End Function

        Private Shared Function ParseSingleNumberValue(field As FieldDescriptor, token As JsonToken) As Object
            Dim value = token.NumberValue
            ' BEGIN TODO : Visual Basic does not support checked statements!
            Try

                Select Case field.FieldType
                    Case FieldType.Int32, FieldType.SInt32, FieldType.SFixed32
                        CheckInteger(value)
                        Return CInt(value)
                    Case FieldType.UInt32, FieldType.Fixed32
                        CheckInteger(value)
                        Return CUInt(value)
                    Case FieldType.Int64, FieldType.SInt64, FieldType.SFixed64
                        CheckInteger(value)
                        Return CLng(value)
                    Case FieldType.UInt64, FieldType.Fixed64
                        CheckInteger(value)
                        Return CULng(value)
                    Case FieldType.Double
                        Return value
                    Case FieldType.Float

                        If Double.IsNaN(value) Then
                            Return Single.NaN
                        End If

                        If value > Single.MaxValue OrElse value < Single.MinValue Then
                            If Double.IsPositiveInfinity(value) Then
                                Return Single.PositiveInfinity
                            End If

                            If Double.IsNegativeInfinity(value) Then
                                Return Single.NegativeInfinity
                            End If

                            Throw New InvalidProtocolBufferException($"Value out of range: {value}")
                        End If

                        Return CSng(value)
                    Case FieldType.Enum
                        CheckInteger(value)
                        ' Just return it as an int, and let the CLR convert it.
                        ' Note that we deliberately don't check that it's a known value.
                        Return CInt(value)
                    Case Else
                        Throw New InvalidProtocolBufferException($"Unsupported conversion from JSON number for field type {field.FieldType}")
                End Select

            Catch __unusedOverflowException1__ As OverflowException
                Throw New InvalidProtocolBufferException($"Value out of range: {value}")
            End Try
            ' END TODO : Visual Basic does not support checked statements!
        End Function

        Private Shared Sub CheckInteger(value As Double)
            If Double.IsInfinity(value) OrElse Double.IsNaN(value) Then
                Throw New InvalidProtocolBufferException($"Value not an integer: {value}")
            End If

            If value <> Math.Floor(value) Then
                Throw New InvalidProtocolBufferException($"Value not an integer: {value}")
            End If
        End Sub

        Private Shared Function ParseSingleStringValue(field As FieldDescriptor, text As String) As Object
            Select Case field.FieldType
                Case FieldType.String
                    Return text
                Case FieldType.Bytes

                    Try
                        Return ByteString.FromBase64(text)
                    Catch e As FormatException
                        Throw InvalidProtocolBufferException.InvalidBase64(e)
                    End Try

                Case FieldType.Int32, FieldType.SInt32, FieldType.SFixed32
                    Return ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, Integer)(AddressOf Integer.Parse))
                Case FieldType.UInt32, FieldType.Fixed32
                    Return ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, UInteger)(AddressOf UInteger.Parse))
                Case FieldType.Int64, FieldType.SInt64, FieldType.SFixed64
                    Return ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, Long)(AddressOf Long.Parse))
                Case FieldType.UInt64, FieldType.Fixed64
                    Return ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, ULong)(AddressOf ULong.Parse))
                Case FieldType.Double
                    Dim d As Double = ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, Double)(AddressOf Double.Parse))
                    ValidateInfinityAndNan(text, Double.IsPositiveInfinity(d), Double.IsNegativeInfinity(d), Double.IsNaN(d))
                    Return d
                Case FieldType.Float
                    Dim f As Single = ParseNumericString(text, New Func(Of String, NumberStyles, IFormatProvider, Single)(AddressOf Single.Parse))
                    ValidateInfinityAndNan(text, Single.IsPositiveInfinity(f), Single.IsNegativeInfinity(f), Single.IsNaN(f))
                    Return f
                Case FieldType.Enum
                    Dim enumValue = field.EnumType.FindValueByName(text)

                    If enumValue Is Nothing Then
                        Throw New InvalidProtocolBufferException($"Invalid enum value: {text} for enum type: {field.EnumType.FullName}")
                    End If
                    ' Just return it as an int, and let the CLR convert it.
                    Return enumValue.Number
                Case Else
                    Throw New InvalidProtocolBufferException($"Unsupported conversion from JSON string for field type {field.FieldType}")
            End Select
        End Function

        ''' <summary>
        ''' Creates a new instance of the message type for the given field.
        ''' </summary>
        Private Shared Function NewMessageForField(field As FieldDescriptor) As IMessage
            Return field.MessageType.Parser.CreateTemplate()
        End Function

        Private Shared Function ParseNumericString(Of T)(text As String, parser As Func(Of String, NumberStyles, IFormatProvider, T)) As T
            ' Can't prohibit this with NumberStyles.
            If text.StartsWith("+") Then
                Throw New InvalidProtocolBufferException($"Invalid numeric value: {text}")
            End If

            If text.StartsWith("0") AndAlso text.Length > 1 Then
                If text(1) >= "0"c AndAlso text(1) <= "9"c Then
                    Throw New InvalidProtocolBufferException($"Invalid numeric value: {text}")
                End If
            ElseIf text.StartsWith("-0") AndAlso text.Length > 2 Then

                If text(2) >= "0"c AndAlso text(2) <= "9"c Then
                    Throw New InvalidProtocolBufferException($"Invalid numeric value: {text}")
                End If
            End If

            Try
                Return parser(text, NumberStyles.AllowLeadingSign Or NumberStyles.AllowDecimalPoint Or NumberStyles.AllowExponent, CultureInfo.InvariantCulture)
            Catch __unusedFormatException1__ As FormatException
                Throw New InvalidProtocolBufferException($"Invalid numeric value for type: {text}")
            Catch __unusedOverflowException2__ As OverflowException
                Throw New InvalidProtocolBufferException($"Value out of range: {text}")
            End Try
        End Function

        ''' <summary>
        ''' Checks that any infinite/NaN values originated from the correct text.
        ''' This corrects the lenient whitespace handling of double.Parse/float.Parse, as well as the
        ''' way that Mono parses out-of-range values as infinity.
        ''' </summary>
        Private Shared Sub ValidateInfinityAndNan(text As String, isPositiveInfinity As Boolean, isNegativeInfinity As Boolean, isNaN As Boolean)
            If isPositiveInfinity AndAlso Not Equals(text, "Infinity") OrElse isNegativeInfinity AndAlso Not Equals(text, "-Infinity") OrElse isNaN AndAlso Not Equals(text, "NaN") Then
                Throw New InvalidProtocolBufferException($"Invalid numeric value: {text}")
            End If
        End Sub

        Private Shared Sub MergeTimestamp(message As IMessage, token As JsonToken)
            If token.Type <> JsonToken.TokenType.StringValue Then
                Throw New InvalidProtocolBufferException("Expected string value for Timestamp")
            End If

            Dim match = TimestampRegex.Match(token.StringValue)

            If Not match.Success Then
                Throw New InvalidProtocolBufferException($"Invalid Timestamp value: {token.StringValue}")
            End If

            Dim dateTime = match.Groups("datetime").Value
            Dim subseconds = match.Groups("subseconds").Value
            Dim offset = match.Groups("offset").Value

            Try
                Dim parsed = Date.ParseExact(dateTime, "yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal Or DateTimeStyles.AdjustToUniversal)
                ' TODO: It would be nice not to have to create all these objects... easy to optimize later though.
                Dim timestamp As Timestamp = Timestamp.FromDateTime(parsed)
                Dim nanosToAdd = 0

                If Not Equals(subseconds, "") Then
                    ' This should always work, as we've got 1-9 digits.
                    Dim parsedFraction = Integer.Parse(subseconds.Substring(1), CultureInfo.InvariantCulture)
                    nanosToAdd = parsedFraction * SubsecondScalingFactors(subseconds.Length)
                End If

                Dim secondsToAdd = 0

                If Not Equals(offset, "Z") Then
                    ' This is the amount we need to *subtract* from the local time to get to UTC - hence - => +1 and vice versa.
                    Dim sign = If(offset(0) = "-"c, 1, -1)
                    Dim hours = Integer.Parse(offset.Substring(1, 2), CultureInfo.InvariantCulture)
                    Dim minutes = Integer.Parse(offset.Substring(4, 2))
                    Dim totalMinutes = hours * 60 + minutes

                    If totalMinutes > 18 * 60 Then
                        Throw New InvalidProtocolBufferException("Invalid Timestamp value: " & token.StringValue)
                    End If

                    If totalMinutes = 0 AndAlso sign = 1 Then
                        ' This is an offset of -00:00, which means "unknown local offset". It makes no sense for a timestamp.
                        Throw New InvalidProtocolBufferException("Invalid Timestamp value: " & token.StringValue)
                    End If
                    ' We need to *subtract* the offset from local time to get UTC.
                    secondsToAdd = sign * totalMinutes * 60
                End If
                ' Ensure we've got the right signs. Currently unnecessary, but easy to do.
                If secondsToAdd < 0 AndAlso nanosToAdd > 0 Then
                    secondsToAdd += 1
                    nanosToAdd = nanosToAdd - Duration.NanosecondsPerSecond
                End If

                If secondsToAdd <> 0 OrElse nanosToAdd <> 0 Then
                    timestamp += New Duration With {
                        .Nanos = nanosToAdd,
                        .Seconds = secondsToAdd
                    }
                    ' The resulting timestamp after offset change would be out of our expected range. Currently the Timestamp message doesn't validate this
                    ' anywhere, but we shouldn't parse it.
                    If timestamp.Seconds < Timestamp.UnixSecondsAtBclMinValue OrElse timestamp.Seconds > Timestamp.UnixSecondsAtBclMaxValue Then
                        Throw New InvalidProtocolBufferException("Invalid Timestamp value: " & token.StringValue)
                    End If
                End If

                message.Descriptor.Fields(Timestamp.SecondsFieldNumber).Accessor.SetValue(message, timestamp.Seconds)
                message.Descriptor.Fields(Timestamp.NanosFieldNumber).Accessor.SetValue(message, timestamp.Nanos)
            Catch __unusedFormatException1__ As FormatException
                Throw New InvalidProtocolBufferException("Invalid Timestamp value: " & token.StringValue)
            End Try
        End Sub

        Private Shared Sub MergeDuration(message As IMessage, token As JsonToken)
            If token.Type <> JsonToken.TokenType.StringValue Then
                Throw New InvalidProtocolBufferException("Expected string value for Duration")
            End If

            Dim match = DurationRegex.Match(token.StringValue)

            If Not match.Success Then
                Throw New InvalidProtocolBufferException("Invalid Duration value: " & token.StringValue)
            End If

            Dim sign = match.Groups("sign").Value
            Dim secondsText = match.Groups("int").Value
            ' Prohibit leading insignficant zeroes
            If secondsText(0) = "0"c AndAlso secondsText.Length > 1 Then
                Throw New InvalidProtocolBufferException("Invalid Duration value: " & token.StringValue)
            End If

            Dim subseconds = match.Groups("subseconds").Value
            Dim multiplier = If(Equals(sign, "-"), -1, 1)

            Try
                Dim seconds = Long.Parse(secondsText, CultureInfo.InvariantCulture) * multiplier
                Dim nanos = 0

                If Not Equals(subseconds, "") Then
                    ' This should always work, as we've got 1-9 digits.
                    Dim parsedFraction = Integer.Parse(subseconds.Substring(1))
                    nanos = parsedFraction * SubsecondScalingFactors(subseconds.Length) * multiplier
                End If

                If Not Duration.IsNormalized(seconds, nanos) Then
                    Throw New InvalidProtocolBufferException($"Invalid Duration value: {token.StringValue}")
                End If

                message.Descriptor.Fields(Duration.SecondsFieldNumber).Accessor.SetValue(message, seconds)
                message.Descriptor.Fields(Duration.NanosFieldNumber).Accessor.SetValue(message, nanos)
            Catch __unusedFormatException1__ As FormatException
                Throw New InvalidProtocolBufferException($"Invalid Duration value: {token.StringValue}")
            End Try
        End Sub

        Private Shared Sub MergeFieldMask(message As IMessage, token As JsonToken)
            If token.Type <> JsonToken.TokenType.StringValue Then
                Throw New InvalidProtocolBufferException("Expected string value for FieldMask")
            End If
            ' TODO: Do we *want* to remove empty entries? Probably okay to treat "" as "no paths", but "foo,,bar"?
            Dim jsonPaths = token.StringValue.Split(FieldMaskPathSeparators, StringSplitOptions.RemoveEmptyEntries)
            Dim messagePaths = CType(message.Descriptor.Fields(FieldMask.PathsFieldNumber).Accessor.GetValue(message), IList)

            For Each path In jsonPaths
                messagePaths.Add(ToSnakeCase(path))
            Next
        End Sub

        ' Ported from src/google/protobuf/util/internal/utility.cc
        Private Shared Function ToSnakeCase(text As String) As String
            Dim builder = New StringBuilder(text.Length * 2)
            ' Note: this is probably unnecessary now, but currently retained to be as close as possible to the
            ' C++, whilst still throwing an exception on underscores.
            Dim wasNotUnderscore = False  ' Initialize to false for case 1 (below)
            Dim wasNotCap = False

            For i = 0 To text.Length - 1
                Dim c = text(i)

                If c >= "A"c AndAlso c <= "Z"c Then ' ascii_isupper
                    ' Consider when the current character B is capitalized:
                    ' 1) At beginning of input:   "B..." => "b..."
                    '    (e.g. "Biscuit" => "biscuit")
                    ' 2) Following a lowercase:   "...aB..." => "...a_b..."
                    '    (e.g. "gBike" => "g_bike")
                    ' 3) At the end of input:     "...AB" => "...ab"
                    '    (e.g. "GoogleLAB" => "google_lab")
                    ' 4) Followed by a lowercase: "...ABc..." => "...a_bc..."
                    '    (e.g. "GBike" => "g_bike")
                    If wasNotUnderscore AndAlso (wasNotCap OrElse i + 1 < text.Length AndAlso text(i + 1) >= "a"c AndAlso text(i + 1) <= "z"c) Then               '            case 1 out
                        ' case 2 in, case 3 out
                        '            case 3 out
                        ' ascii_islower(text[i + 1])
                        ' case 4 in
                        ' We add an underscore for case 2 and case 4.
                        builder.Append("_"c)
                    End If
                    ' ascii_tolower, but we already know that c *is* an upper case ASCII character...
                    builder.Append(Microsoft.VisualBasic.ChrW(AscW(c) + AscW("a"c) - AscW("A"c)))
                    wasNotUnderscore = True
                    wasNotCap = False
                Else
                    builder.Append(c)

                    If c = "_"c Then
                        Throw New InvalidProtocolBufferException($"Invalid field mask: {text}")
                    End If

                    wasNotUnderscore = True
                    wasNotCap = True
                End If
            Next

            Return builder.ToString()
        End Function
#End Region

        ''' <summary>
        ''' Settings controlling JSON parsing.
        ''' </summary>
        Public NotInheritable Class Settings
            ''' <summary>
            ''' Default settings, as used by <see cref="JsonParser.Default"/>. This has the same default
            ''' recursion limit as <see cref="CodedInputStream"/>, and an empty type registry.
            ''' </summary>
            Public Shared ReadOnly Property [Default] As Settings

            ' Workaround for the Mono compiler complaining about XML comments not being on
            ' valid language elements.
            Shared Sub New()
                [Default] = New Settings(CodedInputStream.DefaultRecursionLimit)
            End Sub

            ''' <summary>
            ''' The maximum depth of messages to parse. Note that this limit only applies to parsing
            ''' messages, not collections - so a message within a collection within a message only counts as
            ''' depth 2, not 3.
            ''' </summary>
            Public ReadOnly Property RecursionLimit As Integer

            ''' <summary>
            ''' The type registry used to parse <see cref="Any"/> messages.
            ''' </summary>
            Public ReadOnly Property TypeRegistry As TypeRegistry

            ''' <summary>
            ''' Creates a new <see cref="Settings"/> object with the specified recursion limit.
            ''' </summary>
            ''' <param name="recursionLimit">The maximum depth of messages to parse</param>
            Public Sub New(recursionLimit As Integer)
                Me.New(recursionLimit, TypeRegistry.Empty)
            End Sub

            ''' <summary>
            ''' Creates a new <see cref="Settings"/> object with the specified recursion limit and type registry.
            ''' </summary>
            ''' <param name="recursionLimit">The maximum depth of messages to parse</param>
            ''' <param name="typeRegistry">The type registry used to parse <see cref="Any"/> messages</param>
            Public Sub New(recursionLimit As Integer, typeRegistry As TypeRegistry)
                Me.RecursionLimit = recursionLimit
                Me.TypeRegistry = CheckNotNull(typeRegistry, NameOf(typeRegistry))
            End Sub
        End Class
    End Class
End Namespace
