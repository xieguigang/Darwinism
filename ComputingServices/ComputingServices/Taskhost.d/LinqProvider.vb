Imports System.Reflection
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    ''' <summary>
    ''' 执行得到数据集合然后分独传输数据元素
    ''' </summary>
    ''' 
    <Protocol(GetType(TaskProtocols))>
    Public Class LinqProvider

        ReadOnly __host As TcpSynchronizationServicesSocket = New TcpSynchronizationServicesSocket(GetFirstAvailablePort)

        Sub New()
            __host.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
        End Sub

        <Protocol(TaskProtocols.MoveNext)>
        Private Function __moveNext(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
    End Class
End Namespace