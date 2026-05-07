
Namespace Models
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

End Namespace