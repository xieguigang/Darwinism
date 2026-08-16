Imports System
Imports System.Diagnostics
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports Microsoft.VisualBasic.Devices

''' <summary>
''' Windows 平台节点资源指标采集器。
''' CPU 使用率借助 PerformanceCounter；内存使用率借助 ComputerInfo；
''' 网络速率借助 NetworkInterface 的 IPv4 统计两次采样求差。
''' </summary>
Public Class WindowsMetricsReader
    Implements INodeMetrics

    Private ReadOnly cpuCounter As PerformanceCounter
    Private ReadOnly netIf As NetworkInterface
    Private prevUpBytes As Long = -1
    Private prevDownBytes As Long = -1
    Private prevTime As Long = 0

    Public Sub New()
        cpuCounter = New PerformanceCounter("Processor", "% Processor Time", "_Total")
        ' 预热一次，否则首个 NextValue 通常为 0。
        cpuCounter.NextValue()

        netIf = SelectInterface()
    End Sub

    ''' <summary>
    ''' 选择首个非回环且已启用的 IPv4 网络接口，用于网络速率统计。
    ''' </summary>
    Private Function SelectInterface() As NetworkInterface
        For Each nic As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()
            If nic.OperationalStatus = OperationalStatus.Up AndAlso
               nic.NetworkInterfaceType <> NetworkInterfaceType.Loopback AndAlso
               nic.Supports(NetworkInterfaceComponent.IPv4) Then
                Return nic
            End If
        Next
        Return Nothing
    End Function

    Public Function Sample() As NodeMetrics Implements INodeMetrics.Sample
        Dim m As New NodeMetrics With {
            .cpuCores = Environment.ProcessorCount,
            .machineName = Environment.MachineName
        }

        ' ---- CPU 使用率 ----
        Try
            m.cpuUsage = Math.Round(CDbl(cpuCounter.NextValue()), 1)
        Catch
            m.cpuUsage = 0
        End Try

        ' ---- 内存 ----
        Try
            Dim ci As New ComputerInfo()
            Dim total = ci.TotalPhysicalMemory
            Dim avail = ci.AvailablePhysicalMemory
            m.totalMemoryMB = CLng(total \ (1024 * 1024))
            If total > 0 Then
                m.memoryUsage = Math.Round((1 - CDbl(avail) / CDbl(total)) * 100, 1)
            End If
        Catch
            m.totalMemoryMB = 0
            m.memoryUsage = 0
        End Try

        ' ---- 网络速率 ----
        Dim nowTicks = DateTime.UtcNow.Ticks
        If netIf IsNot Nothing Then
            Try
                Dim stat = netIf.GetIPv4Statistics()
                Dim up = CLng(stat.BytesSent)
                Dim down = CLng(stat.BytesReceived)

                If prevUpBytes >= 0 AndAlso prevTime > 0 Then
                    Dim dt = (nowTicks - prevTime) / TimeSpan.TicksPerSecond
                    If dt > 0 Then
                        m.netUploadRate = Math.Max(0, (up - prevUpBytes) / dt)
                        m.netDownloadRate = Math.Max(0, (down - prevDownBytes) / dt)
                    End If
                End If

                prevUpBytes = up
                prevDownBytes = down
                prevTime = nowTicks
            Catch
                prevUpBytes = -1
                prevTime = 0
            End Try
        End If

        ' ---- IP ----
        m.ipAddress = ResolveLocalIPv4()

        Return m
    End Function

    ''' <summary>
    ''' 解析本机非回环 IPv4 地址。
    ''' </summary>
    Private Function ResolveLocalIPv4() As String
        Try
            Dim host = Dns.GetHostEntry(Dns.GetHostName())
            For Each addr As IPAddress In host.AddressList
                If addr.AddressFamily = AddressFamily.InterNetwork AndAlso
                   Not IPAddress.IsLoopback(addr) Then
                    Return addr.ToString()
                End If
            Next
        Catch
        End Try
        Return "0.0.0.0"
    End Function
End Class
