
Namespace Models
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

End Namespace