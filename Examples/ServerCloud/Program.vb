Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComputingServices.TaskHost

Module Program

    Sub Main()
        Call GetType(Program).RunCLI(App.CommandLine, Function() Start(CommandLine.TryParse("")))
    End Sub

    <ExportAPI("/start", Usage:="/start /port <port, default:1234>")>
    Public Function Start(args As CommandLine.CommandLine) As Integer
        Dim port As Integer = args.GetValue("/port", 1234)
        Return New TaskInvoke(True, port).Run()
    End Function
End Module
