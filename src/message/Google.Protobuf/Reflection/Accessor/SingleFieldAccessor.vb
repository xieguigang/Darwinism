#Region "Microsoft.VisualBasic::48d60a145fea9f2a40937ed3ec20d7ba, src\message\Google.Protobuf\Reflection\Accessor\SingleFieldAccessor.vb"

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
    '    Code Lines: 27 (36.99%)
    ' Comment Lines: 37 (50.68%)
    '    - Xml Docs: 8.11%
    ' 
    '   Blank Lines: 9 (12.33%)
    '     File Size: 3.62 KB


    '     Class SingleFieldAccessor
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: Clear, SetValue
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
    ''' Accessor for single fields.
    ''' </summary>
    Friend NotInheritable Class SingleFieldAccessor
        Inherits FieldAccessorBase
        ' All the work here is actually done in the constructor - it creates the appropriate delegates.
        ' There are various cases to consider, based on the property type (message, string/bytes, or "genuine" primitive)
        ' and proto2 vs proto3 for non-message types, as proto3 doesn't support "full" presence detection or default
        ' values.

        Private ReadOnly setValueDelegate As Action(Of IMessage, Object)
        Private ReadOnly clearDelegate As Action(Of IMessage)

        Friend Sub New([property] As PropertyInfo, descriptor As FieldDescriptor)
            MyBase.New([property], descriptor)

            If Not [property].CanWrite Then
                Throw New ArgumentException("Not all required properties/methods available")
            End If

            setValueDelegate = CreateActionIMessageObject([property].GetSetMethod())
            Dim clrType = [property].PropertyType

            ' TODO: Validate that this is a reasonable single field? (Should be a value type, a message type, or string/ByteString.)
            Dim defaultValue = If(descriptor.FieldType = FieldType.Message, Nothing, If(clrType Is GetType(String), "", If(clrType Is GetType(ByteString), ByteString.Empty, Activator.CreateInstance(clrType))))
            clearDelegate = Sub(message) SetValue(message, defaultValue)
        End Sub

        Public Overrides Sub Clear(message As IMessage)
            clearDelegate(message)
        End Sub

        Public Overrides Sub SetValue(message As IMessage, value As Object)
            setValueDelegate(message, value)
        End Sub
    End Class
End Namespace
