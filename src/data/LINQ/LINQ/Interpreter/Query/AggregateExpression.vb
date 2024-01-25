﻿#Region "Microsoft.VisualBasic::70e8ded3851ec091c59cc014def65b94, LINQ\LINQ\Interpreter\Query\AggregateExpression.vb"

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

    '     Class AggregateExpression
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime

Namespace Interpreter.Query

    ''' <summary>
    ''' aggregate ... into ...
    ''' </summary>
    Public Class AggregateExpression : Inherits QueryExpression

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression))
            Call MyBase.New(symbol, sequence, exec)
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace
