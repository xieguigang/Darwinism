
Namespace Models

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
End Namespace