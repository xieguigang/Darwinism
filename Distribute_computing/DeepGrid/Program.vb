#Region "Microsoft.VisualBasic::ec88cb578009b98bd0c691939e1d914a, Distribute_computing\DeepGrid\Program.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    ' Module Program
    ' 
    '     Function: Main, RunGrid
    ' 
    ' /********************************************************************************/

#End Region

Imports System.ComponentModel
Imports GridDynamics_plugins
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataStructures
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF.Helper
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.GAF.ReplacementStrategy
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.MachineLearning.StoreProcedure
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine)
    End Function

    ''' <summary>
    ''' 使用这个工具几乎可以训练任意规模大小的网格模型, 但是速度会比较慢, 因为这个程序使用硬盘文件来充当内存缓存
    ''' </summary>
    ''' <param name="args"></param>
    ''' <returns></returns>
    <ExportAPI("/run")>
    <Description("Run a deep grid dynamics network model.")>
    <Usage("/run /trainingSet <trainingSet.Xml> [/validateSet <validateSet.Xml> /model <model.Xml> /truncate <default=1000> /rate <default=0.5> /popsize <default=50> /out <model_output.Xml>]")>
    Public Function RunGrid(args As CommandLine) As Integer
        Dim in$ = args <= "/trainingSet"
        Dim validates$ = args <= "/validate"
        Dim model$ = args <= "/model"
        Dim truncate As Double = args("/truncate") Or 1000.0
        Dim rate# = args("/rate") Or 0.5
        Dim popSize = args("/popsize") Or 50
        Dim out$ = args("/out") Or $"{[in].TrimSuffix}.minError_DeepGrid.Xml"
        Dim seed As GridMatrix = Nothing
        Dim cacheZip = out.ParentPath & $"/.{out.BaseName}/"
        Dim diskCaches As LoopArray(Of String) = 5.Sequence _
            .Select(Function(i) $"{cacheZip}/cache_{(i + 666).ToHexString}.zip") _
            .ToArray

        If Not in$.FileExists Then
            Call "No input file was found!".PrintException
        Else
            seed = If(model.FileExists, model.LoadXml(Of GridMatrix), Nothing)

            If Not seed Is Nothing Then
                Call $"Load trained model from {model}".__INFO_ECHO
            End If
        End If

        Dim trainingSet = in$.LoadXml(Of DataSet)
        Dim validateSet = args("/validateSet").LoadXml(Of DataSet)(throwEx:=False)
        Dim factorNames = trainingSet.NormalizeMatrix.names

        Call $"Mutation rate = {rate}".__DEBUG_ECHO
        Call $"Population size = {popSize}".__DEBUG_ECHO

        Dim cor As Vector = trainingSet.DataSamples.AsEnumerable.Correlation

        Call "Create a base chromosome".__DEBUG_ECHO

        Dim chromesome As GridSystem

        If seed Is Nothing Then
            chromesome = Loader.EmptyGridSystem(trainingSet.width, cor).TryCast(Of GridSystem)
        Else
            chromesome = seed.CreateSystem
        End If

        ' 在种群范围内不进行并行计算
        ' 只对蛋白genome内部进行并行化计算
        Dim zip As New PopulationZip(diskCaches.Next, rate, truncate)
        Dim population As Population(Of Genome) = New Genome(chromesome, rate, truncate).InitialPopulation(New Population(Of Genome)(zip, False) With {.capacitySize = popSize})
        Call "Initialize environment".__DEBUG_ECHO
        Dim fitness As Fitness(Of Genome) = New Environment(Of GridSystem, Genome)(trainingSet, FitnessMethods.LabelGroupAverage, validateSet)
        Call "Create algorithm engine".__DEBUG_ECHO
        Dim ga As New GeneticAlgorithm(Of Genome)(
            population:=population,
            fitnessFunc:=fitness,
            replacementStrategy:=Strategies.Naive,
            createPopulation:=Function() New PopulationZip(diskCaches.Next, rate, truncate)
        )
        Call "Load driver".__DEBUG_ECHO

        Dim takeBestSnapshot = Sub(best As Genome, error#)
                                   Call best.chromosome _
                                       .CreateSnapshot(
                                            dist:=trainingSet.NormalizeMatrix,
                                            names:=factorNames,
                                            [error]:=[error]
                                       ) _
                                       .GetXml _
                                       .SaveTo(OutFile.TrimSuffix & $"_localOptimal/{[error]}.Xml")
                               End Sub
        Dim engine As New EnvironmentDriver(Of Genome)(ga, takeBestSnapshot) With {
            .Iterations = 1000000,
            .Threshold = 0.005
        }

        Call engine.AttachReporter(Sub(i, e, g)
                                       Call EnvironmentDriver(Of Genome).CreateReport(i, e, g).ToString.__DEBUG_ECHO
                                       Call g.Best.chromosome _
                                             .CreateSnapshot(trainingSet.NormalizeMatrix, factorNames, e) _
                                             .GetXml _
                                             .SaveTo(OutFile)
                                   End Sub)

        Call "Run GA!".__DEBUG_ECHO
        Call engine.Train()

        Return 0
    End Function

End Module

