﻿#Region "Microsoft.VisualBasic::ff74a872a0976f4912ffe849c17f1b86, src\message\Google.Protobuf\Compatibility\TypeExtensions.vb"

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

    '   Total Lines: 109
    '    Code Lines: 38 (34.86%)
    ' Comment Lines: 58 (53.21%)
    '    - Xml Docs: 46.55%
    ' 
    '   Blank Lines: 13 (11.93%)
    '     File Size: 5.29 KB


    '     Module TypeExtensions
    ' 
    '         Function: GetMethod, GetProperty, IsAssignableFrom
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

Imports System.Reflection
Imports System.Runtime.CompilerServices

#If Not DOTNET35 Then

Namespace Google.Protobuf.Compatibility

    ''' <summary>
    ''' Provides extension methods on Type that just proxy to TypeInfo.
    ''' These are used to support the new type system from .NET 4.5, without
    ''' having calls to GetTypeInfo all over the place. While the methods here are meant to be
    ''' broadly compatible with the desktop framework, there are some subtle differences in behaviour - but
    ''' they're not expected to affect our use cases. While the class is internal, that should be fine: we can
    ''' evaluate each new use appropriately.
    ''' </summary>
    Friend Module TypeExtensions

        ''' <summary>
        ''' See https://msdn.microsoft.com/en-us/library/system.type.isassignablefrom
        ''' </summary>
        <Extension()>
        Friend Function IsAssignableFrom(target As Type, c As Type) As Boolean
            Return target.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo())
        End Function

        ''' <summary>
        ''' Returns a representation of the public property associated with the given name in the given type,
        ''' including inherited properties or null if there is no such public property.
        ''' Here, "public property" means a property where either the getter, or the setter, or both, is public.
        ''' </summary>
        <Extension()>
        Friend Function GetProperty(target As Type, name As String) As PropertyInfo
            ' GetDeclaredProperty only returns properties declared in the given type, so we need to recurse.
            While target IsNot Nothing
                Dim typeInfo = target.GetTypeInfo()
                Dim ret = typeInfo.GetDeclaredProperty(name)

                If ret IsNot Nothing AndAlso (ret.CanRead AndAlso ret.GetMethod.IsPublic OrElse ret.CanWrite AndAlso ret.SetMethod.IsPublic) Then
                    Return ret
                End If

                target = typeInfo.BaseType
            End While

            Return Nothing
        End Function

        ''' <summary>
        ''' Returns a representation of the public method associated with the given name in the given type,
        ''' including inherited methods.
        ''' </summary>
        ''' <remarks>
        ''' This has a few differences compared with Type.GetMethod in the desktop framework. It will throw
        ''' if there is an ambiguous match even between a private method and a public one, but it *won't* throw
        ''' if there are two overloads at different levels in the type hierarchy (e.g. class Base declares public void Foo(int) and
        ''' class Child : Base declares public void Foo(long)).
        ''' </remarks>
        ''' <exception cref="AmbiguousMatchException">One type in the hierarchy declared more than one method with the same name</exception>
        <Extension()>
        Friend Function GetMethod(target As Type, name As String) As MethodInfo
            ' GetDeclaredMethod only returns methods declared in the given type, so we need to recurse.
            While target IsNot Nothing
                Dim typeInfo = target.GetTypeInfo()
                Dim ret = typeInfo.GetDeclaredMethod(name)

                If ret IsNot Nothing AndAlso ret.IsPublic Then
                    Return ret
                End If

                target = typeInfo.BaseType
            End While

            Return Nothing
        End Function
    End Module
End Namespace
#End If
