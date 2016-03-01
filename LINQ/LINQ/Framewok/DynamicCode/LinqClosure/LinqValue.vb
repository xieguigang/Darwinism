''' <summary>
''' From x in $source let value as LinqValue = Project(x) Where value.IsTrue Select value.value
''' </summary>
Public Structure LinqValue

    Public Property IsTrue As Boolean
    Public Property value As Object

    Public Shared Function Unavailable() As LinqValue
        Return New LinqValue With {
            .IsTrue = False,
            .value = Nothing
        }
    End Function

    Sub New(obj As Object)
        IsTrue = True
        value = obj
    End Sub
End Structure
