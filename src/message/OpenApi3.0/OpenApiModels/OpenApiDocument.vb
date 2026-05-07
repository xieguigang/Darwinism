
Namespace Models

    ' -----------------------------------------------------------------------
    ' OpenAPI 文档根对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#openapi-object
    ' -----------------------------------------------------------------------
    Public Class OpenApiDocument
        ''' <summary>OpenAPI 规范版本号，例如 "3.0.1"</summary>
        Public Property openapi As String

        ''' <summary>API 元数据信息</summary>
        Public Property info As InfoObject

        ''' <summary>服务器地址列表</summary>
        Public Property servers As List(Of ServerObject)

        ''' <summary>API 路径定义，键为路径模板字符串（如 "/pets"），值为路径项对象</summary>
        Public Property paths As Dictionary(Of String, PathItemObject)

        ''' <summary>可复用的组件定义（Schema、Response、Parameter 等）</summary>
        Public Property components As ComponentsObject

        ''' <summary>全局安全声明</summary>
        Public Property security As List(Of SecurityRequirementObject)

        ''' <summary>API 标签列表</summary>
        Public Property tags As List(Of TagObject)

        ''' <summary>外部文档链接</summary>
        Public Property externalDocs As ExternalDocumentationObject
    End Class
End Namespace