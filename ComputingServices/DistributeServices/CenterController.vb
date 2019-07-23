Imports Microsoft.VisualBasic.Parallel

Namespace DistributeServices

    ''' <summary>
    ''' 用来请求计算资源的客户端对象
    ''' </summary>
    Public Class CenterController

        ReadOnly registry As Registry

        ''' <summary>
        ''' 当前注册的计算节点数量
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NumberOfNodes As Integer
            Get
                Return registry.Count
            End Get
        End Property

        Sub New(ipRange$, port%)
            registry = New Registry(ipRange$, port%)
        End Sub

        ''' <summary>
        ''' Run a task for scan local network and founding the grid nodes.
        ''' </summary>
        Public Sub ScanTask()
            Call ParallelExtension.RunTask(AddressOf registry.doScanForNodes)
        End Sub

        Public Function [Select](Of T, Tout)(source As IEnumerable(Of T), project As [Delegate], args As Object()) As IEnumerable(Of Tout)

        End Function
    End Class
End Namespace