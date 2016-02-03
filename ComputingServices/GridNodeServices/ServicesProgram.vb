Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComputingServices.FileSystem
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.LINQ.Extensions

''' <summary>
''' 我想要构建的是一个去中心化的网格计算框架
''' </summary>
Module ServicesProgram

    Public Function Main() As Integer

        Dim DIR As New FileSystemHost(1526)
        Dim fs As New FileSystem(DIR.Portal)

        Dim file As New IO.FileStream("Z:\1ST.MP3", System.IO.FileMode.Open, fs)


        Return GetType(ServicesProgram).RunCLI(App.CommandLine, AddressOf TestLocal)
    End Function

    Private Sub __testLinq()
        Dim array = 20.Sequence
        Dim source As New LinqProvider(array, GetType(Integer))
        Call Threading.Thread.Sleep(100)
        Dim reader As New ILinq(Of Integer)(source.Portal)

        Dim xr = (From x In reader Where x > 10 Select x).ToArray
    End Sub

    ''' <summary>
    ''' Example of running on local machine
    ''' </summary>
    ''' <returns></returns>
    Public Function TestLocal() As Integer
        Call __testLinq()

        Dim exe As String = App.ExecutablePath
        Dim port As Integer = Microsoft.VisualBasic.Net.GetFirstAvailablePort

        '   Call Process.Start(exe, $"/remote /port {port}").Start()  ' example test
        '  Call Threading.Thread.Sleep(1500)
        Dim invoke As New TaskInvoke(port)
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
