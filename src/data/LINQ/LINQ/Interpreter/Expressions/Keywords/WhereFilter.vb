﻿#Region "Microsoft.VisualBasic::ebf57f1fcb8193609fdec27478ad3ada, src\data\LINQ\LINQ\Interpreter\Expressions\Keywords\WhereFilter.vb"

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

    '   Total Lines: 44
    '    Code Lines: 20 (45.45%)
    ' Comment Lines: 18 (40.91%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 6 (13.64%)
    '     File Size: 1.33 KB


    '     Class WhereFilter
    ' 
    '         Properties: keyword
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Interpreter.Expressions

    ''' <summary>
    ''' data filter: ``WHERE &lt;condition>``
    ''' </summary>
    Public Class WhereFilter : Inherits KeywordExpression

        ''' <summary>
        ''' A conditional test expression
        ''' </summary>
        Dim filter As Expression
        ''' <summary>
        ''' the index query
        ''' </summary>
        Dim query As LINQ.Query()

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Where"
            End Get
        End Property

        Sub New(filter As Expression)
            Me.filter = filter
        End Sub

        ''' <summary>
        ''' This function should returns a logical value for indicates 
        ''' that current value could be pass the filter or not
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' this function implements as the full table scanning
        ''' </remarks>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return filter.Exec(context)
        End Function

        Public Overrides Function ToString() As String
            Return $"where {filter}"
        End Function
    End Class
End Namespace
