Imports Microsoft.VisualBasic.Parallel.Tasks

Public Class ThreadTask(Of TOut)

    Dim taskList As Queue(Of Func(Of TOut))
    Dim threads As AsyncHandle(Of TOut)()
    Dim size As Integer

    Sub New(task As IEnumerable(Of Func(Of TOut)))
        Me.taskList = New Queue(Of Func(Of TOut))(task)
        Me.size = Me.taskList.Count
    End Sub

    Public Function WithDegreeOfParallelism(n_threads As Integer) As ThreadTask(Of TOut)
        threads = New AsyncHandle(Of TOut)(n_threads) {}
        Return Me
    End Function

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

    Public Overrides Function ToString() As String
        Dim free$ = threads.Where(Function(t) t Is Nothing).Count
        Dim running$ = threads.Where(Function(t) t IsNot Nothing AndAlso Not t.IsCompleted).Count
        Dim finished$ = threads.Where(Function(t) t IsNot Nothing AndAlso t.IsCompleted).Count
        Dim delta As Integer = size - taskList.Count

        Return $"[free: {free}, running: {running}, finished: {finished}, progress: {delta} - {CInt(delta / size * 100)}%]"
    End Function

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
