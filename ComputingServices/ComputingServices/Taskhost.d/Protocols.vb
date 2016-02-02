Imports Microsoft.VisualBasic.Net.Protocol.Reflection

Namespace TaskHost

    ''' <summary>
    ''' 分布式计算之中的远端调用的堆栈协议
    ''' </summary>
    Public Module Protocols

        Public Enum TaskProtocols As Long

#Region "LINQ supports"
            MoveNext
            ReadsDone = -1000L
#End Region
        End Enum

        Public ReadOnly Property ProtocolEntry As Long =
            New Protocol(GetType(TaskProtocols)).EntryPoint

    End Module
End Namespace