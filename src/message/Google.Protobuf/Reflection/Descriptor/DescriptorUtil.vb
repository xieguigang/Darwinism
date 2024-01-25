﻿#Region "Microsoft.VisualBasic::d5ae1af97d3c1973299b228dc60a121b, Google.Protobuf\Reflection\Descriptor\DescriptorUtil.vb"

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

    '     Module DescriptorUtil
    ' 
    ' 
    '         Delegate Function
    ' 
    '             Function: ConvertAndMakeReadOnly
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

Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Namespace Google.Protobuf.Reflection
    ''' <summary>
    ''' Internal class containing utility methods when working with descriptors.
    ''' </summary>
    Friend Module DescriptorUtil
        ''' <summary>
        ''' Equivalent to Func[TInput, int, TOutput] but usable in .NET 2.0. Only used to convert
        ''' arrays.
        ''' </summary>
        Friend Delegate Function IndexedConverter(Of TInput, TOutput)(element As TInput, index As Integer) As TOutput

        ''' <summary>
        ''' Converts the given array into a read-only list, applying the specified conversion to
        ''' each input element.
        ''' </summary>
        Friend Function ConvertAndMakeReadOnly(Of TInput, TOutput)(input As IList(Of TInput), converter As IndexedConverter(Of TInput, TOutput)) As IList(Of TOutput)
            Dim array = New TOutput(input.Count - 1) {}

            For i = 0 To array.Length - 1
                array(i) = converter(input(i), i)
            Next

            Return New ReadOnlyCollection(Of TOutput)(array)
        End Function
    End Module
End Namespace
