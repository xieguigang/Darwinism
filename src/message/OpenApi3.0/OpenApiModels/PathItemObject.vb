
Namespace Models
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

End Namespace