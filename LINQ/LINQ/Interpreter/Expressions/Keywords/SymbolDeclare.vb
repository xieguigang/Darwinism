#Region "Microsoft.VisualBasic::5b6b811a332f3c9dd3cd843c75bf1820, LINQ\LINQ\Interpreter\Expressions\Keywords\SymbolDeclare.vb"

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

    '     Class SymbolDeclare
    ' 
    '         Properties: arguments, keyword, symbolName, type
    ' 
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime

Namespace Interpreter.Expressions

    ''' <summary>
    ''' declare a new temp symbol: ``LET x = ...``
    ''' </summary>
    Public Class SymbolDeclare : Inherits KeywordExpression

        Public Property symbolName As String
        Public Property type As String
        Public Property arguments As Expression()

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Let"
            End Get
        End Property

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"let {symbolName} as {type}{If(arguments.IsNullOrEmpty, "", $"[{arguments.JoinBy(", ")}]")}"
        End Function
    End Class
End Namespace
