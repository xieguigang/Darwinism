#Region "Microsoft.VisualBasic::18a0b3f772e9f502d5a29090d69dab03, src\message\Google.Protobuf\Message\MessageExtensions.vb"

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

    '   Total Lines: 156
    '    Code Lines: 71 (45.51%)
    ' Comment Lines: 76 (48.72%)
    '    - Xml Docs: 61.84%
    ' 
    '   Blank Lines: 9 (5.77%)
    '     File Size: 7.45 KB


    '     Module MessageExtensions
    ' 
    '         Function: ToByteArray, ToByteString
    ' 
    '         Sub: MergeDelimitedFrom, (+3 Overloads) MergeFrom, WriteDelimitedTo, WriteTo
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

Imports System.IO
Imports System.Runtime.CompilerServices

Namespace Google.Protobuf
    ''' <summary>
    ''' Extension methods on <see cref="IMessage"/> and <see cref="IMessageType(OfT)"/>.
    ''' </summary>
    Public Module MessageExtensions
        ''' <summary>
        ''' Merges data from the given byte array into an existing message.
        ''' </summary>
        ''' <param name="message">The message to merge the data into.</param>
        ''' <param name="data">The data to merge, which must be protobuf-encoded binary data.</param>
        <Extension()>
        Public Sub MergeFrom(message As IMessage, data As Byte())
            CheckNotNull(message, "message")
            CheckNotNull(data, "data")
            Dim input As CodedInputStream = New CodedInputStream(data)
            message.MergeFrom(input)
            input.CheckReadEndOfStreamTag()
        End Sub

        ''' <summary>
        ''' Merges data from the given byte string into an existing message.
        ''' </summary>
        ''' <param name="message">The message to merge the data into.</param>
        ''' <param name="data">The data to merge, which must be protobuf-encoded binary data.</param>
        <Extension()>
        Public Sub MergeFrom(message As IMessage, data As ByteString)
            CheckNotNull(message, "message")
            CheckNotNull(data, "data")
            Dim input As CodedInputStream = data.CreateCodedInput()
            message.MergeFrom(input)
            input.CheckReadEndOfStreamTag()
        End Sub

        ''' <summary>
        ''' Merges data from the given stream into an existing message.
        ''' </summary>
        ''' <param name="message">The message to merge the data into.</param>
        ''' <param name="input">Stream containing the data to merge, which must be protobuf-encoded binary data.</param>
        <Extension()>
        Public Sub MergeFrom(message As IMessage, input As Stream)
            CheckNotNull(message, "message")
            CheckNotNull(input, "input")
            Dim codedInput As CodedInputStream = New CodedInputStream(input)
            message.MergeFrom(codedInput)
            codedInput.CheckReadEndOfStreamTag()
        End Sub

        ''' <summary>
        ''' Merges length-delimited data from the given stream into an existing message.
        ''' </summary>
        ''' <remarks>
        ''' The stream is expected to contain a length and then the data. Only the amount of data
        ''' specified by the length will be consumed.
        ''' </remarks>
        ''' <param name="message">The message to merge the data into.</param>
        ''' <param name="input">Stream containing the data to merge, which must be protobuf-encoded binary data.</param>
        <Extension()>
        Public Sub MergeDelimitedFrom(message As IMessage, input As Stream)
            CheckNotNull(message, "message")
            CheckNotNull(input, "input")
            Dim size As Integer = CodedInputStream.ReadRawVarint32(input)
            Dim limitedStream As Stream = New LimitedInputStream(input, size)
            message.MergeFrom(limitedStream)
        End Sub

        ''' <summary>
        ''' Converts the given message into a byte array in protobuf encoding.
        ''' </summary>
        ''' <param name="message">The message to convert.</param>
        ''' <returns>The message data as a byte array.</returns>
        <Extension()>
        Public Function ToByteArray(message As IMessage) As Byte()
            CheckNotNull(message, "message")
            Dim result As Byte() = New Byte(message.CalculateSize() - 1) {}
            Dim output As CodedOutputStream = New CodedOutputStream(result)
            message.WriteTo(output)
            output.CheckNoSpaceLeft()
            Return result
        End Function

        ''' <summary>
        ''' Writes the given message data to the given stream in protobuf encoding.
        ''' </summary>
        ''' <param name="message">The message to write to the stream.</param>
        ''' <param name="output">The stream to write to.</param>
        <Extension()>
        Public Sub WriteTo(message As IMessage, output As Stream)
            CheckNotNull(message, "message")
            CheckNotNull(output, "output")
            Dim codedOutput As CodedOutputStream = New CodedOutputStream(output)
            message.WriteTo(codedOutput)
            codedOutput.Flush()
        End Sub

        ''' <summary>
        ''' Writes the length and then data of the given message to a stream.
        ''' </summary>
        ''' <param name="message">The message to write.</param>
        ''' <param name="output">The output stream to write to.</param>
        <Extension()>
        Public Sub WriteDelimitedTo(message As IMessage, output As Stream)
            CheckNotNull(message, "message")
            CheckNotNull(output, "output")
            Dim codedOutput As CodedOutputStream = New CodedOutputStream(output)
            codedOutput.WriteRawVarint32(CUInt(message.CalculateSize()))
            message.WriteTo(codedOutput)
            codedOutput.Flush()
        End Sub

        ''' <summary>
        ''' Converts the given message into a byte string in protobuf encoding.
        ''' </summary>
        ''' <param name="message">The message to convert.</param>
        ''' <returns>The message data as a byte string.</returns>
        <Extension()>
        Public Function ToByteString(message As IMessage) As ByteString
            CheckNotNull(message, "message")
            Return ByteString.AttachBytes(message.ToByteArray())
        End Function
    End Module
End Namespace
