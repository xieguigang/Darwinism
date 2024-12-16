#Region "Microsoft.VisualBasic::5311f803f465fa2c889dcfb684a58ef6, src\data\LINQ\LINQ\Interpreter\Expressions\Keywords\OutputProjection.vb"

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

    '   Total Lines: 50
    '    Code Lines: 36 (72.00%)
    ' Comment Lines: 3 (6.00%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 11 (22.00%)
    '     File Size: 1.62 KB


    '     Class OutputProjection
    ' 
    '         Properties: fields, keyword
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: Exec, ToString
    ' 
    '         Sub: CheckProjection
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Query
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Expressions

    ''' <summary>
    ''' data projection: ``SELECT &lt;fields>``
    ''' </summary>
    Public Class OutputProjection : Inherits KeywordExpression

        Public Property fields As NamedValue(Of Expression)()

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Select"
            End Get
        End Property

        Dim no_projection As Boolean = False

        Sub New(fields As IEnumerable(Of NamedValue(Of Expression)))
            Me.fields = fields.ToArray
        End Sub

        Public Sub CheckProjection(q As ProjectionExpression)
            If fields.Length = 1 Then
                no_projection = DirectCast(fields(0).Value, SymbolReference).symbolName = q.symbol.symbolName
            End If
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim obj As New JavaScriptObject

            If no_projection Then
                obj = fields(0).Value.Exec(context)
            Else
                For Each field In fields
                    obj(field.Name) = field.Value.Exec(context)
                Next
            End If

            Return obj
        End Function

        Public Overrides Function ToString() As String
            Return $"new {{{fields.Select(Function(a) $"{a.Name} = {a.Value}").JoinBy(", ")}}}"
        End Function
    End Class
End Namespace
