Imports Microsoft.VisualBasic.Parallel.Tasks

Public Class ThreadTask(Of TOut)

    Dim taskList As Queue(Of Func(Of TOut))
    Dim threads As AsyncHandle(Of TOut)()

    Sub New(task As IEnumerable(Of Func(Of TOut)))
        Me.taskList = New Queue(Of Func(Of TOut))(task)
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

    Public Iterator Function RunParallel() As IEnumerable(Of TOut)
        Do While taskList.Count > 0
            Dim i As Integer = GetEmptyThread()

            If i > -1 Then
                threads(i) = New AsyncHandle(Of TOut)(taskList.Dequeue).Run
            End If

            Dim j As Integer = GetCompleteThread()

            If j > -1 Then
                Yield threads(j).GetValue
                threads(j) = Nothing
            End If
        Loop

        Do While Not threads.All(Function(t) t Is Nothing)
            Dim j As Integer = GetCompleteThread()

            If j > -1 Then
                Yield threads(j).GetValue
                threads(j) = Nothing
            End If
        Loop
    End Function

End Class
