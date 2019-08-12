Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices.Plugin
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Parallel.Tasks
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Text
Imports sciBASIC.ComputingServices.TaskHost

Public Class SlaveTask
    Public Property output As String
    Public Property task As AsyncHandle(Of Integer)
End Class

Public Module CalculateFitness

    Dim staticInputMemory As String()
    Dim staticOutputMemory As String()
    Dim staticTrainingSet$

    <Plugin("GA/multiple_process")>
    Public Iterator Function MultipleProcessParallel(comparator As FitnessPool(Of Genome), source As IEnumerable(Of Genome)) As IEnumerable(Of NamedValue(Of Double))
        Dim individuals As Genome() = source.ToArray
        Dim partitionSize = individuals.Length / App.CPUCoreNumbers
        Dim partitions = individuals.Split(partitionSize)

        ' 在这里folk出多条进程进行并行计算
        ' 这个方法可以极大的提升程序在Linux平台上面的计算效率
        Dim compute As [Delegate] = New Func(Of String, String, NamedValue(Of Double)())(AddressOf SlaveProcess)
        Dim slave = CLI.Think.FromEnvironment(App.HOME)
        Dim folks As New List(Of SlaveTask)
        Dim trainingSet = DirectCast(comparator.evaluateFitness, Environment(Of GridSystem, Genome)) _
            .GetTrainingSet() _
            .ToArray _
            .writeMemory(staticTrainingSet)
        Dim index As Integer = Scan0

        staticTrainingSet = trainingSet

        If staticInputMemory.IsNullOrEmpty Then
            staticInputMemory = New String(partitions.Length - 1) {}
            staticOutputMemory = New String(partitions.Length - 1) {}
        End If

        For Each block As Genome() In partitions
            ' 将数据写入内存
            Dim inputs As String = block _
                .Select(Function(g) g.chromosome.CreateSnapshot(Nothing)) _
                .ToArray _
                .writeMemory(staticInputMemory(index))
            Dim application = Base64Codec.Base64String(InvokeInfo.CreateObject(compute, {inputs, trainingSet}).GetJson)
            Dim output = inputs _
                .Select(Function(null)
                            Return New NamedValue(Of Double) With {
                                .Name = New String("-"c, 1024),
                                .Value = 0,
                                .Description = .Name
                            }
                        End Function) _
                .ToArray _
                .writeMemory(staticOutputMemory(index))

            staticInputMemory(index) = inputs
            staticOutputMemory(index) = output
            index += 1
            folks += New SlaveTask With {
                .output = output,
                .task = New AsyncHandle(Of Integer)(Function() slave.Slave(application, output)).Run
            }
        Next

        Do While folks > 0
            Dim success = folks.FirstOrDefault(Function(folk) folk.task.IsCompleted)

            If success Is Nothing Then
                Thread.Sleep(1)
                Continue Do
            Else
                folks -= success
            End If

            Using reader As New StreamReader(CommandLine.OpenForRead(success.output))
                Dim result = reader.ReadToEnd.Split(ASCII.NUL)
                Dim returns As Rtvl = result(Scan0).LoadJSON(Of Rtvl)
                Dim dataset = returns.info.value.LoadJSON(Of NamedValue(Of Double)())

                For Each fitness As NamedValue(Of Double) In dataset
                    Yield fitness
                Next
            End Using
        Loop
    End Function

    <Extension>
    Private Function writeMemory(Of T)(dataset As T, staticFile$) As String
        Dim ref$ = staticFile Or App.GetNextUniqueName($"memory://GA_dataset/{App.PID}_").AsDefault
        Dim json As String = dataset.GetJson
        Dim jsonBytes As Byte() = Encodings.UTF8WithoutBOM _
            .CodePage _
            .GetBytes(json)

        Using writer = CommandLine.OpenForWrite(ref, size:=jsonBytes.Length * 10)
            Call writer.Write(jsonBytes, Scan0, jsonBytes.Length)
        End Using

        Return ref
    End Function

    Private Function readJSON(Of T)(file As String) As T
        Dim jsonStr$ = New StreamReader(CommandLine.OpenForRead(file)).ReadToEnd.Replace(ASCII.NUL, "")
        Dim obj As T = jsonStr.LoadJSON(Of T)

        Return obj
    End Function

    ''' <summary>
    ''' 这个函数输入的两个参数都是内存文件的引用位置
    ''' </summary>
    ''' <param name="genomes$"></param>
    ''' <param name="trainingSet$"></param>
    ''' <returns></returns>
    Public Function SlaveProcess(genomes$, trainingSet$) As NamedValue(Of Double)()
        Dim grids As GridMatrix() = readJSON(Of GridMatrix())(genomes)
        Dim trainingData = readJSON(Of NamedValue(Of Double())())(trainingSet) _
            .Select(Function(d)
                        Return New TrainingSet With {
                            .targetID = d.Name,
                            .Y = d.Description,
                            .X = d.Value
                        }
                    End Function) _
            .ToArray
        Dim outputFitness As New List(Of NamedValue(Of Double))
        Dim model As GridSystem

        For Each snapshot As GridMatrix In grids
            model = snapshot.CreateSystem
            outputFitness += New NamedValue(Of Double) With {
                .Name = GridSystem.ToString(model),
                .Value = New Genome(model, 0, 0, False).LabelGroupAverage(trainingData, parallel:=False)
            }
        Next

        Return outputFitness
    End Function

    Public Function DistributionComputing()
        Throw New NotImplementedException
    End Function
End Module
