﻿#Region "Microsoft.VisualBasic::fb527c4a83e7a13b00bb4f28e9c1fe39, src\message\Google.Protobuf\Reflection\TypeRegistry.vb"

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

    '   Total Lines: 168
    '    Code Lines: 64 (38.10%)
    ' Comment Lines: 88 (52.38%)
    '    - Xml Docs: 62.50%
    ' 
    '   Blank Lines: 16 (9.52%)
    '     File Size: 8.73 KB


    '     Class TypeRegistry
    ' 
    '         Properties: Empty
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Find, (+2 Overloads) FromFiles, (+2 Overloads) FromMessages
    '         Class Builder
    ' 
    '             Constructor: (+1 Overloads) Sub New
    ' 
    '             Function: Build
    ' 
    '             Sub: AddFile, AddMessage
    ' 
    ' 
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
Imports System.Collections.Generic
Imports System.Linq

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' An immutable registry of types which can be looked up by their full name.
    ''' </summary>
    Public NotInheritable Class TypeRegistry
        ''' <summary>
        ''' An empty type registry, containing no types.
        ''' </summary>
        Public Shared ReadOnly Property Empty As TypeRegistry = New TypeRegistry(New Dictionary(Of String, MessageDescriptor)())
        Private ReadOnly fullNameToMessageMap As Dictionary(Of String, MessageDescriptor)

        Private Sub New(fullNameToMessageMap As Dictionary(Of String, MessageDescriptor))
            Me.fullNameToMessageMap = fullNameToMessageMap
        End Sub

        ''' <summary>
        ''' Attempts to find a message descriptor by its full name.
        ''' </summary>
        ''' <param name="fullName">The full name of the message, which is the dot-separated
        ''' combination of package, containing messages and message name</param>
        ''' <returns>The message descriptor corresponding to <paramrefname="fullName"/> or null
        ''' if there is no such message descriptor.</returns>
        Public Function Find(fullName As String) As MessageDescriptor
            Dim ret As MessageDescriptor
            ' Ignore the return value as ret will end up with the right value either way.
            fullNameToMessageMap.TryGetValue(fullName, ret)
            Return ret
        End Function

        ''' <summary>
        ''' Creates a type registry from the specified set of file descriptors.
        ''' </summary>
        ''' <remarks>
        ''' This is a convenience overload for <see cref="FromFiles(IEnumerable(OfFileDescriptor))"/>
        ''' to allow calls such as <c>TypeRegistry.FromFiles(descriptor1, descriptor2)</c>.
        ''' </remarks>
        ''' <param name="fileDescriptors">The set of files to include in the registry. Must not contain null values.</param>
        ''' <returns>A type registry for the given files.</returns>
        Public Shared Function FromFiles(ParamArray fileDescriptors As FileDescriptor()) As TypeRegistry
            Return FromFiles(CType(fileDescriptors, IEnumerable(Of FileDescriptor)))
        End Function

        ''' <summary>
        ''' Creates a type registry from the specified set of file descriptors.
        ''' </summary>
        ''' <remarks>
        ''' All message types within all the specified files are added to the registry, and
        ''' the dependencies of the specified files are also added, recursively.
        ''' </remarks>
        ''' <param name="fileDescriptors">The set of files to include in the registry. Must not contain null values.</param>
        ''' <returns>A type registry for the given files.</returns>
        Public Shared Function FromFiles(fileDescriptors As IEnumerable(Of FileDescriptor)) As TypeRegistry
            CheckNotNull(fileDescriptors, NameOf(fileDescriptors))
            Dim builder = New Builder()

            For Each file In fileDescriptors
                builder.AddFile(file)
            Next

            Return builder.Build()
        End Function

        ''' <summary>
        ''' Creates a type registry from the file descriptor parents of the specified set of message descriptors.
        ''' </summary>
        ''' <remarks>
        ''' This is a convenience overload for <see cref="FromMessages(IEnumerable(OfMessageDescriptor))"/>
        ''' to allow calls such as <c>TypeRegistry.FromFiles(descriptor1, descriptor2)</c>.
        ''' </remarks>
        ''' <param name="messageDescriptors">The set of message descriptors to use to identify file descriptors to include in the registry.
        ''' Must not contain null values.</param>
        ''' <returns>A type registry for the given files.</returns>
        Public Shared Function FromMessages(ParamArray messageDescriptors As MessageDescriptor()) As TypeRegistry
            Return FromMessages(CType(messageDescriptors, IEnumerable(Of MessageDescriptor)))
        End Function

        ''' <summary>
        ''' Creates a type registry from the file descriptor parents of the specified set of message descriptors.
        ''' </summary>
        ''' <remarks>
        ''' The specified message descriptors are only used to identify their file descriptors; the returned registry
        ''' contains all the types within the file descriptors which contain the specified message descriptors (and
        ''' the dependencies of those files), not just the specified messages.
        ''' </remarks>
        ''' <param name="messageDescriptors">The set of message descriptors to use to identify file descriptors to include in the registry.
        ''' Must not contain null values.</param>
        ''' <returns>A type registry for the given files.</returns>
        Public Shared Function FromMessages(messageDescriptors As IEnumerable(Of MessageDescriptor)) As TypeRegistry
            CheckNotNull(messageDescriptors, NameOf(messageDescriptors))
            Return FromFiles(messageDescriptors.Select(Function(md) md.File))
        End Function

        ''' <summary>
        ''' Builder class which isn't exposed, but acts as a convenient alternative to passing round two dictionaries in recursive calls.
        ''' </summary>
        Private Class Builder
            Private ReadOnly types As Dictionary(Of String, MessageDescriptor)
            Private ReadOnly fileDescriptorNames As HashSet(Of String)

            Friend Sub New()
                types = New Dictionary(Of String, MessageDescriptor)()
                fileDescriptorNames = New HashSet(Of String)()
            End Sub

            Friend Sub AddFile(fileDescriptor As FileDescriptor)
                If Not fileDescriptorNames.Add(fileDescriptor.Name) Then
                    Return
                End If

                For Each dependency In fileDescriptor.Dependencies
                    AddFile(dependency)
                Next

                For Each message In fileDescriptor.MessageTypes
                    AddMessage(message)
                Next
            End Sub

            Private Sub AddMessage(messageDescriptor As MessageDescriptor)
                For Each nestedType In messageDescriptor.NestedTypes
                    AddMessage(nestedType)
                Next
                ' This will overwrite any previous entry. Given that each file should
                ' only be added once, this could be a problem such as package A.B with type C,
                ' and package A with type B.C... it's unclear what we should do in that case.
                types(messageDescriptor.FullName) = messageDescriptor
            End Sub

            Friend Function Build() As TypeRegistry
                Return New TypeRegistry(types)
            End Function
        End Class
    End Class
End Namespace
