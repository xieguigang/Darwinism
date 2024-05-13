#Region "Microsoft.VisualBasic::03f99d874addc6e598a646433d613f85, src\message\Google.Protobuf\JSON\JsonToken.vb"

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

    '   Total Lines: 206
    '    Code Lines: 145
    ' Comment Lines: 37
    '   Blank Lines: 24
    '     File Size: 8.21 KB


    '     Class JsonToken
    ' 
    '         Properties: [False], [True], EndArray, EndDocument, EndObject
    '                     Null, StartArray, StartObject
    ' 
    '         Function: Name, (+2 Overloads) Value
    '         Enum TokenType
    ' 
    '             [False], [True], EndArray, EndDocument, EndObject
    '             Name, Null, Number, StartArray, StartObject
    '             StringValue
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: NumberValue, StringValue, Type
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: (+2 Overloads) Equals, GetHashCode, ToString
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

Namespace Google.Protobuf
    Friend NotInheritable Class JsonToken
        Implements IEquatable(Of JsonToken)
        ' Tokens with no value can be reused.
        Private Shared ReadOnly _true As JsonToken = New JsonToken(TokenType.True)
        Private Shared ReadOnly _false As JsonToken = New JsonToken(TokenType.False)
        Private Shared ReadOnly _null As JsonToken = New JsonToken(TokenType.Null)
        Private Shared ReadOnly startObjectField As JsonToken = New JsonToken(TokenType.StartObject)
        Private Shared ReadOnly endObjectField As JsonToken = New JsonToken(TokenType.EndObject)
        Private Shared ReadOnly startArrayField As JsonToken = New JsonToken(TokenType.StartArray)
        Private Shared ReadOnly endArrayField As JsonToken = New JsonToken(TokenType.EndArray)
        Private Shared ReadOnly endDocumentField As JsonToken = New JsonToken(TokenType.EndDocument)

        Friend Shared ReadOnly Property Null As JsonToken
            Get
                Return _null
            End Get
        End Property

        Friend Shared ReadOnly Property [False] As JsonToken
            Get
                Return _false
            End Get
        End Property

        Friend Shared ReadOnly Property [True] As JsonToken
            Get
                Return _true
            End Get
        End Property

        Friend Shared ReadOnly Property StartObject As JsonToken
            Get
                Return startObjectField
            End Get
        End Property

        Friend Shared ReadOnly Property EndObject As JsonToken
            Get
                Return endObjectField
            End Get
        End Property

        Friend Shared ReadOnly Property StartArray As JsonToken
            Get
                Return startArrayField
            End Get
        End Property

        Friend Shared ReadOnly Property EndArray As JsonToken
            Get
                Return endArrayField
            End Get
        End Property

        Friend Shared ReadOnly Property EndDocument As JsonToken
            Get
                Return endDocumentField
            End Get
        End Property

        Friend Shared Function Name(pName As String) As JsonToken
            Return New JsonToken(TokenType.Name, stringValue:=pName)
        End Function

        Friend Shared Function Value(pValue As String) As JsonToken
            Return New JsonToken(TokenType.StringValue, stringValue:=pValue)
        End Function

        Friend Shared Function Value(pValue As Double) As JsonToken
            Return New JsonToken(TokenType.Number, numberValue:=pValue)
        End Function

        Friend Enum TokenType
            Null
            [False]
            [True]
            StringValue
            Number
            Name
            StartObject
            EndObject
            StartArray
            EndArray
            EndDocument
        End Enum

        ' A value is a string, number, array, object, null, true or false
        ' Arrays and objects have start/end
        ' A document consists of a value
        ' Objects are name/value sequences.

        Private ReadOnly typeField As TokenType
        Private ReadOnly stringValueField As String
        Private ReadOnly numberValueField As Double

        Friend ReadOnly Property Type As TokenType
            Get
                Return typeField
            End Get
        End Property

        Friend ReadOnly Property StringValue As String
            Get
                Return stringValueField
            End Get
        End Property

        Friend ReadOnly Property NumberValue As Double
            Get
                Return numberValueField
            End Get
        End Property

        Private Sub New(type As TokenType, Optional stringValue As String = Nothing, Optional numberValue As Double = 0)
            typeField = type
            stringValueField = stringValue
            numberValueField = numberValue
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            Return Equals(TryCast(obj, JsonToken))
        End Function

        Public Overrides Function GetHashCode() As Integer
            ' BEGIN TODO : Visual Basic does not support checked statements!
            Dim hash = 17
            hash = hash * 31 + typeField
            hash = If(Equals(hash * 31 & stringValueField, Nothing), 0, stringValueField.GetHashCode())
            hash = hash * 31 + numberValueField.GetHashCode()
            Return hash
            'END TODO : Visual Basic does not support checked statements!
        End Function

        Public Overrides Function ToString() As String
            Select Case typeField
                Case TokenType.Null
                    Return "null"
                Case TokenType.True
                    Return "true"
                Case TokenType.False
                    Return "false"
                Case TokenType.Name
                    Return "name (" & stringValueField & ")"
                Case TokenType.StringValue
                    Return "value (" & stringValueField & ")"
                Case TokenType.Number
                    Return "number (" & numberValueField & ")"
                Case TokenType.StartObject
                    Return "start-object"
                Case TokenType.EndObject
                    Return "end-object"
                Case TokenType.StartArray
                    Return "start-array"
                Case TokenType.EndArray
                    Return "end-array"
                Case TokenType.EndDocument
                    Return "end-document"
                Case Else
                    Throw New InvalidOperationException("Token is of unknown type " & typeField)
            End Select
        End Function

        Public Overloads Function Equals(other As JsonToken) As Boolean Implements IEquatable(Of JsonToken).Equals
            If ReferenceEquals(other, Nothing) Then
                Return False
            End If
            ' Note use of other.numberValue.Equals rather than ==, so that NaN compares appropriately.
            Return other.typeField = typeField AndAlso Equals(other.stringValueField, stringValueField) AndAlso other.numberValueField.Equals(numberValueField)
        End Function
    End Class
End Namespace
