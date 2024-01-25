#Region "Microsoft.VisualBasic::a19acb40729c5630c319408f12f80af6, Google.Protobuf\Message\MessageParserType.vb"

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

    '     Class MessageParserType
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateTemplate, ParseDelimitedFrom, (+4 Overloads) ParseFrom, ParseJson
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO

Namespace Google.Protobuf
    ''' <summary>
    ''' A parser for a specific message type.
    ''' </summary>
    ''' <remarks>
    ''' <p>
    ''' This delegates most behavior to the
    ''' <see cref="IMessage.MergeFrom"/> implementation within the original type, but
    ''' provides convenient overloads to parse from a variety of sources.
    ''' </p>
    ''' <p>
    ''' Most applications will never need to create their own instances of this type;
    ''' instead, use the static <c>Parser</c> property of a generated message type to obtain a
    ''' parser for that type.
    ''' </p>
    ''' </remarks>
    ''' <typeparam name="T">The type of message to be parsed.</typeparam>
    Public NotInheritable Class MessageParserType(Of T As IMessageType(Of T))
        Inherits MessageParser
        ' Implementation note: all the methods here *could* just delegate up to the base class and cast the result.
        ' The current implementation avoids a virtual method call and a cast, which *may* be significant in some cases.
        ' Benchmarking work is required to measure the significance - but it's only a few lines of code in any case.
        ' The API wouldn't change anyway - just the implementation - so this work can be deferred.
        Private ReadOnly factory As Func(Of T)

        ''' <summary>
        ''' Creates a new parser.
        ''' </summary>
        ''' <remarks>
        ''' The factory method is effectively an optimization over using a generic constraint
        ''' to require a parameterless constructor: delegates are significantly faster to execute.
        ''' </remarks>
        ''' <param name="factory">Function to invoke when a new, empty message is required.</param>
        Public Sub New(factory As Func(Of T))
            MyBase.New(Function() factory())
            Me.factory = factory
        End Sub

        ''' <summary>
        ''' Creates a template instance ready for population.
        ''' </summary>
        ''' <returns>An empty message.</returns>
        Friend Overloads Function CreateTemplate() As T
            Return factory()
        End Function

        ''' <summary>
        ''' Parses a message from a byte array.
        ''' </summary>
        ''' <param name="data">The byte array containing the message. Must not be null.</param>
        ''' <returns>The newly parsed message.</returns>
        Public Overloads Function ParseFrom(data As Byte()) As T
            CheckNotNull(data, "data")
            Dim message As T = factory()
            message.MergeFrom(data)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given byte string.
        ''' </summary>
        ''' <param name="data">The data to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Overloads Function ParseFrom(data As ByteString) As T
            CheckNotNull(data, "data")
            Dim message As T = factory()
            message.MergeFrom(data)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given stream.
        ''' </summary>
        ''' <param name="input">The stream to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Overloads Function ParseFrom(input As Stream) As T
            Dim message As T = factory()
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
        Public Overloads Function ParseDelimitedFrom(input As Stream) As T
            Dim message As T = factory()
            message.MergeDelimitedFrom(input)
            Return message
        End Function

        ''' <summary>
        ''' Parses a message from the given coded input stream.
        ''' </summary>
        ''' <param name="input">The stream to parse.</param>
        ''' <returns>The parsed message.</returns>
        Public Overloads Function ParseFrom(input As CodedInputStream) As T
            Dim message As T = factory()
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
        Public Overloads Function ParseJson(json As String) As T
            Dim message As T = factory()
            JsonParser.Default.Merge(message, json)
            Return message
        End Function
    End Class
End Namespace
