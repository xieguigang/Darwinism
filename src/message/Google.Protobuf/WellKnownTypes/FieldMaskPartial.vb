#Region "Microsoft.VisualBasic::d43117fdf9e7642ca094154e3bbcea1f, G:/GCModeller/src/runtime/Darwinism/src/message/Google.Protobuf//WellKnownTypes/FieldMaskPartial.vb"

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

    '   Total Lines: 119
    '    Code Lines: 53
    ' Comment Lines: 56
    '   Blank Lines: 10
    '     File Size: 5.89 KB


    '     Class FieldMask
    ' 
    '         Function: ToDiagnosticString, ToJson, ValidatePath
    ' 
    ' 
    ' /********************************************************************************/

#End Region

#Region "Copyright notice and license"
' Protocol Buffers - Google's data interchange format
' Copyright 2016 Google Inc.  All rights reserved.
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
Imports System.Collections
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace Google.Protobuf.WellKnownTypes
    ' Manually-written partial class for the FieldMask well-known type.
    Public Partial Class FieldMask
        Implements ICustomDiagnosticMessage
        ''' <summary>
        ''' Converts a timestamp  specified in seconds/nanoseconds to a string.
        ''' </summary>
        ''' <remarks>
        ''' If the value is a normalized duration in the range described in <c>field_mask.proto</c>,
        ''' <paramrefname="diagnosticOnly"/> is ignored. Otherwise, if the parameter is <c>true</c>,
        ''' a JSON object with a warning is returned; if it is <c>false</c>, an <see cref="InvalidOperationException"/> is thrown.
        ''' </remarks>
        ''' <param name="paths">Paths in the field mask</param>
        ''' <param name="diagnosticOnly">Determines the handling of non-normalized values</param>
        ''' <exception cref="InvalidOperationException">The represented duration is invalid, and <paramrefname="diagnosticOnly"/> is <c>false</c>.</exception>
        Friend Shared Function ToJson(paths As IList(Of String), diagnosticOnly As Boolean) As String
            Dim firstInvalid = paths.FirstOrDefault(Function(p) Not ValidatePath(p))

            If Equals(firstInvalid, Nothing) Then
                Dim writer = New StringWriter()
#If DOTNET35
                var query = paths.Select(JsonFormatter.ToCamelCase);
                JsonFormatter.WriteString(writer, string.Join(",", query.ToArray()));
#Else
                JsonFormatter.WriteString(writer, String.Join(",", paths.[Select](New Func(Of String, String)(AddressOf JsonFormatter.ToCamelCase))))
#End If
                Return writer.ToString()
            Else

                If diagnosticOnly Then
                    Dim writer = New StringWriter()
                    writer.Write("{ ""@warning"": ""Invalid FieldMask"", ""paths"": ")
                    JsonFormatter.Default.WriteList(writer, CType(paths, IList))
                    writer.Write(" }")
                    Return writer.ToString()
                Else
                    Throw New InvalidOperationException($"Invalid field mask to be converted to JSON: {firstInvalid}")
                End If
            End If
        End Function

        ''' <summary>
        ''' Camel-case converter with added strictness for field mask formatting.
        ''' </summary>
        ''' <exception cref="InvalidOperationException">The field mask is invalid for JSON representation</exception>
        Private Shared Function ValidatePath(input As String) As Boolean
            For i = 0 To input.Length - 1
                Dim c = input(i)

                If c >= "A"c AndAlso c <= "Z"c Then
                    Return False
                End If

                If c = "_"c AndAlso i < input.Length - 1 Then
                    Dim [next] = input(i + 1)

                    If [next] < "a"c OrElse [next] > "z"c Then
                        Return False
                    End If
                End If
            Next

            Return True
        End Function

        ''' <summary>
        ''' Returns a string representation of this <see cref="FieldMask"/> for diagnostic purposes.
        ''' </summary>
        ''' <remarks>
        ''' Normally the returned value will be a JSON string value (including leading and trailing quotes) but
        ''' when the value is non-normalized or out of range, a JSON object representation will be returned
        ''' instead, including a warning. This is to avoid exceptions being thrown when trying to
        ''' diagnose problems - the regular JSON formatter will still throw an exception for non-normalized
        ''' values.
        ''' </remarks>
        ''' <returns>A string representation of this value.</returns>
        Public Function ToDiagnosticString() As String Implements ICustomDiagnosticMessage.ToDiagnosticString
            Return ToJson(Paths, True)
        End Function
    End Class
End Namespace
