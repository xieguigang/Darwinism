#Region "Microsoft.VisualBasic::bfef8fdf4694e05e47a4034810889063, src\data\LINQ\LINQ\test\valueIndex.vb"

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

    '   Total Lines: 16
    '    Code Lines: 12 (75.00%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 4 (25.00%)
    '     File Size: 508 B


    ' Module valueIndex2
    ' 
    '     Sub: Main
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Public Module valueIndex2

    Sub Main()
        Dim pool = Enumerable.Range(0, 10000000).Select(Function(a) randf.NextDouble(0, 2000)).ToArray
        Dim index As RangeIndex(Of Double) = ValueIndex.DoubleIndex().IndexData(pool)

        Dim search1 = index.Search(100).ToArray
        Dim search2 = index.Search(99, 103).ToArray
        Dim search3 = index.Search(-50).ToArray

        Pause()
    End Sub
End Module
