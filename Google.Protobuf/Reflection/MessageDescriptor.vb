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
Imports System.Collections.ObjectModel
Imports System.Linq
#If DOTNET35
// Needed for ReadOnlyDictionary, which does not exist in .NET 3.5
using Google.Protobuf.Collections;
#End If

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Describes a message type.
    ''' </summary>
    Public NotInheritable Class MessageDescriptor
        Inherits DescriptorBase

        Private Shared ReadOnly WellKnownTypeNames As HashSet(Of String) = New HashSet(Of String) From {
            "google/protobuf/any.proto",
            "google/protobuf/api.proto",
            "google/protobuf/duration.proto",
            "google/protobuf/empty.proto",
            "google/protobuf/wrappers.proto",
            "google/protobuf/timestamp.proto",
            "google/protobuf/field_mask.proto",
            "google/protobuf/source_context.proto",
            "google/protobuf/struct.proto",
            "google/protobuf/type.proto"
        }
        Friend ReadOnly fieldsInDeclarationOrder As IList(Of FieldDescriptor)
        Friend ReadOnly fieldsInNumberOrder As IList(Of FieldDescriptor)
        Friend ReadOnly jsonFieldMap As IDictionary(Of String, FieldDescriptor)

        Friend Sub New(proto As DescriptorProto, file As FileDescriptor, parent As MessageDescriptor, typeIndex As Integer, generatedCodeInfo As GeneratedClrTypeInfo)
            MyBase.New(file, file.ComputeFullName(parent, proto.Name), typeIndex)
            Me.Proto = proto
            Parser = generatedCodeInfo?.Parser
            ClrType = generatedCodeInfo?.ClrType
            ContainingType = parent

            ' Note use of generatedCodeInfo. rather than generatedCodeInfo?. here... we don't expect
            ' to see any nested oneofs, types or enums in "not actually generated" code... we do
            ' expect fields though (for map entry messages).
            Oneofs = DescriptorUtil.ConvertAndMakeReadOnly(proto.OneofDecl, Function(oneof, index) New OneofDescriptor(oneof, file, Me, index, generatedCodeInfo.OneofNames(index)))
            NestedTypes = DescriptorUtil.ConvertAndMakeReadOnly(proto.NestedType, Function(type, index) New MessageDescriptor(type, file, Me, index, generatedCodeInfo.NestedTypes(index)))
            EnumTypes = DescriptorUtil.ConvertAndMakeReadOnly(proto.EnumType, Function(type, index) New EnumDescriptor(type, file, Me, index, generatedCodeInfo.NestedEnums(index)))
            fieldsInDeclarationOrder = DescriptorUtil.ConvertAndMakeReadOnly(proto.Field, Function(field, index) New FieldDescriptor(field, file, Me, index, generatedCodeInfo?.PropertyNames(index)))
            fieldsInNumberOrder = New ReadOnlyCollection(Of FieldDescriptor)(fieldsInDeclarationOrder.OrderBy(Function(field) field.FieldNumber).ToArray())
            ' TODO: Use field => field.Proto.JsonName when we're confident it's appropriate. (And then use it in the formatter, too.)
            jsonFieldMap = CreateJsonFieldMap(fieldsInNumberOrder)
            file.DescriptorPool.AddSymbol(Me)
            Fields = New FieldCollection(Me)
        End Sub

        Private Shared Function CreateJsonFieldMap(fields As IList(Of FieldDescriptor)) As ReadOnlyDictionary(Of String, FieldDescriptor)
            Dim map = New Dictionary(Of String, FieldDescriptor)()

            For Each field In fields
                map(field.Name) = field
                map(field.JsonName) = field
            Next

            Return New ReadOnlyDictionary(Of String, FieldDescriptor)(map)
        End Function

        ''' <summary>
        ''' The brief name of the descriptor's target.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return Proto.Name
            End Get
        End Property

        Friend ReadOnly Property Proto As DescriptorProto

        ''' <summary>
        ''' The CLR type used to represent message instances from this descriptor.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' The value returned by this property will be non-null for all regular fields. However,
        ''' if a message containing a map field is introspected, the list of nested messages will include
        ''' an auto-generated nested key/value pair message for the field. This is not represented in any
        ''' generated type, so this property will return null in such cases.
        ''' </para>
        ''' <para>
        ''' For wrapper types (<see cref="Google.Protobuf.WellKnownTypes.StringValue"/> and the like), the type returned here
        ''' will be the generated message type, not the native type used by reflection for fields of those types. Code
        ''' using reflection should call <see cref="IsWrapperType"/> to determine whether a message descriptor represents
        ''' a wrapper type, and handle the result appropriately.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property ClrType As Type

        ''' <summary>
        ''' A parser for this message type.
        ''' </summary>
        ''' <remarks>
        ''' <para>
        ''' As <see cref="MessageDescriptor"/> is not generic, this cannot be statically
        ''' typed to the relevant type, but it should produce objects of a type compatible with <see cref="ClrType"/>.
        ''' </para>
        ''' <para>
        ''' The value returned by this property will be non-null for all regular fields. However,
        ''' if a message containing a map field is introspected, the list of nested messages will include
        ''' an auto-generated nested key/value pair message for the field. No message parser object is created for
        ''' such messages, so this property will return null in such cases.
        ''' </para>
        ''' <para>
        ''' For wrapper types (<see cref="Google.Protobuf.WellKnownTypes.StringValue"/> and the like), the parser returned here
        ''' will be the generated message type, not the native type used by reflection for fields of those types. Code
        ''' using reflection should call <see cref="IsWrapperType"/> to determine whether a message descriptor represents
        ''' a wrapper type, and handle the result appropriately.
        ''' </para>
        ''' </remarks>
        Public ReadOnly Property Parser As MessageParser

        ''' <summary>
        ''' Returns whether this message is one of the "well known types" which may have runtime/protoc support.
        ''' </summary>
        Friend ReadOnly Property IsWellKnownType As Boolean
            Get
                Return Equals(File.Package, "google.protobuf") AndAlso WellKnownTypeNames.Contains(File.Name)
            End Get
        End Property

        ''' <summary>
        ''' Returns whether this message is one of the "wrapper types" used for fields which represent primitive values
        ''' with the addition of presence.
        ''' </summary>
        Friend ReadOnly Property IsWrapperType As Boolean
            Get
                Return Equals(File.Package, "google.protobuf") AndAlso Equals(File.Name, "google/protobuf/wrappers.proto")
            End Get
        End Property

        ''' <value>
        ''' If this is a nested type, get the outer descriptor, otherwise null.
        ''' </value>
        Public ReadOnly Property ContainingType As MessageDescriptor

        ''' <value>
        ''' A collection of fields, which can be retrieved by name or field number.
        ''' </value>
        Public ReadOnly Property Fields As FieldCollection

        ''' <value>
        ''' An unmodifiable list of this message type's nested types.
        ''' </value>
        Public ReadOnly Property NestedTypes As IList(Of MessageDescriptor)

        ''' <value>
        ''' An unmodifiable list of this message type's enum types.
        ''' </value>
        Public ReadOnly Property EnumTypes As IList(Of EnumDescriptor)

        ''' <value>
        ''' An unmodifiable list of the "oneof" field collections in this message type.
        ''' </value>
        Public ReadOnly Property Oneofs As IList(Of OneofDescriptor)

        ''' <summary>
        ''' Finds a field by field name.
        ''' </summary>
        ''' <param name="name">The unqualified name of the field (e.g. "foo").</param>
        ''' <returns>The field's descriptor, or null if not found.</returns>
        Public Function FindFieldByName(name As String) As FieldDescriptor
            Return File.DescriptorPool.FindSymbol(Of FieldDescriptor)(FullName & "." & name)
        End Function

        ''' <summary>
        ''' Finds a field by field number.
        ''' </summary>
        ''' <param name="number">The field number within this message type.</param>
        ''' <returns>The field's descriptor, or null if not found.</returns>
        Public Function FindFieldByNumber(number As Integer) As FieldDescriptor
            Return File.DescriptorPool.FindFieldByNumber(Me, number)
        End Function

        ''' <summary>
        ''' Finds a nested descriptor by name. The is valid for fields, nested
        ''' message types, oneofs and enums.
        ''' </summary>
        ''' <param name="name">The unqualified name of the descriptor, e.g. "Foo"</param>
        ''' <returns>The descriptor, or null if not found.</returns>
        Public Function FindDescriptor(Of T As {Class, IDescriptor})(name As String) As T
            Return File.DescriptorPool.FindSymbol(Of T)(FullName & "." & name)
        End Function

        ''' <summary>
        ''' Looks up and cross-links all fields and nested types.
        ''' </summary>
        Friend Sub CrossLink()
            For Each message In NestedTypes
                message.CrossLink()
            Next

            For Each field In fieldsInDeclarationOrder
                field.CrossLink()
            Next

            For Each oneof In Oneofs
                oneof.CrossLink()
            Next
        End Sub
    End Class
End Namespace
