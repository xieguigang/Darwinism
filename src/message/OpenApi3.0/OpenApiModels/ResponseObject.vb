
Namespace Models
    ' -----------------------------------------------------------------------
    ' Response 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#responses-object
    ' -----------------------------------------------------------------------
    Public Class ResponseObject
        ''' <summary>响应描述</summary>
        Public Property description As String

        ''' <summary>响应头映射</summary>
        Public Property headers As Dictionary(Of String, HeaderObject)

        ''' <summary>响应内容映射，键为 MIME 类型</summary>
        Public Property content As Dictionary(Of String, MediaTypeObject)

        ''' <summary>响应链接映射</summary>
        Public Property links As Dictionary(Of String, LinkObject)
    End Class
End Namespace