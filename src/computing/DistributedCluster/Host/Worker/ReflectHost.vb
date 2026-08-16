Imports System.IO
Imports System.Reflection
Imports Microsoft.VisualBasic.App
Imports ClusterShared

''' <summary>
''' 反射计算子进程宿主。由节点守护进程通过 Process.Start 启动。
''' 命令行：Host.exe --mode=worker {blockId} {jobId} {assemblyPath} {methodName} {jobRoot}
''' 职责：
'''   1. 从 SMB 读取数据块文件 {jobRoot}/blocks/{blockId}
'''   2. 反射加载 CLR assembly 并调用 methodName
'''   3. 将计算结果写回 {jobRoot}/results/{blockId}
'''   4. 异常时记录描述与栈追踪到 {jobRoot}/logs/{blockId}.log，并设置 ExitCode=1
'''   5. 成功设置 ExitCode=0
''' </summary>
Public Class ReflectHost

    Public Shared Function Run(args As String()) As Integer
        Try
            ' args(0) = --mode=worker，后续为位置参数
            Dim blockId = args(1)
            Dim jobId = args(2)
            Dim assemblyPath = args(3)
            Dim methodName = args(4)
            Dim jobRoot = args(5)

            Dim smb As New SmbPaths(jobRoot) ' jobRoot 已是 jobs/{jobId}
            ' 修正：jobRoot 已经是 job 目录，这里 SmbPaths.Root 应为 jobs 父目录。
            ' 为兼容，直接将 paths 基于 jobRoot 构造。
            Dim blockFile = Path.Combine(jobRoot, "blocks", blockId)
            Dim resultFile = Path.Combine(jobRoot, "results", blockId)
            Dim logFile = Path.Combine(jobRoot, "logs", blockId & ".log")

            For Each d In {Path.GetDirectoryName(resultFile), Path.GetDirectoryName(logFile)}
                If Not Directory.Exists(d) Then Directory.CreateDirectory(d)
            Next

            Console.WriteLine($"[worker] block={blockId} assembly={assemblyPath} method={methodName}")

            ' 反射加载 assembly（使用 Assembly.LoadFrom 以支持任意外部 CLR dll）。
            Dim asm As Assembly = Assembly.LoadFrom(assemblyPath)

            ' methodName 形如 Namespace.Class.Method，定位类型与方法。
            Dim parts = methodName.Split("."c)
            If parts.Length < 3 Then
                Throw New ArgumentException($"methodName 格式应为 Namespace.Class.Method，实际：{methodName}")
            End If

            Dim typeName = String.Join(".", parts.Take(parts.Length - 1))
            Dim methodNameOnly = parts(parts.Length - 1)
            Dim type = asm.GetType(typeName, throwOnError:=True)
            Dim method = type.GetMethod(methodNameOnly, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Static Or BindingFlags.Instance)

            If method Is Nothing Then
                Throw New MissingMethodException(typeName, methodNameOnly)
            End If

            ' 读取输入数据块（字节）。
            Dim inputData As Byte() = If(File.Exists(blockFile), File.ReadAllBytes(blockFile), New Byte() {})

            ' 构造调用参数：约定 (byte[] input, string blockId, string jobRoot) 或单参数 byte[]。
            Dim invokeArgs As Object() = BuildArgs(method, inputData, blockId, jobRoot)
            Dim instance As Object = If(method.IsStatic, Nothing, Activator.CreateInstance(type))

            Dim result = method.Invoke(instance, invokeArgs)

            ' 序列化结果写回 SMB。支持 byte[] / String / 其他（GetJson）。
            Dim output As Byte()
            If TypeOf result Is Byte() Then
                output = DirectCast(result, Byte())
            ElseIf TypeOf result Is String Then
                output = System.Text.Encoding.UTF8.GetBytes(DirectCast(result, String))
            ElseIf result Is Nothing Then
                output = System.Text.Encoding.UTF8.GetBytes("")
            Else
                output = System.Text.Encoding.UTF8.GetBytes(result.GetJson())
            End If

            File.WriteAllBytes(resultFile, output)
            Console.WriteLine($"[worker] 完成，结果已写入 {resultFile}")
            Return 0

        Catch ex As Exception
            ' 捕获所有异常，记录描述与栈追踪到 stdout（由守护进程归档）。
            Console.Error.WriteLine($"[worker][error] {ex.GetType().Name}: {ex.Message}")
            Console.Error.WriteLine(ex.StackTrace)
            File.AppendAllText(
                Path.Combine(If(args.Length > 5, args(5), "."), "logs", If(args.Length > 1, args(1), "unknown") & ".log"),
                $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}{vbCrLf}{ex.StackTrace}{vbCrLf}")
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' 根据方法签名构造调用参数，优先匹配 (byte[], string, string)。
    ''' </summary>
    Private Shared Function BuildArgs(method As MethodInfo, input As Byte(), blockId As String, jobRoot As String) As Object()
        Dim ps = method.GetParameters()

        If ps.Length = 0 Then
            Return New Object() {}
        End If

        Dim list As New List(Of Object)()

        For Each p In ps
            If p.ParameterType Is GetType(Byte()) Then
                list.Add(input)
            ElseIf p.ParameterType Is GetType(String) Then
                ' 第一个 string 给 blockId，第二个给 jobRoot。
                If list.Count = 1 Then
                    list.Add(blockId)
                Else
                    list.Add(jobRoot)
                End If
            Else
                list.Add(Nothing)
            End If
        Next

        Return list.ToArray()
    End Function
End Class
