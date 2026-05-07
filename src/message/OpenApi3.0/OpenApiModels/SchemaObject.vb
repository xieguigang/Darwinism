
Namespace Models

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

End Namespace