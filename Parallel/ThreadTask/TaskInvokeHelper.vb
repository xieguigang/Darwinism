Friend Structure TaskInvokeHelper

    Dim task As Action

    Public Function RunTask() As Integer
        Call task()
        Return 0
    End Function
End Structure