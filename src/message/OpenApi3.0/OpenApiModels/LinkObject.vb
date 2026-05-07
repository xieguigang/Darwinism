
Namespace Models

    ' -----------------------------------------------------------------------
    ' Link 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#link-object
    ' -----------------------------------------------------------------------
    Public Class LinkObject
        Public Property operationRef As String
        Public Property operationId As String
        Public Property parameters As Dictionary(Of String, Object)
        Public Property requestBody As Object
        Public Property description As String
        Public Property server As ServerObject
    End Class
End Namespace