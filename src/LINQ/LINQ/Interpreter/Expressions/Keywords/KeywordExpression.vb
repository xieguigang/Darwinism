﻿#Region "Microsoft.VisualBasic::ccfedf3bfcd6623c8db7a6a65d51bd13, LINQ\LINQ\Interpreter\Expressions\Keywords\KeywordExpression.vb"

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

    '     Class KeywordExpression
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Interpreter.Expressions

    Public MustInherit Class KeywordExpression : Inherits Expression

        Public MustOverride ReadOnly Property keyword As String

    End Class
End Namespace
