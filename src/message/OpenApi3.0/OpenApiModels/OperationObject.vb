
Namespace Models

    ' -----------------------------------------------------------------------
    ' Operation 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#operation-object
    ' -----------------------------------------------------------------------
    Public Class OperationObject
        ''' <summary>操作标签列表，用于分组</summary>
        Public Property tags As List(Of String)

        ''' <summary>操作摘要</summary>
        Public Property summary As String

        ''' <summary>操作详细描述</summary>
        Public Property description As String

        ''' <summary>外部文档链接</summary>
        Public Property externalDocs As ExternalDocumentationObject

        ''' <summary>操作唯一标识符</summary>
        Public Property operationId As String

        ''' <summary>操作级参数列表</summary>
        Public Property parameters As List(Of ParameterObject)

        ''' <summary>请求体定义</summary>
        Public Property requestBody As RequestBodyObject

        ''' <summary>响应定义映射，键为 HTTP 状态码或 "default"</summary>
        Public Property responses As Dictionary(Of String, ResponseObject)

        ''' <summary>回调定义</summary>
        Public Property callbacks As Dictionary(Of String, Object)

        ''' <summary>是否已弃用</summary>
        Public Property deprecated As Boolean

        ''' <summary>安全声明</summary>
        Public Property security As List(Of SecurityRequirementObject)

        ''' <summary>服务器列表</summary>
        Public Property servers As List(Of ServerObject)
    End Class

End Namespace