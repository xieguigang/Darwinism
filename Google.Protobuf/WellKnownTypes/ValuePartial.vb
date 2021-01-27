#Region "Microsoft.VisualBasic::38acecf50c2f38152f7223817802f654, Google.Protobuf\WellKnownTypes\ValuePartial.vb"

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

    '     Class Value
    ' 
    '         Function: ForBool, ForList, ForNull, ForNumber, ForString
    '                   ForStruct
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

Namespace Google.Protobuf.WellKnownTypes
    Public Partial Class Value
        ''' <summary>
        ''' Convenience method to create a Value message with a string value.
        ''' </summary>
        ''' <param name="value">Value to set for the StringValue property.</param>
        ''' <returns>A newly-created Value message with the given value.</returns>
        Public Shared Function ForString(value As String) As Value
            CheckNotNull(value, "value")
            Return New Value With {
                .StringValue = value
            }
        End Function

        ''' <summary>
        ''' Convenience method to create a Value message with a number value.
        ''' </summary>
        ''' <param name="value">Value to set for the NumberValue property.</param>
        ''' <returns>A newly-created Value message with the given value.</returns>
        Public Shared Function ForNumber(value As Double) As Value
            Return New Value With {
                .NumberValue = value
            }
        End Function

        ''' <summary>
        ''' Convenience method to create a Value message with a Boolean value.
        ''' </summary>
        ''' <param name="value">Value to set for the BoolValue property.</param>
        ''' <returns>A newly-created Value message with the given value.</returns>
        Public Shared Function ForBool(value As Boolean) As Value
            Return New Value With {
                .BoolValue = value
            }
        End Function

        ''' <summary>
        ''' Convenience method to create a Value message with a null initial value.
        ''' </summary>
        ''' <returns>A newly-created Value message a null initial value.</returns>
        Public Shared Function ForNull() As Value
            Return New Value With {
                .NullValue = 0
            }
        End Function

        ''' <summary>
        ''' Convenience method to create a Value message with an initial list of values.
        ''' </summary>
        ''' <remarks>The values provided are not cloned; the references are copied directly.</remarks>
        ''' <returns>A newly-created Value message an initial list value.</returns>
        Public Shared Function ForList(ParamArray values As Value()) As Value
            CheckNotNull(values, "values")
            Return New Value With {.ListValue = New ListValue With {.values_ = New Collections.RepeatedField(Of Value)(values)}}
        End Function

        ''' <summary>
        ''' Convenience method to create a Value message with an initial struct value
        ''' </summary>
        ''' <remarks>The value provided is not cloned; the reference is copied directly.</remarks>
        ''' <returns>A newly-created Value message an initial struct value.</returns>
        Public Shared Function ForStruct(value As Struct) As Value
            CheckNotNull(value, "value")
            Return New Value With {
                .StructValue = value
            }
        End Function
    End Class
End Namespace

