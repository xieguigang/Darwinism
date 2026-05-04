' ============================================================================
' OpenApiModels.vb
' ============================================================================
' 本文件定义了与 OpenAPI 3.0.1 规范 YAML 文档结构完全对应的 VB.NET 数据模型类。
' 这些类用于通过 LoadYAML(Of T) 方法将 YAML 文档反序列化为 .NET 对象，
' 以便后续代码生成器遍历文档结构并生成 REST 客户端代码。
'
' OpenAPI 3.0.1 规范参考: https://spec.openapis.org/oas/v3.0.3
' ============================================================================

Namespace OpenApi.CodeGenerator.Models

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

    ' -----------------------------------------------------------------------
    ' Info 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#info-object
    ' -----------------------------------------------------------------------
    Public Class InfoObject
        ''' <summary>API 标题</summary>
        Public Property title As String

        ''' <summary>API 描述</summary>
        Public Property description As String

        ''' <summary>服务条款 URL</summary>
        Public Property termsOfService As String

        ''' <summary>联系信息</summary>
        Public Property contact As ContactObject

        ''' <summary>许可证信息</summary>
        Public Property license As LicenseObject

        ''' <summary>API 版本号</summary>
        Public Property version As String
    End Class

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
    ' Path Item 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#path-item-object
    ' -----------------------------------------------------------------------
    Public Class PathItemObject
        ''' <summary>路径项摘要</summary>
        Public Property summary As String

        ''' <summary>路径项描述</summary>
        Public Property description As String

        ''' <summary>GET 操作</summary>
        Public Property [get] As OperationObject

        ''' <summary>PUT 操作</summary>
        Public Property [put] As OperationObject

        ''' <summary>POST 操作</summary>
        Public Property [post] As OperationObject

        ''' <summary>DELETE 操作</summary>
        Public Property [delete] As OperationObject

        ''' <summary>OPTIONS 操作</summary>
        Public Property [options] As OperationObject

        ''' <summary>HEAD 操作</summary>
        Public Property [head] As OperationObject

        ''' <summary>PATCH 操作</summary>
        Public Property [patch] As OperationObject

        ''' <summary>TRACE 操作</summary>
        Public Property [trace] As OperationObject

        ''' <summary>路径级参数列表（适用于该路径下的所有操作）</summary>
        Public Property servers As List(Of ServerObject)

        ''' <summary>路径级参数列表</summary>
        Public Property parameters As List(Of ParameterObject)

        ''' <summary>
        ''' 获取此路径项中所有已定义的操作（非 Nothing 的 HTTP 方法操作）。
        ''' 返回键值对列表，键为 HTTP 方法名（如 "Get", "Post"），值为操作对象。
        ''' </summary>
        Public ReadOnly Property Operations As IEnumerable(Of KeyValuePair(Of String, OperationObject))
            Get
                Dim ops As New List(Of KeyValuePair(Of String, OperationObject))()
                If [get] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Get", [get]))
                If [put] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Put", [put]))
                If [post] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Post", [post]))
                If [delete] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Delete", [delete]))
                If [options] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Options", [options]))
                If [head] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Head", [head]))
                If [patch] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Patch", [patch]))
                If [trace] IsNot Nothing Then ops.Add(New KeyValuePair(Of String, OperationObject)("Trace", [trace]))
                Return ops
            End Get
        End Property
    End Class

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

    ' -----------------------------------------------------------------------
    ' External Documentation 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#external-documentation-object
    ' -----------------------------------------------------------------------
    Public Class ExternalDocumentationObject
        Public Property description As String
        Public Property url As String
    End Class

    ' -----------------------------------------------------------------------
    ' Parameter 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#parameter-object
    ' -----------------------------------------------------------------------
    Public Class ParameterObject
        ''' <summary>参数名称</summary>
        Public Property name As String

        ''' <summary>参数位置: "query", "header", "path", "cookie"</summary>
        Public Property [in] As String

        ''' <summary>参数描述</summary>
        Public Property description As String

        ''' <summary>是否必填</summary>
        Public Property required As Boolean

        ''' <summary>是否已弃用</summary>
        Public Property deprecated As Boolean

        ''' <summary>是否允许空值</summary>
        Public Property allowEmptyValue As Boolean

        ''' <summary>参数 Schema 定义</summary>
        Public Property schema As SchemaObject

        ''' <summary>参数示例</summary>
        Public Property example As Object

        ''' <summary>参数示例映射</summary>
        Public Property examples As Dictionary(Of String, Object)

        ''' <summary>内容定义（用于复杂参数）</summary>
        Public Property content As Dictionary(Of String, MediaTypeObject)

        ''' <summary>样式（用于序列化）</summary>
        Public Property style As String

        ''' <summary>是否展开</summary>
        Public Property explode As Boolean
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
        Public Property examples As Dictionary(Of String, Object)

        ''' <summary>编码映射</summary>
        Public Property encoding As Dictionary(Of String, EncodingObject)
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

    ' -----------------------------------------------------------------------
    ' Schema 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#schema-object
    ' 这是 OpenAPI 规范中最核心的对象之一，用于定义数据结构。
    ' -----------------------------------------------------------------------
    Public Class SchemaObject
        ''' <summary>Schema 类型: "string", "number", "integer", "boolean", "array", "object"</summary>
        Public Property type As String

        ''' <summary>格式修饰: "int32", "int64", "float", "double", "date", "date-time" 等</summary>
        Public Property format As String

        ''' <summary>标题</summary>
        Public Property title As String

        ''' <summary>描述</summary>
        Public Property description As String

        ''' <summary>默认值</summary>
        Public Property [default] As Object

        ''' <summary>示例</summary>
        Public Property example As Object

        ''' <summary>枚举值列表</summary>
        Public Property [enum] As List(Of Object)

        ''' <summary>对象属性定义（type=object 时使用）</summary>
        Public Property properties As Dictionary(Of String, SchemaObject)

        ''' <summary>必填属性名称列表</summary>
        Public Property required As List(Of String)

        ''' <summary>数组元素 Schema（type=array 时使用）</summary>
        Public Property items As SchemaObject

        ''' <summary>引用路径，如 "#/components/schemas/Pet"</summary>
        Public Property ref As String

        ''' <summary>是否可为空</summary>
        Public Property nullable As Boolean

        ''' <summary>是否只读</summary>
        Public Property [readOnly] As Boolean

        ''' <summary>是否只写</summary>
        Public Property [writeOnly] As Boolean

        ''' <summary>是否已弃用</summary>
        Public Property deprecated As Boolean

        ''' <summary>allOf 组合：必须满足所有子 Schema</summary>
        Public Property allOf As List(Of SchemaObject)

        ''' <summary>anyOf 组合：满足任一子 Schema 即可</summary>
        Public Property anyOf As List(Of SchemaObject)

        ''' <summary>oneOf 组合：必须恰好满足一个子 Schema</summary>
        Public Property oneOf As List(Of SchemaObject)

        ''' <summary>not 组合：不得满足此 Schema</summary>
        Public Property [not] As SchemaObject

        ''' <summary>额外属性定义（可为 Boolean 或 SchemaObject）</summary>
        Public Property additionalProperties As Object

        ''' <summary>字符串最小长度</summary>
        Public Property minLength As Integer?

        ''' <summary>字符串最大长度</summary>
        Public Property maxLength As Integer?

        ''' <summary>字符串正则模式</summary>
        Public Property pattern As String

        ''' <summary>数值最小值</summary>
        Public Property minimum As Double?

        ''' <summary>数值最大值</summary>
        Public Property maximum As Double?

        ''' <summary>是否排除最小值</summary>
        Public Property exclusiveMinimum As Boolean

        ''' <summary>是否排除最大值</summary>
        Public Property exclusiveMaximum As Boolean

        ''' <summary>数值倍数</summary>
        Public Property multipleOf As Double?

        ''' <summary>数组最小元素数</summary>
        Public Property minItems As Integer?

        ''' <summary>数组最大元素数</summary>
        Public Property maxItems As Integer?

        ''' <summary>数组是否不允许重复</summary>
        Public Property uniqueItems As Boolean

        ''' <summary>对象最少属性数</summary>
        Public Property minProperties As Integer?

        ''' <summary>对象最多属性数</summary>
        Public Property maxProperties As Integer?

        ''' <summary>鉴别器定义（用于多态）</summary>
        Public Property discriminator As DiscriminatorObject

        ''' <summary>XML 序列化配置</summary>
        Public Property xml As XmlObject

        ''' <summary>外部 Schema 文档链接</summary>
        Public Property externalDocs As ExternalDocumentationObject
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
    ' Components 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#components-object
    ' -----------------------------------------------------------------------
    Public Class ComponentsObject
        ''' <summary>可复用的 Schema 定义</summary>
        Public Property schemas As Dictionary(Of String, SchemaObject)

        ''' <summary>可复用的响应定义</summary>
        Public Property responses As Dictionary(Of String, ResponseObject)

        ''' <summary>可复用的参数定义</summary>
        Public Property parameters As Dictionary(Of String, ParameterObject)

        ''' <summary>可复用的示例定义</summary>
        Public Property examples As Dictionary(Of String, Object)

        ''' <summary>可复用的请求体定义</summary>
        Public Property requestBodies As Dictionary(Of String, RequestBodyObject)

        ''' <summary>可复用的请求头定义</summary>
        Public Property headers As Dictionary(Of String, HeaderObject)

        ''' <summary>可复用的安全方案定义</summary>
        Public Property securitySchemes As Dictionary(Of String, SecuritySchemeObject)

        ''' <summary>可复用的链接定义</summary>
        Public Property links As Dictionary(Of String, LinkObject)

        ''' <summary>可复用的回调定义</summary>
        Public Property callbacks As Dictionary(Of String, Object)
    End Class

    ' -----------------------------------------------------------------------
    ' Security Scheme 对象
    ' 对应规范: https://spec.openapis.org/oas/v3.0.3#security-scheme-object
    ' -----------------------------------------------------------------------
    Public Class SecuritySchemeObject
        Public Property type As String
        Public Property description As String
        Public Property name As String
        Public Property [in] As String
        Public Property scheme As String
        Public Property bearerFormat As String
        Public Property flows As OAuthFlowsObject
        Public Property openIdConnectUrl As String
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
