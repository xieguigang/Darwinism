Imports System

''' <summary>
''' 节点资源指标的采样结果（平台无关）。
''' 实时速率/使用率已由采集器通过两次采样求差计算完毕，头结点仅需透传存储。
''' </summary>
Public Class NodeMetrics

    ''' <summary>逻辑 CPU 核心数量。</summary>
    Public Property cpuCores As Integer

    ''' <summary>当前 CPU 使用率（0-100）。</summary>
    Public Property cpuUsage As Double

    ''' <summary>物理内存总量（MB）。</summary>
    Public Property totalMemoryMB As Long

    ''' <summary>当前内存使用率（0-100）。</summary>
    Public Property memoryUsage As Double

    ''' <summary>网络上传速率（字节/秒）。</summary>
    Public Property netUploadRate As Double

    ''' <summary>网络下载速率（字节/秒）。</summary>
    Public Property netDownloadRate As Double

    ''' <summary>节点 IPv4 地址。</summary>
    Public Property ipAddress As String

    ''' <summary>计算机名称。</summary>
    Public Property machineName As String
End Class

''' <summary>
''' 计算节点资源指标采集接口。
''' 不同操作系统提供各自实现（Windows / Linux），由 Daemon 在运行时按平台选择。
''' </summary>
Public Interface INodeMetrics

    ''' <summary>
    ''' 采样一次节点资源指标。第一次调用因缺少基准样本，使用率/速率返回 0；
    ''' 后续调用基于与上一次样本的间隔求差计算。
    ''' </summary>
    Function Sample() As NodeMetrics
End Interface
