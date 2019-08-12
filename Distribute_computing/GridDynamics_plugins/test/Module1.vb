Imports GridDynamics_plugins
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Module Module1

    Sub Main()

        Dim vec As Vector = 50000.Sequence.Select(Function(i) CDbl(i)).AsVector
        Dim filePath$ = "./test.dat"


        Using file = filePath.Open
            Call vec.Serialize(file)
        End Using

        Using file = filePath.Open

            Dim vecNew As Vector = file.LoadVector

        End Using
    End Sub

End Module
