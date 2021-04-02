#Region "Microsoft.VisualBasic::0fc20b762aebbc8a889dc7f06695d2b8, Google.Protobuf\WireFormat.vb"

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

    '     Module WireFormat
    ' 
    '         Function: GetTagFieldNumber, GetTagWireType, MakeTag
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

Namespace Google.Protobuf

    ''' <summary>
    ''' This class is used internally by the Protocol Buffer Library and generated
    ''' message implementations. It is public only for the sake of those generated
    ''' messages. Others should not use this class directly.
    ''' <para>
    ''' This class contains constants and helper functions useful for dealing with
    ''' the Protocol Buffer wire format.
    ''' </para>
    ''' </summary>
    Public Module WireFormat

        Private Const TagTypeBits As Integer = 3
        Private Const TagTypeMask As UInteger = (1 << TagTypeBits) - 1

        ''' <summary>
        ''' Given a tag value, determines the wire type (lower 3 bits).
        ''' </summary>
        Public Function GetTagWireType(tag As UInteger) As WireType
            Return tag And TagTypeMask
        End Function

        ''' <summary>
        ''' Given a tag value, determines the field number (the upper 29 bits).
        ''' </summary>
        Public Function GetTagFieldNumber(tag As UInteger) As Integer
            Return CInt(tag) >> TagTypeBits
        End Function

        ''' <summary>
        ''' Makes a tag value given a field number and wire type.
        ''' </summary>
        Public Function MakeTag(fieldNumber As Integer, wireType As WireType) As UInteger
            Return CUInt(fieldNumber << TagTypeBits) Or wireType
        End Function
    End Module
End Namespace
