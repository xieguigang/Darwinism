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
    ' Manually-written partial class for the Duration well-known type,
    ' providing a conversion to TimeSpan and convenience operators.
    Public Partial Class Duration
        Implements ICustomDiagnosticMessage
        ''' <summary>
        ''' The number of nanoseconds in a second.
        ''' </summary>
        Public Const NanosecondsPerSecond As Integer = 1000000000
        ''' <summary>
        ''' The number of nanoseconds in a BCL tick (as used by <see cref="TimeSpan"/> and <see cref="DateTime"/>).
        ''' </summary>
        Public Const NanosecondsPerTick As Integer = 100

        ''' <summary>
        ''' The maximum permitted number of seconds.
        ''' </summary>
        Public Const MaxSeconds As Long = 315576000000L

        ''' <summary>
        ''' The minimum permitted number of seconds.
        ''' </summary>
        Public Const MinSeconds As Long = -315576000000L
        Friend Const MaxNanoseconds As Integer = NanosecondsPerSecond - 1
        Friend Const MinNanoseconds As Integer = -NanosecondsPerSecond + 1

        Friend Shared Function IsNormalized(seconds As Long, nanoseconds As Integer) As Boolean
            ' Simple boundaries
            If seconds < MinSeconds OrElse seconds > MaxSeconds OrElse nanoseconds < MinNanoseconds OrElse nanoseconds > MaxNanoseconds Then
                Return False
            End If
            ' We only have a problem is one is strictly negative and the other is
            ' strictly positive.
            Return Math.Sign(seconds) * Math.Sign(nanoseconds) <> -1
        End Function

        ''' <summary>
        ''' Converts this <see cref="Duration"/> to a <see cref="TimeSpan"/>.
        ''' </summary>
        ''' <remarks>If the duration is not a precise number of ticks, it is truncated towards 0.</remarks>
        ''' <returns>The value of this duration, as a <c>TimeSpan</c>.</returns>
        ''' <exception cref="InvalidOperationException">This value isn't a valid normalized duration, as
        ''' described in the documentation.</exception>
        Public Function ToTimeSpan() As TimeSpan
            'BEGIN TODO : Visual Basic does not support checked statements!
            If Not IsNormalized(Seconds, Nanos) Then
                Throw New InvalidOperationException("Duration was not a valid normalized duration")
            End If

            Dim ticks As Long = Seconds * TimeSpan.TicksPerSecond + Nanos / NanosecondsPerTick
            Return TimeSpan.FromTicks(ticks)
            'END TODO : Visual Basic does not support checked statements!
        End Function

        ''' <summary>
        ''' Converts the given <see cref="TimeSpan"/> to a <see cref="Duration"/>.
        ''' </summary>
        ''' <param name="timeSpan">The <c>TimeSpan</c> to convert.</param>
        ''' <returns>The value of the given <c>TimeSpan</c>, as a <c>Duration</c>.</returns>
        Public Shared Function FromTimeSpan(timeSpan As TimeSpan) As Duration
            'BEGIN TODO : Visual Basic does not support checked statements!
            Dim ticks = timeSpan.Ticks
            Dim seconds As Long = ticks / TimeSpan.TicksPerSecond
            Dim nanos = CInt(ticks Mod TimeSpan.TicksPerSecond) * NanosecondsPerTick
            Return New Duration With {
                .Seconds = seconds,
                .Nanos = nanos
            }
            'END TODO : Visual Basic does not support checked statements!
        End Function

        ''' <summary>
        ''' Returns the result of negating the duration. For example, the negation of 5 minutes is -5 minutes.
        ''' </summary>
        ''' <param name="value">The duration to negate. Must not be null.</param>
        ''' <returns>The negated value of this duration.</returns>
        Public Shared Operator -(value As Duration) As Duration
            CheckNotNull(value, "value")
            'BEGIN TODO : Visual Basic does not support checked statements!
            Return Normalize(-value.Seconds, -value.Nanos)
            'END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Adds the two specified <see cref="Duration"/> values together.
        ''' </summary>
        ''' <param name="lhs">The first value to add. Must not be null.</param>
        ''' <param name="rhs">The second value to add. Must not be null.</param>
        ''' <returns></returns>
        Public Shared Operator +(lhs As Duration, rhs As Duration) As Duration
            CheckNotNull(lhs, "lhs")
            CheckNotNull(rhs, "rhs")
            'BEGIN TODO : Visual Basic does not support checked statements!
            Return Normalize(lhs.Seconds + rhs.Seconds, lhs.Nanos + rhs.Nanos)
            'END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Subtracts one <see cref="Duration"/> from another.
        ''' </summary>
        ''' <param name="lhs">The duration to subtract from. Must not be null.</param>
        ''' <param name="rhs">The duration to subtract. Must not be null.</param>
        ''' <returns>The difference between the two specified durations.</returns>
        Public Shared Operator -(lhs As Duration, rhs As Duration) As Duration
            CheckNotNull(lhs, "lhs")
            CheckNotNull(rhs, "rhs")
            'BEGIN TODO : Visual Basic does not support checked statements!
            Return Normalize(lhs.Seconds - rhs.Seconds, lhs.Nanos - rhs.Nanos)
            ' END TODO : Visual Basic does not support checked statements!
        End Operator

        ''' <summary>
        ''' Creates a duration with the normalized values from the given number of seconds and
        ''' nanoseconds, conforming with the description in the proto file.
        ''' </summary>
        Friend Shared Function Normalize(seconds As Long, nanoseconds As Integer) As Duration
            ' Ensure that nanoseconds is in the range (-1,000,000,000, +1,000,000,000)
            Dim extraSeconds As Integer = nanoseconds / NanosecondsPerSecond
            seconds += extraSeconds
            nanoseconds -= extraSeconds * NanosecondsPerSecond

            ' Now make sure that Sign(seconds) == Sign(nanoseconds) if Sign(seconds) != 0.
            If seconds < 0 AndAlso nanoseconds > 0 Then
                seconds += 1
                nanoseconds -= NanosecondsPerSecond
            ElseIf seconds > 0 AndAlso nanoseconds < 0 Then
                seconds -= 1
                nanoseconds += NanosecondsPerSecond
            End If

            Return New Duration With {
                .Seconds = seconds,
                .Nanos = nanoseconds
            }
        End Function

        ''' <summary>
        ''' Converts a duration specified in seconds/nanoseconds to a string.
        ''' </summary>
        ''' <remarks>
        ''' If the value is a normalized duration in the range described in <c>duration.proto</c>,
        ''' <paramrefname="diagnosticOnly"/> is ignored. Otherwise, if the parameter is <c>true</c>,
        ''' a JSON object with a warning is returned; if it is <c>false</c>, an <see cref="InvalidOperationException"/> is thrown.
        ''' </remarks>
        ''' <param name="seconds">Seconds portion of the duration.</param>
        ''' <param name="nanoseconds">Nanoseconds portion of the duration.</param>
        ''' <param name="diagnosticOnly">Determines the handling of non-normalized values</param>
        ''' <exception cref="InvalidOperationException">The represented duration is invalid, and <paramrefname="diagnosticOnly"/> is <c>false</c>.</exception>
        Friend Shared Function ToJson(seconds As Long, nanoseconds As Integer, diagnosticOnly As Boolean) As String
            If IsNormalized(seconds, nanoseconds) Then
                Dim builder = New StringBuilder()
                builder.Append(""""c)
                ' The seconds part will normally provide the minus sign if we need it, but not if it's 0...
                If seconds = 0 AndAlso nanoseconds < 0 Then
                    builder.Append("-"c)
                End If

                builder.Append(seconds.ToString("d", CultureInfo.InvariantCulture))
                AppendNanoseconds(builder, Math.Abs(nanoseconds))
                builder.Append("s""")
                Return builder.ToString()
            End If

            If diagnosticOnly Then
                ' Note: the double braces here are escaping for braces in format strings.
                Return String.Format(CultureInfo.InvariantCulture, "{{ ""@warning"": ""Invalid Duration"", ""seconds"": ""{0}"", ""nanos"": {1} }}", seconds, nanoseconds)
            Else
                Throw New InvalidOperationException("Non-normalized duration value")
            End If
        End Function

        ''' <summary>
        ''' Returns a string representation of this <see cref="Duration"/> for diagnostic purposes.
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

        ''' <summary>
        ''' Appends a number of nanoseconds to a StringBuilder. Either 0 digits are added (in which
        ''' case no "." is appended), or 3 6 or 9 digits. This is internal for use in Timestamp as well
        ''' as Duration.
        ''' </summary>
        Friend Shared Sub AppendNanoseconds(builder As StringBuilder, nanos As Integer)
            If nanos <> 0 Then
                builder.Append("."c)
                ' Output to 3, 6 or 9 digits.
                If nanos Mod 1000000 = 0 Then
                    builder.Append((nanos / 1000000).ToString("d3", CultureInfo.InvariantCulture))
                ElseIf nanos Mod 1000 = 0 Then
                    builder.Append((nanos / 1000).ToString("d6", CultureInfo.InvariantCulture))
                Else
                    builder.Append(nanos.ToString("d9", CultureInfo.InvariantCulture))
                End If
            End If
        End Sub
    End Class
End Namespace
