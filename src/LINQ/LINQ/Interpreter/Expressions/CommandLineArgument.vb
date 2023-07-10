#Region "Microsoft.VisualBasic::ddd0940cec6cfeae2e20adb7bdaf463a, LINQ\LINQ\Interpreter\Expressions\CommandLineArgument.vb"

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

    '     Class CommandLineArgument
    ' 
    '         Properties: ArgumentName
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Interpreter.Expressions

    ''' <summary>
    ''' $"--opt"
    ''' </summary>
    Public Class CommandLineArgument : Inherits Expression

        Public ReadOnly Property ArgumentName As String

        Sub New(arg As String)
            ArgumentName = arg
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return CType(App.CommandLine(ArgumentName), String)
        End Function

        Public Overrides Function ToString() As String
            Return $"$ARGS['{ArgumentName}']"
        End Function
    End Class
End Namespace
