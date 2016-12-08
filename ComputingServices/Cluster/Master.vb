Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Parallel
Imports sciBASIC.ComputingServices.TaskHost

Namespace Cluster

    ''' <summary>
    ''' Client
    ''' </summary>
    Public Class Master

        Dim _nodes As Dictionary(Of TaskRemote)
        Dim node_port%
        Dim net$

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="net$">``192.168...``</param>
        ''' <param name="node_port%"></param>
        Sub New(net$, node_port%)
            Me.node_port = node_port
        End Sub

        Public Sub Scan()
            Dim request As New RequestStream(Protocols.ProtocolEntry, TaskProtocols.Handshake)


        End Sub

        Public Sub ScanTask()
            Call RunTask(AddressOf Scan)
        End Sub
    End Class
End Namespace