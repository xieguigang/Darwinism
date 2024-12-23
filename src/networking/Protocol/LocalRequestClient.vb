Imports System.Net
Imports Darwinism.IPC.Networking.Protocols.Reflection
Imports Microsoft.VisualBasic.Parallel

Namespace Protocols

    ''' <summary>
    ''' helper for run services module debug
    ''' </summary>
    Public Class LocalRequestClient : Implements IRequestClient

        ReadOnly fakeLocal As IPEndPoint
        ReadOnly host As ProtocolHandler

        Sub New(host As ProtocolHandler)
            Me.host = host
            Me.fakeLocal = New IPEndPoint(0, 1)
        End Sub

        Public Function SendMessage(message As RequestStream) As RequestStream Implements IRequestClient.SendMessage
            Dim pull As New List(Of Byte)
            Dim responseData = host.HandleRequest(message, fakeLocal)

            For Each buf As Byte() In responseData.GetBlocks
                Call pull.AddRange(buf)
            Next

            Return New RequestStream(0, 200, pull.ToArray)
        End Function
    End Class
End Namespace