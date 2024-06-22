#Region "Microsoft.VisualBasic::30aae81439e9e8cfab7b3b6897c8f3bf, src\message\Google.Protobuf\Reflection\Descriptor\DescriptorPool.vb"

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

    '   Total Lines: 318
    '    Code Lines: 173 (54.40%)
    ' Comment Lines: 95 (29.87%)
    '    - Xml Docs: 55.79%
    ' 
    '   Blank Lines: 50 (15.72%)
    '     File Size: 14.62 KB


    '     Class DescriptorPool
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: FindEnumValueByNumber, FindFieldByNumber, FindSymbol, LookupSymbol
    ' 
    '         Sub: AddEnumValueByNumber, AddFieldByNumber, AddPackage, AddSymbol, ImportPublicDependencies
    '              ValidateSymbolName
    '         Structure DescriptorIntPair
    ' 
    '             Constructor: (+1 Overloads) Sub New
    '             Function: (+2 Overloads) Equals, GetHashCode
    ' 
    ' 
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
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Contains lookup tables containing all the descriptors defined in a particular file.
    ''' </summary>
    Friend NotInheritable Class DescriptorPool
        Private ReadOnly descriptorsByName As IDictionary(Of String, IDescriptor) = New Dictionary(Of String, IDescriptor)()
        Private ReadOnly fieldsByNumber As IDictionary(Of DescriptorIntPair, FieldDescriptor) = New Dictionary(Of DescriptorIntPair, FieldDescriptor)()
        Private ReadOnly enumValuesByNumber As IDictionary(Of DescriptorIntPair, EnumValueDescriptor) = New Dictionary(Of DescriptorIntPair, EnumValueDescriptor)()
        Private ReadOnly dependencies As HashSet(Of FileDescriptor)

        Friend Sub New(dependencyFiles As FileDescriptor())
            dependencies = New HashSet(Of FileDescriptor)()

            For i = 0 To dependencyFiles.Length - 1
                dependencies.Add(dependencyFiles(i))
                ImportPublicDependencies(dependencyFiles(i))
            Next

            For Each dependency In dependencyFiles
                AddPackage(dependency.Package, dependency)
            Next
        End Sub

        Private Sub ImportPublicDependencies(file As FileDescriptor)
            For Each dependency In file.PublicDependencies

                If dependencies.Add(dependency) Then
                    ImportPublicDependencies(dependency)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Finds a symbol of the given name within the pool.
        ''' </summary>
        ''' <typeparam name="T">The type of symbol to look for</typeparam>
        ''' <param name="fullName">Fully-qualified name to look up</param>
        ''' <returns>The symbol with the given name and type,
        ''' or null if the symbol doesn't exist or has the wrong type</returns>
        Friend Function FindSymbol(Of T As Class)(fullName As String) As T
            Dim result As IDescriptor
            descriptorsByName.TryGetValue(fullName, result)
            Dim descriptor As T = TryCast(result, T)

            If descriptor IsNot Nothing Then
                Return descriptor
            End If

            ' dependencies contains direct dependencies and any *public* dependencies
            ' of those dependencies (transitively)... so we don't need to recurse here.
            For Each dependency In dependencies
                dependency.DescriptorPool.descriptorsByName.TryGetValue(fullName, result)
                descriptor = TryCast(result, T)

                If descriptor IsNot Nothing Then
                    Return descriptor
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Adds a package to the symbol tables. If a package by the same name
        ''' already exists, that is fine, but if some other kind of symbol
        ''' exists under the same name, an exception is thrown. If the package
        ''' has multiple components, this also adds the parent package(s).
        ''' </summary>
        Friend Sub AddPackage(fullName As String, file As FileDescriptor)
            Dim dotpos = fullName.LastIndexOf("."c)
            Dim name As String

            If dotpos <> -1 Then
                AddPackage(fullName.Substring(0, dotpos), file)
                name = fullName.Substring(dotpos + 1)
            Else
                name = fullName
            End If

            Dim old As IDescriptor

            If descriptorsByName.TryGetValue(fullName, old) Then
                If Not (TypeOf old Is PackageDescriptor) Then
                    Throw New DescriptorValidationException(file, """" & name & """ is already defined (as something other than a " & "package) in file """ & old.File.Name & """.")
                End If
            End If

            descriptorsByName(fullName) = New PackageDescriptor(name, fullName, file)
        End Sub

        ''' <summary>
        ''' Adds a symbol to the symbol table.
        ''' </summary>
        ''' <exception cref="DescriptorValidationException">The symbol already existed
        ''' in the symbol table.</exception>
        Friend Sub AddSymbol(descriptor As IDescriptor)
            ValidateSymbolName(descriptor)
            Dim fullName = descriptor.FullName
            Dim old As IDescriptor

            If descriptorsByName.TryGetValue(fullName, old) Then
                Dim dotPos = fullName.LastIndexOf("."c)
                Dim message As String

                If descriptor.File Is old.File Then
                    If dotPos = -1 Then
                        message = """" & fullName & """ is already defined."
                    Else
                        message = """" & fullName.Substring(dotPos + 1) & """ is already defined in """ & fullName.Substring(0, dotPos) & """."
                    End If
                Else
                    message = """" & fullName & """ is already defined in file """ & old.File.Name & """."
                End If

                Throw New DescriptorValidationException(descriptor, message)
            End If

            descriptorsByName(fullName) = descriptor
        End Sub

        Private Shared ReadOnly ValidationRegex As Regex = New Regex("^[_A-Za-z][_A-Za-z0-9]*$", CompiledRegexWhereAvailable)

        ''' <summary>
        ''' Verifies that the descriptor's name is valid (i.e. it contains
        ''' only letters, digits and underscores, and does not start with a digit).
        ''' </summary>
        ''' <param name="descriptor"></param>
        Private Shared Sub ValidateSymbolName(descriptor As IDescriptor)
            If Equals(descriptor.Name, "") Then
                Throw New DescriptorValidationException(descriptor, "Missing name.")
            End If

            If Not ValidationRegex.IsMatch(descriptor.Name) Then
                Throw New DescriptorValidationException(descriptor, """" & descriptor.Name & """ is not a valid identifier.")
            End If
        End Sub

        ''' <summary>
        ''' Returns the field with the given number in the given descriptor,
        ''' or null if it can't be found.
        ''' </summary>
        Friend Function FindFieldByNumber(messageDescriptor As MessageDescriptor, number As Integer) As FieldDescriptor
            Dim ret As FieldDescriptor
            fieldsByNumber.TryGetValue(New DescriptorIntPair(messageDescriptor, number), ret)
            Return ret
        End Function

        Friend Function FindEnumValueByNumber(enumDescriptor As EnumDescriptor, number As Integer) As EnumValueDescriptor
            Dim ret As EnumValueDescriptor
            enumValuesByNumber.TryGetValue(New DescriptorIntPair(enumDescriptor, number), ret)
            Return ret
        End Function

        ''' <summary>
        ''' Adds a field to the fieldsByNumber table.
        ''' </summary>
        ''' <exception cref="DescriptorValidationException">A field with the same
        ''' containing type and number already exists.</exception>
        Friend Sub AddFieldByNumber(field As FieldDescriptor)
            Dim key As DescriptorIntPair = New DescriptorIntPair(field.ContainingType, field.FieldNumber)
            Dim old As FieldDescriptor

            If fieldsByNumber.TryGetValue(key, old) Then
                Throw New DescriptorValidationException(field, "Field number " & field.FieldNumber & "has already been used in """ & field.ContainingType.FullName & """ by field """ & old.Name & """.")
            End If

            fieldsByNumber(key) = field
        End Sub

        ''' <summary>
        ''' Adds an enum value to the enumValuesByNumber table. If an enum value
        ''' with the same type and number already exists, this method does nothing.
        ''' (This is allowed; the first value defined with the number takes precedence.)
        ''' </summary>
        Friend Sub AddEnumValueByNumber(enumValue As EnumValueDescriptor)
            Dim key As DescriptorIntPair = New DescriptorIntPair(enumValue.EnumDescriptor, enumValue.Number)

            If Not enumValuesByNumber.ContainsKey(key) Then
                enumValuesByNumber(key) = enumValue
            End If
        End Sub

        ''' <summary>
        ''' Looks up a descriptor by name, relative to some other descriptor.
        ''' The name may be fully-qualified (with a leading '.'), partially-qualified,
        ''' or unqualified. C++-like name lookup semantics are used to search for the
        ''' matching descriptor.
        ''' </summary>
        ''' <remarks>
        ''' This isn't heavily optimized, but it's only used during cross linking anyway.
        ''' If it starts being used more widely, we should look at performance more carefully.
        ''' </remarks>
        Friend Function LookupSymbol(name As String, relativeTo As IDescriptor) As IDescriptor
            Dim result As IDescriptor

            If name.StartsWith(".") Then
                ' Fully-qualified name.
                result = FindSymbol(Of IDescriptor)(name.Substring(1))
            Else
                ' If "name" is a compound identifier, we want to search for the
                ' first component of it, then search within it for the rest.
                Dim firstPartLength = name.IndexOf("."c)
                Dim firstPart = If(firstPartLength = -1, name, name.Substring(0, firstPartLength))

                ' We will search each parent scope of "relativeTo" looking for the
                ' symbol.
                Dim scopeToTry As StringBuilder = New StringBuilder(relativeTo.FullName)

                While True
                    ' Chop off the last component of the scope.

                    Dim dotpos As Integer = scopeToTry.ToString().LastIndexOf(".")

                    If dotpos = -1 Then
                        result = FindSymbol(Of IDescriptor)(name)
                        Exit While
                    Else
                        scopeToTry.Length = dotpos + 1

                        ' Append firstPart and try to find.
                        scopeToTry.Append(firstPart)
                        result = FindSymbol(Of IDescriptor)(scopeToTry.ToString())

                        If result IsNot Nothing Then
                            If firstPartLength <> -1 Then
                                ' We only found the first part of the symbol.  Now look for
                                ' the whole thing.  If this fails, we *don't* want to keep
                                ' searching parent scopes.
                                scopeToTry.Length = dotpos + 1
                                scopeToTry.Append(name)
                                result = FindSymbol(Of IDescriptor)(scopeToTry.ToString())
                            End If

                            Exit While
                        End If

                        ' Not found.  Remove the name so we can try again.
                        scopeToTry.Length = dotpos
                    End If
                End While
            End If

            If result Is Nothing Then
                Throw New DescriptorValidationException(relativeTo, """" & name & """ is not defined.")
            Else
                Return result
            End If
        End Function

        ''' <summary>
        ''' Struct used to hold the keys for the fieldByNumber table.
        ''' </summary>
        Private Structure DescriptorIntPair
            Implements IEquatable(Of DescriptorIntPair)

            Private ReadOnly number As Integer
            Private ReadOnly descriptor As IDescriptor

            Friend Sub New(descriptor As IDescriptor, number As Integer)
                Me.number = number
                Me.descriptor = descriptor
            End Sub

            Public Overloads Function Equals(other As DescriptorIntPair) As Boolean Implements IEquatable(Of DescriptorIntPair).Equals
                Return descriptor Is other.descriptor AndAlso number = other.number
            End Function

            Public Overrides Function Equals(obj As Object) As Boolean
                If TypeOf obj Is DescriptorIntPair Then
                    Return Equals(CType(obj, DescriptorIntPair))
                End If

                Return False
            End Function

            Public Overrides Function GetHashCode() As Integer
                Return descriptor.GetHashCode() * ((1 << 16) - 1) + number
            End Function
        End Structure
    End Class
End Namespace
