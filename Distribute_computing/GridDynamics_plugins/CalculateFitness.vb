Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Plugin
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Parallel.Tasks
Imports Microsoft.VisualBasic.Serialization.JSON
Imports sciBASIC.ComputingServices.TaskHost

Public Structure SlaveTask
    Dim output As String
    Dim task As AsyncHandle(Of Integer)
End Structure

Public Module CalculateFitness

    <Plugin("GA/multiple_process")>
    Public Iterator Function MultipleProcessParallel(comparator As FitnessPool(Of Genome), source As IEnumerable(Of Genome)) As IEnumerable(Of NamedValue(Of Double))
        Dim individuals As Genome() = source.ToArray
        Dim partitionSize = individuals.Length / App.CPUCoreNumbers
        Dim partitions = individuals.Split(partitionSize)

        ' 在这里folk出多条进程进行并行计算
        ' 这个方法可以极大的提升程序在Linux平台上面的计算效率
        Dim compute As [Delegate] = New Func(Of String, String, NamedValue(Of Double)())(AddressOf SlaveProcess)
        Dim slave = CLI.thinking.FromEnvironment(App.HOME)
        Dim folks As New List(Of SlaveTask)
        Dim trainingSet = DirectCast(comparator.evaluateFitness, Environment) _
            .GetTrainingSet() _
            .writeMemory

        For Each block As Genome() In partitions
            ' 将数据写入内存
            Dim inputs As String = block _
                .Select(Function(g) g.CreateSnapshot()) _
                .ToArray _
                .writeMemory
            Dim application = Base64Codec.Base64String(InvokeInfo.CreateObject(compute, {inputs, trainingSet}).GetJson)
            Dim output = inputs _
                .Select(Function(null)
                            Return New NamedValue(Of Double) With {
                                .Name = New String("-"c, 32),
                                .Value = 0,
                                .Description = .Name
                            }
                        End Function) _
                .ToArray _
                .writeMemory

            folks += New SlaveTask With {
                .output = output,
                .task = New AsyncHandle(Of Integer)(Function() slave.Slave(application, output)).Run
            }
        Next

        Do While folks > 0
            Dim success = folks.FirstOrDefault(Function(folk) folk.task.IsCompleted)

            If success.output Is Nothing Then
                Thread.Sleep(1)
                Continue Do
            End If

            Using reader As New StreamReader(CommandLine.OpenForRead(success.output))
                Dim result = reader.ReadToEnd
                Dim dataset = result.LoadJSON(Of NamedValue(Of Double)())

                For Each fitness As NamedValue(Of Double) In dataset
                    Yield fitness
                Next
            End Using
        Loop
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
