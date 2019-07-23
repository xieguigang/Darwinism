Imports sciBASIC.ComputingServices
Imports sciBASIC.ComputingServices.TaskHost

Module Module1

    Sub Main()
        Dim seq As Integer() = {34, 573, 4985, 732895, 723, 8954, 7238, 94719, 847, 189, 2, 37, 1, 2893, 81, 231, 2312, 3}
        Dim task As [Delegate] = New Func(Of Integer(), Double())(AddressOf TargetTask.Add100)
        Dim invoke As InvokeInfo = InvokeInfo.CreateObject(task, {seq})
        Dim result = RemoteCall.Invoke(invoke)

        Dim exceptionTask As [Delegate] = New Func(Of String, Integer)(AddressOf TargetTask.ExceptionTest)
        Dim exceptionInvoke = InvokeInfo.CreateObject(exceptionTask, {"Here is the exception message..."})
        Dim exceptionResult As Rtvl = RemoteCall.Invoke(exceptionInvoke)

        Pause()
    End Sub

End Module
