Imports System
Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets

''' <summary>
''' Linux 平台节点资源指标采集器。
''' 通过解析 /proc/stat（CPU 时间）、/proc/meminfo（内存）、/proc/net/dev（网络）
''' 两次采样求差来计算使用率与速率。容器或无 /proc 环境下失败时降级为默认值。
''' </summary>
Public Class LinuxMetricsReader
    Implements INodeMetrics

    Private ReadOnly netIf As NetworkInterface
    Private prevCpuIdle As Long = -1
    Private prevCpuTotal As Long = -1
    Private prevUpBytes As Long = -1
    Private prevDownBytes As Long = -1
    Private prevTime As Long = 0

    Public Sub New()
        netIf = SelectInterface()
    End Sub

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
            .machineName = ReadMachineName()
        }

        SampleCpu(m)
        SampleMemory(m)
        SampleNetwork(m)
        m.ipAddress = ResolveLocalIPv4()

        Return m
    End Function

    ''' <summary>
    ''' 解析 /proc/stat 的 cpu 行，计算 CPU 使用率。
    ''' </summary>
    Private Sub SampleCpu(m As NodeMetrics)
        Try
            Dim line = File.ReadLines("/proc/stat").FirstOrDefault(Function(l) l.StartsWith("cpu "))
            If line Is Nothing Then Return

            Dim parts = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            ' 格式: cpu user nice system idle iowait irq softirq steal guest guest_nice
            Dim vals(parts.Length - 1) As Long
            For i = 1 To parts.Length - 1
                vals(i - 1) = Long.Parse(parts(i))
            Next

            Dim idle = vals(3) + If(vals.Length > 4, vals(4), 0)  ' idle + iowait
            Dim total As Long = 0
            For Each v In vals
                total += v
            Next

            If prevCpuTotal >= 0 AndAlso total > prevCpuTotal Then
                Dim dTotal = total - prevCpuTotal
                Dim dIdle = idle - prevCpuIdle
                m.cpuUsage = Math.Round((1 - CDbl(dIdle) / CDbl(dTotal)) * 100, 1)
            End If

            prevCpuIdle = idle
            prevCpuTotal = total
        Catch
            m.cpuUsage = 0
        End Try
    End Sub

    ''' <summary>
    ''' 解析 /proc/meminfo，计算内存总量与使用率。
    ''' </summary>
    Private Sub SampleMemory(m As NodeMetrics)
        Try
            Dim memTotal As Long = 0
            Dim memAvailable As Long = 0
            For Each line In File.ReadLines("/proc/meminfo")
                If line.StartsWith("MemTotal:") Then
                    memTotal = ParseKb(line)
                ElseIf line.StartsWith("MemAvailable:") Then
                    memAvailable = ParseKb(line)
                End If
            Next

            m.totalMemoryMB = CLng(memTotal \ 1024)
            If memTotal > 0 Then
                If memAvailable > 0 Then
                    m.memoryUsage = Math.Round((1 - CDbl(memAvailable) / CDbl(memTotal)) * 100, 1)
                Else
                    m.memoryUsage = 0
                End If
            End If
        Catch
            m.totalMemoryMB = 0
            m.memoryUsage = 0
        End Try
    End Sub

    Private Function ParseKb(line As String) As Long
        Dim tokens = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        ' tokens(0)=名称, tokens(1)=数值, tokens(2)=单位(kB)
        If tokens.Length >= 2 Then
            Return Long.Parse(tokens(1))
        End If
        Return 0
    End Function

    ''' <summary>
    ''' 解析 /proc/net/dev 两次采样求差计算网络速率。
    ''' </summary>
    Private Sub SampleNetwork(m As NodeMetrics)
        Dim nowTicks = DateTime.UtcNow.Ticks
        Dim up As Long = 0
        Dim down As Long = 0

        Try
            For Each line In File.ReadLines("/proc/net/dev")
                Dim idx = line.IndexOf(":"c)
                If idx < 0 Then Continue For

                Dim ifName = line.Substring(0, idx).Trim()
                If netIf IsNot Nothing AndAlso ifName <> netIf.Name Then Continue For

                Dim nums = line.Substring(idx + 1) _
                    .Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

                ' 接收列: bytes(0); 发送列: bytes(8)
                If nums.Length >= 9 Then
                    down += Long.Parse(nums(0))
                    up += Long.Parse(nums(8))
                End If
            Next
        Catch
            prevUpBytes = -1
            prevTime = 0
            Return
        End Try

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
    End Sub

    Private Function ReadMachineName() As String
        Try
            If File.Exists("/etc/hostname") Then
                Dim name = File.ReadAllText("/etc/hostname").Trim()
                If name.Length > 0 Then Return name
            End If
        Catch
        End Try
        Return Environment.MachineName
    End Function

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
