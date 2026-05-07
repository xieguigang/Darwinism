
Namespace Models

    ' -----------------------------------------------------------------------
    ' Header 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#header-object
    ' -----------------------------------------------------------------------
    Public Class HeaderObject
        Public Property description As String
        Public Property required As Boolean
        Public Property deprecated As Boolean
        Public Property allowEmptyValue As Boolean
        Public Property style As String
        Public Property explode As Boolean
        Public Property allowReserved As Boolean
        Public Property schema As SchemaObject
        Public Property example As Object
        Public Property examples As Dictionary(Of String, Object)
        Public Property content As Dictionary(Of String, MediaTypeObject)
    End Class
End Namespace