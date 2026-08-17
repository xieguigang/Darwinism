Imports System.IO
Imports System.Reflection
Imports System.Runtime.Loader
Imports Microsoft.VisualBasic.ApplicationServices.Development.XmlDoc.Assembly
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace ClusterShared

    ''' <summary>
    ''' 在独立 AssemblyLoadContext 中反射加载目标 dll，扫描符合 worker 计算进程
    ''' （<see cref="Worker.ReflectHost"/>）调用约定的公共方法，并从同名 .NET XML
    ''' 注释文档解析 summary / remarks 文本，随后卸载程序集。
    ''' </summary>
    ''' <remarks>
    ''' 约定来自 Worker\ReflectHost.vb 的 BuildArgs：方法参数类型集合仅允许
    ''' Byte()（映射 input）/ String（第一个=blockId，第二个=jobRoot）/ 其他（填 Nothing）。
    ''' 因此可调用的方法需满足：所有参数类型为 Byte() 或 String，且 String 参数至多 2 个。
    ''' </remarks>
    Public Class AssemblyScanner

        ''' <summary>
        ''' 可被 worker 反射调用的参数类型白名单。
        ''' </summary>
        Private Shared ReadOnly allowedTypes As Type() = {GetType(Byte()), GetType(String)}

        ''' <summary>
        ''' 自定义可卸载的程序集加载上下文。解析依赖时回退到默认上下文（共享运行时类型）。
        ''' </summary>
        Private Class UnloadableContext : Inherits AssemblyLoadContext
            Public Sub New()
                MyBase.New(isCollectible:=True)
            End Sub

            Protected Overrides Function Load(assemblyName As AssemblyName) As Assembly
                ' 依赖在默认上下文加载，避免重复加载到本上下文造成卸载困难
                Return Nothing
            End Function
        End Class

        ''' <summary>
        ''' 扫描目标 dll，返回符合 worker 调用约定的方法列表（含 XML 注释）。
        ''' dll 在独立可卸载上下文加载，扫描完成后强制 GC 回收以卸载。
        ''' </summary>
        ''' <param name="dllPath">目标 dll 的完整路径（需可被当前进程访问，一般位于 webRoot 下 smb 共享）。</param>
        ''' <returns>方法列表；加载失败则返回空列表并在 message 中说明。</returns>
        Public Shared Function Scan(dllPath As String, ByRef message As String) As AssemblyMethod()
            message = ""

            If Not File.Exists(dllPath) Then
                message = $"找不到目标程序集文件: {dllPath}"
                Return New AssemblyMethod() {}
            End If

            Dim ctx As UnloadableContext = Nothing
            Dim asm As Assembly = Nothing

            Try
                ctx = New UnloadableContext()
                ' 使用物理路径加载，避免默认上下文已加载同名程序集导致无法卸载
                asm = ctx.LoadFromAssemblyPath(Path.GetFullPath(dllPath))
            Catch ex As Exception
                message = $"加载程序集失败: {ex.Message}"
                Return New AssemblyMethod() {}
            End Try

            Dim docs As ProjectSpace = LoadXmlDocs(dllPath)
            Dim results As New List(Of AssemblyMethod)

            Try
                For Each t As Type In asm.GetTypes()
                    ' 跳过编译器生成 / 抽象 / 接口 / 泛型
                    If t.IsAbstract OrElse t.IsInterface OrElse t.IsGenericTypeDefinition Then
                        Continue For
                    End If

                    For Each m As MethodInfo In t.GetMethods(BindingFlags.Public Or BindingFlags.Static Or BindingFlags.Instance)
                        If Not IsWorkerCallable(m) Then
                            Continue For
                        End If

                        Dim ns As String = If(t.Namespace, "")
                        Dim cls As String = t.Name
                        Dim summary As String = ""
                        Dim remarks As String = ""

                        Call ResolveComments(docs, ns, cls, m.Name, summary, remarks)

                        results.Add(New AssemblyMethod With {
                            .namespace = ns,
                            .class = cls,
                            .method = m.Name,
                            .signature = $"{ns}.{cls}.{m.Name}({ParamSig(m)})",
                            .summary = summary,
                            .remarks = remarks
                        })
                    Next
                Next
            Catch ex As Exception
                ' 反射扫描异常不应泄漏到 web 端；记录并继续返回已扫描结果
                message = $"部分类型无法反射（已跳过）: {ex.Message}"
            Finally
                ' 强制卸载程序集，回收内存（避免 smb 重复扫描造成句柄/内存累积）
                If ctx IsNot Nothing Then
                    Try
                        ctx.Unload()
                    Catch
                    End Try
                End If

                asm = Nothing
                ctx = Nothing
                GC.Collect()
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try

            Return results.ToArray
        End Function

        ''' <summary>
        ''' 判断方法是否符合 worker 调用约定：参数类型仅允许 Byte() / String，且 String 至多 2 个。
        ''' 含其他类型参数（会被填 Nothing）时不视为可调用的计算入口。
        ''' </summary>
        Private Shared Function IsWorkerCallable(m As MethodInfo) As Boolean
            Dim stringCount As Integer = 0

            For Each p As ParameterInfo In m.GetParameters()
                Dim pt = p.ParameterType

                If pt.IsArray AndAlso pt.GetElementType() Is GetType(Byte) Then
                    Continue For
                End If

                If pt Is GetType(String) Then
                    stringCount += 1

                    If stringCount > 2 Then
                        Return False
                    End If

                    Continue For
                End If

                ' 含其他参数类型，无法由 BuildArgs 构造，排除
                Return False
            Next

            Return True
        End Function

        ''' <summary>
        ''' 生成参数签名片段，例如 "Byte[], String, String"。
        ''' </summary>
        Private Shared Function ParamSig(m As MethodInfo) As String
            Return m.GetParameters() _
                      .Select(Function(p) TypeSig(p.ParameterType)) _
                      .JoinBy(", ")
        End Function

        Private Shared Function TypeSig(t As Type) As String
            If t.IsArray AndAlso t.GetElementType() Is GetType(Byte) Then
                Return "Byte[]"
            ElseIf t Is GetType(String) Then
                Return "String"
            Else
                Return t.Name
            End If
        End Function

        ''' <summary>
        ''' 加载与目标 dll 同名的 .NET XML 注释文档（dll 名去扩展名 + .xml）。
        ''' 文档缺失时返回空 ProjectSpace（注释留空）。
        ''' </summary>
        Private Shared Function LoadXmlDocs(dllPath As String) As ProjectSpace
            Dim xmlPath = Path.ChangeExtension(dllPath, ".xml")
            Dim ps As New ProjectSpace()

            If File.Exists(xmlPath) Then
                Try
                    Call ps.ImportFromXmlDocFile(xmlPath)
                Catch
                    ' 注释文档损坏不影响主流程
                End Try
            End If

            Return ps
        End Function

        ''' <summary>
        ''' 从 XML 注释项目取指定方法的 summary / remarks。
        ''' 通过 ProjectSpace（IEnumerable(Of Project)）逐个查找类型
        ''' [GetType]("Ns.Class").GetMethods(name) 取首个重载注释。
        ''' </summary>
        Private Shared Sub ResolveComments(docs As ProjectSpace, ns$, cls$, methodName$, ByRef summary$, ByRef remarks$)
            Dim pt As ProjectType = Nothing

            For Each proj As Project In docs
                pt = proj.GetType($"{ns}.{cls}")

                If pt IsNot Nothing Then
                    Exit For
                End If
            Next

            If pt Is Nothing Then
                Return
            End If

            Dim methods = pt.GetMethods(methodName)

            If methods Is Nothing OrElse methods.Count = 0 Then
                Return
            End If

            ' 取第一个重载的注释即可
            summary = methods(0).Summary
            remarks = methods(0).Remarks
        End Sub
    End Class
End Namespace
