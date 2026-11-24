#Region "Microsoft.VisualBasic::70e8ded3851ec091c59cc014def65b94, src\data\LINQ\LINQ\Interpreter\Query\AggregateExpression.vb"

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

    '   Total Lines: 19
    '    Code Lines: 12 (63.16%)
    ' Comment Lines: 3 (15.79%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (21.05%)
    '     File Size: 584 B


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
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Query

    ''' <summary>
    ''' aggregate ... into ...
    ''' </summary>
    ''' <remarks>
    ''' 聚合查询的脚本语法：
    ''' 
    ''' ```text
    ''' AGGREGATE x as row
    ''' IN index("data.csv", range = ["PeakQuality"], terms = ["Class"])
    ''' WHERE x.PeakQuality >= 0.9
    ''' GROUP BY x.Class
    ''' INTO n = count(x), maxValue = max(x.Area), avgValue = avg(x.Area)
    ''' ```
    ''' 
    ''' ``GROUP BY``子句是可选的，在没有分组键的时候将会对整个数据集合做全局聚合，输出单行结果。
    ''' </remarks>
    Public Class AggregateExpression : Inherits QueryExpression

        ReadOnly groupBy As GroupBy
        ReadOnly into As AggregateInto
        ReadOnly opt As Options

        Sub New(symbol As SymbolDeclare,
                sequence As Expression,
                exec As IEnumerable(Of Expression),
                groupBy As GroupBy,
                into As AggregateInto,
                opt As Options)

            Call MyBase.New(symbol, sequence, exec)

            Me.groupBy = groupBy
            Me.into = into
            Me.opt = opt
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns>
        ''' 每一个分组一行聚合结果的<see cref="JavaScriptObject"/>数组
        ''' </returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim env As Environment = context.env
            Dim closure As New ExecutableContext With {
                .env = New Environment(parent:=env),
                .throwError = context.throwError
            }

            Call LoadDrivers(context)

            Dim dataset As DataSet = GetDataSet(context)
            Dim queries As LINQ.Query() = GetIndexQueries()
            Dim indexResult As IndexQueryResult = Nothing

            If queries IsNot Nothing Then
                indexResult = dataset.TryQuery(queries)
            End If

            LastIndex = indexResult

            Dim candidates As Integer() = Nothing

            If indexResult IsNot Nothing AndAlso indexResult.Used Then
                candidates = indexResult.Addresses
            End If

            Call closure.env.AddSymbol(symbol.symbolName, symbol.type)

            Dim groups As List(Of AggregateGroup) = PopulateGroups(dataset, candidates, closure)
            Dim output As New List(Of JavaScriptObject)

            For Each group As AggregateGroup In groups
                Dim obj As New JavaScriptObject

                ' 分组键也作为结果的输出字段
                If groupBy IsNot Nothing AndAlso group.Key IsNot Nothing Then
                    For i As Integer = 0 To groupBy.Keys.Length - 1
                        obj(groupBy.Keys(i).Name) = group.Key(i)
                    Next
                End If

                For Each field As NamedValue(Of Expression) In into.Fields
                    obj(field.Name) = AggregateField(field.Value, group, closure)
                Next

                output.Add(obj)
            Next

            Return opt.RunOptionPipeline(output, context).ToArray
        End Function

        ''' <summary>
        ''' 按照``WHERE``条件筛选数据行并且进行分组
        ''' </summary>
        Private Function PopulateGroups(dataset As DataSet, candidates As Integer(), closure As ExecutableContext) As List(Of AggregateGroup)
            Dim groups As New Dictionary(Of String, AggregateGroup)(StringComparer.Ordinal)
            Dim indexGroups As Dictionary(Of String, Integer()) = Nothing
            Dim groupField As String = Nothing

            If groupBy IsNot Nothing AndAlso groupBy.Keys.Length = 1 Then
                If WhereIndexTranslator.TryGetField(groupBy.Keys(0).Value, symbol.symbolName, groupField) Then
                    ' 分组键可以直接使用数据源之中的词项索引
                    indexGroups = dataset.TryGetGroups(groupField)
                End If
            End If

            If indexGroups IsNot Nothing Then
                Call PopulateGroupsByIndex(dataset, candidates, indexGroups, closure, groups)
            Else
                Dim rows As IEnumerable(Of Object) = If(
                    candidates Is Nothing,
                    dataset.PopulatesData(),
                    dataset.PopulatesData(candidates)
                )

                For Each item As Object In rows
                    closure.env.FindSymbol(symbol.symbolName).value = item

                    If Not PassWhere(closure) Then
                        Continue For
                    End If

                    Dim key As Object() = GetGroupKey(closure)
                    Dim keyId As String = GetKeyId(key)
                    Dim group As AggregateGroup = Nothing

                    If Not groups.TryGetValue(keyId, group) Then
                        group = New AggregateGroup(key)
                        groups(keyId) = group
                    End If

                    group.Rows.Add(item)
                Next
            End If

            Return groups _
                .Values _
                .OrderBy(Function(g) GetKeyId(g.Key)) _
                .ToList
        End Function

        ''' <summary>
        ''' 使用数据源之中的词项索引来完成分组：索引直接提供了分组键以及分组键所对应的数据行偏移
        ''' </summary>
        Private Sub PopulateGroupsByIndex(dataset As DataSet,
                                          candidates As Integer(),
                                          indexGroups As Dictionary(Of String, Integer()),
                                          closure As ExecutableContext,
                                          ByRef groups As Dictionary(Of String, AggregateGroup))

            Dim allRows As Object() = dataset.PopulatesData().ToArray
            Dim filter As HashSet(Of Integer) = Nothing

            If candidates IsNot Nothing Then
                filter = New HashSet(Of Integer)(candidates)
            End If

            For Each term As KeyValuePair(Of String, Integer()) In indexGroups
                Dim rows As New List(Of Object)

                For Each i As Integer In term.Value.SafeQuery
                    If filter IsNot Nothing AndAlso Not filter.Contains(i) Then
                        Continue For
                    End If
                    If i < 0 OrElse i >= allRows.Length Then
                        Continue For
                    End If

                    closure.env.FindSymbol(symbol.symbolName).value = allRows(i)

                    If PassWhere(closure) Then
                        rows.Add(allRows(i))
                    End If
                Next

                If rows.Count = 0 Then
                    Continue For
                End If

                closure.env.FindSymbol(symbol.symbolName).value = rows(0)

                groups(term.Key) = New AggregateGroup(GetGroupKey(closure), rows)
            Next
        End Sub

        Private Function GetGroupKey(closure As ExecutableContext) As Object()
            If groupBy Is Nothing Then
                Return Nothing
            Else
                Return DirectCast(groupBy.Exec(closure), Object())
            End If
        End Function

        Private Shared Function GetKeyId(key As Object()) As String
            If key Is Nothing OrElse key.Length = 0 Then
                Return ""
            End If

            Return key _
                .Select(Function(v) any.ToString(v)) _
                .JoinBy(vbTab)
        End Function

        Private Function PassWhere(closure As ExecutableContext) As Boolean
            For Each line As Expression In executeQueue
                If TypeOf line Is WhereFilter Then
                    If Not BinaryExpression.CBoolValue(line.Exec(closure)) Then
                        Return False
                    End If
                End If
            Next

            Return True
        End Function

        ''' <summary>
        ''' 计算一个分组之中的聚合字段的值
        ''' </summary>
        Private Function AggregateField(expr As Expression, group As AggregateGroup, closure As ExecutableContext) As Object
            Dim aggCall As AggregateCall = Nothing

            If Aggregates.TryParse(expr, aggCall) Then
                Dim values As Object() = Nothing

                If aggCall.Argument IsNot Nothing Then
                    Dim list As New List(Of Object)

                    For Each item As Object In group.Rows
                        closure.env.FindSymbol(symbol.symbolName).value = item
                        list.Add(aggCall.Argument.Exec(closure))
                    Next

                    values = list.ToArray
                End If

                Return Aggregates.Aggregate(aggCall, values, group.Rows.Count)
            Else
                ' 非聚合函数的表达式：按照分组之中的第一行数据进行求值
                If group.Rows.Count = 0 Then
                    Return Nothing
                Else
                    closure.env.FindSymbol(symbol.symbolName).value = group.Rows(0)

                    Return expr.Exec(closure)
                End If
            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"{groupBy} {into}"
        End Function

        ''' <summary>
        ''' 聚合查询过程之中的一个分组
        ''' </summary>
        Private Class AggregateGroup

            ''' <summary>
            ''' 分组键的取值
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Key As Object()
            ''' <summary>
            ''' 当前分组之中所包含的数据行
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Rows As New List(Of Object)

            Sub New(key As Object())
                Me.Key = key
            End Sub

            Sub New(key As Object(), rows As IEnumerable(Of Object))
                Call Me.New(key)
                Me.Rows.AddRange(rows)
            End Sub
        End Class
    End Class
End Namespace
