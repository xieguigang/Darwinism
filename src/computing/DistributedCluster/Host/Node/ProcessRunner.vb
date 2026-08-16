Imports System.Diagnostics
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Shared

''' <summary>
''' 封装反射 worker 子进程的启动、stdout 读取、心跳归档与回执逻辑。
''' </summary>
Public Class ProcessRunner

    Private ReadOnly cfg As Config
    Private ReadOnly smb As SmbPaths
    Private ReadOnly http As New HttpClient()

    Sub New(cfg As Config)
        Me.cfg = cfg
        Me.smb = New SmbPaths(cfg.smbRoot)
    End Sub

    ''' <summary>
    ''' 运行一个数据块：启动 worker 子进程，持续读取 stdout 归档并每轮发送心跳，
    ''' 最后依据 ExitCode 向头结点回执 done/failed。
    ''' </summary>
    Public Sub RunBlock(block As TaskBlock)
        Dim logPath = smb.LogPath(block.jobId, block.blockId)
        Dim logDir = Path.GetDirectoryName(logPath)
        If Not Directory.Exists(logDir) Then Directory.CreateDirectory(logDir)

        ' 子进程参数：--mode=worker blockId jobId assemblyPath methodName jobRoot
        Dim psi As New ProcessStartInfo() With {
            .FileName = cfg.WorkerExecutable,
            .Arguments = String.Join(" ",
                "--mode=worker",
                block.blockId,
                block.jobId,
                """" & block.assemblyPath & """",
                """" & block.methodName & """",
                """" & block.jobRoot & """"),
            .UseShellExecute = False,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True,
            .CreateNoWindow = True
        }

        Using proc As New Process()
            proc.StartInfo = psi

            ' 异步读取 stdout / stderr，避免阻塞子进程。
            Dim sbOut As New StringBuilder()
            Dim logLock As New Object()

            proc.Start()

            proc.BeginOutputReadLine()
            proc.BeginErrorReadLine()

            AddHandler proc.OutputDataReceived, Sub(sender, e)
                                                    If e.Data Is Nothing Then Return
                                                    SyncLock logLock
                                                        sbOut.AppendLine(e.Data)
                                                        File.AppendAllText(logPath, e.Data & vbCrLf)
                                                    End SyncLock
                                                    Call SendHeartbeat(block, e.Data)
                                                End Sub

            AddHandler proc.ErrorDataReceived, Sub(sender, e)
                                                   If e.Data Is Nothing Then Return
                                                   SyncLock logLock
                                                       sbOut.AppendLine("[err] " & e.Data)
                                                       File.AppendAllText(logPath, "[err] " & e.Data & vbCrLf)
                                                   End SyncLock
                                                   Call SendHeartbeat(block, "[err] " & e.Data)
                                               End Sub

            ' 等待子进程退出（带超时保护，避免僵尸进程）。
            If Not proc.WaitForExit(timeoutMilliseconds:=cfg.pollInterval * 600) Then
                Try
                    proc.Kill()
                Catch
                End Try
                Call ReportFailed(block, "子进程超时未退出，已被强制终止", "")
                Return
            End If

            Dim output = sbOut.ToString()

            If proc.ExitCode = 0 Then
                Call ReportDone(block)
            Else
                ' 从归档日志提取异常信息（worker 已写入描述与栈追踪）。
                Dim message = "worker 退出码 " & proc.ExitCode
                Dim stack = output
                Call ReportFailed(block, message, stack)
            End If
        End Using
    End Sub

    ' ============ 与头结点通信 ============

    Private Sub SendHeartbeat(block As TaskBlock, line As String)
        Try
            Dim url = $"{cfg.headNodeUrl}/api/heartbeat?nodeId={Uri.EscapeDataString(cfg.nodeId)}&currentBlock={Uri.EscapeDataString(block.blockId)}&cores={Environment.ProcessorCount}&log={Uri.EscapeDataString(line)}"
            Call http.PostAsync(New Uri(url), Nothing).Wait(2000)
        Catch
            ' 心跳失败不阻断主流程。
        End Try
    End Sub

    Private Sub ReportDone(block As TaskBlock)
        Try
            Dim url = $"{cfg.headNodeUrl}/api/task/done?blockId={Uri.EscapeDataString(block.blockId)}&jobId={Uri.EscapeDataString(block.jobId)}&nodeId={Uri.EscapeDataString(cfg.nodeId)}"
            Call http.PostAsync(New Uri(url), Nothing).Wait(2000)
        Catch ex As Exception
            ' 静默失败，头结点会判定块超时重发。
        End Try
    End Sub

    Private Sub ReportFailed(block As TaskBlock, message As String, stack As String)
        Try
            Dim url = $"{cfg.headNodeUrl}/api/task/failed?blockId={Uri.EscapeDataString(block.blockId)}&jobId={Uri.EscapeDataString(block.jobId)}&nodeId={Uri.EscapeDataString(cfg.nodeId)}&errorMessage={Uri.EscapeDataString(message)}&stackTrace={Uri.EscapeDataString(stack)}"
            Call http.PostAsync(New Uri(url), Nothing).Wait(2000)
        Catch
        End Try
    End Sub
End Class
