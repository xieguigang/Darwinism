#Region "Microsoft.VisualBasic::1f0659d1e7ffa313eb51f3c230bcd0b7, Google.Protobuf\WellKnownTypes\Timestamp\TimeExtensions.vb"

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

    '     Module TimeExtensions
    ' 
    '         Function: ToDuration, (+2 Overloads) ToTimestamp
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
Imports System.Runtime.CompilerServices

Namespace Google.Protobuf.WellKnownTypes
    ''' <summary>
    ''' Extension methods on BCL time-related types, converting to protobuf types.
    ''' </summary>
    Public Module TimeExtensions
        ''' <summary>
        ''' Converts the given <see cref="DateTime"/> to a <see cref="Timestamp"/>.
        ''' </summary>
        ''' <param name="dateTime">The date and time to convert to a timestamp.</param>
        ''' <exception cref="ArgumentException">The <paramrefname="dateTime"/> value has a <see cref="DateTime.Kind"/>other than <c>Utc</c>.</exception>
        ''' <returns>The converted timestamp.</returns>
        <Extension()>
        Public Function ToTimestamp(dateTime As Date) As Timestamp
            Return Timestamp.FromDateTime(dateTime)
        End Function

        ''' <summary>
        ''' Converts the given <see cref="DateTimeOffset"/> to a <see cref="Timestamp"/>
        ''' </summary>
        ''' <remarks>The offset is taken into consideration when converting the value (so the same instant in time
        ''' is represented) but is not a separate part of the resulting value. In other words, there is no
        ''' roundtrip operation to retrieve the original <c>DateTimeOffset</c>.</remarks>
        ''' <param name="dateTimeOffset">The date and time (with UTC offset) to convert to a timestamp.</param>
        ''' <returns>The converted timestamp.</returns>
        <Extension()>
        Public Function ToTimestamp(dateTimeOffset As DateTimeOffset) As Timestamp
            Return Timestamp.FromDateTimeOffset(dateTimeOffset)
        End Function

        ''' <summary>
        ''' Converts the given <see cref="TimeSpan"/> to a <see cref="Duration"/>.
        ''' </summary>
        ''' <param name="timeSpan">The time span to convert.</param>
        ''' <returns>The converted duration.</returns>
        <Extension()>
        Public Function ToDuration(timeSpan As TimeSpan) As Duration
            Return Duration.FromTimeSpan(timeSpan)
        End Function
    End Module
End Namespace

