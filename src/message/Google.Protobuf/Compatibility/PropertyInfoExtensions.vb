#Region "Microsoft.VisualBasic::02c486a858d937b0b87c48a9479a274d, src\message\Google.Protobuf\Compatibility\PropertyInfoExtensions.vb"

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

    '   Total Lines: 73
    '    Code Lines: 26 (35.62%)
    ' Comment Lines: 42 (57.53%)
    '    - Xml Docs: 30.95%
    ' 
    '   Blank Lines: 5 (6.85%)
    '     File Size: 3.24 KB


    '     Module PropertyInfoExtensions
    ' 
    '         Function: GetGetMethod, GetSetMethod
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

Namespace Google.Protobuf.Compatibility

    ''' <summary>
    ''' Extension methods for <see cref="PropertyInfo"/>, effectively providing
    ''' the familiar members from previous desktop framework versions while
    ''' targeting the newer releases, .NET Core etc.
    ''' </summary>
    Friend Module PropertyInfoExtensions

        ''' <summary>
        ''' Returns the public getter of a property, or null if there is no such getter
        ''' (either because it's read-only, or the getter isn't public).
        ''' </summary>
        <Extension()>
        Friend Function GetGetMethod(target As PropertyInfo) As MethodInfo
#If DOTNET35 Then
            Dim method = target.GetGetMethod()
#Else
            Dim method = target.GetMethod
#End If
            Return If(method IsNot Nothing AndAlso method.IsPublic, method, Nothing)
        End Function

        ''' <summary>
        ''' Returns the public setter of a property, or null if there is no such setter
        ''' (either because it's write-only, or the setter isn't public).
        ''' </summary>
        <Extension()>
        Friend Function GetSetMethod(target As PropertyInfo) As MethodInfo
#If DOTNET35 Then
            Dim method = target.GetSetMethod()
#Else
            Dim method = target.SetMethod
#End If
            Return If(method IsNot Nothing AndAlso method.IsPublic, method, Nothing)
        End Function
    End Module
End Namespace
