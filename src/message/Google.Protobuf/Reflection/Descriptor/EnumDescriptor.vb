#Region "Microsoft.VisualBasic::ef2a3071c5865793b092a92f5b8e6178, src\message\Google.Protobuf\Reflection\Descriptor\EnumDescriptor.vb"

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

    '   Total Lines: 124
    '    Code Lines: 55 (44.35%)
    ' Comment Lines: 56 (45.16%)
    '    - Xml Docs: 44.64%
    ' 
    '   Blank Lines: 13 (10.48%)
    '     File Size: 5.37 KB


    '     Class EnumDescriptor
    ' 
    '         Properties: ClrType, ContainingType, Name, Proto, Values
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: FindValueByName, FindValueByNumber
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
Imports System.Collections.Generic

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Descriptor for an enum type in a .proto file.
    ''' </summary>
    Public NotInheritable Class EnumDescriptor
        Inherits DescriptorBase

        Private ReadOnly protoField As EnumDescriptorProto
        Private ReadOnly containingTypeField As MessageDescriptor
        Private ReadOnly valuesField As IList(Of EnumValueDescriptor)
        Private ReadOnly clrTypeField As Type

        Friend Sub New(proto As EnumDescriptorProto, file As FileDescriptor, parent As MessageDescriptor, index As Integer, clrType As Type)
            MyBase.New(file, file.ComputeFullName(parent, proto.Name), index)
            protoField = proto
            clrTypeField = clrType
            containingTypeField = parent

            If proto.Value.Count = 0 Then
                ' We cannot allow enums with no values because this would mean there
                ' would be no valid default value for fields of this type.
                Throw New DescriptorValidationException(Me, "Enums must contain at least one value.")
            End If

            valuesField = DescriptorUtil.ConvertAndMakeReadOnly(proto.Value, Function(value, i) New EnumValueDescriptor(value, file, Me, i))
            MyBase.File.DescriptorPool.AddSymbol(Me)
        End Sub

        Friend ReadOnly Property Proto As EnumDescriptorProto
            Get
                Return protoField
            End Get
        End Property

        ''' <summary>
        ''' The brief name of the descriptor's target.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return protoField.Name
            End Get
        End Property

        ''' <summary>
        ''' The CLR type for this enum. For generated code, this will be a CLR enum type.
        ''' </summary>
        Public ReadOnly Property ClrType As Type
            Get
                Return clrTypeField
            End Get
        End Property

        ''' <value>
        ''' If this is a nested type, get the outer descriptor, otherwise null.
        ''' </value>
        Public ReadOnly Property ContainingType As MessageDescriptor
            Get
                Return containingTypeField
            End Get
        End Property

        ''' <value>
        ''' An unmodifiable list of defined value descriptors for this enum.
        ''' </value>
        Public ReadOnly Property Values As IList(Of EnumValueDescriptor)
            Get
                Return valuesField
            End Get
        End Property

        ''' <summary>
        ''' Finds an enum value by number. If multiple enum values have the
        ''' same number, this returns the first defined value with that number.
        ''' If there is no value for the given number, this returns <c>null</c>.
        ''' </summary>
        Public Function FindValueByNumber(number As Integer) As EnumValueDescriptor
            Return File.DescriptorPool.FindEnumValueByNumber(Me, number)
        End Function

        ''' <summary>
        ''' Finds an enum value by name.
        ''' </summary>
        ''' <param name="name">The unqualified name of the value (e.g. "FOO").</param>
        ''' <returns>The value's descriptor, or null if not found.</returns>
        Public Function FindValueByName(name As String) As EnumValueDescriptor
            Return File.DescriptorPool.FindSymbol(Of EnumValueDescriptor)(FullName & "." & name)
        End Function
    End Class
End Namespace
