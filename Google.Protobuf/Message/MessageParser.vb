#Region "Microsoft.VisualBasic::2c6c61e1d3fa8fbcccaa7b3910b5e4be, Google.Protobuf\Message\MessageParser.vb"

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

    '     Class MessageParser
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateTemplate, ParseDelimitedFrom, (+4 Overloads) ParseFrom, ParseJson
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

Namespace Google.Protobuf
    ''' <summary>
    ''' A general message parser, typically used by reflection-based code as all the methods
    ''' return simple <see cref="IMessage"/>.
    ''' </summary>
    Public Class MessageParser

        Private factory As Func(Of IMessage)

        Sub New(factory As Func(Of IMessage))
            Me.factory = factory
        End Sub

        ''' <summary>
        ''' Creates a template instance ready for population.
        ''' </summary>
        ''' <returns>An empty message.</returns>
        Friend Function CreateTemplate() As IMessage
            Return factory()
        End Function

        ''' <summary>
        ''' Parses a message from a byte array.
        ''' </summary>
        ''' <param name="data">The byte array containing the message. Must not be null.</param>
        ''' <returns>The newly parsed message.</returns>
        Public Function ParseFrom(data As Byte()) As IMessage
            CheckNotNull(data, "data")
            Dim message As IMessage = factory()
            message.MergeFrom(data)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given byte string.
        ''' </summary>
        ''' <param name="data">The data to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Function ParseFrom(data As ByteString) As IMessage
            CheckNotNull(data, "data")
            Dim message As IMessage = factory()
            message.MergeFrom(data)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given stream.
        ''' </summary>
        ''' <param name="input">The stream to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Function ParseFrom(input As Stream) As IMessage
            Dim message As IMessage = factory()
            message.MergeFrom(input)
            Return message
        End Function

        ''' <summary>
        ''' Parses a length-delimited message from the given stream.
        ''' </summary>
        ''' <remarks>
        ''' The stream is expected to contain a length and then the data. Only the amount of data
        ''' specified by the length will be consumed.
        ''' </remarks>
        ''' <param name="input">The stream to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Function ParseDelimitedFrom(input As Stream) As IMessage
            Dim message As IMessage = factory()
            message.MergeDelimitedFrom(input)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given coded input stream.
        ''' </summary>
        ''' <param name="input">The stream to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Function ParseFrom(input As CodedInputStream) As IMessage
            Dim message As IMessage = factory()
            message.MergeFrom(input)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given JSON.
        ''' </summary>
        ''' <param name="json">The JSON to parse.</param>
        ''' <returns>The parsed message.</returns>
        ''' <exception cref="InvalidJsonException">The JSON does not comply with RFC 7159</exception>
        ''' <exception cref="InvalidProtocolBufferException">The JSON does not represent a Protocol Buffers message correctly</exception>
        Public Function ParseJson(json As String) As IMessage
            Dim message As IMessage = factory()
            JsonParser.Default.Merge(message, json)
            Return message
        End Function
    End Class
End Namespace

