#Region "Microsoft.VisualBasic::bc80bc8d8a4419472abddc5d785c6c6a, src\message\Google.Protobuf\Reflection\FieldType.vb"

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

    '   Total Lines: 111
    '    Code Lines: 24
    ' Comment Lines: 86
    '   Blank Lines: 1
    '     File Size: 3.87 KB


    '     Enum FieldType
    ' 
    '         [Double], [Enum], [String], Bool, Bytes
    '         Fixed32, Fixed64, Float, Group, Int32
    '         Int64, Message, SFixed32, SFixed64, SInt32
    '         SInt64, UInt32, UInt64
    ' 
    '  
    ' 
    ' 
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

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Enumeration of all the possible field types.
    ''' </summary>
    Public Enum FieldType
        ''' <summary>
        ''' The <c>double</c> field type.
        ''' </summary>
        [Double]
        ''' <summary>
        ''' The <c>float</c> field type.
        ''' </summary>
        Float
        ''' <summary>
        ''' The <c>int64</c> field type.
        ''' </summary>
        Int64
        ''' <summary>
        ''' The <c>uint64</c> field type.
        ''' </summary>
        UInt64
        ''' <summary>
        ''' The <c>int32</c> field type.
        ''' </summary>
        Int32
        ''' <summary>
        ''' The <c>fixed64</c> field type.
        ''' </summary>
        Fixed64
        ''' <summary>
        ''' The <c>fixed32</c> field type.
        ''' </summary>
        Fixed32
        ''' <summary>
        ''' The <c>bool</c> field type.
        ''' </summary>
        Bool
        ''' <summary>
        ''' The <c>string</c> field type.
        ''' </summary>
        [String]
        ''' <summary>
        ''' The field type used for groups (not supported in this implementation).
        ''' </summary>
        Group
        ''' <summary>
        ''' The field type used for message fields.
        ''' </summary>
        Message
        ''' <summary>
        ''' The <c>bytes</c> field type.
        ''' </summary>
        Bytes
        ''' <summary>
        ''' The <c>uint32</c> field type.
        ''' </summary>
        UInt32
        ''' <summary>
        ''' The <c>sfixed32</c> field type.
        ''' </summary>
        SFixed32
        ''' <summary>
        ''' The <c>sfixed64</c> field type.
        ''' </summary>
        SFixed64
        ''' <summary>
        ''' The <c>sint32</c> field type.
        ''' </summary>
        SInt32
        ''' <summary>
        ''' The <c>sint64</c> field type.
        ''' </summary>
        SInt64
        ''' <summary>
        ''' The field type used for enum fields.
        ''' </summary>
        [Enum]
    End Enum
End Namespace
