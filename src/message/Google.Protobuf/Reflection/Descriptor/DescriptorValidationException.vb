#Region "Microsoft.VisualBasic::9adea3ac5b6075b7dd3c14f273cca457, src\message\Google.Protobuf\Reflection\Descriptor\DescriptorValidationException.vb"

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

    '   Total Lines: 80
    '    Code Lines: 30
    ' Comment Lines: 43
    '   Blank Lines: 7
    '     File Size: 3.52 KB


    '     Class DescriptorValidationException
    ' 
    '         Properties: Description, ProblemSymbolName
    ' 
    '         Constructor: (+2 Overloads) Sub New
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
    ''' Thrown when building descriptors fails because the source DescriptorProtos
    ''' are not valid.
    ''' </summary>
    Public NotInheritable Class DescriptorValidationException
        Inherits Exception

        Private ReadOnly name As String
        Private ReadOnly descriptionField As String

        ''' <value>
        ''' The full name of the descriptor where the error occurred.
        ''' </value>
        Public ReadOnly Property ProblemSymbolName As String
            Get
                Return name
            End Get
        End Property

        ''' <value>
        ''' A human-readable description of the error. (The Message property
        ''' is made up of the descriptor's name and this description.)
        ''' </value>
        Public ReadOnly Property Description As String
            Get
                Return descriptionField
            End Get
        End Property

        Friend Sub New(problemDescriptor As IDescriptor, description As String)
            MyBase.New(problemDescriptor.FullName & ": " & description)
            ' Note that problemDescriptor may be partially uninitialized, so we
            ' don't want to expose it directly to the user.  So, we only provide
            ' the name and the original proto.
            name = problemDescriptor.FullName
            descriptionField = description
        End Sub

        Friend Sub New(problemDescriptor As IDescriptor, description As String, cause As Exception)
            MyBase.New(problemDescriptor.FullName & ": " & description, cause)
            name = problemDescriptor.FullName
            descriptionField = description
        End Sub
    End Class
End Namespace
