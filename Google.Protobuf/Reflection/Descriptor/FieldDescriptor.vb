#Region "Microsoft.VisualBasic::a2296fe33c8bd460a495d653fb91baaa, Google.Protobuf\Reflection\Descriptor\FieldDescriptor.vb"

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

    '     Class FieldDescriptor
    ' 
    '         Properties: Accessor, ContainingOneof, ContainingType, EnumType, FieldNumber
    '                     FieldType, IsMap, IsPacked, IsRepeated, JsonName
    '                     MessageType, Name, Proto
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: CompareTo, CreateAccessor, GetFieldTypeFromProtoType
    ' 
    '         Sub: CrossLink
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

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Descriptor for a field or extension within a message in a .proto file.
    ''' </summary>
    Public NotInheritable Class FieldDescriptor
        Inherits DescriptorBase
        Implements IComparable(Of FieldDescriptor)

        Private enumTypeField As EnumDescriptor
        Private messageTypeField As MessageDescriptor
        Private fieldTypeField As FieldType
        Private ReadOnly propertyName As String ' Annoyingly, needed in Crosslink.
        Private accessorField As IFieldAccessor

        ''' <summary>
        ''' Get the field's containing message type.
        ''' </summary>
        Public ReadOnly Property ContainingType As MessageDescriptor

        ''' <summary>
        ''' Returns the oneof containing this field, or <c>null</c> if it is not part of a oneof.
        ''' </summary>
        Public ReadOnly Property ContainingOneof As OneofDescriptor

        ''' <summary>
        ''' The effective JSON name for this field. This is usually the lower-camel-cased form of the field name,
        ''' but can be overridden using the <c>json_name</c> option in the .proto file.
        ''' </summary>
        Public ReadOnly Property JsonName As String
        Friend ReadOnly Property Proto As FieldDescriptorProto

        Friend Sub New(proto As FieldDescriptorProto, file As FileDescriptor, parent As MessageDescriptor, index As Integer, propertyName As String)
            MyBase.New(file, file.ComputeFullName(parent, proto.Name), index)
            Me.Proto = proto

            If proto.Type <> 0 Then
                fieldTypeField = FieldDescriptor.GetFieldTypeFromProtoType(proto.Type)
            End If

            If FieldNumber <= 0 Then
                Throw New DescriptorValidationException(Me, "Field numbers must be positive integers.")
            End If

            ContainingType = parent
            ' OneofIndex "defaults" to -1 due to a hack in FieldDescriptor.OnConstruction.
            If proto.OneofIndex <> -1 Then
                If proto.OneofIndex < 0 OrElse proto.OneofIndex >= parent.Proto.OneofDecl.Count Then
                    Throw New DescriptorValidationException(Me, $"FieldDescriptorProto.oneof_index is out of range for type {parent.Name}")
                End If

                ContainingOneof = parent.Oneofs(proto.OneofIndex)
            End If

            file.DescriptorPool.AddSymbol(Me)
            ' We can't create the accessor until we've cross-linked, unfortunately, as we
            ' may not know whether the type of the field is a map or not. Remember the property name
            ' for later.
            ' We could trust the generated code and check whether the type of the property is
            ' a MapField, but that feels a tad nasty.
            Me.propertyName = propertyName
            JsonName = If(Equals(Me.Proto.JsonName, ""), JsonFormatter.ToCamelCase(Me.Proto.Name), Me.Proto.JsonName)
        End Sub


        ''' <summary>
        ''' The brief name of the descriptor's target.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return Proto.Name
            End Get
        End Property

        ''' <summary>
        ''' Returns the accessor for this field.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' While a <see cref="FieldDescriptor"/> describes the field, it does not provide
        ''' any way of obtaining or changing the value of the field within a specific message;
        ''' that is the responsibility of the accessor.
        ''' </para>
        ''' <para>
        ''' The value returned by this property will be non-null for all regular fields. However,
        ''' if a message containing a map field is introspected, the list of nested messages will include
        ''' an auto-generated nested key/value pair message for the field. This is not represented in any
        ''' generated type, and the value of the map field itself is represented by a dictionary in the
        ''' reflection API. There are never instances of those "hidden" messages, so no accessor is provided
        ''' and this property will return null.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property Accessor As IFieldAccessor
            Get
                Return accessorField
            End Get
        End Property

        ''' <summary>
        ''' Maps a field type as included in the .proto file to a FieldType.
        ''' </summary>
        Private Shared Function GetFieldTypeFromProtoType(type As FieldDescriptorProto.Types.Type) As FieldType
            Select Case type
                Case FieldDescriptorProto.Types.Type.Double
                    Return FieldType.Double
                Case FieldDescriptorProto.Types.Type.Float
                    Return FieldType.Float
                Case FieldDescriptorProto.Types.Type.Int64
                    Return FieldType.Int64
                Case FieldDescriptorProto.Types.Type.Uint64
                    Return FieldType.UInt64
                Case FieldDescriptorProto.Types.Type.Int32
                    Return FieldType.Int32
                Case FieldDescriptorProto.Types.Type.Fixed64
                    Return FieldType.Fixed64
                Case FieldDescriptorProto.Types.Type.Fixed32
                    Return FieldType.Fixed32
                Case FieldDescriptorProto.Types.Type.Bool
                    Return FieldType.Bool
                Case FieldDescriptorProto.Types.Type.String
                    Return FieldType.String
                Case FieldDescriptorProto.Types.Type.Group
                    Return FieldType.Group
                Case FieldDescriptorProto.Types.Type.Message
                    Return FieldType.Message
                Case FieldDescriptorProto.Types.Type.Bytes
                    Return FieldType.Bytes
                Case FieldDescriptorProto.Types.Type.Uint32
                    Return FieldType.UInt32
                Case FieldDescriptorProto.Types.Type.Enum
                    Return FieldType.Enum
                Case FieldDescriptorProto.Types.Type.Sfixed32
                    Return FieldType.SFixed32
                Case FieldDescriptorProto.Types.Type.Sfixed64
                    Return FieldType.SFixed64
                Case FieldDescriptorProto.Types.Type.Sint32
                    Return FieldType.SInt32
                Case FieldDescriptorProto.Types.Type.Sint64
                    Return FieldType.SInt64
                Case Else
                    Throw New ArgumentException("Invalid type specified")
            End Select
        End Function

        ''' <summary>
        ''' Returns <c>true</c> if this field is a repeated field; <c>false</c> otherwise.
        ''' </summary>
        Public ReadOnly Property IsRepeated As Boolean
            Get
                Return Proto.Label = FieldDescriptorProto.Types.Label.Repeated
            End Get
        End Property

        ''' <summary>
        ''' Returns <c>true</c> if this field is a map field; <c>false</c> otherwise.
        ''' </summary>
        Public ReadOnly Property IsMap As Boolean
            Get
                Return fieldTypeField = FieldType.Message AndAlso messageTypeField.Proto.Options IsNot Nothing AndAlso messageTypeField.Proto.Options.MapEntry
            End Get
        End Property

        ''' <summary>
        ''' Returns <c>true</c> if this field is a packed, repeated field; <c>false</c> otherwise.
        ''' </summary>
        Public ReadOnly Property IsPacked As Boolean
            Get
                ' Note the || rather than && here - we're effectively defaulting to packed, because that *is*
                ' the default in proto3, which is all we support. We may give the wrong result for the protos
                ' within descriptor.proto, but that's okay, as they're never exposed and we don't use IsPacked
                ' within the runtime.
                Return Proto.Options Is Nothing OrElse Proto.Options.Packed
            End Get
        End Property

        ''' <summary>
        ''' Returns the type of the field.
        ''' </summary>
        Public ReadOnly Property FieldType As FieldType
            Get
                Return fieldTypeField
            End Get
        End Property

        ''' <summary>
        ''' Returns the field number declared in the proto file.
        ''' </summary>
        Public ReadOnly Property FieldNumber As Integer
            Get
                Return Proto.Number
            End Get
        End Property

        ''' <summary>
        ''' Compares this descriptor with another one, ordering in "canonical" order
        ''' which simply means ascending order by field number. <paramrefname="other"/>
        ''' must be a field of the same type, i.e. the <see cref="ContainingType"/> of
        ''' both fields must be the same.
        ''' </summary>
        Public Function CompareTo(other As FieldDescriptor) As Integer Implements IComparable(Of FieldDescriptor).CompareTo
            If other.ContainingType IsNot ContainingType Then
                Throw New ArgumentException("FieldDescriptors can only be compared to other FieldDescriptors " & "for fields of the same message type.")
            End If

            Return FieldNumber - other.FieldNumber
        End Function

        ''' <summary>
        ''' For enum fields, returns the field's type.
        ''' </summary>
        Public ReadOnly Property EnumType As EnumDescriptor
            Get

                If fieldTypeField <> FieldType.Enum Then
                    Throw New InvalidOperationException("EnumType is only valid for enum fields.")
                End If

                Return enumTypeField
            End Get
        End Property

        ''' <summary>
        ''' For embedded message and group fields, returns the field's type.
        ''' </summary>
        Public ReadOnly Property MessageType As MessageDescriptor
            Get

                If fieldTypeField <> FieldType.Message Then
                    Throw New InvalidOperationException("MessageType is only valid for message fields.")
                End If

                Return messageTypeField
            End Get
        End Property

        ''' <summary>
        ''' Look up and cross-link all field types etc.
        ''' </summary>
        Friend Sub CrossLink()
            If Not Equals(Proto.TypeName, "") Then
                Dim typeDescriptor = File.DescriptorPool.LookupSymbol(Proto.TypeName, Me)

                If Proto.Type <> 0 Then
                    ' Choose field type based on symbol.
                    If TypeOf typeDescriptor Is MessageDescriptor Then
                        fieldTypeField = FieldType.Message
                    ElseIf TypeOf typeDescriptor Is EnumDescriptor Then
                        fieldTypeField = FieldType.Enum
                    Else
                        Throw New DescriptorValidationException(Me, $"""{Proto.TypeName}"" is not a type.")
                    End If
                End If

                If fieldTypeField = FieldType.Message Then
                    If Not (TypeOf typeDescriptor Is MessageDescriptor) Then
                        Throw New DescriptorValidationException(Me, $"""{Proto.TypeName}"" is not a message type.")
                    End If

                    messageTypeField = CType(typeDescriptor, MessageDescriptor)

                    If Not Equals(Proto.DefaultValue, "") Then
                        Throw New DescriptorValidationException(Me, "Messages can't have default values.")
                    End If
                ElseIf fieldTypeField = FieldType.Enum Then

                    If Not (TypeOf typeDescriptor Is EnumDescriptor) Then
                        Throw New DescriptorValidationException(Me, $"""{Proto.TypeName}"" is not an enum type.")
                    End If

                    enumTypeField = CType(typeDescriptor, EnumDescriptor)
                Else
                    Throw New DescriptorValidationException(Me, "Field with primitive type has type_name.")
                End If
            Else

                If fieldTypeField = FieldType.Message OrElse fieldTypeField = FieldType.Enum Then
                    Throw New DescriptorValidationException(Me, "Field with message or enum type missing type_name.")
                End If
            End If

            ' Note: no attempt to perform any default value parsing

            File.DescriptorPool.AddFieldByNumber(Me)

            If ContainingType IsNot Nothing AndAlso ContainingType.Proto.Options IsNot Nothing AndAlso ContainingType.Proto.Options.MessageSetWireFormat Then
                Throw New DescriptorValidationException(Me, "MessageSet format is not supported.")
            End If

            accessorField = CreateAccessor()
        End Sub

        Private Function CreateAccessor() As IFieldAccessor
            ' If we're given no property name, that's because we really don't want an accessor.
            ' (At the moment, that means it's a map entry message...)
            If Equals(propertyName, Nothing) Then
                Return Nothing
            End If

            Dim [property] = ContainingType.ClrType.GetProperty(propertyName)

            If [property] Is Nothing Then
                Throw New DescriptorValidationException(Me, $"Property {propertyName} not found in {ContainingType.ClrType}")
            End If

            Return If(IsMap, New MapFieldAccessor([property], Me), If(IsRepeated, New RepeatedFieldAccessor([property], Me), CType(New SingleFieldAccessor([property], Me), IFieldAccessor)))
        End Function
    End Class
End Namespace
