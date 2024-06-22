#Region "Microsoft.VisualBasic::ac6cf61bee97cff9cae3e814e135b6e9, src\message\Google.Protobuf\Reflection\Descriptor\DescriptorBase.vb"

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

    '   Total Lines: 87
    '    Code Lines: 31 (35.63%)
    ' Comment Lines: 49 (56.32%)
    '    - Xml Docs: 40.82%
    ' 
    '   Blank Lines: 7 (8.05%)
    '     File Size: 3.69 KB


    '     Class DescriptorBase
    ' 
    '         Properties: File, FullName, Index
    ' 
    '         Constructor: (+1 Overloads) Sub New
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
    ''' Base class for nearly all descriptors, providing common functionality.
    ''' </summary>
    Public MustInherit Class DescriptorBase
        Implements IDescriptor

        Private ReadOnly fileField As FileDescriptor
        Private ReadOnly fullNameField As String
        Private ReadOnly indexField As Integer

        Friend Sub New(file As FileDescriptor, fullName As String, index As Integer)
            fileField = file
            fullNameField = fullName
            indexField = index
        End Sub

        ''' <value>
        ''' The index of this descriptor within its parent descriptor. 
        ''' </value>
        ''' <remarks>
        ''' This returns the index of this descriptor within its parent, for
        ''' this descriptor's type. (There can be duplicate values for different
        ''' types, e.g. one enum type with index 0 and one message type with index 0.)
        ''' </remarks>
        Public ReadOnly Property Index As Integer
            Get
                Return indexField
            End Get
        End Property

        ''' <summary>
        ''' Returns the name of the entity (field, message etc) being described.
        ''' </summary>
        Public MustOverride ReadOnly Property Name As String Implements IDescriptor.Name

        ''' <summary>
        ''' The fully qualified name of the descriptor's target.
        ''' </summary>
        Public ReadOnly Property FullName As String Implements IDescriptor.FullName
            Get
                Return fullNameField
            End Get
        End Property

        ''' <value>
        ''' The file this descriptor was declared in.
        ''' </value>
        Public ReadOnly Property File As FileDescriptor Implements IDescriptor.File
            Get
                Return fileField
            End Get
        End Property
    End Class
End Namespace
