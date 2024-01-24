Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Parallel.ThreadTask

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(
            args:=App.CommandLine,
            executeFile:=AddressOf Program.runJobs,
            executeEmpty:=AddressOf showHelp
        )
    End Function

    <ExportAPI("/parallel")>
    <Usage("--port <port_number> [--master <master_node_ipaddress, default=""localhost"">]")>
    Public Function runParallel(args As CommandLine) As Integer
        Dim port As Integer = args <= "--port"
        Dim master As String = args("--master") Or "localhost"

        Return Host.Solve(master, port)
    End Function

    Private Function showHelp() As Integer
        Call Console.WriteLine("jobs.sh [-j <n_threads, default=4> --debug --verbose -delay <sleep in seconds, default=0.5>]")
        Return 0
    End Function

    Private Function runJobs(bash As String, args As CommandLine) As Integer
        Dim input As String = args.Name
        Dim njobs As Integer = args("-j") Or 4
        Dim jobs As String() = input.ReadAllLines
        Dim debug As Boolean = args("--debug")
        Dim verbose As Boolean = args("--verbose")
        Dim delay As Double = args("-delay") Or 0.5
        Dim threads As New ThreadTask(Of Integer)(
            task:=jobs.PopulateTask,
            debugMode:=debug,
            verbose:=verbose,
            taskInterval:=delay * 1000
        )
        Dim jobResults = threads _
            .WithDegreeOfParallelism(njobs) _
            .RunParallel _
            .ToArray

        If jobResults.All(Function(d) d = 0) Then
            Return 0
        Else
            Call Console.WriteLine("[warning] part of the job has error!")
            Return 1
        End If
    End Function

    ''' <summary>
    ''' Create task handler for shell start a commandline job
    ''' </summary>
    ''' <param name="jobs"></param>
    ''' <returns></returns>
    <Extension>
    Private Iterator Function PopulateTask(jobs As IEnumerable(Of String)) As IEnumerable(Of Func(Of Integer))
        For Each job_cli As String In jobs
            job_cli = Strings.Trim(job_cli).TrimNewLine().Trim()

            If job_cli.StringEmpty Then
                Continue For
            End If

            Yield AddressOf New ShellJobWrapper(job_cli).Shell
        Next
    End Function

    Private Class ShellJobWrapper

        Dim proc As Process

        Sub New(job_cli As String)
            Dim parse As String() = CommandLine.ParseTokens(job_cli)
            Dim proc As New Process With {
                .StartInfo = New ProcessStartInfo With {
                    .UseShellExecute = True,
                    .FileName = parse(Scan0),
                    .Arguments = parse _
                        .Skip(1) _
                        .Select(Function(t) t.CLIToken) _
                        .JoinBy(" ")
                }
            }

            Me.proc = proc
        End Sub

        Public Function Shell() As Integer
            Call proc.Start()
            Call proc.WaitForExit()
            Return proc.ExitCode
        End Function
    End Class
End Module
