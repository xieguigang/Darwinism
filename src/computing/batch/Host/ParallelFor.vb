Imports Microsoft.VisualBasic.MIME.application.json
Imports Parallel
Imports Parallel.IpcStream
Imports Parallel.ThreadTask

Public Class ParallelFor(Of T)

    ReadOnly args As Argument

    Public ReadOnly Property debugMode As Boolean
        Get
            Return Not args.debugPort Is Nothing
        End Get
    End Property

    Sub New(args As Argument)
        Me.args = args
    End Sub

    Public Iterator Function GetResult(task As IEnumerable(Of Func(Of T))) As IEnumerable(Of T)
        Dim [for] As New ThreadTask(Of T)(task,
            debugMode:=debugMode,
            verbose:=args.verbose,
            taskInterval:=args.thread_interval)

        For Each yout As T In [for] _
            .WithDegreeOfParallelism(args.n_threads) _
            .RunParallel

            Yield yout
        Next
    End Function

    Public Overrides Function ToString() As String
        Return args.GetJson
    End Function

    Public Shared Iterator Function CreateFunction(par As Argument, task As [Delegate], [loop] As SocketRef(), ParamArray args As SocketRef()) As IEnumerable(Of Func(Of T))
        For Each xi As SocketRef In [loop]
            Dim host As SlaveTask = par.CreateHost
            Dim post As SocketRef() = New SocketRef(args.Length) {}

            post(0) = xi
            post.SetRange(args, offset:=1)

            Yield Function() As T
                      Dim result As T = host.RunTask(Of T)(task, post)
                      Return result
                  End Function
        Next
    End Function

End Class
