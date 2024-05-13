#Region "Microsoft.VisualBasic::8254e340e7781de9c93e502de427f388, src\data\LINQ\LINQ\Interpreter\QueryInterpreter.vb"

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

    '   Total Lines: 17
    '    Code Lines: 8
    ' Comment Lines: 3
    '   Blank Lines: 6
    '     File Size: 332 B


    '     Class QueryInterpreter
    ' 
    '         Properties: env
    ' 
    '         Function: Exec
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter

    ''' <summary>
    ''' interpreter of the linq query expression
    ''' </summary>
    Public Class QueryInterpreter

        Public Property env As Environment

        Public Function Exec(query As String) As Object

        End Function

    End Class
End Namespace
