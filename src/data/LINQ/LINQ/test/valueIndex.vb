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
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions

Public Module valueIndex2

    Sub Main()
        testQGramIndex()

        Pause()
    End Sub

    Sub testQGramIndex()
        Dim index As New QGramIndex(5)

        For Each name As String In {"Prochlorococcus marinus subsp. marinus CCMP1375",
"Prochlorococcus marinus subsp. pastoris CCMP1986",
"Prochlorococcus marinus MIT 9313",
"Prochlorococcus marinus NATL2A",
"Prochlorococcus marinus MIT 9312",
"Prochlorococcus marinus AS9601",
"Prochlorococcus marinus MIT 9515",
"Prochlorococcus marinus MIT 9303",
"Prochlorococcus marinus MIT 9301",
"Prochlorococcus marinus MIT 9215",
"Prochlorococcus marinus MIT 9211",
"Prochlorococcus marinus NATL1A",
"Prochlorococcus sp. MIT 0604",
"Prochlorococcus sp. MIT 0801",
"Woronichinia naegeliana",
"Chamaesiphon minutus",
"Cyanothece sp. PCC 7425",
"Crinalium epipsammum",
"Thermostichus vulcanus",
"Parathermosynechococcus lividus",
"Thermosynechococcus vestitus",
"Thermosynechococcus sp. NK55",
"Thermosynechococcus sp. CL-1",
"Thermosynechococcus sp. TA-1",
"Thermosynechococcus sichuanensis"}

            Call index.AddString(name)
        Next

        Dim test = index.FindSimilar("chococcus", 0.6)

        Pause()
    End Sub

    Sub tesdtIndex()
        Dim pool = Enumerable.Range(0, 10000000).Select(Function(a) randf.NextDouble(0, 2000)).ToArray
        Dim index As RangeIndex(Of Double) = ValueIndex.DoubleIndex().IndexData(pool)

        Dim search1 = index.Search(100).ToArray
        Dim search2 = index.Search(99, 103).ToArray
        Dim search3 = index.Search(-50).ToArray
    End Sub
End Module
