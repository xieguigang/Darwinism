#Region "Microsoft.VisualBasic::968c358571379422469e97858060b42c, src\data\LINQ\Spy\Shell.vb"

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

'   Total Lines: 67
'    Code Lines: 47 (70.15%)
' Comment Lines: 9 (13.43%)
'    - Xml Docs: 33.33%
' 
'   Blank Lines: 11 (16.42%)
'     File Size: 2.12 KB


' Module LinqShell
' 
'     Function: RunTerminal
' 
'     Sub: ExecLinq
' 
' /********************************************************************************/

#End Region

Imports LINQ.Interpreter
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Runtime
Imports LINQ.Script
Imports Microsoft.VisualBasic.ApplicationServices.Terminal
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.LineEdit
Imports Microsoft.VisualBasic.Data.Framework.IO
Imports Microsoft.VisualBasic.Language.UnixBash
Imports Microsoft.VisualBasic.My.JavaScript

''' <summary>
''' Linq shell
''' </summary>
Module LinqShell

    Dim env As New GlobalEnvironment(New Registry)
    Dim console As Shell

    ReadOnly ps1_ready As New PS1("> ")
    ReadOnly ps1_incomplete As New PS1("+ ")

    Public Function RunTerminal() As Integer
        Dim editor As New LineEditor("LINQ", 5000) With {
            .HeuristicsMode = True,
            .TabAtStartCompletes = False,
            .MaxWidth = 120
        }

        console = New Shell(ps1_ready, AddressOf ExecLinq, dev:=New LineReader(editor)) With {
            .Quite = "!.LINQ#::quit" & Rnd()
        }

        Return console.Run
    End Function

    ' # file driver is detected via the file extension name suffix
    ' let x = load "/path/to/file"
    ' let y = from xi in x where xi.score > 80 select xi.name

    Private Sub ExecLinq(script As String)
        Dim tokens As Token() = LINQ.Language.GetTokens(script).ToArray
        Dim query As ProjectionExpression = tokens.PopulateQueryExpression
        Dim context As New ExecutableContext With {
            .env = env,
            .throwError = True,
            .linq = query
        }
        Dim result As JavaScriptObject() = query.Exec(context)

        If result.CheckTabular Then
            ' result as table
            Dim table As DataFrameResolver = result.CreateTableDataSet
            ' print on the console
            Dim text As String()() = table _
                .csv _
                .Select(Function(c) c.ToArray) _
                .ToArray

            Call text.PrintTable
        Else
            ' result as object set
            Throw New NotImplementedException
        End If
    End Sub

End Module
