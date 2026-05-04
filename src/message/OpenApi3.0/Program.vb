' ============================================================================
' Program.vb
' ============================================================================
' OpenAPI 3.0.1 VB.NET 代码生成器 - 程序入口
'
' 本程序是代码生成器的主入口点，负责：
'   1. 接收命令行参数（YAML 文件路径、输出目录等）
'   2. 使用 LoadYAML(Of T) 反序列化 OpenAPI YAML 文档
'   3. 调用 OpenApiCodeGenerator 生成 VB.NET 代码文件
'
' 使用方式:
'   OpenApiCodeGenerator.exe <yaml文件路径> [输出目录] [根命名空间]
'
' 示例:
'   OpenApiCodeGenerator.exe petstore.yaml
'   OpenApiCodeGenerator.exe petstore.yaml ./output PetStoreApi
' ============================================================================

Imports System
Imports System.IO
Imports System.Collections.Generic
Imports Microsoft.VisualBasic.MIME.text.yaml.Serialization
Imports OpenApi.CodeGenerator.Models

Namespace OpenApi.CodeGenerator

    Module Program

        ''' <summary>
        ''' 程序主入口点。
        ''' </summary>
        ''' <param name="args">
        ''' 命令行参数:
        '''   args(0) - OpenAPI YAML 文件路径（必填）
        '''   args(1) - 输出目录（可选，默认为 "./Generated"）
        '''   args(2) - 根命名空间（可选，默认为 "GeneratedApi"）
        ''' </param>
        Sub Main(args As String())
            Console.WriteLine("============================================================")
            Console.WriteLine("  OpenAPI 3.0.1 VB.NET 代码生成器")
            Console.WriteLine("  基于 Microsoft.VisualBasic.MIME.text.yaml.Serialization")
            Console.WriteLine("============================================================")
            Console.WriteLine()

            ' ---------------------------------------------------------------
            ' 第一步：解析命令行参数
            ' ---------------------------------------------------------------
            If args.Length = 0 Then
                PrintUsage()
                Return
            End If

            Dim yamlPath As String = args(0)
            Dim outputDir As String = If(args.Length > 1, args(1), "./Generated")
            Dim rootNamespace As String = If(args.Length > 2, args(2), "GeneratedApi")

            ' 验证 YAML 文件是否存在
            If Not File.Exists(yamlPath) Then
                Console.WriteLine($"[错误] 找不到 YAML 文件: {yamlPath}")
                PrintUsage()
                Return
            End If

            Console.WriteLine($"[配置] YAML 文件: {Path.GetFullPath(yamlPath)}")
            Console.WriteLine($"[配置] 输出目录: {Path.GetFullPath(outputDir)}")
            Console.WriteLine($"[配置] 根命名空间: {rootNamespace}")
            Console.WriteLine()

            ' ---------------------------------------------------------------
            ' 第二步：使用 LoadYAML(Of T) 反序列化 YAML 文档
            ' ---------------------------------------------------------------
            Console.WriteLine("[步骤1] 正在解析 OpenAPI YAML 文档...")

            Dim doc As OpenApiDocument = Nothing
            Try
                ' 使用指定的 LoadYAML API 进行反序列化
                ' LoadYAML 会读取 YAML 文件并将其映射到 OpenApiDocument 类的属性
                doc = LoadYAML(Of OpenApiDocument)(yamlPath)
                Console.WriteLine("[步骤1] YAML 文档解析成功！")
            Catch ex As Exception
                Console.WriteLine($"[错误] YAML 文档解析失败: {ex.Message}")
                Console.WriteLine($"        请确认文件格式符合 OpenAPI 3.0.1 规范。")
                Return
            End Try

            ' ---------------------------------------------------------------
            ' 第三步：验证文档基本信息
            ' ---------------------------------------------------------------
            Console.WriteLine()
            Console.WriteLine("[步骤2] 验证文档信息...")

            If doc Is Nothing Then
                Console.WriteLine("[错误] 反序列化结果为空。")
                Return
            End If

            ' 检查 OpenAPI 版本
            If String.IsNullOrEmpty(doc.openapi) Then
                Console.WriteLine("[警告] 文档未指定 openapi 版本，将尝试按 3.0.x 规范处理。")
            ElseIf Not doc.openapi.StartsWith("3.0") Then
                Console.WriteLine($"[警告] 文档版本为 {doc.openapi}，本工具针对 OpenAPI 3.0.x 规范设计，可能存在兼容性问题。")
            Else
                Console.WriteLine($"  OpenAPI 版本: {doc.openapi}")
            End If

            ' 显示 API 基本信息
            If doc.info IsNot Nothing Then
                Console.WriteLine($"  API 标题: {doc.info.title}")
                Console.WriteLine($"  API 版本: {doc.info.version}")
                If Not String.IsNullOrEmpty(doc.info.description) Then
                    Console.WriteLine($"  API 描述: {doc.info.description.Substring(0, Math.Min(doc.info.description.Length, 100))}...")
                End If
            End If

            ' 统计文档内容
            Dim pathCount As Integer = If(doc.paths?.Count, 0)
            Dim schemaCount As Integer = If(doc.components?.schemas?.Count, 0)
            Dim operationCount As Integer = 0

            If doc.paths IsNot Nothing Then
                For Each pathKvp In doc.paths
                    operationCount += pathKvp.Value?.Operations?.Count() ?? 0
                Next
            End If

            Console.WriteLine($"  路径数量: {pathCount}")
            Console.WriteLine($"  操作数量: {operationCount}")
            Console.WriteLine($"  Schema 数量: {schemaCount}")
            Console.WriteLine()

            ' ---------------------------------------------------------------
            ' 第四步：执行代码生成
            ' ---------------------------------------------------------------
            Console.WriteLine("[步骤3] 开始生成 VB.NET 代码...")

            Try
                Dim generator As New OpenApiCodeGenerator(doc, outputDir, rootNamespace)
                generator.GenerateAll()
            Catch ex As Exception
                Console.WriteLine($"[错误] 代码生成过程中发生异常: {ex.Message}")
                Console.WriteLine($"        堆栈跟踪: {ex.StackTrace}")
                Return
            End Try

            ' ---------------------------------------------------------------
            ' 第五步：输出结果摘要
            ' ---------------------------------------------------------------
            Console.WriteLine()
            Console.WriteLine("============================================================")
            Console.WriteLine("  代码生成完成！")
            Console.WriteLine("============================================================")
            Console.WriteLine()
            Console.WriteLine($"  输出目录: {Path.GetFullPath(outputDir)}")
            Console.WriteLine()
            Console.WriteLine("  生成的文件结构:")
            Console.WriteLine($"    {outputDir}/")
            Console.WriteLine($"      Models/          - 数据模型类（基于 components/schemas）")
            Console.WriteLine($"      Clients/         - REST API 客户端类（基于 paths）")
            Console.WriteLine($"      Infrastructure/  - 基础设施类（ApiHttpClient, ApiException）")
            Console.WriteLine()
            Console.WriteLine("  使用说明:")
            Console.WriteLine("    1. 将生成的代码文件添加到 VB.NET 项目中")
            Console.WriteLine("    2. 安装 Newtonsoft.Json NuGet 包")
            Console.WriteLine("    3. 创建客户端实例并调用 API 方法，例如:")
            Console.WriteLine($"       Dim client As New {rootNamespace}.Clients.PetClient(""https://api.example.com"")")
            Console.WriteLine("       Dim result = Await client.ListPets()")
            Console.WriteLine()
        End Sub

        ''' <summary>
        ''' 打印使用说明。
        ''' </summary>
        Private Sub PrintUsage()
            Console.WriteLine("使用方式:")
            Console.WriteLine("  OpenApiCodeGenerator.exe <yaml文件路径> [输出目录] [根命名空间]")
            Console.WriteLine()
            Console.WriteLine("参数说明:")
            Console.WriteLine("  yaml文件路径  - OpenAPI 3.0.1 规范的 YAML 文件路径（必填）")
            Console.WriteLine("  输出目录      - 生成代码的输出目录（可选，默认: ./Generated）")
            Console.WriteLine("  根命名空间    - 生成代码的根命名空间（可选，默认: GeneratedApi）")
            Console.WriteLine()
            Console.WriteLine("示例:")
            Console.WriteLine("  OpenApiCodeGenerator.exe petstore.yaml")
            Console.WriteLine("  OpenApiCodeGenerator.exe petstore.yaml ./output PetStoreApi")
        End Sub

    End Module

End Namespace
