#Region "Microsoft.VisualBasic::7df578f9c460a3774643b67b42fec9ec, LINQ\LINQ\Interpreter\Expressions\Expression.vb"

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

    '     Class Expression
    ' 
    '         Properties: name
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class Expression

        Public ReadOnly Property name As String
            Get
                Return MyClass.GetType.Name.ToLower
            End Get
        End Property

        Public MustOverride Function Exec(context As ExecutableContext) As Object

    End Class
End Namespace
