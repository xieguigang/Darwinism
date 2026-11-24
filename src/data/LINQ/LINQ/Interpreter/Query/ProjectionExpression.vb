#Region "Microsoft.VisualBasic::59fbb8056e9a706b4b17fb3a8c30e8c8, src\data\LINQ\LINQ\Interpreter\Query\ProjectionExpression.vb"

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

    '   Total Lines: 77
    '    Code Lines: 49 (63.64%)
    ' Comment Lines: 13 (16.88%)
    '    - Xml Docs: 69.23%
    ' 
    '   Blank Lines: 15 (19.48%)
    '     File Size: 2.68 KB


    '     Class ProjectionExpression
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    ''' <summary>
    ''' from ... select ...
    ''' </summary>
    Public Class ProjectionExpression : Inherits QueryExpression

        Dim opt As Options
        Dim project As OutputProjection

        Sub New(symbol As SymbolDeclare, sequence As Expression, exec As IEnumerable(Of Expression), proj As OutputProjection, opt As Options)
            Call MyBase.New(symbol, sequence, exec)

            Me.opt = opt
            Me.project = proj
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns>
        ''' array of <see cref="JavaScriptObject"/>
        ''' </returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim projections As New List(Of JavaScriptObject)
            Dim env As Environment = context.env
            Dim closure As New ExecutableContext With {
                .env = New Environment(parent:=env),
                .throwError = context.throwError
            }

            Call LoadDrivers(context)
            Call project.CheckProjection(Me)

            Dim dataset As DataSet = GetDataSet(context)
            Dim addresses As Integer() = GetCandidates(dataset)
            Dim ordered As Integer() = GetOrderedAddresses(dataset)
            Dim skipOrderBy As Boolean = False

            If ordered IsNot Nothing Then
                ' 排序字段已经建立并且使用了数值索引：数据行已经按照索引键的顺序排列
                ' 所以在这里可以跳过管线之中的order by操作
                If addresses Is Nothing Then
                    addresses = ordered
                Else
                    Dim candidates As New HashSet(Of Integer)(addresses)

                    addresses = ordered _
                        .Where(Function(i) candidates.Contains(i)) _
                        .ToArray
                End If

                skipOrderBy = True
            End If

            Call closure.env.AddSymbol(symbol.symbolName, symbol.type)

            Dim rows As IEnumerable(Of Object) = If(
                addresses Is Nothing,
                dataset.PopulatesData(),
                dataset.PopulatesData(addresses)
            )
            Dim skipVal As Boolean
            Dim target As Symbol = closure.env.FindSymbol(symbol.symbolName)

            For Each item As Object In rows
                target.value = item
                skipVal = False

                For Each line As Expression In executeQueue
                    ' full table scanning
                    If TypeOf line Is WhereFilter Then
                        skipVal = Not BinaryExpression.CBoolValue(line.Exec(closure))

                        If skipVal Then
                            Exit For
                        End If
                    End If
                Next

                If Not skipVal Then
                    If project Is Nothing Then
                        ' 当SELECT投影操作为空的时候，默认不进行投影操作
                        ' 即返回最原始的变量值
                        projections.Add(item)
                    Else
                        projections.Add(project.Exec(closure))
                    End If
                End If
            Next

            Return opt.RunOptionPipeline(projections, context, skipFirst:=skipOrderBy).ToArray
        End Function

        ''' <summary>
        ''' 使用数据源的索引来筛选出候选的数据行
        ''' </summary>
        ''' <param name="dataset"></param>
        ''' <returns>
        ''' 当数据源不支持索引筛选的时候，函数返回<c>Nothing</c>，此时将会回退为全表扫描。
        ''' </returns>
        Private Function GetCandidates(dataset As DataSet) As Integer()
            Dim queries As LINQ.Query() = GetIndexQueries()
            Dim result As IndexQueryResult = Nothing

            If queries IsNot Nothing Then
                result = dataset.TryQuery(queries)
            End If

            LastIndex = result

            If result IsNot Nothing AndAlso result.Used Then
                Return result.Addresses
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' 尝试通过数据源的数值索引来获取排序之后的数据行偏移
        ''' </summary>
        ''' <param name="dataset"></param>
        ''' <returns>
        ''' 当无法使用索引排序的时候，函数返回<c>Nothing</c>
        ''' </returns>
        ''' <remarks>
        ''' 只有在管线之中的第一段操作是``ORDER BY``，排序键为数据源之中的字段，
        ''' 并且投影的输出之中也包含这个字段的时候才会使用索引排序。
        ''' </remarks>
        Private Function GetOrderedAddresses(dataset As DataSet) As Integer()
            If opt.Pipeline.Length = 0 Then
                Return Nothing
            End If
            If Not TypeOf opt.Pipeline(Scan0) Is OrderBy Then
                Return Nothing
            End If

            Dim sortKey As OrderBy = DirectCast(opt.Pipeline(Scan0), OrderBy)
            Dim field As String = sortKey.GetSortField(symbol.symbolName)

            If String.IsNullOrEmpty(field) Then
                Return Nothing
            End If
            If Not (project Is Nothing OrElse project.HasField(field)) Then
                Return Nothing
            End If

            Return dataset.TryGetOrdered(field, sortKey.Descending)
        End Function

        Public Overrides Function ToString() As String
            Return project.ToString
        End Function
    End Class
End Namespace
