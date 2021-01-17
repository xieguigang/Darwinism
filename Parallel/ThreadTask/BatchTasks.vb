Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.Language

Public Module BatchTasks

    ''' <summary>
    ''' Folk this program itself for the large amount data batch processing.
    ''' </summary>
    ''' <param name="CLI">Self folk processing commandline collection.</param>
    ''' <param name="parallel">If this parameter value less than 1, then will be a single 
    ''' thread task. Any positive value that greater than 1 will be parallel task.
    ''' (小于等于零表示非并行化，单线程任务)
    ''' </param>
    ''' <param name="smart">Smart mode CPU load threshold, if the <paramref name="parallel"/> 
    ''' parameter value is less than or equals to 1, then this parameter will be disabled.
    ''' </param>
    ''' <returns>
    ''' Returns the total executation time for running this task collection.
    ''' (返回任务的执行的总时长)
    ''' </returns>
    Public Function SelfFolks&(CLI As IEnumerable(Of String),
                               Optional parallel% = 0,
                               Optional smart# = 0)

        Dim sw As Stopwatch = Stopwatch.StartNew

        If parallel <= 0 Then
            For Each args As String In CLI
                Call App.SelfFolk(args).Run()
            Next
        Else
            Dim Tasks As Func(Of Integer)() = LinqAPI.Exec(Of Func(Of Integer)) <=
 _
                From args As String
                In CLI
                Let io As IIORedirectAbstract = App.SelfFolk(args)
                Let task As Func(Of Integer) = AddressOf io.Run
                Select task

            Call New ThreadTask(Of Integer)(Tasks).WithDegreeOfParallelism(parallel).RunParallel.ToArray
        End If

        Return sw.ElapsedMilliseconds
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tasks"></param>
    ''' <param name="numOfThreads">同时执行的句柄的数目</param>
    ''' <remarks></remarks>
    <Extension>
    Public Sub Invoke(tasks As Action(), numOfThreads As Integer)
        Dim getTask As Func(Of Action, Func(Of Integer)) =
            Function(task)
                Return AddressOf New invokeHelper With {
                    .task = task
                }.RunTask
            End Function
        Dim invokes = From action As Action In tasks Select getTask(action)

        Call New ThreadTask(Of Integer)(invokes) _
            .WithDegreeOfParallelism(numOfThreads) _
            .RunParallel() _
            .ToArray
    End Sub

    Private Structure invokeHelper

        Dim task As Action

        Public Function RunTask() As Integer
            Call task()
            Return 0
        End Function
    End Structure
End Module
