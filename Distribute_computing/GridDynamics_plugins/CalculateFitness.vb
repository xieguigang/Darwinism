Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Plugin
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Parallel.Tasks
Imports Microsoft.VisualBasic.Serialization.JSON
Imports sciBASIC.ComputingServices.TaskHost

Public Module CalculateFitness

    <Plugin("GA/multiple_process")>
    Public Function MultipleProcessParallel(comparator As FitnessPool(Of Genome), source As IEnumerable(Of Genome)) As IEnumerable(Of NamedValue(Of Double))
        Dim individuals As Genome() = source.ToArray
        Dim partitionSize = individuals.Length / App.CPUCoreNumbers
        Dim partitions = individuals.Split(partitionSize)

        ' 在这里folk出多条进程进行并行计算
        ' 这个方法可以极大的提升程序在Linux平台上面的计算效率
        Dim compute As [Delegate] = New Func(Of String, String, NamedValue(Of Double)())(AddressOf SlaveProcess)
        Dim endPoint As String = Base64Codec.Base64String(InvokeInfo.CreateObject(compute, {}).GetJson)
        Dim slave = CLI.thinking.FromEnvironment(App.HOME)
        Dim folks As New List(Of AsyncHandle(Of Integer))
        Dim trainingSet = DirectCast(comparator.evaluateFitness, Environment) _
            .GetTrainingSet() _
            .writeMemory

        For Each block As Genome() In partitions
            ' 将数据写入内存
            Dim inputs = block.Select(Function(g) g.CreateSnapshot())
        Next
    End Function

    <Extension>
    Private Function writeMemory(Of T)(dataset As T) As String
        Dim ref$ = App.GetNextUniqueName($"memory://GA_dataset/{App.PID}_")

        Using writer As New StreamWriter(CommandLine.OpenForWrite(ref))
            Call writer.WriteLine(dataset.GetJson)
        End Using

        Return ref
    End Function

    ''' <summary>
    ''' 这个函数输入的两个参数都是内存文件的引用位置
    ''' </summary>
    ''' <param name="genomes$"></param>
    ''' <param name="trainingSet$"></param>
    ''' <returns></returns>
    Public Function SlaveProcess(genomes$, trainingSet$) As NamedValue(Of Double)()

    End Function

    Public Function DistributionComputing()
        Throw New NotImplementedException
    End Function
End Module
