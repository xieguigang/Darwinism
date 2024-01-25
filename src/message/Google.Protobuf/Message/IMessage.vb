#Region "Microsoft.VisualBasic::081296e26d2715d9b245cd418544721f, Google.Protobuf\Message\IMessage.vb"

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

    '     Interface IMessage
    ' 
    '         Properties: Descriptor
    ' 
    '         Function: CalculateSize
    ' 
    '         Sub: MergeFrom, WriteTo
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

Imports System
Imports Google.Protobuf.Reflection

Namespace Google.Protobuf
    ''' <summary>
    ''' Interface for a Protocol Buffers message, supporting
    ''' basic operations required for serialization.
    ''' </summary>
    Public Interface IMessage
        ''' <summary>
        ''' Merges the data from the specified coded input stream with the current message.
        ''' </summary>
        ''' <remarks>See the user guide for precise merge semantics.</remarks>
        ''' <param name="input"></param>
        Sub MergeFrom(input As CodedInputStream)

        ''' <summary>
        ''' Writes the data to the given coded output stream.
        ''' </summary>
        ''' <param name="output">Coded output stream to write the data to. Must not be null.</param>
        Sub WriteTo(output As CodedOutputStream)

        ''' <summary>
        ''' Calculates the size of this message in Protocol Buffer wire format, in bytes.
        ''' </summary>
        ''' <returns>The number of bytes required to write this message
        ''' to a coded output stream.</returns>
        Function CalculateSize() As Integer

        ''' <summary>
        ''' Descriptor for this message. All instances are expected to return the same descriptor,
        ''' and for generated types this will be an explicitly-implemented member, returning the
        ''' same value as the static property declared on the type.
        ''' </summary>
        ReadOnly Property Descriptor As MessageDescriptor
    End Interface

End Namespace
