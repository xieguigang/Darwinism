#Region "Microsoft.VisualBasic::0dbd09cb9689d1c0ede426f425a5a482, src\data\LINQ\LINQ\Interpreter\Program.vb"

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

    '   Total Lines: 23
    '    Code Lines: 16 (69.57%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 7 (30.43%)
    '     File Size: 620 B


    '     Class Program
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateProgram, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Script

Namespace Interpreter

    Public Class Program

        ReadOnly execQueue As Expression()

        Friend Sub New(exec As IEnumerable(Of Expression))
            execQueue = exec.ToArray
        End Sub

        Public Overrides Function ToString() As String
            Return execQueue.JoinBy(" ;" & vbCrLf)
        End Function

        Public Shared Function CreateProgram(script As String) As Program
            Return New Program(Language.GetTokens(script).PopulateQueryExpression)
        End Function

    End Class
End Namespace
