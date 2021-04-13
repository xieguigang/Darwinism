#Region "Microsoft.VisualBasic::644655adc7f77f6ef32b35710752b7fc, Distribute_computing\GridDynamics_plugins\test\Module1.vb"

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

    ' Module Module1
    ' 
    '     Sub: Main, testGrid, testPopulation, testVector
    ' 
    ' /********************************************************************************/

#End Region

Imports GridDynamics_plugins
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Module Module1

    Sub Main()

        Call testPopulation()

        ' Call testGrid()

    End Sub

    Sub testPopulation()
        Dim vec As Vector = 22500.Sequence.Select(Function(i) -CDbl(i)).AsVector
        Dim filePath$ = "./test_pop.zip"
        Dim grid As New GridSystem With {
            .AC = -999999,
            .A = vec,
            .C = vec.Sequence _
                .Select(Function(null)
                            Return New Correlation With {.B = vec, .BC = -99}
                        End Function) _
                .ToArray
        }

        Dim pop As New PopulationZip(filePath, 0, 0)

        Call pop.Add(grid)
        Call pop.Add(grid)

        Dim newGrid = pop.GetIndividual(1)

        Dim X As Vector = Vector.rand(-10, 10, vec.Length)

        Dim result1 = grid.Evaluate(X)
        Dim result2 = newGrid.Evaluate(X)


        Console.WriteLine(result1 = result2)

        Pause()
    End Sub

    Sub testGrid()
        Dim vec As Vector = 500.Sequence.Select(Function(i) -CDbl(i)).AsVector
        Dim filePath$ = "./test_grid.dat"
        Dim grid As New GridSystem With {
            .AC = -999999,
            .A = vec,
            .C = vec.Sequence _
                .Select(Function(null)
                            Return New Correlation With {.B = vec, .BC = -99}
                        End Function) _
                .ToArray
        }

        Using file = filePath.Open
            Call grid.Serialize(file, 2048)
        End Using

        Dim newGrid As GridSystem

        Using file = filePath.Open

            newGrid = file.LoadGridSystem

        End Using

        Dim X As Vector = Replicate(1.0, vec.Length).AsVector

        Dim result1 = grid.Evaluate(X)
        Dim result2 = newGrid.Evaluate(X)


        Console.WriteLine(result1 = result2)

        Pause()
    End Sub

    Sub testVector()
        Dim vec As Vector = 50000.Sequence.Select(Function(i) CDbl(i)).AsVector
        Dim filePath$ = "./test.dat"


        Using file = filePath.Open
            Call vec.Serialize(file)
        End Using

        Dim vecNew As Vector

        Using file = filePath.Open

            vecNew = file.LoadVector

        End Using


        Dim oldSum = vec.Sum
        Dim newSum = vecNew.Sum

        Console.WriteLine(oldSum = newSum)

        Pause()
    End Sub

End Module
