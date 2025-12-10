Public Class Content

    ''' <summary>
    ''' file path
    ''' </summary>
    ''' <returns></returns>
    Public Property Key As String
    Public Property LastModified As String
    Public Property ETag As String
    Public Property Size As Long
    Public Property StorageClass As String
    Public Property Owner As Owner

End Class

Public Class Owner

    Public Property ID As String

End Class