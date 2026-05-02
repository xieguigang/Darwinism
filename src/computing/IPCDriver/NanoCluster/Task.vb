Public Class Task

    Public Property task_id As String
    Public Property program As String
    Public Property args As String()
    Public Property env As Dictionary(Of String, String)
    Public Property resources As TaskResource

End Class

Public Class TaskResource

    Public Property cpu As Integer
    Public Property memory_gb As Single
    Public Property gpu As Integer

End Class
