Imports System.Text
Imports Darwinism.Docker.Arguments

''' <summary>
''' The container environment module
''' </summary>
Public Class Environment

    Public ReadOnly Property [Shared] As Mount
    Public ReadOnly Property container As Image

    Sub New(container As Image)
        Me.container = container
    End Sub

    Public Function Mount(local$, virtual$) As Environment
        _Shared = New Mount With {.local = local, .virtual = virtual}
        Return Me
    End Function

    Public Function Mount([shared] As Mount) As Environment
        _Shared = [shared]
        Return Me
    End Function

    Const InvalidMount$ = "Shared Drive argument is presented, but value is invalid, -v option will be ignored!"

    Public Function GetDockerCommand(command As String) As String
        Dim options As New StringBuilder

        If Not [Shared] Is Nothing Then
            If [Shared].IsValid Then
                Call options.AppendLine($"-v {[Shared]}")
            Else
                Call InvalidMount.Warning
            End If
        End If

        Return $"docker run {options} {container} {command}"
    End Function
End Class
