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

Namespace Google.Protobuf.WellKnownTypes
    Public Partial Class Any
        Private Const DefaultPrefix As String = "type.googleapis.com"

        ' This could be moved to MessageDescriptor if we wanted to, but keeping it here means
        ' all the Any-specific code is in the same place.
        Private Shared Function GetTypeUrl(descriptor As MessageDescriptor, prefix As String) As String
            Return If(prefix.EndsWith("/"), prefix & descriptor.FullName, prefix & "/" & descriptor.FullName)
        End Function

        ''' <summary>
        ''' Retrieves the type name for a type URL. This is always just the last part of the URL,
        ''' after the trailing slash. No validation of anything before the trailing slash is performed.
        ''' If the type URL does not include a slash, an empty string is returned rather than an exception
        ''' being thrown; this won't match any types, and the calling code is probably in a better position
        ''' to give a meaningful error.
        ''' There is no handling of fragments or queries  at the moment.
        ''' </summary>
        ''' <param name="typeUrl">The URL to extract the type name from</param>
        ''' <returns>The type name</returns>
        Friend Shared Function GetTypeName(typeUrl As String) As String
            Dim lastSlash = typeUrl.LastIndexOf("/"c)
            Return If(lastSlash = -1, "", typeUrl.Substring(lastSlash + 1))
        End Function

        ''' <summary>
        ''' Unpacks the content of this Any message into the target message type,
        ''' which must match the type URL within this Any message.
        ''' </summary>
        ''' <typeparam name="T">The type of message to unpack the content into.</typeparam>
        ''' <returns>The unpacked message.</returns>
        ''' <exception cref="InvalidProtocolBufferException">The target message type doesn't match the type URL in this message</exception>
        Public Function Unpack(Of T As {IMessage, New})() As T
            ' Note: this doesn't perform as well is it might. We could take a MessageParser<T> in an alternative overload,
            ' which would be expected to perform slightly better... although the difference is likely to be negligible.
            Dim target As T = New T()

            If Not Equals(GetTypeName(TypeUrl), target.Descriptor.FullName) Then
                Throw New InvalidProtocolBufferException($"Full type name for {target.Descriptor.Name} is {target.Descriptor.FullName}; Any message's type url is {TypeUrl}")
            End If

            target.MergeFrom(Value)
            Return target
        End Function

        ''' <summary>
        ''' Packs the specified message into an Any message using a type URL prefix of "type.googleapis.com".
        ''' </summary>
        ''' <param name="message">The message to pack.</param>
        ''' <returns>An Any message with the content and type URL of <paramrefname="message"/>.</returns>
        Public Shared Function Pack(message As IMessage) As Any
            Return Pack(message, DefaultPrefix)
        End Function

        ''' <summary>
        ''' Packs the specified message into an Any message using the specified type URL prefix.
        ''' </summary>
        ''' <param name="message">The message to pack.</param>
        ''' <param name="typeUrlPrefix">The prefix for the type URL.</param>
        ''' <returns>An Any message with the content and type URL of <paramrefname="message"/>.</returns>
        Public Shared Function Pack(message As IMessage, typeUrlPrefix As String) As Any
            CheckNotNull(message, NameOf(message))
            CheckNotNull(typeUrlPrefix, NameOf(typeUrlPrefix))
            Return New Any With {
                .TypeUrl = GetTypeUrl(message.Descriptor, typeUrlPrefix),
                .Value = message.ToByteString()
            }
        End Function
    End Class
End Namespace
