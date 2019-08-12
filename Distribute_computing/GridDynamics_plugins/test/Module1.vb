Imports GridDynamics_plugins
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.Darwinism.NonlinearGridTopology
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Module Module1

    Sub Main()
        Call testGrid()

    End Sub

    Sub testGrid()
        Dim vec As Vector = 22500.Sequence.Select(Function(i) CDbl(i)).AsVector
        Dim filePath$ = "./test_grid.dat"
        Dim grid As New GridSystem With {
            .AC = -999999,
            .A = vec,
            .C = vec.Sequence _
                .Select(Function(null)
                            Return New Correlation With {.B = vec, .BC = 9999999}
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

        Dim X As Vector = Replicate(1.0E-200, vec.Length).AsVector

        Dim result1 = grid.Evaluate(X)
        Dim result2 = grid.Evaluate(X)


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
