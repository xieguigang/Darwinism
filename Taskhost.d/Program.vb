#Region "Microsoft.VisualBasic::a1c102abac445c4617fc5ca2ea899f5c, ..\sciBASIC.ComputingServices\Taskhost.d\Program.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports Microsoft.VisualBasic.CommandLine.Reflection

''' <summary>
''' Running on the server cluster nodes
''' </summary>
Module Program

    Sub Main()
        Call GetType(Program).RunCLI(App.CommandLine, Function() Start(CommandLine.TryParse("")))
    End Sub

    <ExportAPI("/start", Usage:="/start /port <port, default:1234>")>
    Public Function Start(args As CommandLine.CommandLine) As Integer
        Dim port As Integer = args.GetValue("/port", 1234)
        Return New TaskInvoke(port).Run()  ' No more code needs on your cloud server, just needs 2 lines code to running your task host.
    End Function
End Module
