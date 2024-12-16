#Region "Microsoft.VisualBasic::f3ba2740d56ba5e8eca309fb4415d121, src\data\LINQ\LINQ\Interpreter\ExecutableContext.vb"

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

    '   Total Lines: 30
    '    Code Lines: 14 (46.67%)
    ' Comment Lines: 11 (36.67%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 5 (16.67%)
    '     File Size: 796 B


    '     Class ExecutableContext
    ' 
    '         Properties: env, GlobalEnvir, linq, throwError
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Query
Imports LINQ.Runtime

Namespace Interpreter

    ''' <summary>
    ''' Execute context of LINQ query
    ''' </summary>
    Public Class ExecutableContext

        ''' <summary>
        ''' Symbol and data environment
        ''' </summary>
        ''' <returns></returns>
        Public Property env As Environment
        Public Property throwError As Boolean = True
        ''' <summary>
        ''' current executed linq query expression
        ''' </summary>
        ''' <returns></returns>
        Public Property linq As QueryExpression

        Public ReadOnly Property GlobalEnvir As GlobalEnvironment
            Get
                Return env.GlobalEnvir
            End Get
        End Property

    End Class
End Namespace
