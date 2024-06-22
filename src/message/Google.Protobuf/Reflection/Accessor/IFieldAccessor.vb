#Region "Microsoft.VisualBasic::a99b19a8f5203c9207c369130b52e8e6, src\message\Google.Protobuf\Reflection\Accessor\IFieldAccessor.vb"

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

    '   Total Lines: 67
    '    Code Lines: 10 (14.93%)
    ' Comment Lines: 52 (77.61%)
    '    - Xml Docs: 44.23%
    ' 
    '   Blank Lines: 5 (7.46%)
    '     File Size: 3.11 KB


    '     Interface IFieldAccessor
    ' 
    '         Properties: Descriptor
    ' 
    '         Function: GetValue
    ' 
    '         Sub: Clear, SetValue
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
    ''' Allows fields to be reflectively accessed.
    ''' </summary>
    Public Interface IFieldAccessor
        ''' <summary>
        ''' Returns the descriptor associated with this field.
        ''' </summary>
        ReadOnly Property Descriptor As FieldDescriptor

        ''' <summary>
        ''' Clears the field in the specified message. (For repeated fields,
        ''' this clears the list.)
        ''' </summary>
        Sub Clear(message As IMessage)

        ''' <summary>
        ''' Fetches the field value. For repeated values, this will be an
        ''' <see cref="IList"/> implementation. For map values, this will be an
        ''' <see cref="IDictionary"/> implementation.
        ''' </summary>
        Function GetValue(message As IMessage) As Object

        ''' <summary>
        ''' Mutator for single "simple" fields only.
        ''' </summary>
        ''' <remarks>
        ''' Repeated fields are mutated by fetching the value and manipulating it as a list.
        ''' Map fields are mutated by fetching the value and manipulating it as a dictionary.
        ''' </remarks>
        ''' <exception cref="InvalidOperationException">The field is not a "simple" field.</exception>
        Sub SetValue(message As IMessage, value As Object)
    End Interface
End Namespace
