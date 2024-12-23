Imports System.Runtime.CompilerServices
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Parallel

Namespace Protocols

    Public Class RequestClient : Implements IRequestClient

        ReadOnly host As IPEndPoint

        Dim timeout_sec As Double = 6

        Sub New(host As IPEndPoint)
            Me.host = host
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function HandleRequest(req As RequestStream) As RequestStream Implements IRequestClient.SendMessage
            Return New TcpRequest(host).SetTimeOut(TimeSpan.FromSeconds(timeout_sec)).SendMessage(req)
        End Function

        Public Overrides Function ToString() As String
            Return host.ToString
        End Function
    End Class
End Namespace