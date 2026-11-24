Imports System.IO

''' <summary>
''' Streaming CRC-32 calculator used by the QGram full-text cache format.
''' </summary>
Friend Class QGramCacheChecksum

    Private Shared ReadOnly Table As UInteger() = BuildTable()
    Private crc As UInteger = &HFFFFFFFFUI

    Public Sub Append(buffer As Byte(), count As Integer)
        If buffer Is Nothing Then
            Throw New ArgumentNullException(NameOf(buffer))
        End If
        If count < 0 OrElse count > buffer.Length Then
            Throw New ArgumentOutOfRangeException(NameOf(count))
        End If

        For i As Integer = 0 To count - 1
            Dim index As Integer = CInt((crc Xor CUInt(buffer(i))) And &HFFUI)
            crc = (crc >> 8) Xor Table(index)
        Next
    End Sub

    Public ReadOnly Property Value As UInteger
        Get
            Return crc Xor &HFFFFFFFFUI
        End Get
    End Property

    ''' <summary>
    ''' Computes CRC-32 for <paramref name="count"/> bytes starting at the current stream position.
    ''' </summary>
    Public Shared Function Compute(stream As Stream, count As Long) As UInteger
        If stream Is Nothing Then
            Throw New ArgumentNullException(NameOf(stream))
        End If
        If count < 0 Then
            Throw New ArgumentOutOfRangeException(NameOf(count))
        End If

        Dim checksum As New QGramCacheChecksum()
        Dim buffer(8191) As Byte
        Dim remaining As Long = count

        While remaining > 0
            Dim wanted As Integer = CInt(Math.Min(CLng(buffer.Length), remaining))
            Dim read As Integer = stream.Read(buffer, 0, wanted)

            If read <= 0 Then
                Throw New EndOfStreamException("unexpected end of stream while computing cache checksum")
            End If

            Call checksum.Append(buffer, read)
            remaining -= read
        End While

        Return checksum.Value
    End Function

    Private Shared Function BuildTable() As UInteger()
        Dim table(255) As UInteger

        For i As Integer = 0 To 255
            Dim value As UInteger = CUInt(i)

            For k As Integer = 0 To 7
                If (value And 1UI) <> 0UI Then
                    value = (value >> 1) Xor &HEDB88320UI
                Else
                    value = value >> 1
                End If
            Next

            table(i) = value
        Next

        Return table
    End Function

End Class
