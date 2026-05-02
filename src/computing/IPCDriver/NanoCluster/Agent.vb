''' <summary>
''' Client agent
''' </summary>
Public Class Agent

    ReadOnly config As Config

    Sub New(config As Config)
        Me.config = config
        Me.MakeRegister()
    End Sub

    ''' <summary>
    ''' make client register to the master node
    ''' </summary>
    Private Sub MakeRegister()

    End Sub

    Public Function SubmitTask(task_id As String, program_path As String, args As String(), env As Dictionary(Of String, String())) As Boolean

    End Function

    Public Function QueryTask(task_id As String) As String

    End Function

    Public Function CancelTask(task_id As String) As Boolean

    End Function

End Class
