#Region "Microsoft.VisualBasic::11ad6e7a17e5eeeac053db851ca0ee3f, src\data\LINQ\LINQ\Interpreter\Expressions\Expression.vb"

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
    '    Code Lines: 11 (45.83%)
    ' Comment Lines: 8 (33.33%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 5 (20.83%)
    '     File Size: 610 B


    '     Class Expression
    ' 
    '         Properties: name
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    ''' <summary>
    ''' the Linq expression
    ''' </summary>
    Public MustInherit Class Expression

        Public ReadOnly Property name As String
            Get
                Return MyClass.GetType.Name.ToLower
            End Get
        End Property

        ''' <summary>
        ''' Evaluate the expression
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns></returns>
        Public MustOverride Function Exec(context As ExecutableContext) As Object

    End Class
End Namespace
