Imports System.Threading

Public Module BackgroundTaskUtils

    ''' <summary>
    ''' if parent is exists then kill current web server 
    ''' </summary>
    ''' <param name="parentId"></param>
    ''' <param name="kill"></param>
    Public Sub BindToMaster(parentId As String, kill As IDisposable)
        ' not specific the parent process id
        If parentId.StringEmpty OrElse Val(parentId) <= 0 Then
            Return
        Else
#If WINDOWS Then
            Dim task As New ThreadStart(
                Sub()
                    BackgroundTaskUtils.checkMasterHeartbeat(Integer.Parse(parentId), kill)
                End Sub)

            Call New Thread(task).Start()
#End If
        End If
    End Sub

    Private Sub checkMasterHeartbeat(parentId As Integer, kill As IDisposable)
        Dim parent As Process

        Try
            parent = Process.GetProcessById(parentId)
        Catch ex As Exception
            Call kill.Dispose()
            Return
        End Try

        Do While True
            Try
                If parent.HasExited Then
                    Call kill.Dispose()
                    Call App.Exit()

                    Exit Do
                End If
            Catch ex As Exception
                Call kill.Dispose()
                Call App.Exit()

                Exit Do
            End Try

            Call Thread.Sleep(100)
        Loop

        Call App.Exit()
    End Sub

End Module