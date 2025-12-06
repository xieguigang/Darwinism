Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' IDM批量下载管理类
''' 通过命令行调用IDM进行文件下载
''' </summary>
Public Class IDMBatchDownloader
    ''' <summary>
    ''' IDM可执行文件路径
    ''' </summary>
    Private _idmPath As String

    ''' <summary>
    ''' 默认下载目录
    ''' </summary>
    Private _defaultDownloadPath As String

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="idmPath">IDM可执行文件路径，如果为空则自动查找</param>
    ''' <param name="defaultDownloadPath">默认下载目录</param>
    Public Sub New(Optional idmPath As String = "", Optional defaultDownloadPath As String = "")
        If String.IsNullOrEmpty(idmPath) Then
            _idmPath = FindIDMPath()
        Else
            _idmPath = idmPath
        End If

        If String.IsNullOrEmpty(defaultDownloadPath) Then
            _defaultDownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads"
        Else
            _defaultDownloadPath = defaultDownloadPath
        End If
    End Sub

    ''' <summary>
    ''' 查找IDM安装路径
    ''' </summary>
    ''' <returns>IDM可执行文件路径，如果未找到则返回空字符串</returns>
    Private Function FindIDMPath() As String
        ' 常见的IDM安装路径
        Dim commonPaths As String() = {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Internet Download Manager\IDMan.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Internet Download Manager\IDMan.exe"),
            "C:\Program Files\Internet Download Manager\IDMan.exe",
            "C:\Program Files (x86)\Internet Download Manager\IDMan.exe"
        }

#If WINDOWS Then
        ' 检查注册表中的安装路径
        Try
            If Environment.OSVersion.Platform = PlatformID.Win32NT Then
                Using regKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IDMan.exe")
                    If regKey IsNot Nothing Then
                        Dim regPath As String = regKey.GetValue("").ToString()
                        If File.Exists(regPath) Then
                            Return regPath
                        End If
                    End If
                End Using
            End If
        Catch
            ' 忽略注册表访问错误
        End Try
#End If

        ' 检查常见路径
        For Each path As String In commonPaths
            If File.Exists(path) Then
                Return path
            End If
        Next

        Return ""
    End Function

    ''' <summary>
    ''' 下载单个文件
    ''' </summary>
    ''' <param name="url">下载URL</param>
    ''' <param name="fileName">保存的文件名（可选）</param>
    ''' <param name="savePath">保存路径（可选）</param>
    ''' <returns>下载任务是否成功添加到IDM</returns>
    Public Function DownloadFile(url As String, Optional fileName As String = "", Optional savePath As String = "") As Boolean
        If String.IsNullOrEmpty(_idmPath) Then
            Throw New InvalidOperationException("未找到IDM安装路径，请确保IDM已正确安装")
        End If

        If String.IsNullOrEmpty(url) Then
            Throw New ArgumentException("下载URL不能为空")
        End If

        Dim actualSavePath As String = If(String.IsNullOrEmpty(savePath), _defaultDownloadPath, savePath)

        ' 确保目录存在
        If Not Directory.Exists(actualSavePath) Then
            Directory.CreateDirectory(actualSavePath)
        End If

        ' 构建命令行参数
        Dim arguments As New StringBuilder()
        arguments.Append("/d """ & url & """") ' /d 参数指定下载URL

        If Not String.IsNullOrEmpty(fileName) Then
            arguments.Append(" /f """ & fileName & """") ' /f 参数指定文件名
        End If

        arguments.Append(" /p """ & actualSavePath & """") ' /p 参数指定保存路径
        arguments.Append(" /n") ' /n 参数表示不弹出对话框
        arguments.Append(" /a") ' /a 参数表示添加到队列但不立即开始

        Try
            Dim processInfo As New ProcessStartInfo() With {
                .FileName = _idmPath,
                .Arguments = arguments.ToString(),
                .UseShellExecute = False,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }

            Using process As Process = Process.Start(processInfo)
                Thread.Sleep(2000)
                Return process.ExitCode = 0
            End Using
        Catch ex As Exception
            Throw New Exception("调用IDM失败: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' 批量下载文件
    ''' </summary>
    ''' <param name="downloadList">下载项列表</param>
    ''' <returns>成功添加的下载项数量</returns>
    Public Function BatchDownload(downloadList As IEnumerable(Of DownloadItem)) As Integer
        Dim successCount As Integer = 0
        Dim errors As New List(Of String)

        For Each item As DownloadItem In TqdmWrapper.Wrap(downloadList.ToArray)
            Try
                If DownloadFile(item.Url, item.FileName, item.SavePath) Then
                    successCount += 1
                Else
                    errors.Add($"下载失败: {item.Url}")
                End If
            Catch ex As Exception
                errors.Add($"下载异常: {item.Url} - {ex.Message}")
            End Try
        Next

        If errors.Count > 0 Then
            Throw New AggregateException("部分下载任务失败", errors.Select(Function(e) New Exception(e)))
        End If

        Return successCount
    End Function

    ''' <summary>
    ''' 开始所有队列中的下载
    ''' </summary>
    Public Sub StartAllDownloads()
        If String.IsNullOrEmpty(_idmPath) Then
            Throw New InvalidOperationException("未找到IDM安装路径")
        End If

        Try
            Dim processInfo As New ProcessStartInfo() With {
                .FileName = _idmPath,
                .Arguments = "/s", ' /s 参数表示开始所有下载
                .UseShellExecute = False,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }

            Using process As Process = Process.Start(processInfo)
                process.WaitForExit(3000)
            End Using
        Catch ex As Exception
            Throw New Exception("启动IDM下载失败: " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' 检查IDM是否可用
    ''' </summary>
    ''' <returns>True表示IDM可用</returns>
    Public Function IsIDMAvailable() As Boolean
        Return Not String.IsNullOrEmpty(_idmPath) AndAlso File.Exists(_idmPath)
    End Function

    ''' <summary>
    ''' 获取IDM版本信息
    ''' </summary>
    ''' <returns>版本字符串</returns>
    Public Function GetIDMVersion() As String
        If Not IsIDMAvailable() Then
            Return "IDM不可用"
        End If

        Try
            Dim versionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(_idmPath)
            Return $"IDM {versionInfo.FileVersion}"
        Catch
            Return "无法获取版本信息"
        End Try
    End Function
End Class

''' <summary>
''' 下载项类
''' </summary>
Public Class DownloadItem
    ''' <summary>
    ''' 下载URL
    ''' </summary>
    Public Property Url As String

    ''' <summary>
    ''' 保存的文件名
    ''' </summary>
    Public Property FileName As String = ""

    ''' <summary>
    ''' 保存路径
    ''' </summary>
    Public Property SavePath As String = ""

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="url">下载URL</param>
    ''' <param name="fileName">文件名</param>
    ''' <param name="savePath">保存路径</param>
    Public Sub New(url As String, Optional fileName As String = "", Optional savePath As String = "")
        Me.Url = url
        Me.FileName = fileName
        Me.SavePath = savePath
    End Sub
End Class