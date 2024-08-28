Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF.Population
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.Models

Public Class GAIPCParallelPopulationCompute(Of chr As {Class, Chromosome(Of chr)}) : Inherits ParallelComputeFitness(Of chr)

    Public Overrides Function ComputeFitness(comparator As FitnessPool(Of chr), source As PopulationCollection(Of chr)) As IEnumerable(Of NamedValue(Of Double))
        Throw New NotImplementedException()
    End Function
End Class