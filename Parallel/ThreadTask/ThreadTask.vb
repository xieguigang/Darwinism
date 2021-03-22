#Region "Microsoft.VisualBasic::c970e504d23e8ca06d6e00adf594cff8, Parallel\ThreadTask\ThreadTask.vb"

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

' Class ThreadTask
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: (+2 Overloads) CreateThreads, GetCompleteThread, GetEmptyThread, RunParallel, ToString
'               WithDegreeOfParallelism
' 
' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Parallel.Tasks

Namespace ThreadTask

    ''' <summary>
    ''' Using parallel linq that may stuck the program when a linq task partion wait a long time task to complete. 
    ''' By using this parallel function that you can avoid this problem from parallel linq, and also you can 
    ''' controls the task thread number manually by using this parallel task function.
    ''' (由于LINQ是分片段来执行的，当某个片段有一个线程被卡住之后整个进程都会被卡住，所以执行大型的计算任务的时候效率不太好，
    ''' 使用这个并行化函数可以避免这个问题，同时也可以自己手动控制线程的并发数)
    ''' </summary>
    ''' <typeparam name="TOut"></typeparam>
    Public Class ThreadTask(Of TOut)

        Dim taskList As Queue(Of Func(Of TOut))
        Dim threads As AsyncHandle(Of TOut)()
        Dim size As Integer

        ''' <summary>
        ''' create parallel task pool from a given collection of task handler
        ''' </summary>
        ''' <param name="task"></param>
        Sub New(task As IEnumerable(Of Func(Of TOut)))
            Me.taskList = New Queue(Of Func(Of TOut))(task)
            Me.size = Me.taskList.Count
        End Sub

        Public Shared Function CreateThreads(Of T)(items As IEnumerable(Of T), task As Func(Of T, Func(Of TOut))) As ThreadTask(Of TOut)
            Return New ThreadTask(Of TOut)(items.Select(task))
        End Function
        ''' <typeparam name="T"></typeparam>
        ''' <param name="items"></param>
        ''' <param name="task"></param>
        ''' <returns></returns>
        Public Shared Function CreateThreads(Of T)(items As IEnumerable(Of T), task As Func(Of T, Func(Of TOut))) As ThreadTask(Of TOut)
            Return New ThreadTask(Of TOut)(items.Select(task))
        End Function

        ''' <summary>
        ''' Create a parallel thread task pool and then get the task result value
        End Function

        ''' <param name="items"></param>
        ''' <param name="task"></param>
        ''' <returns></returns>
        Public Shared Function CreateThreads(Of T)(items As IEnumerable(Of T), task As Func(Of T, TOut)) As ThreadTask(Of TOut)
            Return New ThreadTask(Of TOut)(items.Select(Function(i) New Func(Of TOut)(Function() task(i))))
        End Function

        ''' <summary>
        ''' You can controls the parallel tasks number from this parameter, smaller or equals to ZERO means auto 
        ''' config the thread number, If want single thread, not parallel, set this value to 1, and positive 
        ''' value greater than 1 will makes the tasks parallel.
        ''' (可以在这里手动的控制任务的并发数，这个数值小于或者等于零则表示自动配置线程的数量, 1为单线程)
        ''' </summary>
        ''' <param name="n_threads"></param>
        ''' <returns></returns>
        Public Function WithDegreeOfParallelism(n_threads As Integer) As ThreadTask(Of TOut)
            threads = New AsyncHandle(Of TOut)(n_threads) {}
            Return Me
        End Function

        Private Function GetEmptyThread() As Integer
            For i As Integer = 0 To threads.Length - 1
                If threads(i) Is Nothing Then
                    Return i
    Private Function GetEmptyThread() As Integer
            For i As Integer = 0 To threads.Length - 1
                If threads(i) Is Nothing Then
                    Return i
                End If
            Next

            Return -1
        End Function

        Private Function GetCompleteThread() As Integer
            For i As Integer = 0 To threads.Length - 1
                If (Not threads(i) Is Nothing) AndAlso threads(i).IsCompleted Then
                    Return i
                End If
            Next

            Return -1
        End Function
        Next

        Return -1
        End Function

        ''' <summary>
        ''' view thread pool status
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String

            Return $"[free: {free}, running: {running}, finished: {finished}, progress: {delta} - {CInt(delta / size * 100)}%]"
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <summary>
        ''' Run parallel task list
        ''' </summary>
        ''' <returns></returns>
        Public Iterator Function RunParallel() As IEnumerable(Of TOut)
            Do While taskList.Count > 0
                Dim i As Integer = GetEmptyThread()

                If i > -1 Then
                    threads(i) = New AsyncHandle(Of TOut)(taskList.Dequeue).Run
                    Call Console.WriteLine($"{ToString()} submit new task on thread [{i + 1}]!")
                End If

                Dim j As Integer = GetCompleteThread()

                If j > -1 Then
                    Yield threads(j).GetValue
                    threads(j) = Nothing
                    Call Console.WriteLine($"{ToString()} [thread_{j + 1}] job done!")
                End If
            Loop

            Do While Not threads.All(Function(t) t Is Nothing)
                Dim j As Integer = GetCompleteThread()

                If j > -1 Then
                    Yield threads(j).GetValue
                    threads(j) = Nothing
                    Call Console.WriteLine($"{ToString()} [thread_{j + 1}] job done!")
                End If
            Loop
        End Function

    End Class
End Namespace
