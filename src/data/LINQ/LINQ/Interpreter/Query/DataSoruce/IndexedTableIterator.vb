#Region "Microsoft.VisualBasic::0d1e2f3a4b5c6d7e8f9a0b1c2d3e4f5a, src\data\LINQ\LINQ\Interpreter\Query\DataSoruce\IndexedTableIterator.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class IndexedTableIterator
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: PopulatesData, TryGetGroups, TryGetOrdered, TryQuery
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime
Imports LINQ.Runtime.Drivers
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Interpreter.Query

    ''' <summary>
    ''' 带查询索引的数据集：使用<see cref="IndexedTable"/>之中的索引来筛选候选数据行，
    ''' 而不是对数据表进行逐行的遍历扫描。
    ''' </summary>
    Public Class IndexedTableIterator : Inherits DataSet

        ReadOnly source As IndexTableSource
        ReadOnly m_table As IndexedTable

        ''' <summary>
        ''' 当前数据集所使用的索引表对象
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Table As IndexedTable
            Get
                Return m_table
            End Get
        End Property

        Sub New(query As QueryExpression, source As IndexTableSource, env As Environment)
            MyBase.New(query.symbol, env)

            Me.source = source
            Me.m_table = New IndexedTable(
                df:=DataFrameDriver.LoadTable(source.Uri),
                terms:=source.Terms,
                ranges:=source.Ranges,
                fulltext:=source.FullText,
                levenshtein:=source.Levenshtein
            )
        End Sub

        Public Overrides Iterator Function PopulatesData() As IEnumerable(Of Object)
            For i As Integer = 0 To m_table.TotalRows - 1
                Yield GetRow(i)
            Next
        End Function

        ''' <summary>
        ''' 按照给定的数据行偏移枚举数据（索引筛选出来的候选行）
        ''' </summary>
        ''' <param name="addresses"></param>
        ''' <returns></returns>
        Public Overrides Iterator Function PopulatesData(addresses As Integer()) As IEnumerable(Of Object)
            For Each i As Integer In addresses.SafeQuery
                Yield GetRow(i)
            Next
        End Function

        Private Function GetRow(i As Integer) As JavaScriptObject
            Dim values As Object() = m_table.GetRowValues(i)
            Dim obj As New JavaScriptObject

            For offset As Integer = 0 To m_table.Headers.Length - 1
                obj(m_table.Headers(offset)) = values(offset)
            Next

            Return obj
        End Function

        ''' <summary>
        ''' 基于索引来筛选候选数据行
        ''' </summary>
        ''' <param name="filter"></param>
        ''' <returns></returns>
        Public Overrides Function TryQuery(filter As IEnumerable(Of LINQ.Query)) As IndexQueryResult
            Return m_table.TryGetIndex(filter)
        End Function

        Public Overrides Function TryGetGroups(field As String) As Dictionary(Of String, Integer())
            Return m_table.TryGetGroups(field)
        End Function

        Public Overrides Function TryGetOrdered(field As String, descending As Boolean) As Integer()
            Return m_table.TryGetOrdered(field, descending)
        End Function
    End Class
End Namespace
