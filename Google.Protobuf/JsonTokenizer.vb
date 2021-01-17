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
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Language

Namespace Google.Protobuf
    ''' <summary>
    ''' Simple but strict JSON tokenizer, rigidly following RFC 7159.
    ''' </summary>
    ''' <remarks>
    ''' <para>
    ''' This tokenizer is stateful, and only returns "useful" tokens - names, values etc.
    ''' It does not create tokens for the separator between names and values, or for the comma
    ''' between values. It validates the token stream as it goes - so callers can assume that the
    ''' tokens it produces are appropriate. For example, it would never produce "start object, end array."
    ''' </para>
    ''' <para>Implementation details: the base class handles single token push-back and </para>
    ''' <para>Not thread-safe.</para>
    ''' </remarks>
    Friend MustInherit Class JsonTokenizer

        ''' <summary>
        ''' Returns the depth of the stack, purely in objects (not collections).
        ''' Informally, this is the number of remaining unclosed '{' characters we have.
        ''' </summary>
        Private _ObjectDepth As Integer
        Private bufferedToken As JsonToken

        ''' <summary>
        '''  Creates a tokenizer that reads from the given text reader.
        ''' </summary>
        Friend Shared Function FromTextReader(reader As TextReader) As JsonTokenizer
            Return New JsonTextTokenizer(reader)
        End Function

        ''' <summary>
        ''' Creates a tokenizer that first replays the given list of tokens, then continues reading
        ''' from another tokenizer. Note that if the returned tokenizer is "pushed back", that does not push back
        ''' on the continuation tokenizer, or vice versa. Care should be taken when using this method - it was
        ''' created for the sake of Any parsing.
        ''' </summary>
        Friend Shared Function FromReplayedTokens(tokens As IList(Of JsonToken), continuation As JsonTokenizer) As JsonTokenizer
            Return New JsonReplayTokenizer(tokens, continuation)
        End Function

        Friend Property ObjectDepth As Integer
            Get
                Return _ObjectDepth
            End Get
            Private Set(value As Integer)
                _ObjectDepth = value
            End Set
        End Property

        ' TODO: Why do we allow a different token to be pushed back? It might be better to always remember the previous
        ' token returned, and allow a parameterless Rewind() method (which could only be called once, just like the current PushBack).
        Friend Sub PushBack(token As JsonToken)
            If bufferedToken IsNot Nothing Then
                Throw New InvalidOperationException("Can't push back twice")
            End If

            bufferedToken = token

            If token.Type = JsonToken.TokenType.StartObject Then
                ObjectDepth -= 1
            ElseIf token.Type = JsonToken.TokenType.EndObject Then
                ObjectDepth += 1
            End If
        End Sub

        ''' <summary>
        ''' Returns the next JSON token in the stream. An EndDocument token is returned to indicate the end of the stream,
        ''' after which point <c>Next()</c> should not be called again.
        ''' </summary>
        ''' <remarks>This implementation provides single-token buffering, and calls <see cref="NextImpl"/> if there is no buffered token.</remarks>
        ''' <returns>The next token in the stream. This is never null.</returns>
        ''' <exception cref="InvalidOperationException">This method is called after an EndDocument token has been returned</exception>
        ''' <exception cref="InvalidJsonException">The input text does not comply with RFC 7159</exception>
        Friend Function [Next]() As JsonToken
            Dim tokenToReturn As JsonToken

            If bufferedToken IsNot Nothing Then
                tokenToReturn = bufferedToken
                bufferedToken = Nothing
            Else
                tokenToReturn = NextImpl()
            End If

            If tokenToReturn.Type = JsonToken.TokenType.StartObject Then
                ObjectDepth += 1
            ElseIf tokenToReturn.Type = JsonToken.TokenType.EndObject Then
                ObjectDepth -= 1
            End If

            Return tokenToReturn
        End Function

        ''' <summary>
        ''' Returns the next JSON token in the stream, when requested by the base class. (The <see cref="Next"/> method delegates
        ''' to this if it doesn't have a buffered token.)
        ''' </summary>
        ''' <exception cref="InvalidOperationException">This method is called after an EndDocument token has been returned</exception>
        ''' <exception cref="InvalidJsonException">The input text does not comply with RFC 7159</exception>
        Protected MustOverride Function NextImpl() As JsonToken

        ''' <summary>
        ''' Tokenizer which first exhausts a list of tokens, then consults another tokenizer.
        ''' </summary>
        Private Class JsonReplayTokenizer
            Inherits JsonTokenizer

            Private ReadOnly tokens As IList(Of JsonToken)
            Private ReadOnly nextTokenizer As JsonTokenizer
            Private nextTokenIndex As Integer

            Friend Sub New(tokens As IList(Of JsonToken), nextTokenizer As JsonTokenizer)
                Me.tokens = tokens
                Me.nextTokenizer = nextTokenizer
            End Sub

            ' FIXME: Object depth not maintained...
            Protected Overrides Function NextImpl() As JsonToken
                If nextTokenIndex >= tokens.Count Then
                    Return nextTokenizer.Next()
                End If

                Return tokens(Math.Min(Threading.Interlocked.Increment(nextTokenIndex), nextTokenIndex - 1))
            End Function
        End Class

        ''' <summary>
        ''' Tokenizer which does all the *real* work of parsing JSON.
        ''' </summary>
        Private NotInheritable Class JsonTextTokenizer
            Inherits JsonTokenizer
            ' The set of states in which a value is valid next token.
            Private Shared ReadOnly ValueStates As StateType = StateType.ArrayStart Or StateType.ArrayAfterComma Or StateType.ObjectAfterColon Or StateType.StartOfDocument
            Private ReadOnly containerStack As Stack(Of ContainerType) = New Stack(Of ContainerType)()
            Private ReadOnly reader As PushBackReader
            Private state As StateType

            Friend Sub New(reader As TextReader)
                Me.reader = New PushBackReader(reader)
                state = StateType.StartOfDocument
                containerStack.Push(ContainerType.Document)
            End Sub

            ''' <remarks>
            ''' This method essentially just loops through characters skipping whitespace, validating and
            ''' changing state (e.g. from ObjectBeforeColon to ObjectAfterColon)
            ''' until it reaches something which will be a genuine token (e.g. a start object, or a value) at which point
            ''' it returns the token. Although the method is large, it would be relatively hard to break down further... most
            ''' of it is the large switch statement, which sometimes returns and sometimes doesn't.
            ''' </remarks>
            Protected Overrides Function NextImpl() As JsonToken
                If state = StateType.ReaderExhausted Then
                    Throw New InvalidOperationException("Next() called after end of document")
                End If

                While True
                    Dim [next] = reader.Read()

                    If [next] Is Nothing Then
                        ValidateState(StateType.ExpectedEndOfDocument, "Unexpected end of document in state: ")
                        state = StateType.ReaderExhausted
                        Return JsonToken.EndDocument
                    End If

                    Select Case [next].Value
                        ' Skip whitespace between tokens
                        Case " "c, Microsoft.VisualBasic.Strings.ChrW(9), Microsoft.VisualBasic.Strings.ChrW(13), Microsoft.VisualBasic.Strings.ChrW(10)
                        Case ":"c
                            ValidateState(StateType.ObjectBeforeColon, "Invalid state to read a colon: ")
                            state = StateType.ObjectAfterColon
                        Case ","c
                            ValidateState(StateType.ObjectAfterProperty Or StateType.ArrayAfterValue, "Invalid state to read a colon: ")
                            state = If(state = StateType.ObjectAfterProperty, StateType.ObjectAfterComma, StateType.ArrayAfterComma)
                        Case """"c
                            Dim stringValue As String = ReadString()

                            If (state And (StateType.ObjectStart Or StateType.ObjectAfterComma)) <> 0 Then
                                state = StateType.ObjectBeforeColon
                                Return JsonToken.Name(stringValue)
                            Else
                                ValidateAndModifyStateForValue("Invalid state to read a double quote: ")
                                Return JsonToken.Value(stringValue)
                            End If

                        Case "{"c
                            ValidateState(ValueStates, "Invalid state to read an open brace: ")
                            state = StateType.ObjectStart
                            containerStack.Push(ContainerType.Object)
                            Return JsonToken.StartObject
                        Case "}"c
                            ValidateState(StateType.ObjectAfterProperty Or StateType.ObjectStart, "Invalid state to read a close brace: ")
                            PopContainer()
                            Return JsonToken.EndObject
                        Case "["c
                            ValidateState(ValueStates, "Invalid state to read an open square bracket: ")
                            state = StateType.ArrayStart
                            containerStack.Push(ContainerType.Array)
                            Return JsonToken.StartArray
                        Case "]"c
                            ValidateState(StateType.ArrayAfterValue Or StateType.ArrayStart, "Invalid state to read a close square bracket: ")
                            PopContainer()
                            Return JsonToken.EndArray
                        Case "n"c ' Start of null
                            ConsumeLiteral("null")
                            ValidateAndModifyStateForValue("Invalid state to read a null literal: ")
                            Return JsonToken.Null
                        Case "t"c ' Start of true
                            ConsumeLiteral("true")
                            ValidateAndModifyStateForValue("Invalid state to read a true literal: ")
                            Return JsonToken.True
                        Case "f"c ' Start of false
                            ConsumeLiteral("false")
                            ValidateAndModifyStateForValue("Invalid state to read a false literal: ")
                            Return JsonToken.False
                        Case "-"c, "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c ' Start of a number
                            Dim number = ReadNumber([next].Value)
                            ValidateAndModifyStateForValue("Invalid state to read a number token: ")
                            Return JsonToken.Value(number)
                        Case Else
                            Throw New InvalidJsonException("Invalid first character of token: " & [next].Value)
                    End Select
                End While
            End Function

            Private Sub ValidateState(validStates As StateType, errorPrefix As String)
                If (validStates And state) = 0 Then
                    Throw reader.CreateException(errorPrefix & state)
                End If
            End Sub

            ''' <summary>
            ''' Reads a string token. It is assumed that the opening " has already been read.
            ''' </summary>
            Private Function ReadString() As String
                Dim value = New StringBuilder()
                Dim haveHighSurrogate = False

                While True
                    Dim c = reader.ReadOrFail("Unexpected end of text while reading string")

                    If c < " "c Then
                        Throw reader.CreateException(String.Format(CultureInfo.InvariantCulture, "Invalid character in string literal: U+{0:x4}", Microsoft.VisualBasic.AscW(c)))
                    End If

                    If c = """"c Then
                        If haveHighSurrogate Then
                            Throw reader.CreateException("Invalid use of surrogate pair code units")
                        End If

                        Return value.ToString()
                    End If

                    If c = "\"c Then
                        c = ReadEscapedCharacter()
                    End If
                    ' TODO: Consider only allowing surrogate pairs that are either both escaped,
                    ' or both not escaped. It would be a very odd text stream that contained a "lone" high surrogate
                    ' followed by an escaped low surrogate or vice versa... and that couldn't even be represented in UTF-8.
                    If haveHighSurrogate <> Char.IsLowSurrogate(c) Then
                        Throw reader.CreateException("Invalid use of surrogate pair code units")
                    End If

                    haveHighSurrogate = Char.IsHighSurrogate(c)
                    value.Append(c)
                End While
            End Function

            ''' <summary>
            ''' Reads an escaped character. It is assumed that the leading backslash has already been read.
            ''' </summary>
            Private Function ReadEscapedCharacter() As Char
                Dim c = reader.ReadOrFail("Unexpected end of text while reading character escape sequence")

                Select Case c
                    Case "n"c
                        Return Microsoft.VisualBasic.Strings.ChrW(10)
                    Case "\"c
                        Return "\"c
                    Case "b"c
                        Return Microsoft.VisualBasic.Strings.ChrW(8)
                    Case "f"c
                        Return Microsoft.VisualBasic.Strings.ChrW(12)
                    Case "r"c
                        Return Microsoft.VisualBasic.Strings.ChrW(13)
                    Case "t"c
                        Return Microsoft.VisualBasic.Strings.ChrW(9)
                    Case """"c
                        Return """"c
                    Case "/"c
                        Return "/"c
                    Case "u"c
                        Return ReadUnicodeEscape()
                    Case Else
                        Throw reader.CreateException(String.Format(CultureInfo.InvariantCulture, "Invalid character in character escape sequence: U+{0:x4}", Microsoft.VisualBasic.AscW(c)))
                End Select
            End Function

            ''' <summary>
            ''' Reads an escaped Unicode 4-nybble hex sequence. It is assumed that the leading \u has already been read.
            ''' </summary>
            Private Function ReadUnicodeEscape() As Char
                Dim result = 0

                For i = 0 To 4 - 1
                    Dim c As chr = reader.ReadOrFail("Unexpected end of text while reading Unicode escape sequence")
                    Dim nybble As Integer

                    If c >= "0"c AndAlso c <= "9"c Then
                        nybble = c - "0"c
                    ElseIf c >= "a"c AndAlso c <= "f"c Then
                        nybble = c - "a"c + 10
                    ElseIf c >= "A"c AndAlso c <= "F"c Then
                        nybble = c - "A"c + 10
                    Else
                        Throw reader.CreateException(String.Format(CultureInfo.InvariantCulture, "Invalid character in character escape sequence: U+{0:x4}", CInt(c)))
                    End If

                    result = (result << 4) + nybble
                Next

                Return Microsoft.VisualBasic.ChrW(result)
            End Function

            ''' <summary>
            ''' Consumes a text-only literal, throwing an exception if the read text doesn't match it.
            ''' It is assumed that the first letter of the literal has already been read.
            ''' </summary>
            Private Sub ConsumeLiteral(text As String)
                For i = 1 To text.Length - 1
                    Dim [next] As Char? = reader.Read()

                    If [next] Is Nothing Then
                        Throw reader.CreateException("Unexpected end of text while reading literal token " & text)
                    End If

                    If [next].Value <> text(i) Then
                        Throw reader.CreateException("Unexpected character while reading literal token " & text)
                    End If
                Next
            End Sub

            Private Function ReadNumber(initialCharacter As Char) As Double
                Dim builder As StringBuilder = New StringBuilder()

                If initialCharacter = "-"c Then
                    builder.Append("-")
                Else
                    reader.PushBack(initialCharacter)
                End If
                ' Each method returns the character it read that doesn't belong in that part,
                ' so we know what to do next, including pushing the character back at the end.
                ' null is returned for "end of text".
                Dim [next] = ReadInt(builder)

                If [next] = "."c Then
                    [next] = ReadFrac(builder)
                End If

                If [next] = "e"c OrElse [next] = "E"c Then
                    [next] = ReadExp(builder)
                End If
                ' If we read a character which wasn't part of the number, push it back so we can read it again
                ' to parse the next token.
                If [next] IsNot Nothing Then
                    reader.PushBack([next].Value)
                End If

                ' TODO: What exception should we throw if the value can't be represented as a double?
                Try
                    Return Double.Parse(builder.ToString(), NumberStyles.AllowLeadingSign Or NumberStyles.AllowDecimalPoint Or NumberStyles.AllowExponent, CultureInfo.InvariantCulture)
                Catch __unusedOverflowException1__ As OverflowException
                    Throw reader.CreateException("Numeric value out of range: " & builder.ToString)
                End Try
            End Function

            Private Function ReadInt(builder As StringBuilder) As Char?
                Dim first = reader.ReadOrFail("Invalid numeric literal")

                If first < "0"c OrElse first > "9"c Then
                    Throw reader.CreateException("Invalid numeric literal")
                End If

                builder.Append(first)
                Dim digitCount As Integer
                Dim [next] = ConsumeDigits(builder, digitCount)

                If first = "0"c AndAlso digitCount <> 0 Then
                    Throw reader.CreateException("Invalid numeric literal: leading 0 for non-zero value.")
                End If

                Return [next]
            End Function

            Private Function ReadFrac(builder As StringBuilder) As Char?
                builder.Append("."c) ' Already consumed this
                Dim digitCount As Integer
                Dim [next] = ConsumeDigits(builder, digitCount)

                If digitCount = 0 Then
                    Throw reader.CreateException("Invalid numeric literal: fraction with no trailing digits")
                End If

                Return [next]
            End Function

            Private Function ReadExp(builder As StringBuilder) As Char?
                builder.Append("E"c) ' Already consumed this (or 'e')
                Dim [next] As Char? = reader.Read()

                If [next] Is Nothing Then
                    Throw reader.CreateException("Invalid numeric literal: exponent with no trailing digits")
                End If

                If [next] = "-"c OrElse [next] = "+"c Then
                    builder.Append([next].Value)
                Else
                    reader.PushBack([next].Value)
                End If

                Dim digitCount As Integer
                [next] = ConsumeDigits(builder, digitCount)

                If digitCount = 0 Then
                    Throw reader.CreateException("Invalid numeric literal: exponent without value")
                End If

                Return [next]
            End Function

            Private Function ConsumeDigits(builder As StringBuilder, <Out> ByRef count As Integer) As Char?
                count = 0

                While True
                    Dim [next] As Char? = reader.Read()

                    If [next] Is Nothing OrElse [next].Value < "0"c OrElse [next].Value > "9"c Then
                        Return [next]
                    End If

                    count += 1
                    builder.Append([next].Value)
                End While
            End Function

            ''' <summary>
            ''' Validates that we're in a valid state to read a value (using the given error prefix if necessary)
            ''' and changes the state to the appropriate one, e.g. ObjectAfterColon to ObjectAfterProperty.
            ''' </summary>
            Private Sub ValidateAndModifyStateForValue(errorPrefix As String)
                ValidateState(ValueStates, errorPrefix)

                Select Case state
                    Case StateType.StartOfDocument
                        state = StateType.ExpectedEndOfDocument
                        Return
                    Case StateType.ObjectAfterColon
                        state = StateType.ObjectAfterProperty
                        Return
                    Case StateType.ArrayStart, StateType.ArrayAfterComma
                        state = StateType.ArrayAfterValue
                        Return
                    Case Else
                        Throw New InvalidOperationException("ValidateAndModifyStateForValue does not handle all value states (and should)")
                End Select
            End Sub

            ''' <summary>
            ''' Pops the top-most container, and sets the state to the appropriate one for the end of a value
            ''' in the parent container.
            ''' </summary>
            Private Sub PopContainer()
                containerStack.Pop()
                Dim parent = containerStack.Peek()

                Select Case parent
                    Case ContainerType.Object
                        state = StateType.ObjectAfterProperty
                    Case ContainerType.Array
                        state = StateType.ArrayAfterValue
                    Case ContainerType.Document
                        state = StateType.ExpectedEndOfDocument
                    Case Else
                        Throw New InvalidOperationException("Unexpected container type: " & parent)
                End Select
            End Sub

            Private Enum ContainerType
                Document
                [Object]
                Array
            End Enum

            ''' <summary>
            ''' Possible states of the tokenizer.
            ''' </summary>
            ''' <remarks>
            ''' <para>This is a flags enum purely so we can simply and efficiently represent a set of valid states
            ''' for checking.</para>
            ''' <para>
            ''' Each is documented with an example,
            ''' where ^ represents the current position within the text stream. The examples all use string values,
            ''' but could be any value, including nested objects/arrays.
            ''' The complete state of the tokenizer also includes a stack to indicate the contexts (arrays/objects).
            ''' Any additional notional state of "AfterValue" indicates that a value has been completed, at which 
            ''' point there's an immediate transition to ExpectedEndOfDocument,  ObjectAfterProperty or ArrayAfterValue.
            ''' </para>
            ''' <para>
            ''' These states were derived manually by reading RFC 7159 carefully.
            ''' </para>
            ''' </remarks>
            <Flags>
            Private Enum StateType
                ''' <summary>
                ''' ^ { "foo": "bar" }
                ''' Before the value in a document. Next states: ObjectStart, ArrayStart, "AfterValue"
                ''' </summary>
                StartOfDocument = 1 << 0
                ''' <summary>
                ''' { "foo": "bar" } ^
                ''' After the value in a document. Next states: ReaderExhausted
                ''' </summary>
                ExpectedEndOfDocument = 1 << 1
                ''' <summary>
                ''' { "foo": "bar" } ^ (and already read to the end of the reader)
                ''' Terminal state.
                ''' </summary>
                ReaderExhausted = 1 << 2
                ''' <summary>
                ''' { ^ "foo": "bar" }
                ''' Before the *first* property in an object.
                ''' Next states:
                ''' "AfterValue" (empty object)
                ''' ObjectBeforeColon (read a name)
                ''' </summary>
                ObjectStart = 1 << 3
                ''' <summary>
                ''' { "foo" ^ : "bar", "x": "y" }
                ''' Next state: ObjectAfterColon
                ''' </summary>
                ObjectBeforeColon = 1 << 4
                ''' <summary>
                ''' { "foo" : ^ "bar", "x": "y" }
                ''' Before any property other than the first in an object.
                ''' (Equivalently: after any property in an object) 
                ''' Next states:
                ''' "AfterValue" (value is simple)
                ''' ObjectStart (value is object)
                ''' ArrayStart (value is array)
                ''' </summary>
                ObjectAfterColon = 1 << 5
                ''' <summary>
                ''' { "foo" : "bar" ^ , "x" : "y" }
                ''' At the end of a property, so expecting either a comma or end-of-object
                ''' Next states: ObjectAfterComma or "AfterValue"
                ''' </summary>
                ObjectAfterProperty = 1 << 6
                ''' <summary>
                ''' { "foo":"bar", ^ "x":"y" }
                ''' Read the comma after the previous property, so expecting another property.
                ''' This is like ObjectStart, but closing brace isn't valid here
                ''' Next state: ObjectBeforeColon.
                ''' </summary>
                ObjectAfterComma = 1 << 7
                ''' <summary>
                ''' [ ^ "foo", "bar" ]
                ''' Before the *first* value in an array.
                ''' Next states:
                ''' "AfterValue" (read a value)
                ''' "AfterValue" (end of array; will pop stack)
                ''' </summary>
                ArrayStart = 1 << 8
                ''' <summary>
                ''' [ "foo" ^ , "bar" ]
                ''' After any value in an array, so expecting either a comma or end-of-array
                ''' Next states: ArrayAfterComma or "AfterValue"
                ''' </summary>
                ArrayAfterValue = 1 << 9
                ''' <summary>
                ''' [ "foo", ^ "bar" ]
                ''' After a comma in an array, so there *must* be another value (simple or complex).
                ''' Next states: "AfterValue" (simple value), StartObject, StartArray
                ''' </summary>
                ArrayAfterComma = 1 << 10
            End Enum

            ''' <summary>
            ''' Wrapper around a text reader allowing small amounts of buffering and location handling.
            ''' </summary>
            Private Class PushBackReader
                ' TODO: Add locations for errors etc.

                Private ReadOnly reader As TextReader

                Friend Sub New(reader As TextReader)
                    ' TODO: Wrap the reader in a BufferedReader?
                    Me.reader = reader
                End Sub

                ''' <summary>
                ''' The buffered next character, if we have one.
                ''' </summary>
                Private nextChar As Char?

                ''' <summary>
                ''' Returns the next character in the stream, or null if we have reached the end.
                ''' </summary>
                ''' <returns></returns>
                Friend Function Read() As Char?
                    If nextChar IsNot Nothing Then
                        Dim tmp = nextChar
                        nextChar = Nothing
                        Return tmp
                    End If

                    Dim [next] As Integer = reader.Read()
                    Return If([next] = -1, Nothing, ChrW([next]))
                End Function

                Friend Function ReadOrFail(messageOnFailure As String) As Char
                    Dim [next] As Char? = Read()

                    If [next] Is Nothing Then
                        Throw CreateException(messageOnFailure)
                    End If

                    Return [next].Value
                End Function

                Friend Sub PushBack(c As Char)
                    If nextChar IsNot Nothing Then
                        Throw New InvalidOperationException("Cannot push back when already buffering a character")
                    End If

                    nextChar = c
                End Sub

                ''' <summary>
                ''' Creates a new exception appropriate for the current state of the reader.
                ''' </summary>
                Friend Function CreateException(message As String) As InvalidJsonException
                    ' TODO: Keep track of and use the location.
                    Return New InvalidJsonException(message)
                End Function
            End Class
        End Class
    End Class
End Namespace
