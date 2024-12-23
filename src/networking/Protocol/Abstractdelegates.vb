Imports Darwinism.IPC.Networking.Protocols.Reflection
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.Parallel

Namespace Protocols

    ''' <summary>
    ''' Object for handles the request <see cref="ProtocolAttribute"/>.
    ''' </summary>
    Public MustInherit Class IProtocolHandler

        MustOverride ReadOnly Property ProtocolEntry As Long
        MustOverride Function HandleRequest(request As RequestStream, remoteDevcie As System.Net.IPEndPoint) As BufferPipe

    End Class

    Public Interface IRequestClient

        Function SendMessage(message As RequestStream) As RequestStream

    End Interface

#Region "Delegate Abstract Interface"

    Public Delegate Function SendMessageInvoke(Message As String) As String

    Public Delegate Sub ForceCloseHandle(socket As StateObject)

#End Region

    Public Delegate Sub ProcessMessagePush(message As RequestStream)
End Namespace