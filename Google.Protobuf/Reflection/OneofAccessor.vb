﻿#Region "Copyright notice and license"
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
Imports System.Reflection

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Reflection access for a oneof, allowing clear and "get case" actions.
    ''' </summary>
    Public NotInheritable Class OneofAccessor
        Private ReadOnly caseDelegate As Func(Of IMessage, Integer)
        Private ReadOnly clearDelegate As Action(Of IMessage)
        Private descriptorField As OneofDescriptor

        Friend Sub New(caseProperty As PropertyInfo, clearMethod As MethodInfo, descriptor As OneofDescriptor)
            If Not caseProperty.CanRead Then
                Throw New ArgumentException("Cannot read from property")
            End If

            descriptorField = descriptor
            caseDelegate = CreateFuncIMessageT(Of Integer)(caseProperty.GetGetMethod())
            descriptorField = descriptor
            clearDelegate = CreateActionIMessage(clearMethod)
        End Sub

        ''' <summary>
        ''' Gets the descriptor for this oneof.
        ''' </summary>
        ''' <value>
        ''' The descriptor of the oneof.
        ''' </value>
        Public ReadOnly Property Descriptor As OneofDescriptor
            Get
                Return descriptorField
            End Get
        End Property

        ''' <summary>
        ''' Clears the oneof in the specified message.
        ''' </summary>
        Public Sub Clear(message As IMessage)
            clearDelegate(message)
        End Sub

        ''' <summary>
        ''' Indicates which field in the oneof is set for specified message
        ''' </summary>
        Public Function GetCaseFieldDescriptor(message As IMessage) As FieldDescriptor
            Dim fieldNumber = caseDelegate(message)

            If fieldNumber > 0 Then
                Return descriptorField.ContainingType.FindFieldByNumber(fieldNumber)
            End If

            Return Nothing
        End Function
    End Class
End Namespace