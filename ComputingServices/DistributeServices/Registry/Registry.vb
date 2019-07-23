Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Net.Tcp

Namespace DistributeServices

    ''' <summary>
    ''' The node registry in current grid
    ''' </summary>
    ''' <remarks>
    ''' 在这个对象中,还会存在一个网络服务用来动态的自动添加和删除网络中的计算节点
    ''' </remarks>
    ''' 
    <Protocol(GetType(RegistryProtocols))>
    Public Class Registry : Implements IEnumerable(Of IPEndPoint)

        ''' <summary>
        ''' 这是一个网络范围,不是一个具体的IP端点
        ''' </summary>
        ReadOnly grid As IPEndPoint
        ReadOnly nodes As New List(Of IPEndPoint)
        ReadOnly services As TcpServicesSocket

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ipRange">
        ''' IP地址范围表达式:
        ''' 
        ''' 1. Use all of the ip address in range: ``192.168.*``
        ''' 2. Use selected ip address in range: ``192.168.1.* - 192.168.10.200``
        ''' 
        ''' </param>
        ''' <param name="port%"></param>
        Sub New(ipRange$, port%)
            grid = New IPEndPoint(ipRange, port)
        End Sub

        Public Sub doScanForNodes()
            For Each ip As String In DistributeServices.GetIPAddressList(grid.IPAddress)

            Next
        End Sub

        Public Function GetEnumerator() As IEnumerator(Of IPEndPoint) Implements IEnumerable(Of IPEndPoint).GetEnumerator
            Return nodes.GetEnumerator
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield GetEnumerator()
        End Function
    End Class
End Namespace