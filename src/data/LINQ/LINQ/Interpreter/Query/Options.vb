#Region "Microsoft.VisualBasic::58a8244873f3fb35cb8b0370172d7ec9, src\data\LINQ\LINQ\Interpreter\Query\Options.vb"

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

    '   Total Lines: 40
    '    Code Lines: 30 (75.00%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 10 (25.00%)
    '     File Size: 1.27 KB


    '     Class Options
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: RunOptionPipeline, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    Public Class Options

        Dim m_pipeline As PipelineKeyword()

        Sub New(pipeline As IEnumerable(Of Expression))
            Me.m_pipeline = pipeline _
                .Select(Function(l) DirectCast(l, PipelineKeyword)) _
                .ToArray
        End Sub

        ''' <summary>
        ''' 查询结果的数据处理管线：``ORDER BY`` / ``SKIP`` / ``TAKE``
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Pipeline As PipelineKeyword()
            Get
                Return m_pipeline
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="output"></param>
        ''' <param name="context"></param>
        ''' <param name="skipFirst">
        ''' 是否跳过管线之中的第一段操作。当排序操作已经通过数据源的索引完成的时候，
        ''' 排序之后的数据行顺序已经符合要求，此时可以跳过``ORDER BY``操作。
        ''' </param>
        ''' <returns></returns>
        Public Function RunOptionPipeline(output As IEnumerable(Of JavaScriptObject),
                                          context As ExecutableContext,
                                          Optional skipFirst As Boolean = False) As IEnumerable(Of JavaScriptObject)

            Dim raw As JavaScriptObject() = output.ToArray

            If raw.Length = 0 Then
                ' 数据源经过索引筛选之后没有结果
                ' 没有任何数据行可以用于后续的管线处理
                Return raw
            End If

            Dim start As Integer = If(skipFirst, 1, 0)

            If start < pipeline.Length Then
                Dim allNames As String() = raw(Scan0).GetNames
                Dim env As Environment = context.env

                For Each name As String In allNames
                    If Not env.HasSymbol(name) Then
                        Call env.AddSymbol(name, "any")
                    End If
                Next

                For i As Integer = start To pipeline.Length - 1
                    raw = pipeline(i).Exec(raw, context).ToArray
                Next
            End If

            Return raw
        End Function

        Public Overrides Function ToString() As String
            Return pipeline.JoinBy(" -> ")
        End Function

    End Class
End Namespace
