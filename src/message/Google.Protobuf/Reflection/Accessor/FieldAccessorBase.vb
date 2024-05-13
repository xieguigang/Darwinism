#Region "Microsoft.VisualBasic::420de18a3c0c16d9f7fc6262241a924b, src\message\Google.Protobuf\Reflection\Accessor\FieldAccessorBase.vb"

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

    '   Total Lines: 64
    '    Code Lines: 25
    ' Comment Lines: 32
    '   Blank Lines: 7
    '     File Size: 2.93 KB


    '     Class FieldAccessorBase
    ' 
    '         Properties: Descriptor
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetValue
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
Imports System.Reflection

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Base class for field accessors.
    ''' </summary>
    Friend MustInherit Class FieldAccessorBase
        Implements IFieldAccessor

        Private ReadOnly getValueDelegate As Func(Of IMessage, Object)
        Private ReadOnly descriptorField As FieldDescriptor

        Friend Sub New([property] As PropertyInfo, descriptor As FieldDescriptor)
            descriptorField = descriptor
            getValueDelegate = CreateFuncIMessageObject([property].GetGetMethod())
        End Sub

        Public ReadOnly Property Descriptor As FieldDescriptor Implements IFieldAccessor.Descriptor
            Get
                Return descriptorField
            End Get
        End Property

        Public Function GetValue(message As IMessage) As Object Implements IFieldAccessor.GetValue
            Return getValueDelegate(message)
        End Function

        Public MustOverride Sub Clear(message As IMessage) Implements IFieldAccessor.Clear
        Public MustOverride Sub SetValue(message As IMessage, value As Object) Implements IFieldAccessor.SetValue
    End Class
End Namespace
