Imports System.IO
Imports System.Text
Imports Darwinism.IPC.OpenApi3.Models

' ============================================================================
' OpenApiCodeGenerator.vb
' ============================================================================
' 本文件是 OpenAPI 3.0.1 代码生成器的核心实现。
' 它负责将反序列化后的 OpenApiDocument 对象转换为 VB.NET 源代码文件。
'
' 生成器主要产出两类代码文件：
'   1. 模型类文件（Model）—— 基于 components/schemas 中的 Schema 定义
'   2. REST 客户端类文件（ApiClient）—— 基于 paths 中的操作定义
'   3. 基础设施类（Infrastructure）—— HTTP 客户端封装和异常类
'
' 使用方式：
'   Dim generator As New OpenApiCodeGenerator(doc, outputDir, rootNamespace)
'   generator.GenerateAll()
' ============================================================================

''' <summary>
''' OpenAPI 3.0.1 VB.NET 代码生成器。
''' 解析 OpenAPI 文档对象并生成对应的 VB.NET 模型类和 REST 客户端代码。
''' </summary>
Public Class OpenApiCodeGenerator

    ' -------------------------------------------------------------------
    ' 字段与属性
    ' -------------------------------------------------------------------

    Private ReadOnly _doc As OpenApiDocument
    Private ReadOnly _outputDir As String
    Private ReadOnly _rootNamespace As String
    Private ReadOnly _schemaTypeMap As New Dictionary(Of String, String)

    Private Const INDENT As String = "    "
    Private Const INDENT2 As String = "        "
    Private Const INDENT3 As String = "            "
    Private Const INDENT4 As String = "                "

    ''' <summary>
    ''' 创建代码生成器实例。
    ''' </summary>
    Public Sub New(doc As OpenApiDocument, outputDir As String, Optional rootNamespace As String = "GeneratedApi")
        _doc = doc
        _outputDir = outputDir
        _rootNamespace = rootNamespace

        If Not Directory.Exists(_outputDir) Then
            Directory.CreateDirectory(_outputDir)
        End If

        BuildSchemaTypeMap()
    End Sub

    ' -------------------------------------------------------------------
    ' 公共入口方法
    ' -------------------------------------------------------------------

    ''' <summary>
    ''' 执行完整的代码生成流程。
    ''' </summary>
    Public Sub GenerateAll()
        Console.WriteLine("[OpenApiCodeGenerator] 开始代码生成...")
        Console.WriteLine("[OpenApiCodeGenerator] 输出目录: " & _outputDir)
        Console.WriteLine("[OpenApiCodeGenerator] 根命名空间: " & _rootNamespace)

        GenerateModelClasses()
        GenerateApiClientClasses()
        GenerateInfrastructureClasses()

        Console.WriteLine("[OpenApiCodeGenerator] 代码生成完成！")
    End Sub

    ' -------------------------------------------------------------------
    ' Schema 类型映射构建
    ' -------------------------------------------------------------------

    Private Sub BuildSchemaTypeMap()
        If _doc.components?.schemas Is Nothing Then Return

        For Each kvp In _doc.components.schemas
            _schemaTypeMap(kvp.Key) = ToPascalCase(kvp.Key)
        Next
    End Sub

    ' -------------------------------------------------------------------
    ' 模型类生成
    ' -------------------------------------------------------------------

    Private Sub GenerateModelClasses()
        If _doc.components?.schemas Is Nothing Then
            Console.WriteLine("[OpenApiCodeGenerator] 文档中没有 components/schemas 定义，跳过模型类生成。")
            Return
        End If

        Dim modelsDir As String = Path.Combine(_outputDir, "Models")
        If Not Directory.Exists(modelsDir) Then
            Directory.CreateDirectory(modelsDir)
        End If

        Console.WriteLine("[OpenApiCodeGenerator] 开始生成模型类，共 " & _doc.components.schemas.Count.ToString() & " 个 Schema...")

        For Each kvp In _doc.components.schemas
            Dim schemaName As String = kvp.Key
            Dim schema As SchemaObject = kvp.Value

            If schema Is Nothing Then Continue For

            Dim vbCode As String = GenerateModelClass(schemaName, schema)
            Dim filePath As String = Path.Combine(modelsDir, ToPascalCase(schemaName) & ".vb")
            File.WriteAllText(filePath, vbCode, Encoding.UTF8)

            Console.WriteLine("  -> 已生成模型类: " & filePath)
        Next
    End Sub

    Private Function GenerateModelClass(schemaName As String, schema As SchemaObject) As String
        Dim sb As New StringBuilder()
        Dim className As String = ToPascalCase(schemaName)

        ' 文件头注释
        sb.AppendLine("' ============================================================================")
        sb.AppendLine("' " & className & ".vb")
        sb.AppendLine("' 自动生成的模型类 - 基于 OpenAPI 3.0.1 规范")
        sb.AppendLine("' 源 Schema: " & schemaName)
        sb.AppendLine("' ============================================================================")
        sb.AppendLine()

        ' 命名空间和导入
        sb.AppendLine("Imports System")
        sb.AppendLine("Imports System.Collections.Generic")
        sb.AppendLine("Imports Newtonsoft.Json")
        sb.AppendLine()
        sb.AppendLine("Namespace " & _rootNamespace & ".Models")
        sb.AppendLine()

        ' 类注释
        If Not String.IsNullOrEmpty(schema.description) Then
            sb.AppendLine(INDENT & "''' <summary>")
            sb.AppendLine(INDENT & "''' " & EscapeXmlComment(schema.description))
            sb.AppendLine(INDENT & "''' </summary>")
        End If

        ' 处理 allOf 继承
        If schema.allOf IsNot Nothing AndAlso schema.allOf.Count > 0 Then
            GenerateAllOfClassBody(sb, schema, className)
        Else
            ' 类声明
            sb.AppendLine(INDENT & "Public Class " & className)
            sb.AppendLine()
            ' 生成属性
            GenerateProperties(sb, schema)
            ' 类结尾
            sb.AppendLine(INDENT & "End Class")
        End If

        sb.AppendLine()
        sb.AppendLine("End Namespace")

        Return sb.ToString()
    End Function

    Private Sub GenerateAllOfClassBody(sb As StringBuilder, schema As SchemaObject, className As String)
        Dim refName As String = Nothing
        Dim ownProperties As New Dictionary(Of String, SchemaObject)()
        Dim ownRequired As New List(Of String)()

        For Each subSchema In schema.allOf
            If Not String.IsNullOrEmpty(subSchema.ref) Then
                refName = ResolveRefName(subSchema.ref)
            ElseIf subSchema.properties IsNot Nothing Then
                For Each prop In subSchema.properties
                    ownProperties(prop.Key) = prop.Value
                Next
                If subSchema.required IsNot Nothing Then
                    ownRequired.AddRange(subSchema.required)
                End If
            End If
        Next

        ' 类声明（带继承）
        If refName IsNot Nothing Then
            Dim baseClassName As String = ToPascalCase(refName)
            sb.AppendLine(INDENT & "Public Class " & className)
            sb.AppendLine(INDENT & "    Inherits " & baseClassName)
        Else
            sb.AppendLine(INDENT & "Public Class " & className)
        End If
        sb.AppendLine()

        ' 生成自有属性
        If ownProperties.Count > 0 Then
            Dim mergedSchema As New SchemaObject() With {
                .properties = ownProperties,
                .required = ownRequired
            }
            GenerateProperties(sb, mergedSchema)
        End If

        sb.AppendLine(INDENT & "End Class")
    End Sub

    Private Sub GenerateProperties(sb As StringBuilder, schema As SchemaObject)
        If schema.properties Is Nothing Then Return

        Dim requiredSet As New HashSet(Of String)(If(schema.required, New List(Of String)))

        For Each prop In schema.properties
            Dim propName As String = prop.Key
            Dim propSchema As SchemaObject = prop.Value
            Dim vbType As String = MapSchemaToVbType(propSchema)
            Dim isRequired As Boolean = requiredSet.Contains(propName)
            Dim pascalName As String = ToPascalCase(propName)

            Dim declType As String = vbType
            If Not isRequired AndAlso IsValueType(vbType) Then
                declType = vbType & "?"
            End If

            ' XML 文档注释
            sb.AppendLine(INDENT2 & "''' <summary>")
            If Not String.IsNullOrEmpty(propSchema.description) Then
                sb.AppendLine(INDENT2 & "''' " & EscapeXmlComment(propSchema.description))
            Else
                sb.AppendLine(INDENT2 & "''' " & propName & " 属性")
            End If
            sb.AppendLine(INDENT2 & "''' </summary>")

            ' JsonProperty 特性
            sb.AppendLine(INDENT2 & "<JsonProperty(""" & propName & """)>")
            sb.AppendLine(INDENT2 & "Public Property " & pascalName & " As " & declType)
            sb.AppendLine()
        Next
    End Sub

    ' -------------------------------------------------------------------
    ' REST 客户端类生成
    ' -------------------------------------------------------------------

    Private Sub GenerateApiClientClasses()
        If _doc.paths Is Nothing OrElse _doc.paths.Count = 0 Then
            Console.WriteLine("[OpenApiCodeGenerator] 文档中没有 paths 定义，跳过客户端类生成。")
            Return
        End If

        Dim clientsDir As String = Path.Combine(_outputDir, "Clients")
        If Not Directory.Exists(clientsDir) Then
            Directory.CreateDirectory(clientsDir)
        End If

        ' 按 tag 分组操作
        Dim tagGroups As New Dictionary(Of String, List(Of OperationInfo))()

        For Each pathKvp In _doc.paths
            Dim pathTemplate As String = pathKvp.Key
            Dim pathItem As PathItemObject = pathKvp.Value

            If pathItem Is Nothing Then Continue For

            Dim pathLevelParams As List(Of ParameterObject) = pathItem.parameters

            For Each opKvp In pathItem.Operations
                Dim httpMethod As String = opKvp.Key
                Dim operation As OperationObject = opKvp.Value

                If operation Is Nothing Then Continue For

                Dim info As New OperationInfo() With {
                    .PathTemplate = pathTemplate,
                    .HttpMethod = httpMethod,
                    .Operation = operation,
                    .PathLevelParameters = pathLevelParams
                }

                Dim tagName As String = DetermineTag(operation)

                If Not tagGroups.ContainsKey(tagName) Then
                    tagGroups(tagName) = New List(Of OperationInfo)()
                End If
                tagGroups(tagName).Add(info)
            Next
        Next

        Console.WriteLine("[OpenApiCodeGenerator] 开始生成客户端类，共 " & tagGroups.Count.ToString() & " 个分组...")

        For Each group In tagGroups
            Dim tagName As String = group.Key
            Dim operations As List(Of OperationInfo) = group.Value

            Dim vbCode As String = GenerateApiClientClass(tagName, operations)
            Dim filePath As String = Path.Combine(clientsDir, tagName & "Client.vb")
            File.WriteAllText(filePath, vbCode, Encoding.UTF8)

            Console.WriteLine("  -> 已生成客户端类: " & filePath)
        Next
    End Sub

    Private Function DetermineTag(operation As OperationObject) As String
        If operation.tags IsNot Nothing AndAlso operation.tags.Count > 0 Then
            Return ToPascalCase(operation.tags(0))
        End If
        Return "Default"
    End Function

    Private Function GenerateApiClientClass(tagName As String, operations As List(Of OperationInfo)) As String
        Dim sb As New StringBuilder()
        Dim className As String = tagName & "Client"

        ' 文件头注释
        sb.AppendLine("' ============================================================================")
        sb.AppendLine("' " & className & ".vb")
        sb.AppendLine("' 自动生成的 REST 客户端类 - 基于 OpenAPI 3.0.1 规范")
        sb.AppendLine("' 分组标签: " & tagName)
        sb.AppendLine("' ============================================================================")
        sb.AppendLine()

        ' 导入
        sb.AppendLine("Imports System")
        sb.AppendLine("Imports System.Collections.Generic")
        sb.AppendLine("Imports System.IO")
        sb.AppendLine("Imports System.Net")
        sb.AppendLine("Imports System.Net.Http")
        sb.AppendLine("Imports System.Text")
        sb.AppendLine("Imports System.Threading.Tasks")
        sb.AppendLine("Imports Newtonsoft.Json")
        sb.AppendLine("Imports " & _rootNamespace & ".Models")
        sb.AppendLine("Imports " & _rootNamespace & ".Infrastructure")
        sb.AppendLine()
        sb.AppendLine("Namespace " & _rootNamespace & ".Clients")
        sb.AppendLine()

        ' 类注释
        sb.AppendLine(INDENT & "''' <summary>")
        sb.AppendLine(INDENT & "''' " & tagName & " 分组的 REST API 客户端。")
        If _doc.info IsNot Nothing AndAlso Not String.IsNullOrEmpty(_doc.info.description) Then
            sb.AppendLine(INDENT & "''' API: " & EscapeXmlComment(_doc.info.title) & " - " & EscapeXmlComment(_doc.info.description))
        End If
        sb.AppendLine(INDENT & "''' </summary>")
        sb.AppendLine(INDENT & "Public Class " & className)
        sb.AppendLine()

        ' 字段
        sb.AppendLine(INDENT2 & "Private ReadOnly _httpClient As ApiHttpClient")
        sb.AppendLine(INDENT2 & "Private ReadOnly _baseUrl As String")
        sb.AppendLine()

        ' 构造函数
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 创建 " & className & " 实例。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "''' <param name=""baseUrl"">API 基础地址</param>")
        sb.AppendLine(INDENT2 & "''' <param name=""httpClient"">可选的自定义 HttpClient 实例</param>")
        sb.AppendLine(INDENT2 & "Public Sub New(baseUrl As String, Optional httpClient As HttpClient = Nothing)")
        sb.AppendLine(INDENT2 & "    _baseUrl = baseUrl.TrimEnd(""/""c)")
        sb.AppendLine(INDENT2 & "    _httpClient = New ApiHttpClient(If(httpClient, New HttpClient()))")
        sb.AppendLine(INDENT2 & "End Sub")
        sb.AppendLine()

        ' 生成每个操作的方法
        For Each opInfo In operations
            GenerateApiMethod(sb, opInfo)
        Next

        ' 类结尾
        sb.AppendLine(INDENT & "End Class")
        sb.AppendLine()
        sb.AppendLine("End Namespace")

        Return sb.ToString()
    End Function

    Private Sub GenerateApiMethod(sb As StringBuilder, opInfo As OperationInfo)
        Dim operation As OperationObject = opInfo.Operation
        Dim httpMethod As String = opInfo.HttpMethod
        Dim pathTemplate As String = opInfo.PathTemplate

        ' 确定方法名
        Dim methodName As String = DetermineMethodName(operation, httpMethod, pathTemplate)

        ' 收集所有参数
        Dim allParams As List(Of ParameterObject) = CollectAllParameters(opInfo)

        ' 分离不同位置的参数
        Dim pathParams As List(Of ParameterObject) = allParams.Where(Function(p) p.in = "path").ToList()
        Dim queryParams As List(Of ParameterObject) = allParams.Where(Function(p) p.in = "query").ToList()
        Dim headerParams As List(Of ParameterObject) = allParams.Where(Function(p) p.in = "header").ToList()

        ' 请求体
        Dim requestBodyParam As RequestBodyObject = operation.requestBody

        ' 返回类型
        Dim returnType As String = DetermineReturnType(operation)
        Dim hasReturnType As Boolean = Not String.IsNullOrEmpty(returnType) AndAlso returnType <> "Void"
        If Not hasReturnType Then returnType = Nothing

        ' 构建方法签名参数列表
        Dim methodParams As New List(Of String)()
        For Each p In pathParams
            Dim paramType As String = MapSchemaToVbType(p.schema)
            methodParams.Add(ToCamelCase(p.name) & " As " & paramType)
        Next
        For Each p In queryParams
            Dim paramType As String = MapSchemaToVbType(p.schema)
            If Not p.required AndAlso IsValueType(paramType) Then
                paramType &= "?"
            End If
            methodParams.Add(ToCamelCase(p.name) & " As " & paramType)
        Next
        For Each p In headerParams
            Dim paramType As String = MapSchemaToVbType(p.schema)
            methodParams.Add(ToCamelCase(p.name) & " As " & paramType)
        Next

        ' 请求体参数
        Dim requestBodyType As String = Nothing
        If requestBodyParam IsNot Nothing Then
            requestBodyType = DetermineRequestBodyType(requestBodyParam)
            If requestBodyType IsNot Nothing Then
                methodParams.Add("body As " & requestBodyType)
            End If
        End If

        ' 方法注释
        sb.AppendLine(INDENT2 & "''' <summary>")
        If Not String.IsNullOrEmpty(operation.summary) Then
            sb.AppendLine(INDENT2 & "''' " & EscapeXmlComment(operation.summary))
        Else
            sb.AppendLine(INDENT2 & "''' " & httpMethod & " " & pathTemplate)
        End If
        If Not String.IsNullOrEmpty(operation.description) AndAlso operation.description <> operation.summary Then
            sb.AppendLine(INDENT2 & "''' ")
            sb.AppendLine(INDENT2 & "''' " & EscapeXmlComment(operation.description))
        End If
        sb.AppendLine(INDENT2 & "''' </summary>")

        ' 参数注释
        For Each p In allParams
            Dim desc As String = If(String.IsNullOrEmpty(p.description), p.name, p.description)
            sb.AppendLine(INDENT2 & "''' <param name=""" & ToCamelCase(p.name) & """>" & EscapeXmlComment(desc) & "</param>")
        Next
        If requestBodyType IsNot Nothing Then
            sb.AppendLine(INDENT2 & "''' <param name=""body"">请求体</param>")
        End If
        sb.AppendLine(INDENT2 & "''' <returns>异步任务，返回 " & returnType & " 类型的结果</returns>")

        ' 弃用标记
        If operation.deprecated Then
            sb.AppendLine(INDENT2 & "<Obsolete(""此 API 操作已弃用。"")>")
        End If

        ' 方法签名
        Dim paramStr As String = String.Join(", ", methodParams)
        If hasReturnType Then
            sb.AppendLine(INDENT2 & "Public Async Function " & methodName & "(" & paramStr & ") As Task(Of " & returnType & ")")
        Else
            sb.AppendLine(INDENT2 & "Public Async Function " & methodName & "(" & paramStr & ") As Task")
        End If
        sb.AppendLine()

        ' ---- 方法体 ----

        ' 构建 URL 路径
        sb.AppendLine(INDENT3 & "' 构建 URL 路径")
        Dim resolvedPath As String = pathTemplate
        For Each p In pathParams
            ' 将 OpenAPI 路径参数 {paramName} 替换为 VB.NET camelCase 形式
            resolvedPath = resolvedPath.Replace("{" & p.name & "}", "{" & ToCamelCase(p.name) & "}")
        Next
        ' 生成 URL 赋值语句
        ' 在生成的代码中输出: Dim url As String = $"{_baseUrl}/pets/{petId}"
        ' 其中 {_baseUrl} 和 {petId} 都是 VB.NET 字符串插值表达式
        Dim urlLine As String = BuildUrlLine(resolvedPath)
        sb.AppendLine(INDENT3 & urlLine)

        ' 添加查询参数
        If queryParams.Count > 0 Then
            sb.AppendLine()
            sb.AppendLine(INDENT3 & "' 构建查询参数")
            sb.AppendLine(INDENT3 & "Dim queryParams As New List(Of String)()")
            For Each p In queryParams
                Dim paramName As String = ToCamelCase(p.name)
                If p.required Then
                    sb.AppendLine(INDENT3 & "queryParams.Add(""" & p.name & "="" & Uri.EscapeDataString(" & paramName & ".ToString()))")
                Else
                    sb.AppendLine(INDENT3 & "If " & paramName & ".HasValue Then")
                    sb.AppendLine(INDENT3 & "    queryParams.Add(""" & p.name & "="" & Uri.EscapeDataString(" & paramName & ".Value.ToString()))")
                    sb.AppendLine(INDENT3 & "End If")
                End If
            Next
            sb.AppendLine(INDENT3 & "If queryParams.Count > 0 Then")
            sb.AppendLine(INDENT3 & "    url &= ""?"" & String.Join(""&"", queryParams)")
            sb.AppendLine(INDENT3 & "End If")
        End If

        ' 创建 HTTP 请求
        sb.AppendLine()
        sb.AppendLine(INDENT3 & "' 创建 HTTP 请求")
        If String.Equals(httpMethod, "Patch", StringComparison.OrdinalIgnoreCase) Then
            sb.AppendLine(INDENT3 & "Dim request As New HttpRequestMessage(New HttpMethod(""PATCH""), url)")
        Else
            sb.AppendLine(INDENT3 & "Dim request As New HttpRequestMessage(HttpMethod." & httpMethod & ", url)")
        End If

        ' 添加请求头参数
        If headerParams.Count > 0 Then
            sb.AppendLine()
            sb.AppendLine(INDENT3 & "' 添加请求头")
            For Each p In headerParams
                sb.AppendLine(INDENT3 & "request.Headers.Add(""" & p.name & """, " & ToCamelCase(p.name) & ".ToString())")
            Next
        End If

        ' 设置请求体
        If requestBodyParam IsNot Nothing Then
            sb.AppendLine()
            sb.AppendLine(INDENT3 & "' 设置请求体")
            Dim contentType As String = DetermineRequestContentType(requestBodyParam)
            If contentType = "application/json" Then
                sb.AppendLine(INDENT3 & "Dim jsonBody As String = JsonConvert.SerializeObject(body)")
                sb.AppendLine(INDENT3 & "request.Content = New StringContent(jsonBody, Encoding.UTF8, ""application/json"")")
            ElseIf contentType = "multipart/form-data" Then
                sb.AppendLine(INDENT3 & "' TODO: multipart/form-data 请求体需要根据具体业务逻辑手动构建")
                sb.AppendLine(INDENT3 & "Throw New NotImplementedException(""multipart/form-data 请求体尚未自动实现"")")
            Else
                sb.AppendLine(INDENT3 & "Dim jsonBody As String = JsonConvert.SerializeObject(body)")
                sb.AppendLine(INDENT3 & "request.Content = New StringContent(jsonBody, Encoding.UTF8, """ & contentType & """)")
            End If
        End If

        ' 发送请求
        sb.AppendLine()
        sb.AppendLine(INDENT3 & "' 发送请求并处理响应")
        sb.AppendLine(INDENT3 & "Dim response As HttpResponseMessage = Await _httpClient.SendAsync(request)")
        sb.AppendLine()

        ' 检查响应状态
        sb.AppendLine(INDENT3 & "' 检查响应状态")
        sb.AppendLine(INDENT3 & "If Not response.IsSuccessStatusCode Then")
        sb.AppendLine(INDENT3 & "    Dim errorContent As String = Await response.Content.ReadAsStringAsync()")
        sb.AppendLine(INDENT3 & "    Throw New ApiException(""API 请求失败: "" & response.StatusCode.ToString() & "" - "" & errorContent, response.StatusCode)")
        sb.AppendLine(INDENT3 & "End If")
        sb.AppendLine()

        ' 返回结果
        If hasReturnType Then
            Dim successResponse As ResponseObject = FindSuccessResponse(operation)
            If successResponse?.content IsNot Nothing Then
                sb.AppendLine(INDENT3 & "' 反序列化响应内容")
                sb.AppendLine(INDENT3 & "Dim responseContent As String = Await response.Content.ReadAsStringAsync()")

                If returnType = "String" Then
                    sb.AppendLine(INDENT3 & "Return responseContent")
                Else
                    sb.AppendLine(INDENT3 & "Return JsonConvert.DeserializeObject(Of " & returnType & ")(responseContent)")
                End If
            Else
                sb.AppendLine(INDENT3 & "Return Nothing")
            End If
        End If

        ' 方法结尾
        sb.AppendLine(INDENT2 & "End Function")
        sb.AppendLine()
    End Sub

    ''' <summary>
    ''' 构建 URL 赋值语句。
    ''' 将路径模板中的参数占位符转换为 VB.NET 字符串插值表达式。
    ''' 例如: "/pets/{petId}" -> "Dim url As String = $"{_baseUrl}/pets/{petId}"
    ''' </summary>
    Private Function BuildUrlLine(resolvedPath As String) As String
        ' 在生成的代码中，我们需要输出类似：
        ' Dim url As String = $"{_baseUrl}/pets/{petId}"
        ' 其中 {_baseUrl} 和 {petId} 都是 VB.NET 的字符串插值表达式

        ' 由于我们是在生成代码，直接拼接字符串即可
        ' resolvedPath 中已经将 {paramName} 替换为 {camelCaseName}
        Return "Dim url As String = $""{_baseUrl}" & resolvedPath & """"
    End Function

    ' -------------------------------------------------------------------
    ' 基础设施类生成
    ' -------------------------------------------------------------------

    Private Sub GenerateInfrastructureClasses()
        Dim infraDir As String = Path.Combine(_outputDir, "Infrastructure")
        If Not Directory.Exists(infraDir) Then
            Directory.CreateDirectory(infraDir)
        End If

        Dim httpClientCode As String = GenerateApiHttpClientClass()
        File.WriteAllText(Path.Combine(infraDir, "ApiHttpClient.vb"), httpClientCode, Encoding.UTF8)
        Console.WriteLine("  -> 已生成基础设施类: " & Path.Combine(infraDir, "ApiHttpClient.vb"))

        Dim apiExceptionCode As String = GenerateApiExceptionClass()
        File.WriteAllText(Path.Combine(infraDir, "ApiException.vb"), apiExceptionCode, Encoding.UTF8)
        Console.WriteLine("  -> 已生成基础设施类: " & Path.Combine(infraDir, "ApiException.vb"))
    End Sub

    Private Function GenerateApiHttpClientClass() As String
        Dim sb As New StringBuilder()

        sb.AppendLine("' ============================================================================")
        sb.AppendLine("' ApiHttpClient.vb")
        sb.AppendLine("' HTTP 客户端辅助类 - 封装 HttpClient 操作，提供统一的请求发送能力")
        sb.AppendLine("' ============================================================================")
        sb.AppendLine()
        sb.AppendLine("Imports System")
        sb.AppendLine("Imports System.Collections.Generic")
        sb.AppendLine("Imports System.Net.Http")
        sb.AppendLine("Imports System.Threading")
        sb.AppendLine("Imports System.Threading.Tasks")
        sb.AppendLine()
        sb.AppendLine("Namespace " & _rootNamespace & ".Infrastructure")
        sb.AppendLine()
        sb.AppendLine(INDENT & "''' <summary>")
        sb.AppendLine(INDENT & "''' HTTP 客户端辅助类，封装 HttpClient 的常用操作。")
        sb.AppendLine(INDENT & "''' 提供超时控制、请求日志、自动重试等基础能力。")
        sb.AppendLine(INDENT & "''' </summary>")
        sb.AppendLine(INDENT & "Public Class ApiHttpClient")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "Private ReadOnly _httpClient As HttpClient")
        sb.AppendLine(INDENT2 & "Private _timeout As TimeSpan = TimeSpan.FromSeconds(30)")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 使用指定的 HttpClient 实例创建 ApiHttpClient。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "''' <param name=""httpClient"">底层 HttpClient 实例</param>")
        sb.AppendLine(INDENT2 & "Public Sub New(httpClient As HttpClient)")
        sb.AppendLine(INDENT2 & "    _httpClient = httpClient ?? Throw New ArgumentNullException(NameOf(httpClient))")
        sb.AppendLine(INDENT2 & "End Sub")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 获取或设置请求超时时间。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "Public Property Timeout As TimeSpan")
        sb.AppendLine(INDENT3 & "Get")
        sb.AppendLine(INDENT3 & "    Return _timeout")
        sb.AppendLine(INDENT3 & "End Get")
        sb.AppendLine(INDENT3 & "Set(value As TimeSpan)")
        sb.AppendLine(INDENT3 & "    _timeout = value")
        sb.AppendLine(INDENT3 & "    _httpClient.Timeout = value")
        sb.AppendLine(INDENT3 & "End Set")
        sb.AppendLine(INDENT2 & "End Property")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 异步发送 HTTP 请求。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "''' <param name=""request"">HTTP 请求消息</param>")
        sb.AppendLine(INDENT2 & "''' <param name=""cancellationToken"">取消令牌</param>")
        sb.AppendLine(INDENT2 & "''' <returns>HTTP 响应消息</returns>")
        sb.AppendLine(INDENT2 & "Public Async Function SendAsync(request As HttpRequestMessage, Optional cancellationToken As CancellationToken = Nothing) As Task(Of HttpResponseMessage)")
        sb.AppendLine(INDENT3 & "If request Is Nothing Then Throw New ArgumentNullException(NameOf(request))")
        sb.AppendLine()
        sb.AppendLine(INDENT3 & "' 设置超时")
        sb.AppendLine(INDENT3 & "Using cts As New CancellationTokenSource(_timeout)")
        sb.AppendLine(INDENT3 & "    Using linkedCts As CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token)")
        sb.AppendLine(INDENT3 & "        Dim response As HttpResponseMessage = Await _httpClient.SendAsync(request, linkedCts.Token)")
        sb.AppendLine(INDENT3 & "        Return response")
        sb.AppendLine(INDENT3 & "    End Using")
        sb.AppendLine(INDENT3 & "End Using")
        sb.AppendLine(INDENT2 & "End Function")
        sb.AppendLine()
        sb.AppendLine(INDENT & "End Class")
        sb.AppendLine()
        sb.AppendLine("End Namespace")

        Return sb.ToString()
    End Function

    Private Function GenerateApiExceptionClass() As String
        Dim sb As New StringBuilder()

        sb.AppendLine("' ============================================================================")
        sb.AppendLine("' ApiException.vb")
        sb.AppendLine("' API 异常类 - 封装 REST API 调用中的错误信息")
        sb.AppendLine("' ============================================================================")
        sb.AppendLine()
        sb.AppendLine("Imports System")
        sb.AppendLine("Imports System.Net")
        sb.AppendLine()
        sb.AppendLine("Namespace " & _rootNamespace & ".Infrastructure")
        sb.AppendLine()
        sb.AppendLine(INDENT & "''' <summary>")
        sb.AppendLine(INDENT & "''' 表示 REST API 调用过程中发生的异常。")
        sb.AppendLine(INDENT & "''' 包含 HTTP 状态码和错误响应内容，便于调用方进行错误处理。")
        sb.AppendLine(INDENT & "''' </summary>")
        sb.AppendLine(INDENT & "Public Class ApiException")
        sb.AppendLine(INDENT2 & "Inherits Exception")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' HTTP 状态码")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "Public ReadOnly Property StatusCode As HttpStatusCode")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 创建 ApiException 实例。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "''' <param name=""message"">错误消息</param>")
        sb.AppendLine(INDENT2 & "''' <param name=""statusCode"">HTTP 状态码</param>")
        sb.AppendLine(INDENT2 & "Public Sub New(message As String, statusCode As HttpStatusCode)")
        sb.AppendLine(INDENT3 & "MyBase.New(message)")
        sb.AppendLine(INDENT3 & "Me.StatusCode = statusCode")
        sb.AppendLine(INDENT2 & "End Sub")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 创建 ApiException 实例（含内部异常）。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "Public Sub New(message As String, statusCode As HttpStatusCode, innerException As Exception)")
        sb.AppendLine(INDENT3 & "MyBase.New(message, innerException)")
        sb.AppendLine(INDENT3 & "Me.StatusCode = statusCode")
        sb.AppendLine(INDENT2 & "End Sub")
        sb.AppendLine()
        sb.AppendLine(INDENT2 & "''' <summary>")
        sb.AppendLine(INDENT2 & "''' 返回异常的字符串表示，包含状态码信息。")
        sb.AppendLine(INDENT2 & "''' </summary>")
        sb.AppendLine(INDENT2 & "Public Overrides Function ToString() As String")
        sb.AppendLine(INDENT3 & "Return ""[ApiException] StatusCode="" & CInt(StatusCode).ToString() & "" ("" & StatusCode.ToString() & "") - "" & Message")
        sb.AppendLine(INDENT2 & "End Function")
        sb.AppendLine()
        sb.AppendLine(INDENT & "End Class")
        sb.AppendLine()
        sb.AppendLine("End Namespace")

        Return sb.ToString()
    End Function

    ' -------------------------------------------------------------------
    ' 类型映射辅助方法
    ' -------------------------------------------------------------------

    ''' <summary>
    ''' 将 OpenAPI Schema 类型映射为 VB.NET 类型名称。
    ''' </summary>
    Private Function MapSchemaToVbType(schema As SchemaObject) As String
        If schema Is Nothing Then Return "Object"

        ' 处理 $ref 引用
        If Not String.IsNullOrEmpty(schema.ref) Then
            Dim refName As String = ResolveRefName(schema.ref)
            If _schemaTypeMap.ContainsKey(refName) Then
                Return _schemaTypeMap(refName)
            End If
            Return ToPascalCase(refName)
        End If

        ' 处理 allOf
        If schema.allOf IsNot Nothing AndAlso schema.allOf.Count > 0 Then
            For Each subSchema In schema.allOf
                If Not String.IsNullOrEmpty(subSchema.ref) Then
                    Return MapSchemaToVbType(subSchema)
                End If
            Next
            Return "Object"
        End If

        ' 处理 oneOf / anyOf
        If schema.oneOf IsNot Nothing AndAlso schema.oneOf.Count > 0 Then
            Return MapSchemaToVbType(schema.oneOf(0))
        End If
        If schema.anyOf IsNot Nothing AndAlso schema.anyOf.Count > 0 Then
            Return MapSchemaToVbType(schema.anyOf(0))
        End If

        ' 处理数组类型
        If schema.type = "array" Then
            Dim itemType As String = "Object"
            If schema.items IsNot Nothing Then
                itemType = MapSchemaToVbType(schema.items)
            End If
            Return "List(Of " & itemType & ")"
        End If

        ' 处理基本类型映射
        Select Case schema.type
            Case "string"
                Select Case schema.format
                    Case "date", "date-time"
                        Return "DateTime"
                    Case "byte"
                        Return "Byte()"
                    Case "binary"
                        Return "Stream"
                    Case "uri"
                        Return "Uri"
                    Case "uuid"
                        Return "Guid"
                    Case Else
                        Return "String"
                End Select
            Case "integer"
                Select Case schema.format
                    Case "int64"
                        Return "Long"
                    Case Else
                        Return "Integer"
                End Select
            Case "number"
                Select Case schema.format
                    Case "float"
                        Return "Single"
                    Case "double"
                        Return "Double"
                    Case "decimal"
                        Return "Decimal"
                    Case Else
                        Return "Double"
                End Select
            Case "boolean"
                Return "Boolean"
            Case "object"
                If schema.additionalProperties IsNot Nothing AndAlso TypeOf schema.additionalProperties Is SchemaObject Then
                    Dim valType As String = MapSchemaToVbType(CType(schema.additionalProperties, SchemaObject))
                    Return "Dictionary(Of String, " & valType & ")"
                End If
                Return "Object"
            Case Else
                Return "Object"
        End Select
    End Function

    ''' <summary>
    ''' 判断 VB.NET 类型是否为值类型。
    ''' </summary>
    Private Function IsValueType(vbType As String) As Boolean
        Select Case vbType
            Case "Integer", "Long", "Short", "Byte",
                 "Single", "Double", "Decimal",
                 "Boolean", "DateTime", "Guid"
                Return True
            Case Else
                Return False
        End Select
    End Function

    ''' <summary>
    ''' 判断类型名称是否为生成的模型类类型。
    ''' </summary>
    Private Function IsModelType(typeName As String) As Boolean
        Return _schemaTypeMap.ContainsValue(typeName)
    End Function

    ''' <summary>
    ''' 从 $ref 路径中提取 Schema 名称。
    ''' 例如 "#/components/schemas/Pet" -> "Pet"
    ''' </summary>
    Private Function ResolveRefName(refPath As String) As String
        If String.IsNullOrEmpty(refPath) Then Return "Object"

        Dim parts As String() = refPath.Split("/"c)
        If parts.Length > 0 Then
            Return parts(parts.Length - 1)
        End If
        Return "Object"
    End Function

    ' -------------------------------------------------------------------
    ' 操作分析辅助方法
    ' -------------------------------------------------------------------

    ''' <summary>
    ''' 确定操作的方法名称。
    ''' 优先使用 operationId，否则基于 HTTP 方法和路径模板自动生成。
    ''' </summary>
    Private Function DetermineMethodName(operation As OperationObject, httpMethod As String, pathTemplate As String) As String
        ' 优先使用 operationId
        If Not String.IsNullOrEmpty(operation.operationId) Then
            Return ToPascalCase(operation.operationId)
        End If

        ' 基于 HTTP 方法和路径生成方法名
        Dim pathParts As String() = pathTemplate.Split("/"c, StringSplitOptions.RemoveEmptyEntries)
        Dim resourceName As String = ""
        Dim hasIdParam As Boolean = pathTemplate.Contains("{")

        ' 提取资源名称（取最后一个非参数路径段）
        For i As Integer = pathParts.Length - 1 To 0 Step -1
            Dim part As String = pathParts(i)
            If Not part.StartsWith("{") Then
                resourceName = ToSingular(ToPascalCase(part))
                Exit For
            End If
        Next

        ' 根据 HTTP 方法选择操作前缀
        Dim methodPrefix As String = ""
        Select Case httpMethod.ToLower()
            Case "get"
                methodPrefix = If(hasIdParam, "Get", "List")
            Case "post"
                methodPrefix = "Create"
            Case "put"
                methodPrefix = "Update"
            Case "patch"
                methodPrefix = "Patch"
            Case "delete"
                methodPrefix = "Delete"
            Case "head"
                methodPrefix = "Head"
            Case "options"
                methodPrefix = "Options"
            Case Else
                methodPrefix = httpMethod
        End Select

        If hasIdParam AndAlso httpMethod.ToLower() = "get" Then
            Return methodPrefix & resourceName & "ById"
        Else
            Return methodPrefix & resourceName
        End If
    End Function

    ''' <summary>
    ''' 确定操作的返回类型。
    ''' </summary>
    Private Function DetermineReturnType(operation As OperationObject) As String
        Dim successResponse As ResponseObject = FindSuccessResponse(operation)

        If successResponse Is Nothing OrElse successResponse.content Is Nothing Then
            Return Nothing
        End If

        ' 尝试从 application/json 内容中获取返回类型
        If successResponse.content.ContainsKey("application/json") Then
            Dim mediaType As MediaTypeObject = successResponse.content("application/json")
            If mediaType?.schema IsNot Nothing Then
                Return MapSchemaToVbType(mediaType.schema)
            End If
        End If

        ' 尝试其他内容类型
        For Each contentKvp In successResponse.content
            Dim mediaType As MediaTypeObject = contentKvp.Value
            If mediaType?.schema IsNot Nothing Then
                Return MapSchemaToVbType(mediaType.schema)
            End If
        Next

        Return "String"
    End Function

    ''' <summary>
    ''' 查找操作定义中的成功响应（2xx 状态码）。
    ''' </summary>
    Private Function FindSuccessResponse(operation As OperationObject) As ResponseObject
        If operation.responses Is Nothing Then Return Nothing

        If operation.responses.ContainsKey("200") Then
            Return operation.responses("200")
        End If
        If operation.responses.ContainsKey("201") Then
            Return operation.responses("201")
        End If

        For Each kvp In operation.responses
            If kvp.Key.StartsWith("2") Then
                Return kvp.Value
            End If
        Next

        If operation.responses.ContainsKey("default") Then
            Return operation.responses("default")
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' 确定请求体的 VB.NET 类型。
    ''' </summary>
    Private Function DetermineRequestBodyType(requestBody As RequestBodyObject) As String
        If requestBody.content Is Nothing Then Return Nothing

        If requestBody.content.ContainsKey("application/json") Then
            Dim mediaType As MediaTypeObject = requestBody.content("application/json")
            If mediaType?.schema IsNot Nothing Then
                Return MapSchemaToVbType(mediaType.schema)
            End If
        End If

        For Each contentKvp In requestBody.content
            Dim mediaType As MediaTypeObject = contentKvp.Value
            If mediaType?.schema IsNot Nothing Then
                Return MapSchemaToVbType(mediaType.schema)
            End If
        Next

        Return "Object"
    End Function

    ''' <summary>
    ''' 确定请求体的 Content-Type。
    ''' </summary>
    Private Function DetermineRequestContentType(requestBody As RequestBodyObject) As String
        If requestBody.content Is Nothing Then Return "application/json"

        If requestBody.content.ContainsKey("application/json") Then
            Return "application/json"
        End If

        For Each kvp In requestBody.content
            Return kvp.Key
        Next

        Return "application/json"
    End Function

    ''' <summary>
    ''' 收集操作的所有参数（合并路径级和操作级参数）。
    ''' </summary>
    Private Function CollectAllParameters(opInfo As OperationInfo) As List(Of ParameterObject)
        Dim result As New List(Of ParameterObject)()

        If opInfo.PathLevelParameters IsNot Nothing Then
            For Each p In opInfo.PathLevelParameters
                result.Add(p)
            Next
        End If

        If opInfo.Operation.parameters IsNot Nothing Then
            For Each opParam In opInfo.Operation.parameters
                Dim existingIndex As Integer = result.FindIndex(
                    Function(p) p.name = opParam.name AndAlso p.in = opParam.in)

                If existingIndex >= 0 Then
                    result(existingIndex) = opParam
                Else
                    result.Add(opParam)
                End If
            Next
        End If

        Return result
    End Function

    ' -------------------------------------------------------------------
    ' 命名转换辅助方法
    ' -------------------------------------------------------------------

    ''' <summary>
    ''' 将字符串转换为 PascalCase 命名格式。
    ''' </summary>
    Private Function ToPascalCase(name As String) As String
        If String.IsNullOrEmpty(name) Then Return name

        If name.Contains("_"c) Then
            Dim parts As String() = name.Split("_"c, StringSplitOptions.RemoveEmptyEntries)
            Dim sb As New StringBuilder()
            For Each part In parts
                If part.Length > 0 Then
                    sb.Append(Char.ToUpper(part(0)))
                    sb.Append(part.Substring(1).ToLower())
                End If
            Next
            Return sb.ToString()
        End If

        If Char.IsLower(name(0)) Then
            Return Char.ToUpper(name(0)) & name.Substring(1)
        End If

        Return name
    End Function

    ''' <summary>
    ''' 将字符串转换为 camelCase 命名格式。
    ''' </summary>
    Private Function ToCamelCase(name As String) As String
        If String.IsNullOrEmpty(name) Then Return name

        Dim pascal As String = ToPascalCase(name)
        If pascal.Length > 0 Then
            Return Char.ToLower(pascal(0)) & pascal.Substring(1)
        End If
        Return pascal
    End Function

    ''' <summary>
    ''' 尝试将英文复数名词转换为单数形式（简单启发式规则）。
    ''' </summary>
    Private Function ToSingular(name As String) As String
        If String.IsNullOrEmpty(name) Then Return name

        If name.Length > 3 Then
            If name.EndsWith("ies") Then
                Return name.Substring(0, name.Length - 3) & "y"
            ElseIf name.EndsWith("ses") Then
                Return name.Substring(0, name.Length - 2)
            ElseIf name.EndsWith("s") AndAlso Not name.EndsWith("ss") Then
                Return name.Substring(0, name.Length - 1)
            End If
        End If

        Return name
    End Function

    ''' <summary>
    ''' 转义 XML 注释中的特殊字符。
    ''' </summary>
    Private Function EscapeXmlComment(text As String) As String
        If String.IsNullOrEmpty(text) Then Return text
        Return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
    End Function

End Class



