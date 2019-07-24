Imports Microsoft.VisualBasic.ApplicationServices.Plugin
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology

Public Module CalculateFitness

    <Plugin("GA/multiple_process")>
    Public Function MultipleProcessParallel(comparator As FitnessPool(Of Genome), source As IEnumerable(Of Genome)) As IEnumerable(Of NamedValue(Of Double))
        Dim individuals As Genome() = source.ToArray
        Dim partitionSize = individuals.Length / App.CPUCoreNumbers
        Dim partitions = individuals.Split(partitionSize)

        ' 在这里folk出多条进程进行并行计算
        ' 这个方法可以极大的提升程序在Linux平台上面的计算效率

    End Function

    Public Function DistributionComputing()

    End Function
End Module
