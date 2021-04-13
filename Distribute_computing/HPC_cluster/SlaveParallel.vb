Imports HPC_cluster.CLI
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Parallel

Public Class SlaveParallel

    ''' <summary>
    ''' Create a slave task factory
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function CreateSlave(Optional debugPort As Integer? = Nothing, Optional verbose As Boolean = False) As SlaveTask
        Return New SlaveTask(SlaveParallel.CreateProcessor, AddressOf SlaveParallel.SlaveTask, debugPort, verbose:=verbose)
    End Function

    Public Shared Function CreateProcessor() As Taskhost_d
        Return Taskhost_d.FromEnvironment(App.HOME)
    End Function

    Public Shared Function SlaveTask(processor As InteropService, port As Integer) As String
        Dim cli As String = DirectCast(processor, Taskhost_d).GetParallelCommandLine(master:=port)
        Return cli
    End Function
End Class
