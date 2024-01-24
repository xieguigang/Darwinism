Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Parallel

Public Module Host

    ''' <summary>
    ''' Create a slave task factory
    ''' </summary>
    ''' <returns></returns>
    Public Function CreateSlave(Optional debugPort As Integer? = Nothing, Optional verbose As Boolean = False) As SlaveTask
        Return New SlaveTask(Host.GetCurrentThread, cli:=AddressOf Host.SlaveTask, debugPort, verbose:=verbose)
    End Function

    ''' <summary>
    ''' get ``batch.exe``
    ''' </summary>
    ''' <returns></returns>
    Private Function GetCurrentThread() As InteropService
        Dim path As String = GetType(Host).Assembly.Location
        Dim program As New InteropService(path)

#If NETCOREAPP Then
        Call program.SetDotNetCoreDll()
#End If

        Return program
    End Function

    ''' <summary>
    ''' create commandline argument for launch a slave task
    ''' </summary>
    ''' <param name="host"></param>
    ''' <param name="port"></param>
    ''' <returns></returns>
    Private Function SlaveTask(host As InteropService, port As Integer) As String
        Return $"--port {port} --master ""localhost"" /@set --internal_pipeline=true"
    End Function

    ''' <summary>
    ''' solve a parallel task
    ''' </summary>
    ''' <param name="master"></param>
    ''' <param name="port"></param>
    ''' <returns></returns>
    Public Function Solve(master As String, port As Integer) As Integer
        Return New TaskBuilder(port, master).Run
    End Function

End Module
