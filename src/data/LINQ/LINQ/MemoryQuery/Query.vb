Public Class Query

    Public Enum Type
        FullText
        HashTerm
        ValueRange
        ValueMatch
    End Enum

    Public Property search As Type
    Public Property field As String
    Public Property value As Object

End Class
