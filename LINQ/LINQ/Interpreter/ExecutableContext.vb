#Region "Microsoft.VisualBasic::1ef8eb26f223aa943331f6632a1d03b8, LINQ\LINQ\Interpreter\ExecutableContext.vb"

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

    '     Class ExecutableContext
    ' 
    '         Properties: env, throwError
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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

        Public ReadOnly Property GlobalEnvir As GlobalEnvironment
            Get
                Return env.GlobalEnvir
            End Get
        End Property

    End Class
End Namespace
