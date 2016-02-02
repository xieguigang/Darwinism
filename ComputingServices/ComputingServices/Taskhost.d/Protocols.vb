Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Abstract
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection

Namespace TaskHost

    ''' <summary>
    ''' 分布式计算之中的远端调用的堆栈协议
    ''' </summary>
    Public Module Protocols

        Public Enum TaskProtocols As Long

#Region "Task"
            Invoke
            InvokeLinq
#End Region

#Region "LINQ supports"
            MoveNext
            Reset
            ReadsDone = -1000L
#End Region
        End Enum

        Public ReadOnly Property ProtocolEntry As Long =
            New Protocol(GetType(TaskProtocols)).EntryPoint

        Public Function LinqReset() As RequestStream
            Return New RequestStream(ProtocolEntry, TaskProtocols.Reset)
        End Function

        <Extension> Public Function GetPortal(Of Tsvr As IServicesSocket)(master As IMasterBase(Of Tsvr), local As Boolean) As IPEndPoint
            Dim ip As String = If(local, AsynInvoke.LocalIPAddress, GetMyIPAddress())
            Dim port As Integer = master.__host.LocalPort
            Return New IPEndPoint(ip, port)
        End Function
    End Module
End Namespace