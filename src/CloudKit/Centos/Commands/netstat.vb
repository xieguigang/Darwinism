
''' <summary>
''' 
''' </summary>
''' <remarks>
''' sudo yum install net-tools
''' </remarks>
Public Class netstat

    Public Property Proto As String
    Public Property RecvQ As String
    Public Property SendQ As String
    Public Property LocalAddress As String
    Public Property ForeignAddress As String
    Public Property State As String
    ''' <summary>
    ''' PID/Program name
    ''' </summary>
    ''' <returns></returns>
    Public Property Program As String

    Public ReadOnly Property LocalListenPort As Integer
        Get
            If State = "LISTEN" Then
                Return Integer.Parse(LocalAddress.Split(":"c).Last)
            Else
                Return 0
            End If
        End Get
    End Property

    ''' <summary>
    ''' netstat -tulnp
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    Public Shared Iterator Function tulnp(text As String) As IEnumerable(Of netstat)
        ' (Not all processes could be identified, non-owned process info
        '  will not be shown, you would have to be root to see it all.)
        ' Active Internet connections (only servers)
        ' Proto Recv-Q Send-Q Local Address           Foreign Address         State       PID/Program name
        ' tcp        0      0 0.0.0.0:22              0.0.0.0:*               LISTEN      -
        ' tcp        0      0 0.0.0.0:111             0.0.0.0:*               LISTEN      -
        ' tcp        0      0 127.0.0.1:631           0.0.0.0:*               LISTEN      -
        ' tcp6       0      0 :::22                   :::*                    LISTEN      -
        ' tcp6       0      0 :::111                  :::*                    LISTEN      -
        ' tcp6       0      0 ::1:631                 :::*                    LISTEN      -
        ' udp        0      0 0.0.0.0:111             0.0.0.0:*                           -
        ' udp        0      0 127.0.0.1:323           0.0.0.0:*                           -
        ' udp        0      0 0.0.0.0:5353            0.0.0.0:*                           -
        ' udp        0      0 0.0.0.0:34247           0.0.0.0:*                           -
        ' udp6       0      0 :::43332                :::*                                -
        ' udp6       0      0 :::111                  :::*                                -
        ' udp6       0      0 ::1:323                 :::*                                -
        ' udp6       0      0 :::5353                 :::*                                -
        Dim lines As String() = text.LineTokens
        Dim offset = lines _
            .Select(Function(line, i) (line.Split().First, i)) _
            .Where(Function(t) t.First = "Proto") _
            .FirstOrDefault

        If offset.First.StringEmpty Then
            Return
        End If

        For i As Integer = offset.i + 1 To lines.Length - 1
            If Not lines(i).StringEmpty Then
                Dim tokens As String() = lines(i).StringSplit("\s+")

                Yield New netstat With {
                    .Proto = tokens.ElementAtOrDefault(0),
                    .RecvQ = tokens.ElementAtOrDefault(1),
                    .SendQ = tokens.ElementAtOrDefault(2),
                    .LocalAddress = tokens.ElementAtOrDefault(3),
                    .ForeignAddress = tokens.ElementAtOrDefault(4),
                    .State = tokens.ElementAtOrDefault(5),
                    .Program = tokens.Skip(6).JoinBy(" ")
                }
            End If
        Next
    End Function

End Class
