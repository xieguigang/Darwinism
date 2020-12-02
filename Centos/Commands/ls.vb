Public Class ls

    Public Property permission As String
    Public Property number1 As Integer
    Public Property user As String
    Public Property group As String
    Public Property size As String
    Public Property [date] As String
    Public Property file As String
    Public Property link As String

    Public Overrides Function ToString() As String
        Dim fileName As String = If(link.StringEmpty, file, $"{file} -> {link}")
        Dim line As String = New String() {
            permission, number1, user, group, size, [date], fileName
        }.JoinBy(vbTab)

        Return line
    End Function

    Public Shared Iterator Function Parse(stdout As String) As IEnumerable(Of ls)
        For Each line As String In stdout.LineTokens.Skip(1)
            Yield ParseLine(line)
        Next
    End Function

    Private Shared Function ParseLine(line As String) As ls

    End Function

End Class
