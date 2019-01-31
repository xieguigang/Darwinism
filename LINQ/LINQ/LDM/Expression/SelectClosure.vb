#Region "Microsoft.VisualBasic::37787fc233a1e2e80449d74607f33b61, LINQ\LINQ\LDM\Expression\SelectClosure.vb"

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

    '     Class SelectClosure
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: __parsing
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.CodeDom
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode
Imports Microsoft.VisualBasic.LINQ.Framework.DynamicCode.VBC

Namespace LDM.Expression

    Public Class SelectClosure : Inherits Closure

        Friend SelectMethod As System.Reflection.MethodInfo

        Sub New(source As Statements.Tokens.SelectClosure)
            Call MyBase.New(source)


        End Sub

        Protected Overrides Function __parsing() As CodeExpression

        End Function
    End Class
End Namespace
