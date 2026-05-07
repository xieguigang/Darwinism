' ============================================================================
' OpenApiModels.vb
' ============================================================================
' 本文件定义了与 OpenAPI 3.0.1 规范 YAML 文档结构完全对应的 VB.NET 数据模型类。
' 这些类用于通过 LoadYAML(Of T) 方法将 YAML 文档反序列化为 .NET 对象，
' 以便后续代码生成器遍历文档结构并生成 REST 客户端代码。
'
' OpenAPI 3.0.1 规范参考: https://spec.openapis.org/oas/v3.0.3
' ============================================================================

Namespace Models

    ' -----------------------------------------------------------------------
    ' Contact 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#contact-object
    ' -----------------------------------------------------------------------
    Public Class ContactObject
        Public Property name As String
        Public Property url As String
        Public Property email As String
    End Class

    ' -----------------------------------------------------------------------
    ' License 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#license-object
    ' -----------------------------------------------------------------------
    Public Class LicenseObject
        Public Property name As String
        Public Property url As String
    End Class

    ' -----------------------------------------------------------------------
    ' Server 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#server-object
    ' -----------------------------------------------------------------------
    Public Class ServerObject
        Public Property url As String
        Public Property description As String
        Public Property variables As Dictionary(Of String, ServerVariableObject)
    End Class

    ' -----------------------------------------------------------------------
    ' Server Variable 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#server-variable-object
    ' -----------------------------------------------------------------------
    Public Class ServerVariableObject
        Public Property [enum] As List(Of String)
        Public Property [default] As String
        Public Property description As String
    End Class


    ' -----------------------------------------------------------------------
    ' External Documentation 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#external-documentation-object
    ' -----------------------------------------------------------------------
    Public Class ExternalDocumentationObject
        Public Property description As String
        Public Property url As String
    End Class


    ' -----------------------------------------------------------------------
    ' Request Body 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#request-body-object
    ' -----------------------------------------------------------------------
    Public Class RequestBodyObject
        ''' <summary>请求体描述</summary>
        Public Property description As String

        ''' <summary>请求体内容映射，键为 MIME 类型</summary>
        Public Property content As Dictionary(Of String, MediaTypeObject)

        ''' <summary>是否必填</summary>
        Public Property required As Boolean
    End Class

    ' -----------------------------------------------------------------------
    ' Media Type 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#media-type-object
    ' -----------------------------------------------------------------------
    Public Class MediaTypeObject
        ''' <summary>媒体类型的 Schema 定义</summary>
        Public Property schema As SchemaObject

        ''' <summary>示例</summary>
        Public Property example As Object

        ''' <summary>示例映射</summary>
        Public Property examples As Dictionary(Of String, ExampleValue)

        ''' <summary>编码映射</summary>
        Public Property encoding As Dictionary(Of String, EncodingObject)
    End Class

    Public Class ExampleValue

        Public Property value As Object
        Public Property summary As String

    End Class

    ' -----------------------------------------------------------------------
    ' Encoding 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#encoding-object
    ' -----------------------------------------------------------------------
    Public Class EncodingObject
        Public Property contentType As String
        Public Property headers As Dictionary(Of String, HeaderObject)
        Public Property style As String
        Public Property explode As Boolean
        Public Property allowReserved As Boolean
    End Class





    ' -----------------------------------------------------------------------
    ' Discriminator 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#discriminator-object
    ' -----------------------------------------------------------------------
    Public Class DiscriminatorObject
        Public Property propertyName As String
        Public Property mapping As Dictionary(Of String, String)
    End Class

    ' -----------------------------------------------------------------------
    ' XML 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#xml-object
    ' -----------------------------------------------------------------------
    Public Class XmlObject
        Public Property name As String
        Public Property [namespace] As String
        Public Property prefix As String
        Public Property attribute As Boolean
        Public Property wrapped As Boolean
    End Class



    ' -----------------------------------------------------------------------
    ' OAuth Flows 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#oauth-flows-object
    ' -----------------------------------------------------------------------
    Public Class OAuthFlowsObject
        Public Property implicit As OAuthFlowObject
        Public Property password As OAuthFlowObject
        Public Property clientCredentials As OAuthFlowObject
        Public Property authorizationCode As OAuthFlowObject
    End Class

    ' -----------------------------------------------------------------------
    ' OAuth Flow 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#oauth-flow-object
    ' -----------------------------------------------------------------------
    Public Class OAuthFlowObject
        Public Property authorizationUrl As String
        Public Property tokenUrl As String
        Public Property refreshUrl As String
        Public Property scopes As Dictionary(Of String, String)
    End Class

    ' -----------------------------------------------------------------------
    ' Security Requirement 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#security-requirement-object
    ' -----------------------------------------------------------------------
    Public Class SecurityRequirementObject
        Inherits Dictionary(Of String, List(Of String))
    End Class

    ' -----------------------------------------------------------------------
    ' Tag 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#tag-object
    ' -----------------------------------------------------------------------
    Public Class TagObject
        Public Property name As String
        Public Property description As String
        Public Property externalDocs As ExternalDocumentationObject
    End Class

End Namespace
