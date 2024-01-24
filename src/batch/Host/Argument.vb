Imports Microsoft.VisualBasic.MIME.application.json
Imports Parallel

Public Class Argument

    Public Property debugPort As Integer? = Nothing
    Public Property verbose As Boolean = False
    Public Property ignoreError As Boolean = False
    Public Property n_threads As Integer = 32

    Sub New()
    End Sub

    Sub New(n_threads As Integer)
        Me.n_threads = n_threads
    End Sub

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function

    Public Function CreateHost() As SlaveTask
        Return Host.CreateSlave(debugPort, verbose:=verbose, ignoreError:=ignoreError)
    End Function

End Class
