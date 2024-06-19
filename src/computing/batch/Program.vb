#Region "Microsoft.VisualBasic::bd099a066fedc197777125957d1c7535, src\computing\batch\Program.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 108
'    Code Lines: 83
' Comment Lines: 10
'   Blank Lines: 15
'     File Size: 3.76 KB


' Module Program
' 
'     Function: Main, PopulateTask, runJobs, runParallel, showHelp
'     Class ShellJobWrapper
' 
'         Constructor: (+1 Overloads) Sub New
'         Function: Shell
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Darwinism.HPC.Parallel.ThreadTask
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(
            args:=App.CommandLine,
            executeFile:=AddressOf Program.runJobs,
            executeEmpty:=AddressOf showHelp
        )
    End Function

    ''' <summary>
    ''' implements an internal parallel host for clr function
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <ExportAPI("/parallel")>
    <Usage("--port <port_number> [--master <master_node_ipaddress, default=""localhost""> --timeout <default=15> --verbose]")>
    Public Function runParallel(args As CommandLine) As Integer
        Dim port As Integer = args <= "--port"
        Dim master As String = args("--master") Or "localhost"
        Dim timeout As Double = args("--timeout") Or 15.0
        Dim verbose As Boolean = args("--verbose")

        Return Host.Solve(master, port, timeout:=timeout, verbose:=verbose)
    End Function

    Private Function showHelp() As Integer
        Call Console.WriteLine("jobs.sh [-j <n_threads, default=4> --debug --verbose -delay <sleep in seconds, default=0.5>]")
        Return 0
    End Function

    Private Function runJobs(bash As String, args As CommandLine) As Integer
        Dim input As String = args.Name
        Dim njobs As Integer = args("-j") Or 4
        Dim jobs As IEnumerable(Of String) = input.IterateAllLines
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
