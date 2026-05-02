''' <summary>
''' Client agent
''' </summary>
Public Class Agent : Implements IDisposable

    ReadOnly config As Config
    Private disposedValue As Boolean

    Sub New(config As Config)
        Me.config = config
        Call Threading.Tasks.Task.Run(Sub() Call Me.MakeRegister())
    End Sub

    ''' <summary>
    ''' make client register to the master node
    ''' </summary>
    Private Sub MakeRegister()
        Do While App.Running AndAlso Not disposedValue
            Dim stat As New Status
            Dim url As String = $"http://{config.master_ip}:{config.master_port}/active"

            Call url.POST(stat.CreateJSONPayload,)
            Call Threading.Thread.Sleep(config.heart_beats)
        Loop
    End Sub

    Public Function SubmitTask(task_id As String, program_path As String, args As String(), env As Dictionary(Of String, String())) As Boolean

    End Function

    Public Function QueryTask(task_id As String) As String

    End Function

    Public Function CancelTask(task_id As String) As Boolean

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

Public Class Status

    Public Function CreateJSONPayload() As Dictionary(Of String, String())

    End Function
End Class