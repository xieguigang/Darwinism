#Region "Microsoft.VisualBasic::48de2a72c722953448db0ad8d462902d, src\data\LINQ\LINQ\Runtime\DataFrameExtensions.vb"

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

    '   Total Lines: 33
    '    Code Lines: 27 (81.82%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 6 (18.18%)
    '     File Size: 1.22 KB


    '     Module DataFrameExtensions
    ' 
    '         Function: CheckTabular, CreateTableDataSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

Namespace Runtime

    Public Module DataFrameExtensions

        <Extension>
        Public Function CreateTableDataSet(output As JavaScriptObject()) As DataFrameResolver
            Dim allNames As String() = output.Select(Function(obj) obj.GetNames) _
                .IteratesALL _
                .Distinct _
                .ToArray
            Dim dataset As New DataFrameResolver(allNames)
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
                         Return a.data.All(Function(d) d.CheckLiteral)
                     End Function)
        End Function
    End Module
End Namespace
