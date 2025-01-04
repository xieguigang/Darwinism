#Region "Microsoft.VisualBasic::fd7199a6e90b5733e27b3b03bcedb7b2, src\computing\Parallel\test\clr_parallelTask_demo.vb"

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


' Code Statistics:

'   Total Lines: 59
'    Code Lines: 43 (72.88%)
' Comment Lines: 0 (0.00%)
'    - Xml Docs: 0.00%
' 
'   Blank Lines: 16 (27.12%)
'     File Size: 1.88 KB


' Module clr_parallelTask_demo
' 
'     Function: compute_function, generateDemoData
' 
'     Sub: Main2, runParallel
' 
' Class vectorData
' 
'     Properties: Data
' 
' /********************************************************************************/

#End Region

Imports batch
Imports Darwinism.HPC.Parallel.IpcStream
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Correlations
Imports Microsoft.VisualBasic.MIME.application.json
Imports rnd = Microsoft.VisualBasic.Math.RandomExtensions

Module clr_parallelTask_demo

    Public Function compute_function(i As vectorData, pool As vectorData()) As Double()
        Dim sum As Double() = New Double(pool.Length - 1) {}

        For offset As Integer = 0 To pool.Length - 1
            sum(offset) = i.DistanceTo(pool(offset))
        Next

        Return sum
    End Function

    Sub Main2()
        Call runParallel()
    End Sub

    Private Function generateDemoData() As vectorData()
        Dim width As Integer = 1000
        Dim pool As vectorData() = New vectorData(10000) {}
        Dim v As Double()

        For i As Integer = 0 To pool.Length - 1
            v = Enumerable.Range(0, width).Select(Function(any) rnd.NextDouble(0, any + 1)).ToArray
            pool(i) = New vectorData With {.Data = v}
        Next

        Call Console.WriteLine("memory data create job done!")

        Return pool
    End Function

    Private Sub runParallel()
        Dim pool = generateDemoData()

        Call Console.WriteLine("create parallrl task...")

        Dim args As New Argument(8) With {.ignoreError = False, .verbose = True}
        Dim memory_symbol1 As SocketRef = SocketRef.WriteBuffer(pool)
        Dim result = Host.ParallelFor(Of vectorData, Double())(args, New Func(Of vectorData, vectorData(), Double())(AddressOf compute_function), pool, memory_symbol1)

        For Each item In result
            Call Console.WriteLine(item.GetJson)
        Next
    End Sub
End Module

Public Class vectorData : Implements IVector

    Public Property Data As Double() Implements IVector.Data

End Class
