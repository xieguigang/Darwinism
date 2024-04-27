#Region "Microsoft.VisualBasic::c2efa20f5e1e465d3e3d776518d7b769, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//Reflection/Descriptor/FileDescriptor.vb"

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

    '   Total Lines: 326
    '    Code Lines: 140
    ' Comment Lines: 142
    '   Blank Lines: 44
    '     File Size: 15.78 KB


    '     Class FileDescriptor
    ' 
    '         Properties: Dependencies, DescriptorPool, DescriptorProtoFileDescriptor, EnumTypes, File
    '                     FullName, MessageTypes, Name, Package, Proto
    '                     PublicDependencies, SerializedData, Services
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: BuildFrom, ComputeFullName, DeterminePublicDependencies, FindTypeByName, FromGeneratedCode
    '                   ToString
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

Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Describes a .proto file, including everything defined within.
    ''' IDescriptor is implemented such that the File property returns this descriptor,
    ''' and the FullName is the same as the Name.
    ''' </summary>
    Public NotInheritable Class FileDescriptor
        Implements IDescriptor

        Private Sub New(descriptorData As ByteString, proto As FileDescriptorProto, dependencies As FileDescriptor(), pool As DescriptorPool, allowUnknownDependencies As Boolean, generatedCodeInfo As GeneratedClrTypeInfo)
            SerializedData = descriptorData
            DescriptorPool = pool
            Me.Proto = proto
            Me.Dependencies = New ReadOnlyCollection(Of FileDescriptor)(CType(dependencies.Clone(), FileDescriptor()))
            PublicDependencies = DeterminePublicDependencies(Me, proto, dependencies, allowUnknownDependencies)
            pool.AddPackage(Package, Me)
            MessageTypes = DescriptorUtil.ConvertAndMakeReadOnly(proto.MessageType, Function(message, index) New MessageDescriptor(message, Me, Nothing, index, generatedCodeInfo.NestedTypes(index)))
            EnumTypes = DescriptorUtil.ConvertAndMakeReadOnly(proto.EnumType, Function(enumType, index) New EnumDescriptor(enumType, Me, Nothing, index, generatedCodeInfo.NestedEnums(index)))
            Services = DescriptorUtil.ConvertAndMakeReadOnly(proto.Service, Function(service, index) New ServiceDescriptor(service, Me, index))
        End Sub

        ''' <summary>
        ''' Computes the full name of a descriptor within this file, with an optional parent message.
        ''' </summary>
        Friend Function ComputeFullName(parent As MessageDescriptor, name As String) As String
            If parent IsNot Nothing Then
                Return parent.FullName & "." & name
            End If

            If Package.Length > 0 Then
                Return Package & "." & name
            End If

            Return name
        End Function

        ''' <summary>
        ''' Extracts public dependencies from direct dependencies. This is a static method despite its
        ''' first parameter, as the value we're in the middle of constructing is only used for exceptions.
        ''' </summary>
        Private Shared Function DeterminePublicDependencies(this As FileDescriptor, proto As FileDescriptorProto, dependencies As FileDescriptor(), allowUnknownDependencies As Boolean) As IList(Of FileDescriptor)
            Dim nameToFileMap = New Dictionary(Of String, FileDescriptor)()

            For Each file As FileDescriptor In dependencies
                nameToFileMap(file.Name) = file
            Next

            Dim publicDependencies = New List(Of FileDescriptor)()

            For i = 0 To proto.PublicDependency.Count - 1
                Dim index = proto.PublicDependency(i)

                If index < 0 OrElse index >= proto.Dependency.Count Then
                    Throw New DescriptorValidationException(this, "Invalid public dependency index.")
                End If

                Dim name = proto.Dependency(index)
                Dim file = nameToFileMap(name)

                If file Is Nothing Then
                    If Not allowUnknownDependencies Then
                        Throw New DescriptorValidationException(this, "Invalid public dependency: " & name)
                        ' Ignore unknown dependencies.
                    End If
                Else
                    publicDependencies.Add(file)
                End If
            Next

            Return New ReadOnlyCollection(Of FileDescriptor)(publicDependencies)
        End Function

        ''' <value>
        ''' The descriptor in its protocol message representation.
        ''' </value>
        Friend ReadOnly Property Proto As FileDescriptorProto

        ''' <value>
        ''' The file name.
        ''' </value>
        Public ReadOnly Property Name As String Implements IDescriptor.Name
            Get
                Return Proto.Name
            End Get
        End Property

        ''' <summary>
        ''' The package as declared in the .proto file. This may or may not
        ''' be equivalent to the .NET namespace of the generated classes.
        ''' </summary>
        Public ReadOnly Property Package As String
            Get
                Return Proto.Package
            End Get
        End Property

        ''' <value>
        ''' Unmodifiable list of top-level message types declared in this file.
        ''' </value>
        Public ReadOnly Property MessageTypes As IList(Of MessageDescriptor)

        ''' <value>
        ''' Unmodifiable list of top-level enum types declared in this file.
        ''' </value>
        Public ReadOnly Property EnumTypes As IList(Of EnumDescriptor)

        ''' <value>
        ''' Unmodifiable list of top-level services declared in this file.
        ''' </value>
        Public ReadOnly Property Services As IList(Of ServiceDescriptor)

        ''' <value>
        ''' Unmodifiable list of this file's dependencies (imports).
        ''' </value>
        Public ReadOnly Property Dependencies As IList(Of FileDescriptor)

        ''' <value>
        ''' Unmodifiable list of this file's public dependencies (public imports).
        ''' </value>
        Public ReadOnly Property PublicDependencies As IList(Of FileDescriptor)

        ''' <value>
        ''' The original serialized binary form of this descriptor.
        ''' </value>
        Public ReadOnly Property SerializedData As ByteString

        ''' <value>
        ''' Implementation of IDescriptor.FullName - just returns the same as Name.
        ''' </value>
        Private ReadOnly Property FullName As String Implements IDescriptor.FullName
            Get
                Return Name
            End Get
        End Property

        ''' <value>
        ''' Implementation of IDescriptor.File - just returns this descriptor.
        ''' </value>
        Private ReadOnly Property File As FileDescriptor Implements IDescriptor.File
            Get
                Return Me
            End Get
        End Property

        ''' <value>
        ''' Pool containing symbol descriptors.
        ''' </value>
        Friend ReadOnly Property DescriptorPool As DescriptorPool

        ''' <summary>
        ''' Finds a type (message, enum, service or extension) in the file by name. Does not find nested types.
        ''' </summary>
        ''' <param name="name">The unqualified type name to look for.</param>
        ''' <typeparam name="T">The type of descriptor to look for</typeparam>
        ''' <returns>The type's descriptor, or null if not found.</returns>
        Public Function FindTypeByName(Of T As {Class, IDescriptor})(name As String) As T
            ' Don't allow looking up nested types.  This will make optimization
            ' easier later.
            If name.IndexOf("."c) <> -1 Then
                Return Nothing
            End If

            If Package.Length > 0 Then
                name = Package & "." & name
            End If

            Dim result = DescriptorPool.FindSymbol(Of T)(name)

            If result IsNot Nothing AndAlso result.File Is Me Then
                Return result
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' Builds a FileDescriptor from its protocol buffer representation.
        ''' </summary>
        ''' <param name="descriptorData">The original serialized descriptor data.
        ''' We have only limited proto2 support, so serializing FileDescriptorProto
        ''' would not necessarily give us this.</param>
        ''' <param name="proto">The protocol message form of the FileDescriptor.</param>
        ''' <param name="dependencies">FileDescriptors corresponding to all of the
        ''' file's dependencies, in the exact order listed in the .proto file. May be null,
        ''' in which case it is treated as an empty array.</param>
        ''' <param name="allowUnknownDependencies">Whether unknown dependencies are ignored (true) or cause an exception to be thrown (false).</param>
        ''' <param name="generatedCodeInfo">Details about generated code, for the purposes of reflection.</param>
        ''' <exception cref="DescriptorValidationException">If <paramrefname="proto"/> is not
        ''' a valid descriptor. This can occur for a number of reasons, such as a field
        ''' having an undefined type or because two messages were defined with the same name.</exception>
        Private Shared Function BuildFrom(descriptorData As ByteString, proto As FileDescriptorProto, dependencies As FileDescriptor(), allowUnknownDependencies As Boolean, generatedCodeInfo As GeneratedClrTypeInfo) As FileDescriptor
            ' Building descriptors involves two steps: translating and linking.
            ' In the translation step (implemented by FileDescriptor's
            ' constructor), we build an object tree mirroring the
            ' FileDescriptorProto's tree and put all of the descriptors into the
            ' DescriptorPool's lookup tables.  In the linking step, we look up all
            ' type references in the DescriptorPool, so that, for example, a
            ' FieldDescriptor for an embedded message contains a pointer directly
            ' to the Descriptor for that message's type.  We also detect undefined
            ' types in the linking step.
            If dependencies Is Nothing Then
                dependencies = New FileDescriptor(-1) {}
            End If

            Dim pool As DescriptorPool = New DescriptorPool(dependencies)
            Dim result As FileDescriptor = New FileDescriptor(descriptorData, proto, dependencies, pool, allowUnknownDependencies, generatedCodeInfo)

            ' Validate that the dependencies we've been passed (as FileDescriptors) are actually the ones we
            ' need.
            If dependencies.Length <> proto.Dependency.Count Then
                Throw New DescriptorValidationException(result, "Dependencies passed to FileDescriptor.BuildFrom() don't match " & "those listed in the FileDescriptorProto.")
            End If

            For i = 0 To proto.Dependency.Count - 1

                If Not Equals(dependencies(i).Name, proto.Dependency(i)) Then
                    Throw New DescriptorValidationException(result, "Dependencies passed to FileDescriptor.BuildFrom() don't match " & "those listed in the FileDescriptorProto. Expected: " & proto.Dependency(i) & " but was: " & dependencies(i).Name)
                End If
            Next

            result.CrossLink()
            Return result
        End Function

        Private Sub CrossLink()
            For Each message In MessageTypes
                message.CrossLink()
            Next

            For Each service In Services
                service.CrossLink()
            Next
        End Sub

        ''' <summary>
        ''' Creates a descriptor for generated code.
        ''' </summary>
        ''' <remarks>
        ''' This method is only designed to be used by the results of generating code with protoc,
        ''' which creates the appropriate dependencies etc. It has to be public because the generated
        ''' code is "external", but should not be called directly by end users.
        ''' </remarks>
        Public Shared Function FromGeneratedCode(descriptorData As Byte(), dependencies As FileDescriptor(), generatedCodeInfo As GeneratedClrTypeInfo) As FileDescriptor
            Dim proto As FileDescriptorProto

            Try
                proto = FileDescriptorProto.Parser.ParseFrom(descriptorData)
            Catch e As InvalidProtocolBufferException
                Throw New ArgumentException("Failed to parse protocol buffer descriptor for generated code.", e)
            End Try

            Try
                ' When building descriptors for generated code, we allow unknown
                ' dependencies by default.
                Return BuildFrom(ByteString.CopyFrom(descriptorData), proto, dependencies, True, generatedCodeInfo)
            Catch e As DescriptorValidationException
                Throw New ArgumentException($"Invalid embedded descriptor for ""{proto.Name}"".", e)
            End Try
        End Function

        ''' <summary>
        ''' Returns a <see cref="System.String"/> that represents this instance.
        ''' </summary>
        ''' <returns>
        ''' A <see cref="System.String"/> that represents this instance.
        ''' </returns>
        Public Overrides Function ToString() As String
            Return $"FileDescriptor for {Name}"
        End Function

        ''' <summary>
        ''' Returns the file descriptor for descriptor.proto.
        ''' </summary>
        ''' <remarks>
        ''' This is used for protos which take a direct dependency on <c>descriptor.proto</c>, typically for
        ''' annotations. While <c>descriptor.proto</c> is a proto2 file, it is built into the Google.Protobuf
        ''' runtime for reflection purposes. The messages are internal to the runtime as they would require
        ''' proto2 semantics for full support, but the file descriptor is available via this property. The
        ''' C# codegen in protoc automatically uses this property when it detects a dependency on <c>descriptor.proto</c>.
        ''' </remarks>
        ''' <value>
        ''' The file descriptor for <c>descriptor.proto</c>.
        ''' </value>
        Public Shared ReadOnly Property DescriptorProtoFileDescriptor As FileDescriptor
            Get
                Return Descriptor
            End Get
        End Property
    End Class
End Namespace
