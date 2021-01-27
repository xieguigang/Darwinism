#Region "Microsoft.VisualBasic::3913e2df98fc134ef7d6bcd5c6db2a5b, Google.Protobuf\Reflection\Descriptor\ServiceDescriptor.vb"

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

    '     Class ServiceDescriptor
    ' 
    '         Properties: Methods, Name, Proto
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: FindMethodByName
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

Imports System.Collections.Generic

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Describes a service type.
    ''' </summary>
    Public NotInheritable Class ServiceDescriptor
        Inherits DescriptorBase

        Private ReadOnly protoField As ServiceDescriptorProto
        Private ReadOnly methodsField As IList(Of MethodDescriptor)

        Friend Sub New(proto As ServiceDescriptorProto, file As FileDescriptor, index As Integer)
            MyBase.New(file, file.ComputeFullName(Nothing, proto.Name), index)
            protoField = proto
            methodsField = DescriptorUtil.ConvertAndMakeReadOnly(proto.Method, Function(method, i) New MethodDescriptor(method, file, Me, i))
            file.DescriptorPool.AddSymbol(Me)
        End Sub

        ''' <summary>
        ''' The brief name of the descriptor's target.
        ''' </summary>
        Public Overrides ReadOnly Property Name As String
            Get
                Return protoField.Name
            End Get
        End Property

        Friend ReadOnly Property Proto As ServiceDescriptorProto
            Get
                Return protoField
            End Get
        End Property

        ''' <value>
        ''' An unmodifiable list of methods in this service.
        ''' </value>
        Public ReadOnly Property Methods As IList(Of MethodDescriptor)
            Get
                Return methodsField
            End Get
        End Property

        ''' <summary>
        ''' Finds a method by name.
        ''' </summary>
        ''' <param name="name">The unqualified name of the method (e.g. "Foo").</param>
        ''' <returns>The method's decsriptor, or null if not found.</returns>
        Public Function FindMethodByName(name As String) As MethodDescriptor
            Return File.DescriptorPool.FindSymbol(Of MethodDescriptor)(FullName & "." & name)
        End Function

        Friend Sub CrossLink()
            For Each method In methodsField
                method.CrossLink()
            Next
        End Sub
    End Class
End Namespace

