Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.InteropService
Imports Parallel
Imports Parallel.IpcStream

Public Module Host

    ''' <summary>
    ''' Create a slave task factory
    ''' </summary>
    ''' <returns></returns>
    Public Function CreateSlave(Optional debugPort As Integer? = Nothing,
                                Optional verbose As Boolean = False,
                                Optional ignoreError As Boolean = False,
                                Optional libpath As String = Nothing) As SlaveTask

        Return New SlaveTask(Host.GetCurrentThread(libpath), cli:=AddressOf Host.SlaveTask,
                             debugPort:=debugPort,
                             verbose:=verbose,
                             ignoreError:=ignoreError)
    End Function

    ''' <summary>
    ''' get ``batch.exe``
    ''' </summary>
    ''' <param name="libpath">
    ''' parameter for R# package: due to the reason of Rscript build R# package
    ''' will skip do file copy of some common modules, example like:
    ''' Microsoft.VisualBasic.Runtime.dll, so used the default location will
    ''' case the assembly file not found error, this parameter for config the
    ''' program location that contains the missing assembly file.
    ''' </param>
    ''' <returns></returns>
    Private Function GetCurrentThread(libpath As String) As InteropService
        Dim path As String = GetType(Host).Assembly.Location
        Dim assembly As String = path.BaseName & ".dll"
        Dim program As New InteropService(If(libpath.StringEmpty, path, $"{libpath}/{assembly}"))

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
        Return $"/parallel --port {port} --master ""localhost"" /@set --internal_pipeline=true"
    End Function

    ''' <summary>
    ''' solve a parallel task
    ''' </summary>
    ''' <param name="master"></param>
    ''' <param name="port"></param>
    ''' <returns></returns>
    Friend Function Solve(master As String, port As Integer) As Integer
        Return New TaskBuilder(port, master).Run
    End Function

    <Extension>
    Public Function ParallelFor(Of I, T)(par As Argument, task As [Delegate], [loop] As I(), ParamArray args As SocketRef()) As IEnumerable(Of T)
        Dim foreach As New ParallelFor(Of T)(par)
        Dim loopVal As SocketRef() = [loop] _
            .Select(Function(obj) SocketRef.WriteBuffer(obj)) _
            .ToArray
        Dim run = batch.ParallelFor(Of T).CreateFunction(par, task, loopVal, args)

        Return foreach.GetResult(run)
    End Function

    <Extension>
    Friend Sub SetRange(ByRef args As SocketRef(), appendAfter As SocketRef(), offset As Integer)
        For i As Integer = 0 To appendAfter.Length - 1
            args(i + offset) = appendAfter(i)
        Next
    End Sub

End Module


