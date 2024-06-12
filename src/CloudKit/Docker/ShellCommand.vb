#Region "Microsoft.VisualBasic::f274af0dd3656eb333793a9fef3205fe, src\CloudKit\Docker\ShellCommand.vb"

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

    '   Total Lines: 24
    '    Code Lines: 20
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 792 B


    ' Module ShellCommand
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: CommandHistory, Run
    ' 
    ' /********************************************************************************/

#End Region

Public Module ShellCommand

    Friend ReadOnly shell As Func(Of String, String, String)
    Friend ReadOnly logs As New List(Of String)

    Public Iterator Function CommandHistory() As IEnumerable(Of String)
        For Each line As String In logs
            Yield line
        Next
    End Function

    Sub New()
        shell = Function(app, args) As String
                    Dim lines As New List(Of String)
                    Call logs.Add($"{app} {args}")
                    Call CommandLine.ExecSub(app, args, onReadLine:=AddressOf lines.Add)
                    Return lines.JoinBy(vbLf)
                End Function
    End Sub

    ''' <summary>
    ''' run commandline and then returns the std output of the command
    ''' </summary>
    ''' <param name="app"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Public Function Run(app As String, args As String) As String
        Return shell(app, args)
    End Function
End Module
