Public Module ShellCommand

    Friend ReadOnly shell As Func(Of String, String, String)
    Friend ReadOnly logs As New List(Of String)

    Public Iterator Function CommandHistory() As IEnumerable(Of String)
        For Each line As String In logs
            Yield line
        Next
    End Function

    Sub New()
        shell = Function(app, args) As String
                    Dim lines As New List(Of String)
                    Call logs.Add($"{app} {args}")
                    Call CommandLine.ExecSub(app, args, onReadLine:=AddressOf lines.Add)
                    Return lines.JoinBy(vbLf)
                End Function
    End Sub

    Public Function Run(app As String, args As String) As String
        Return shell(app, args)
    End Function
End Module
