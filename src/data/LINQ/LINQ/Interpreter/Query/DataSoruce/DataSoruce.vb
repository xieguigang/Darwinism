#Region "Microsoft.VisualBasic::8af4711d352fde1006be4a159d0cbe40, src\data\LINQ\LINQ\Interpreter\Query\DataSoruce\DataSoruce.vb"

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

    '   Total Lines: 34
    '    Code Lines: 26 (76.47%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (23.53%)
    '     File Size: 1.16 KB


    '     Class DataSet
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateDataSet
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions
Imports LINQ.Runtime
Imports Microsoft.VisualBasic.Linq

Namespace Interpreter.Query

    Public MustInherit Class DataSet

        Protected ReadOnly env As Environment
        Protected ReadOnly symbolDeclare As SymbolDeclare

        Sub New(symbolDeclare As SymbolDeclare, env As Environment)
            Me.env = env
            Me.symbolDeclare = symbolDeclare
        End Sub

        Public MustOverride Function PopulatesData() As IEnumerable(Of Object)

        ''' <summary>
        ''' 按照给定的数据行偏移枚举数据
        ''' </summary>
        ''' <param name="addresses">数据行的偏移</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 默认的实现方式是先枚举出全部数据然后再按照偏移进行选择操作。
        ''' 支持索引的数据源会重写这个函数以避免不必要的遍历操作。
        ''' </remarks>
        Public Overridable Function PopulatesData(addresses As Integer()) As IEnumerable(Of Object)
            Dim all As Object() = PopulatesData().ToArray
            Dim output As New List(Of Object)

            For Each i As Integer In If(addresses, New Integer() {})
                If i >= 0 AndAlso i < all.Length Then
                    output.Add(all(i))
                End If
            Next

            Return output
        End Function

        ''' <summary>
        ''' 尝试使用当前数据源的索引来筛选出满足条件的候选数据行
        ''' </summary>
        ''' <param name="filter">查询条件集合</param>
        ''' <returns>
        ''' 当当前数据源不支持索引查询的时候，函数返回<c>Nothing</c>，调用方应当回退为全表扫描。
        ''' </returns>
        Public Overridable Function TryQuery(filter As IEnumerable(Of LINQ.Query)) As IndexQueryResult
            Return Nothing
        End Function

        ''' <summary>
        ''' 尝试使用当前数据源的分组字段索引来获取分组信息
        ''' </summary>
        ''' <param name="field"></param>
        ''' <returns>
        ''' 当当前数据源没有对应字段的分组索引的时候，函数返回<c>Nothing</c>。
        ''' </returns>
        Public Overridable Function TryGetGroups(field As String) As Dictionary(Of String, Integer())
            Return Nothing
        End Function

        ''' <summary>
        ''' 尝试使用当前数据源的数值索引来获取排序之后的数据行偏移
        ''' </summary>
        ''' <param name="field"></param>
        ''' <param name="descending"></param>
        ''' <returns>
        ''' 当当前数据源没有对应字段的数值索引的时候，函数返回<c>Nothing</c>。
        ''' </returns>
        Public Overridable Function TryGetOrdered(field As String, descending As Boolean) As Integer()
            Return Nothing
        End Function

        Public Shared Function CreateDataSet(query As QueryExpression, context As ExecutableContext) As DataSet
            Dim env As Environment = context.env

            If query.IsURISource Then
                Return New URIIteratorDriver(query, query.GetSeqValue(Nothing), env)
            Else
                Dim seqVal As Object = query.GetSeqValue(context)

                If TypeOf seqVal Is IndexTableSource Then
                    Return New IndexedTableIterator(query, DirectCast(seqVal, IndexTableSource), env)
                ElseIf TypeOf seqVal Is String Then
                    Return New URIIteratorDriver(query, seqVal, env)
                Else
                    Return New SymbolIterator(query, seqVal, env)
                End If
            End If
        End Function
    End Class
End Namespace
