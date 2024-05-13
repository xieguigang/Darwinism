#Region "Microsoft.VisualBasic::6fde37c8f4df7549a619d5aa3e8dda33, src\data\LINQ\LINQ\test\Program.vb"

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

    '   Total Lines: 36
    '    Code Lines: 30
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 1.13 KB


    ' Module Program
    ' 
    '     Sub: Main, parserTest
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter
Imports LINQ.Interpreter.Query
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.My.JavaScript

Public Module Program

    Dim y = 0
    Dim test = From x As Double In {(1 + y) * 8, 2, 3, 4, 5, 6, 7, 8, 9}
               Where x ^ 3 > (5 * x)
               Select x = x ^ 2 + 99, y = x * 2
               Order By y Ascending

    Sub Main()
        Call parserTest()
    End Sub

    Sub parserTest()
        Dim script = "
from x as double in [(1+y)*8,2,3,4,5,6,7,8,9]  # this is comment text
where x^3 > (5 *x)
select x = x ^ 2+99 , y = x*2
order by y
"
        Dim tokens = LINQ.Language.GetTokens(script).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim env As New GlobalEnvironment(New Registry, New NamedValue(Of Object)("y", 1))

        Dim result As JavaScriptObject() = query.Exec(New ExecutableContext With {.env = env, .throwError = True})
        Dim table = result.CreateTableDataSet

        Pause()
    End Sub
End Module
