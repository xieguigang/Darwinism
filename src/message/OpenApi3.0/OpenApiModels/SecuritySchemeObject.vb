
Namespace Models

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
End Namespace