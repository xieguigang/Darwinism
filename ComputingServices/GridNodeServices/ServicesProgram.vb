Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComputingServices.TaskHost

''' <summary>
''' 我想要构建的是一个去中心化的网格计算框架
''' </summary>
Module ServicesProgram

    Public Function Main() As Integer
        Return GetType(ServicesProgram).RunCLI(App.CommandLine, AddressOf TestLocal)
    End Function

    ''' <summary>
    ''' Example of running on local machine
    ''' </summary>
    ''' <returns></returns>
    Public Function TestLocal() As Integer
        Dim exe As String = App.ExecutablePath
        Dim port As Integer = Microsoft.VisualBasic.Net.GetFirstAvailablePort

        Call Process.Start(exe, $"/remote /port {port}").Start()  ' example test
        Call Threading.Thread.Sleep(1500)

        Dim remote As New TaskHost("127.0.0.1", port)
        Dim msg As String = "Code execute on the remotes!"
        Dim remoteFunc As Func(Of String, Integer) = AddressOf TestExample1  ' Gets the function pointer on the remote machine

        Call remote.Invoke(Of Integer)(remoteFunc, msg).__DEBUG_ECHO ' gets the value that returns from the remote machine

    End Function

    Public Function TestExample1(msg As String) As Integer
        Call MsgBox(msg, MsgBoxStyle.Information, App.Command)  ' remotes have the commandline but the local is empty
        Return 100000 * RandomDouble()
    End Function

    ''' <summary>
    ''' Example of running on a remote machine.
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <ExportAPI("/remote", Usage:="/remote [/port <port, default:=1234>]")>
    Public Function TestRemote(args As CommandLine.CommandLine) As Integer
        Dim port As Integer = args.GetValue("/port", 1234)
        Dim invoke As New TaskInvoke(port)

        Call Pause()

        Return 0
    End Function
End Module
