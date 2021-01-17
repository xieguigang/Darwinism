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

Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Describes a "oneof" field collection in a message type: a set of
    ''' fields of which at most one can be set in any particular message.
    ''' </summary>
    Public NotInheritable Class OneofDescriptor
        Inherits DescriptorBase

        Private ReadOnly proto As OneofDescriptorProto
        Private containingTypeField As MessageDescriptor
        Private fieldsField As IList(Of FieldDescriptor)
        Private ReadOnly accessorField As OneofAccessor

        Friend Sub New(proto As OneofDescriptorProto, file As FileDescriptor, parent As MessageDescriptor, index As Integer, clrName As String)
            MyBase.New(file, file.ComputeFullName(parent, proto.Name), index)
            Me.proto = proto
            containingTypeField = parent
            file.DescriptorPool.AddSymbol(Me)
            accessorField = CreateAccessor(clrName)
        End Sub

        ''' <summary>
        ''' The brief name of the descriptor's target.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return proto.Name
            End Get
        End Property

        ''' <summary>
        ''' Gets the message type containing this oneof.
        ''' </summary>
        ''' <value>
        ''' The message type containing this oneof.
        ''' </value>
        Public ReadOnly Property ContainingType As MessageDescriptor
            Get
                Return containingTypeField
            End Get
        End Property

        ''' <summary>
        ''' Gets the fields within this oneof, in declaration order.
        ''' </summary>
        ''' <value>
        ''' The fields within this oneof, in declaration order.
        ''' </value>
        Public ReadOnly Property Fields As IList(Of FieldDescriptor)
            Get
                Return fieldsField
            End Get
        End Property

        ''' <summary>
        ''' Gets an accessor for reflective access to the values associated with the oneof
        ''' in a particular message.
        ''' </summary>
        ''' <value>
        ''' The accessor used for reflective access.
        ''' </value>
        Public ReadOnly Property Accessor As OneofAccessor
            Get
                Return accessorField
            End Get
        End Property

        Friend Sub CrossLink()
            Dim fieldCollection As List(Of FieldDescriptor) = New List(Of FieldDescriptor)()

            For Each field In ContainingType.Fields.InDeclarationOrder()

                If field.ContainingOneof Is Me Then
                    fieldCollection.Add(field)
                End If
            Next

            fieldsField = New ReadOnlyCollection(Of FieldDescriptor)(fieldCollection)
        End Sub

        Private Function CreateAccessor(clrName As String) As OneofAccessor
            Dim caseProperty = containingTypeField.ClrType.GetProperty(clrName & "Case")

            If caseProperty Is Nothing Then
                Throw New DescriptorValidationException(Me, $"Property {clrName}Case not found in {containingTypeField.ClrType}")
            End If

            Dim clearMethod = containingTypeField.ClrType.GetMethod("Clear" & clrName)

            If clearMethod Is Nothing Then
                Throw New DescriptorValidationException(Me, $"Method Clear{clrName} not found in {containingTypeField.ClrType}")
            End If

            Return New OneofAccessor(caseProperty, clearMethod, Me)
        End Function
    End Class
End Namespace
