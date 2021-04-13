''' <summary>
''' this works on linux system via ssh command from bash
''' </summary>
Public Class SSH

    Protected ReadOnly user As String
    Protected ReadOnly password As String
    Protected ReadOnly endpoint As String
    Protected ReadOnly port As Integer
    Protected ReadOnly debug As Boolean

    Sub New(user$, password$,
            Optional endpoint$ = "127.0.0.1",
            Optional port% = 22,
            Optional debug As Boolean = False)

        Me.user = user
        Me.password = password
        Me.endpoint = endpoint
        Me.port = port
        Me.debug = debug
    End Sub

    Public Overridable Function Run(command As String) As String
        Return CommandLine.Call("ssh", $"-o StrictHostKeyChecking=no {user}@{endpoint}:{port} {command}")
    End Function

    Public Overrides Function ToString() As String
        Return $"ssh {user}@{endpoint}:{port}"
    End Function
End Class
