Imports Microsoft.VisualBasic.ComputingServices.TaskHost

''' <summary>
''' 我想要构建的是一个去中心化的网格计算框架
''' </summary>
Module ServicesProgram

    Public Function remoteLINQTest(n As Integer) As Integer()
        Return {4, 56, 4, 6, 5, 74, 98, 145, 3132, 45, 6, 4, 8}.Join((-1).CopyVector(n)).ToArray
    End Function

    Sub Main()

        Dim source = {1, 2, 3, 4, 5, 6, 7, 8, 9, 1000}
        Dim sourceRead As LinqProvider = LinqProvider.CreateObject(source)
        Dim retur As ILinq(Of Integer) = New ILinq(Of Integer)(sourceRead)

        Dim all = retur.ToArray
        Dim xls = (From x In retur Where x > 6 Select x).ToArray

        '   Pause()


        Dim host As New TaskInvoke
        Dim portal = host.Portal
        Dim invokeInterface As New TaskHost(portal)
        Dim ddd As Func(Of Integer, Integer()) = AddressOf remoteLINQTest  ' 得到远程主机上面的函数指针

        Using source2 As ILinq(Of Integer) = invokeInterface.AsLinq(Of Integer)(ddd, 5)
            Dim result As Integer() = (From x As Integer In source2 Select x).ToArray
        End Using

        Pause()

        '  Call Microsoft.VisualBasic.ComputingServices.CLI_InitStart("./GridNode.exe", "cli /wan 127.0.0.1")

        Dim master As New Microsoft.VisualBasic.ComputingServices.Asymmetric.Master("1234567890")
        Call Microsoft.VisualBasic.Parallel.Run(AddressOf master.Run)
        Call Threading.Thread.Sleep(1000)

        Dim nodeMgr As New Microsoft.VisualBasic.ComputingServices.Asymmetric.Parasitifer("./GridNode.exe", "127.0.0.1", "1234567890")
    End Sub
End Module
