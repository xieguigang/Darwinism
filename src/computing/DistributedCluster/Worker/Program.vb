Module Program

    Sub Main(args As String())
        Console.WriteLine($"Darwinism Distributed Cluster :: Worker")
        ' 直接调用反射宿主；ExitCode 透传 worker 的计算结果（0=成功，1=失败）。
        Environment.ExitCode = ReflectHost.Run(args)
    End Sub
End Module
