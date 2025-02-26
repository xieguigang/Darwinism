#Region "Microsoft.VisualBasic::3f0c4cae11e3453a41755bb53fd0d676, src\data\LINQ\LINQ\Program.vb"

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

    '   Total Lines: 53
    '    Code Lines: 39 (73.58%)
    ' Comment Lines: 8 (15.09%)
    '    - Xml Docs: 75.00%
    ' 
    '   Blank Lines: 6 (11.32%)
    '     File Size: 1.81 KB


    ' Module Program
    ' 
    '     Function: Main, RunQuery
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.ApplicationServices.Terminal
Imports Microsoft.VisualBasic.CommandLine
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.My.JavaScript
Imports Microsoft.VisualBasic.Text

Module Program

    Public Function Main() As Integer
        Return GetType(Program).RunCLI(App.CommandLine, executeFile:=AddressOf RunQuery)
    End Function

    ''' <summary>
    ''' &lt;xxx.linq> [/output &lt;result.csv>]
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Private Function RunQuery(file As String, args As CommandLine) As Integer
        Dim tokens As Token() = LINQ.Language.GetTokens(file.ReadAllText).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim env As New GlobalEnvironment(New Registry)
        Dim context As New ExecutableContext With {.env = env, .throwError = True}
        Dim result As JavaScriptObject() = query.Exec(context)
        Dim table As DataFrameResolver = result.CreateTableDataSet
        Dim output As String = args <= "/output"

        If output.StringEmpty Then
            ' print on the console
            Dim text As String()() = table _
                .csv _
                .Select(Function(c) c.ToArray) _
                .ToArray

            Call text.PrintTable
        Else
            ' save to csv file
            Call table _
                .csv _
                .Save(
                    path:=output,
                    encoding:=Encodings.UTF8WithoutBOM
                )
        End If

        Return 0
    End Function
End Module
