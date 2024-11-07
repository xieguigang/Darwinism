#Region "Microsoft.VisualBasic::efec12e5fbc3420987eed667928b2852, src\data\LINQ\LINQ\Runtime\DataFrameExtensions.vb"

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

    '   Total Lines: 25
    '    Code Lines: 20 (80.00%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 5 (20.00%)
    '     File Size: 923 B


    '     Module DataFrameExtensions
    ' 
    '         Function: CreateTableDataSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

Namespace Runtime

    Public Module DataFrameExtensions

        <Extension>
        Public Function CreateTableDataSet(output As JavaScriptObject()) As DataFrame
            Dim allNames As String() = output.Select(Function(obj) obj.GetNames).IteratesALL.Distinct.ToArray
            Dim dataset As New DataFrame(allNames)
            Dim row As RowObject

            For Each item As JavaScriptObject In output
                row = allNames.Select(Function(name) any.ToString(item(name))).DoCall(Function(cells) New RowObject(cells))
                dataset.AppendLine(row)
            Next

            Return dataset.MeasureTypeSchema
        End Function

        <Extension>
        Public Function CheckTabular(output As JavaScriptObject()) As Boolean
            Return Not output _
                .Any(Function(a)

                     End Function)
        End Function
    End Module
End Namespace
