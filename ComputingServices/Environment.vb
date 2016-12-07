Imports System.Runtime.CompilerServices

''' <summary>
''' 分布式计算环境，因为这里是为了做高性能计算而构建的一个内部网络的计算集群，
''' 所以数据再网络传输的过程之中加密与否已经无所谓了
''' </summary>
Public Module Environment

    Public Sub Open()

    End Sub

    ''' <summary>
    ''' Running this task sequence in distribution mode.(使用分布式的方式来执行这个任务集合序列)
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <typeparam name="Tout"></typeparam>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Iterator Function AsDistributed(Of T, Tout)(source As IEnumerable(Of T), task As Func(Of T, Tout)) As IEnumerable(Of Tout)

    End Function
End Module
