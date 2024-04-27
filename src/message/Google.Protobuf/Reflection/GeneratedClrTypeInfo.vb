#Region "Microsoft.VisualBasic::af7fa1d561408ac4a15ac765dd5c5bac, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//Reflection/GeneratedClrTypeInfo.vb"

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

    '   Total Lines: 108
    '    Code Lines: 34
    ' Comment Lines: 64
    '   Blank Lines: 10
    '     File Size: 5.29 KB


    '     Class GeneratedClrTypeInfo
    ' 
    '         Properties: ClrType, NestedEnums, NestedTypes, OneofNames, Parser
    '                     PropertyNames
    ' 
    '         Constructor: (+2 Overloads) Sub New
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
Imports System

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Extra information provided by generated code when initializing a message or file descriptor.
    ''' These are constructed as required, and are not long-lived. Hand-written code should
    ''' never need to use this type.
    ''' </summary>
    Public NotInheritable Class GeneratedClrTypeInfo

        ''' <summary>
        ''' Irrelevant for file descriptors; the CLR type for the message for message descriptors.
        ''' </summary>
        Private _ClrType As System.Type
        Private Shared ReadOnly EmptyNames As String() = New String(-1) {}
        Private Shared ReadOnly EmptyCodeInfo As GeneratedClrTypeInfo() = New GeneratedClrTypeInfo(-1) {}

        Public Property ClrType As Type
            Get
                Return _ClrType
            End Get
            Private Set(value As Type)
                _ClrType = value
            End Set
        End Property

        ''' <summary>
        ''' Irrelevant for file descriptors; the parser for message descriptors.
        ''' </summary>
        Public ReadOnly Property Parser As MessageParser

        ''' <summary>
        ''' Irrelevant for file descriptors; the CLR property names (in message descriptor field order)
        ''' for fields in the message for message descriptors.
        ''' </summary>
        Public ReadOnly Property PropertyNames As String()

        ''' <summary>
        ''' Irrelevant for file descriptors; the CLR property "base" names (in message descriptor oneof order)
        ''' for oneofs in the message for message descriptors. It is expected that for a oneof name of "Foo",
        ''' there will be a "FooCase" property and a "ClearFoo" method.
        ''' </summary>
        Public ReadOnly Property OneofNames As String()

        ''' <summary>
        ''' The reflection information for types within this file/message descriptor. Elements may be null
        ''' if there is no corresponding generated type, e.g. for map entry types.
        ''' </summary>
        Public ReadOnly Property NestedTypes As GeneratedClrTypeInfo()

        ''' <summary>
        ''' The CLR types for enums within this file/message descriptor.
        ''' </summary>
        Public ReadOnly Property NestedEnums As Type()

        ''' <summary>
        ''' Creates a GeneratedClrTypeInfo for a message descriptor, with nested types, nested enums, the CLR type, property names and oneof names.
        ''' Each array parameter may be null, to indicate a lack of values.
        ''' The parameter order is designed to make it feasible to format the generated code readably.
        ''' </summary>
        Public Sub New(clrType As Type, parser As MessageParser, propertyNames As String(), oneofNames As String(), nestedEnums As Type(), nestedTypes As GeneratedClrTypeInfo())
            Me.NestedTypes = If(nestedTypes, EmptyCodeInfo)
            Me.NestedEnums = If(nestedEnums, EmptyTypes)
            Me.ClrType = clrType
            Me.Parser = parser
            Me.PropertyNames = If(propertyNames, EmptyNames)
            Me.OneofNames = If(oneofNames, EmptyNames)
        End Sub

        ''' <summary>
        ''' Creates a GeneratedClrTypeInfo for a file descriptor, with only types and enums.
        ''' </summary>
        Public Sub New(nestedEnums As Type(), nestedTypes As GeneratedClrTypeInfo())
            Me.New(Nothing, Nothing, Nothing, Nothing, nestedEnums, nestedTypes)
        End Sub
    End Class
End Namespace
