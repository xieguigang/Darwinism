Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.InteropService.SharedORM
Imports Microsoft.VisualBasic.CommandLine.Reflection

<CLI> Module CLI

    ''' <summary>
    ''' Run current program as grid node service process.
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Public Function RunServices(args As CommandLine) As Integer

    End Function

    ''' <summary>
    ''' 在并行计算中,多进程的计算效率要高于多线程的应用程序
    ''' 可以将任务分解,然后通过调用这个函数创建多进程的并行计算任务
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    ''' 
    <ExportAPI("/slave")>
    <Description("Program running in slave mode")>
    <Usage("/slave /application <json_base64> /arguments <memory_mapfile>")>
    Public Function Slave(args As CommandLine) As Integer
        Dim endpointJSON$ = args("/application").Base64Decode
        Dim parametersJSON$ = args.OpenStreamInput("/arguments").ReadToEnd
    End Function
End Module
