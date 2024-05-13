#Region "Microsoft.VisualBasic::2551141d9297debcc25cfcd1533f07d4, src\message\Google.Protobuf\InvalidProtocolBufferException.vb"

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

    '   Total Lines: 105
    '    Code Lines: 54
    ' Comment Lines: 36
    '   Blank Lines: 15
    '     File Size: 5.77 KB


    '     Class InvalidProtocolBufferException
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: InvalidBase64, InvalidEndTag, InvalidMessageStreamTag, InvalidTag, JsonRecursionLimitExceeded
    '                   MalformedVarint, MoreDataAvailable, NegativeSize, RecursionLimitExceeded, SizeLimitExceeded
    '                   TruncatedMessage
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

Imports System.IO

Namespace Google.Protobuf
    ''' <summary>
    ''' Thrown when a protocol message being parsed is invalid in some way,
    ''' e.g. it contains a malformed varint or a negative byte length.
    ''' </summary>
    Public NotInheritable Class InvalidProtocolBufferException
        Inherits IOException

        Friend Sub New(message As String)
            MyBase.New(message)
        End Sub

        Friend Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Friend Shared Function MoreDataAvailable() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Completed reading a message while more data was available in the stream.")
        End Function

        Friend Shared Function TruncatedMessage() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("While parsing a protocol message, the input ended unexpectedly " &
                                                      "in the middle of a field.  This could mean either than the " &
                                                      "input has been truncated or that an embedded message " &
                                                      "misreported its own length.")
        End Function

        Friend Shared Function NegativeSize() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("CodedInputStream encountered an embedded string or message " &
                                                      "which claimed to have negative size.")
        End Function

        Friend Shared Function MalformedVarint() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("CodedInputStream encountered a malformed varint.")
        End Function

        ''' <summary>
        ''' Creates an exception for an error condition of an invalid tag being encountered.
        ''' </summary>
        Friend Shared Function InvalidTag() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Protocol message contained an invalid tag (zero).")
        End Function

        Friend Shared Function InvalidBase64(innerException As Exception) As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Invalid base64 data", innerException)
        End Function

        Friend Shared Function InvalidEndTag() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Protocol message end-group tag did not match expected tag.")
        End Function

        Friend Shared Function RecursionLimitExceeded() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Protocol message had too many levels of nesting.  May be malicious.  " &
                                                      "Use CodedInputStream.SetRecursionLimit() to increase the depth limit.")
        End Function

        Friend Shared Function JsonRecursionLimitExceeded() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Protocol message had too many levels of nesting.  May be malicious.  " &
                                                      "Use JsonParser.Settings to increase the depth limit.")
        End Function

        Friend Shared Function SizeLimitExceeded() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Protocol message was too large.  May be malicious.  " &
                                                      "Use CodedInputStream.SetSizeLimit() to increase the size limit.")
        End Function

        Friend Shared Function InvalidMessageStreamTag() As InvalidProtocolBufferException
            Return New InvalidProtocolBufferException("Stream of protocol messages had invalid tag. Expected tag is length-delimited field 1.")
        End Function
    End Class
End Namespace
