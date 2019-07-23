Module TargetTask

    Public Function ExceptionTest(message$) As Integer
        Throw New InvalidCastException(message)
    End Function

    Public Function Add100(i As Integer()) As Double()
        Return i.Select(Function(n) Add(n, 100)).ToArray
    End Function

    Private Function Add(a#, b#) As Double
        Return a + b
    End Function
End Module
