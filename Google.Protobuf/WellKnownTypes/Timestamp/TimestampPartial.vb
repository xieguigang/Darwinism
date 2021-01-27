#Region "Microsoft.VisualBasic::fca3d0fedbef4a470e2eda59cda98968, Google.Protobuf\WellKnownTypes\Timestamp\TimestampPartial.vb"

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

    '     Class Timestamp
    ' 
    '         Function: FromDateTime, FromDateTimeOffset, IsNormalized, Normalize, ToDateTime
    '                   ToDateTimeOffset, ToDiagnosticString, ToJson
    '         Operators: (+2 Overloads) -, +
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
Imports System.Globalization
Imports System.Text

Namespace Google.Protobuf.WellKnownTypes
    Public Partial Class Timestamp
        Implements ICustomDiagnosticMessage

        Private Shared ReadOnly UnixEpoch As Date = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        ' Constants determined programmatically, but then hard-coded so they can be constant expressions.
        Private Const BclSecondsAtUnixEpoch As Long = 62135596800
        Friend Const UnixSecondsAtBclMaxValue As Long = 253402300799
        Friend Const UnixSecondsAtBclMinValue As Long = -BclSecondsAtUnixEpoch
        Friend Const MaxNanos As Integer = Duration.NanosecondsPerSecond - 1

        Private Shared Function IsNormalized(seconds As Long, nanoseconds As Integer) As Boolean
            Return nanoseconds >= 0 AndAlso nanoseconds <= MaxNanos AndAlso seconds >= UnixSecondsAtBclMinValue AndAlso seconds <= UnixSecondsAtBclMaxValue
        End Function

        ''' <summary>
        ''' Returns the difference between one <see cref="Timestamp"/> and another, as a <see cref="Duration"/>.
        ''' </summary>
        ''' <param name="lhs">The timestamp to subtract from. Must not be null.</param>
        ''' <param name="rhs">The timestamp to subtract. Must not be null.</param>
        ''' <returns>The difference between the two specified timestamps.</returns>
        Public Shared Operator -(lhs As Timestamp, rhs As Timestamp) As Duration
            CheckNotNull(lhs, "lhs")
            CheckNotNull(rhs, "rhs")
            'BEGIN TODO : Visual Basic does not support checked statements!
            Return Duration.Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos)
            'END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Adds a <see cref="Duration"/> to a <see cref="Timestamp"/>, to obtain another <c>Timestamp</c>.
        ''' </summary>
        ''' <param name="lhs">The timestamp to add the duration to. Must not be null.</param>
        ''' <param name="rhs">The duration to add. Must not be null.</param>
        ''' <returns>The result of adding the duration to the timestamp.</returns>
        Public Shared Operator +(lhs As Timestamp, rhs As Duration) As Timestamp
            CheckNotNull(lhs, "lhs")
            CheckNotNull(rhs, "rhs")
            'BEGIN TODO : Visual Basic does not support checked statements!
            Return Normalize(lhs.Seconds + rhs.Seconds, lhs.Nanos + rhs.Nanos)
            'END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Subtracts a <see cref="Duration"/> from a <see cref="Timestamp"/>, to obtain another <c>Timestamp</c>.
        ''' </summary>
        ''' <param name="lhs">The timestamp to subtract the duration from. Must not be null.</param>
        ''' <param name="rhs">The duration to subtract.</param>
        ''' <returns>The result of subtracting the duration from the timestamp.</returns>
        Public Shared Operator -(lhs As Timestamp, rhs As Duration) As Timestamp
            CheckNotNull(lhs, "lhs")
            CheckNotNull(rhs, "rhs")
            ' BEGIN TODO : Visual Basic does not support checked statements!
            Return Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos)
            ' END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Converts this timestamp into a <see cref="DateTime"/>.
        ''' </summary>
        ''' <remarks>
        ''' The resulting <c>DateTime</c> will always have a <c>Kind</c> of <c>Utc</c>.
        ''' If the timestamp is not a precise number of ticks, it will be truncated towards the start
        ''' of time. For example, a timestamp with a <see cref="Nanos"/> value of 99 will result in a
        ''' <see cref="DateTime"/> value precisely on a second.
        ''' </remarks>
        ''' <returns>This timestamp as a <c>DateTime</c>.</returns>
        ''' <exception cref="InvalidOperationException">The timestamp contains invalid values; either it is
        ''' incorrectly normalized or is outside the valid range.</exception>
        Public Function ToDateTime() As Date
            If Not IsNormalized(Seconds, Nanos) Then
                Throw New InvalidOperationException("Timestamp contains invalid values: Seconds={Seconds}; Nanos={Nanos}")
            End If

            Return UnixEpoch.AddSeconds(Seconds).AddTicks(Nanos / Duration.NanosecondsPerTick)
        End Function

        ''' <summary>
        ''' Converts this timestamp into a <see cref="DateTimeOffset"/>.
        ''' </summary>
        ''' <remarks>
        ''' The resulting <c>DateTimeOffset</c> will always have an <c>Offset</c> of zero.
        ''' If the timestamp is not a precise number of ticks, it will be truncated towards the start
        ''' of time. For example, a timestamp with a <see cref="Nanos"/> value of 99 will result in a
        ''' <see cref="DateTimeOffset"/> value precisely on a second.
        ''' </remarks>
        ''' <returns>This timestamp as a <c>DateTimeOffset</c>.</returns>
        ''' <exception cref="InvalidOperationException">The timestamp contains invalid values; either it is
        ''' incorrectly normalized or is outside the valid range.</exception>
        Public Function ToDateTimeOffset() As DateTimeOffset
            Return New DateTimeOffset(ToDateTime(), TimeSpan.Zero)
        End Function

        ''' <summary>
        ''' Converts the specified <see cref="DateTime"/> to a <see cref="Timestamp"/>.
        ''' </summary>
        ''' <param name="dateTime"></param>
        ''' <exception cref="ArgumentException">The <c>Kind</c> of <paramrefname="dateTime"/> is not <c>DateTimeKind.Utc</c>.</exception>
        ''' <returns>The converted timestamp.</returns>
        Public Shared Function FromDateTime(dateTime As Date) As Timestamp
            If dateTime.Kind <> DateTimeKind.Utc Then
                Throw New ArgumentException("Conversion from DateTime to Timestamp requires the DateTime kind to be Utc", "dateTime")
            End If
            ' Do the arithmetic using DateTime.Ticks, which is always non-negative, making things simpler.
            Dim secondsSinceBclEpoch As Long = dateTime.Ticks / TimeSpan.TicksPerSecond
            Dim nanoseconds = CInt(dateTime.Ticks Mod TimeSpan.TicksPerSecond) * Duration.NanosecondsPerTick
            Return New Timestamp With {
                .Seconds = secondsSinceBclEpoch - BclSecondsAtUnixEpoch,
                .Nanos = nanoseconds
            }
        End Function

        ''' <summary>
        ''' Converts the given <see cref="DateTimeOffset"/> to a <see cref="Timestamp"/>
        ''' </summary>
        ''' <remarks>The offset is taken into consideration when converting the value (so the same instant in time
        ''' is represented) but is not a separate part of the resulting value. In other words, there is no
        ''' roundtrip operation to retrieve the original <c>DateTimeOffset</c>.</remarks>
        ''' <param name="dateTimeOffset">The date and time (with UTC offset) to convert to a timestamp.</param>
        ''' <returns>The converted timestamp.</returns>
        Public Shared Function FromDateTimeOffset(dateTimeOffset As DateTimeOffset) As Timestamp
            ' We don't need to worry about this having negative ticks: DateTimeOffset is constrained to handle
            ' values whose *UTC* value is in the range of DateTime.
            Return FromDateTime(dateTimeOffset.UtcDateTime)
        End Function

        Friend Shared Function Normalize(seconds As Long, nanoseconds As Integer) As Timestamp
            Dim extraSeconds As Integer = nanoseconds / Duration.NanosecondsPerSecond
            seconds += extraSeconds
            nanoseconds -= extraSeconds * Duration.NanosecondsPerSecond

            If nanoseconds < 0 Then
                nanoseconds += Duration.NanosecondsPerSecond
                seconds -= 1
            End If

            Return New Timestamp With {
                .Seconds = seconds,
                .Nanos = nanoseconds
            }
        End Function

        ''' <summary>
        ''' Converts a timestamp specified in seconds/nanoseconds to a string.
        ''' </summary>
        ''' <remarks>
        ''' If the value is a normalized duration in the range described in <c>timestamp.proto</c>,
        ''' <paramrefname="diagnosticOnly"/> is ignored. Otherwise, if the parameter is <c>true</c>,
        ''' a JSON object with a warning is returned; if it is <c>false</c>, an <see cref="InvalidOperationException"/> is thrown.
        ''' </remarks>
        ''' <param name="seconds">Seconds portion of the duration.</param>
        ''' <param name="nanoseconds">Nanoseconds portion of the duration.</param>
        ''' <param name="diagnosticOnly">Determines the handling of non-normalized values</param>
        ''' <exception cref="InvalidOperationException">The represented duration is invalid, and <paramrefname="diagnosticOnly"/> is <c>false</c>.</exception>
        Friend Shared Function ToJson(seconds As Long, nanoseconds As Integer, diagnosticOnly As Boolean) As String
            If IsNormalized(seconds, nanoseconds) Then
                ' Use .NET's formatting for the value down to the second, including an opening double quote (as it's a string value)
                Dim dateTime = UnixEpoch.AddSeconds(seconds)
                Dim builder = New StringBuilder()
                builder.Append(""""c)
                builder.Append(dateTime.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss", CultureInfo.InvariantCulture))
                Duration.AppendNanoseconds(builder, nanoseconds)
                builder.Append("Z""")
                Return builder.ToString()
            End If

            If diagnosticOnly Then
                Return String.Format(CultureInfo.InvariantCulture, "{{ ""@warning"": ""Invalid Timestamp"", ""seconds"": ""{0}"", ""nanos"": {1} }}", seconds, nanoseconds)
            Else
                Throw New InvalidOperationException("Non-normalized timestamp value")
            End If
        End Function

        ''' <summary>
        ''' Returns a string representation of this <see cref="Timestamp"/> for diagnostic purposes.
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
            Return ToJson(Seconds, Nanos, True)
        End Function
    End Class
End Namespace

