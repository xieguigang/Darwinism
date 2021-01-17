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
    ''' Descriptor for a single enum value within an enum in a .proto file.
    ''' </summary>
    Public NotInheritable Class EnumValueDescriptor
        Inherits DescriptorBase

        Private ReadOnly enumDescriptorField As EnumDescriptor
        Private ReadOnly protoField As EnumValueDescriptorProto

        Friend Sub New(proto As EnumValueDescriptorProto, file As FileDescriptor, parent As EnumDescriptor, index As Integer)
            MyBase.New(file, parent.FullName & "." & proto.Name, index)
            protoField = proto
            enumDescriptorField = parent
            file.DescriptorPool.AddSymbol(Me)
            file.DescriptorPool.AddEnumValueByNumber(Me)
        End Sub

        Friend ReadOnly Property Proto As EnumValueDescriptorProto
            Get
                Return protoField
            End Get
        End Property

        ''' <summary>
        ''' Returns the name of the enum value described by this object.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return protoField.Name
            End Get
        End Property

        ''' <summary>
        ''' Returns the number associated with this enum value.
        ''' </summary>
        Public ReadOnly Property Number As Integer
            Get
                Return Proto.Number
            End Get
        End Property

        ''' <summary>
        ''' Returns the enum descriptor that this value is part of.
        ''' </summary>
        Public ReadOnly Property EnumDescriptor As EnumDescriptor
            Get
                Return enumDescriptorField
            End Get
        End Property
    End Class
End Namespace
