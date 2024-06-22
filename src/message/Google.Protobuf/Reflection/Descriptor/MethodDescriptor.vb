#Region "Microsoft.VisualBasic::78829091ab99edcc0f5b024bd25452e0, src\message\Google.Protobuf\Reflection\Descriptor\MethodDescriptor.vb"

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

    '   Total Lines: 129
    '    Code Lines: 64 (49.61%)
    ' Comment Lines: 50 (38.76%)
    '    - Xml Docs: 42.00%
    ' 
    '   Blank Lines: 15 (11.63%)
    '     File Size: 4.99 KB


    '     Class MethodDescriptor
    ' 
    '         Properties: InputType, IsClientStreaming, IsServerStreaming, Name, OutputType
    '                     Proto, Service
    ' 
    '         Constructor: (+1 Overloads) Sub New
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

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Describes a single method in a service.
    ''' </summary>
    Public NotInheritable Class MethodDescriptor
        Inherits DescriptorBase

        Private ReadOnly protoField As MethodDescriptorProto
        Private ReadOnly serviceField As ServiceDescriptor
        Private inputTypeField As MessageDescriptor
        Private outputTypeField As MessageDescriptor

        ''' <value>
        ''' The service this method belongs to.
        ''' </value>
        Public ReadOnly Property Service As ServiceDescriptor
            Get
                Return serviceField
            End Get
        End Property

        ''' <value>
        ''' The method's input type.
        ''' </value>
        Public ReadOnly Property InputType As MessageDescriptor
            Get
                Return inputTypeField
            End Get
        End Property

        ''' <value>
        ''' The method's input type.
        ''' </value>
        Public ReadOnly Property OutputType As MessageDescriptor
            Get
                Return outputTypeField
            End Get
        End Property

        ''' <value>
        ''' Indicates if client streams multiple requests.
        ''' </value>
        Public ReadOnly Property IsClientStreaming As Boolean
            Get
                Return protoField.ClientStreaming
            End Get
        End Property

        ''' <value>
        ''' Indicates if server streams multiple responses.
        ''' </value>
        Public ReadOnly Property IsServerStreaming As Boolean
            Get
                Return protoField.ServerStreaming
            End Get
        End Property

        Friend Sub New(proto As MethodDescriptorProto, file As FileDescriptor, parent As ServiceDescriptor, index As Integer)
            MyBase.New(file, parent.FullName & "." & proto.Name, index)
            protoField = proto
            serviceField = parent
            file.DescriptorPool.AddSymbol(Me)
        End Sub

        Friend ReadOnly Property Proto As MethodDescriptorProto
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

        Friend Sub CrossLink()
            Dim lookup = File.DescriptorPool.LookupSymbol(Proto.InputType, Me)

            If Not (TypeOf lookup Is MessageDescriptor) Then
                Throw New DescriptorValidationException(Me, """" & Proto.InputType & """ is not a message type.")
            End If

            inputTypeField = CType(lookup, MessageDescriptor)
            lookup = File.DescriptorPool.LookupSymbol(Proto.OutputType, Me)

            If Not (TypeOf lookup Is MessageDescriptor) Then
                Throw New DescriptorValidationException(Me, """" & Proto.OutputType & """ is not a message type.")
            End If

            outputTypeField = CType(lookup, MessageDescriptor)
        End Sub
    End Class
End Namespace
