#Region "Microsoft.VisualBasic::9e0f1f1b9a0d4f7c8d3e2b5a6c7d8e9f, src\data\LINQ\LINQ\MemoryQuery\IndexedTable.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class IndexedTable
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CheckScalar, GetData, GetRowValues, TryGetGroups, TryGetOrdered
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.Framework.StorageProvider
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports any = Microsoft.VisualBasic.Scripting

''' <summary>
''' 带有查询索引的内存数据表：基于<see cref="DataFrameResolver"/>之中的表格数据，
''' 按照脚本之中声明的索引字段建立数值范围索引、词项哈希索引、全文索引以及编辑距离索引，
''' 从而可以基于索引来完成数据记录的过滤、分组以及排序。
''' </summary>
''' <remarks>
''' 索引的键值直接取自于<see cref="DataFrameResolver.EnumerateRowObjects"/>所返回的类型化行数据，
''' 与查询引擎全表扫描路径上面的取值方式保持一致，避免出现两条路径结果不一致的情况。
''' </remarks>
Public Class IndexedTable : Inherits MemoryIndex

    ReadOnly df As DataFrameResolver
    ReadOnly rows As Object()()
    ReadOnly m_headers As String()
    ReadOnly types As New Dictionary(Of String, Type)(StringComparer.OrdinalIgnoreCase)
    ReadOnly ordinals As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
    ReadOnly m_notes As New List(Of String)

    ''' <summary>
    ''' 数据表之中的总行数
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TotalRows As Integer
        Get
            Return rows.Length
        End Get
    End Property

    ''' <summary>
    ''' 数据表的列名集合
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Headers As String()
        Get
            Return m_headers
        End Get
    End Property

    ''' <summary>
    ''' 在建立索引的时候所产生的降级/忽略说明信息
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Notes As String()
        Get
            Return m_notes.ToArray
        End Get
    End Property

    ''' <summary>
    ''' 已经成功建立索引的字段集合
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IndexedFields As String()
        Get
            Dim fields As New List(Of String)

            fields.AddRange(m_valueindex.Keys)
            fields.AddRange(m_hashindex.Keys)
            fields.AddRange(m_fulltext.Keys)
            fields.AddRange(m_levenshtein.Keys)

            Return fields.Distinct.ToArray
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="df">表格数据，需要已经完成了<see cref="DataFrameResolver.MeasureTypeSchema"/>列类型测量</param>
    ''' <param name="terms">需要建立词项哈希索引的字段</param>
    ''' <param name="ranges">需要建立数值范围索引的字段</param>
    ''' <param name="fulltext">需要建立全文搜索索引的字段</param>
    ''' <param name="levenshtein">需要建立编辑距离索引的字段</param>
    Sub New(df As DataFrameResolver,
            Optional terms As String() = Nothing,
            Optional ranges As String() = Nothing,
            Optional fulltext As String() = Nothing,
            Optional levenshtein As String() = Nothing)

        Me.df = df
        Me.rows = df.EnumerateRowObjects().ToArray
        Me.m_headers = df.HeadTitles

        Call BuildSchema()

        For Each field As String In If(ranges, New String() {})
            Call BuildRangeIndex(field)
        Next

        For Each field As String In If(terms, New String() {})
            Call BuildTermIndex(field)
        Next

        For Each field As String In If(fulltext, New String() {})
            Call BuildFullTextIndex(field)
        Next

        For Each field As String In If(levenshtein, New String() {})
            Call BuildLevenshteinIndex(field)
        Next
    End Sub

    Private Sub BuildSchema()
        Dim schema As NamedValue(Of Type)() = df.Headers

        For i As Integer = 0 To m_headers.Length - 1
            ordinals(m_headers(i)) = i

            If Not schema.IsNullOrEmpty AndAlso i < schema.Length Then
                types(m_headers(i)) = schema(i).Value
            Else
                types(m_headers(i)) = GetType(String)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 查找字段在数据表之中的列下标
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns></returns>
    Private Function GetOrdinal(field As String) As Integer
        Dim offset As Integer

        If ordinals.TryGetValue(field, offset) Then
            Return offset
        Else
            Throw New MissingPrimaryKeyException($"missing data field '{field}' in the data source!")
        End If
    End Function

    Private Sub BuildRangeIndex(field As String)
        Dim type As Type = types(GetHeaders(field))

        Select Case type
            Case GetType(Double)
                Call ValueRange(field, GetType(Double))
            Case GetType(Integer)
                Call ValueRange(field, GetType(Integer))
            Case GetType(Date)
                Call ValueRange(field, GetType(Date))
            Case Else
                ' 非数值类型的字段无法建立范围索引，降级为词项哈希索引
                m_notes.Add($"the field '{field}' is not a numeric field({type.Name}), a term hash index was created instead.")
                Call BuildTermIndex(field)
        End Select
    End Sub

    Private Sub BuildTermIndex(field As String)
        Dim name As String = GetHeaders(field)

        Call HashIndex(name)
    End Sub

    Private Sub BuildFullTextIndex(field As String)
        Dim name As String = GetHeaders(field)

        Call FullText(name)
    End Sub

    Private Sub BuildLevenshteinIndex(field As String)
        Dim name As String = GetHeaders(field)

        Call LevenshteinTree(name)
    End Sub

    ''' <summary>
    ''' 返回数据表之中的标准列名（忽略大小写）
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns></returns>
    Private Function GetHeaders(field As String) As String
        Call GetOrdinal(field)

        Return m_headers(GetOrdinal(field))
    End Function

    ''' <summary>
    ''' 尝试解析字段所对应的标准列名，字段不存在的时候函数返回<c>False</c>而不是抛出异常
    ''' </summary>
    ''' <param name="field"></param>
    ''' <param name="header"></param>
    ''' <returns></returns>
    Private Function TryGetHeaderName(field As String, ByRef header As String) As Boolean
        Dim offset As Integer

        If ordinals.TryGetValue(field, offset) Then
            header = m_headers(offset)
            Return True
        Else
            header = Nothing
            Return False
        End If
    End Function

    ''' <summary>
    ''' 取出指定数据行的原始类型化数据
    ''' </summary>
    ''' <param name="i"></param>
    ''' <returns></returns>
    Public Function GetRowValues(i As Integer) As Object()
        Return rows(i)
    End Function

    Protected Overrides Function GetData(Of V)(field As String) As V()
        Dim offset As Integer = GetOrdinal(field)
        Dim vec As V() = New V(rows.Length - 1) {}

        For i As Integer = 0 To rows.Length - 1
            vec(i) = CastValue(Of V)(rows(i)(offset))
        Next

        Return vec
    End Function

    ''' <summary>
    ''' 将表格之中的原始数据值转换为索引所需要的类型，<c>Nothing</c>会转换为目标类型的默认值，
    ''' 其行为与VB之中的晚绑定数值比较一致(<c>Nothing</c>参与数值运算的时候被视为零值)。
    ''' </summary>
    Private Shared Function CastValue(Of V)(value As Object) As V
        If value Is Nothing Then
            If GetType(V) Is GetType(String) Then
                Return DirectCast(CObj(""), V)
            Else
                Return Nothing
            End If
        ElseIf TypeOf value Is V Then
            Return DirectCast(value, V)
        Else
            Return DirectCast(CObj(any.CTypeDynamic(any.ToString(value), GetType(V))), V)
        End If
    End Function

    ''' <summary>
    ''' 数据字段之中的元素类型总是标量类型
    ''' </summary>
    Protected Overrides Function CheckScalar(field As String) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' 尝试使用词项哈希索引来获取指定字段的分组信息：分组键 -> 数据行偏移集合
    ''' </summary>
    ''' <param name="field"></param>
    ''' <returns>
    ''' 当对应的字段没有建立词项哈希索引的时候，函数返回<c>Nothing</c>
    ''' </returns>
    Public Function TryGetGroups(field As String) As Dictionary(Of String, Integer())
        Dim name As String

        If Not TryGetHeaderName(field, name) Then
            Return Nothing
        End If
        If Not m_hashindex.ContainsKey(name) Then
            Return Nothing
        End If

        Dim hash As TermHashIndex = m_hashindex(name)

        If hash Is Nothing Then
            Return Nothing
        End If

        Dim offset As Integer = GetOrdinal(name)
        Dim groups As New Dictionary(Of String, Integer())

        For Each term As KeyValuePair(Of String, Integer()) In hash.GetHashIndex
            Dim addresses As Integer() = term.Value
            ' 索引之中的键值已经进行了小写归一化处理
            ' 所以这里需要从第一行数据之中还原出原始的分组标签
            Dim label As String = If(addresses.IsNullOrEmpty,
                                     term.Key,
                                     any.ToString(rows(addresses(Scan0))(offset)))

            groups(label) = addresses
        Next

        Return groups
    End Function

    ''' <summary>
    ''' 尝试按照指定字段的数值索引的顺序来枚举数据行的偏移
    ''' </summary>
    ''' <param name="field"></param>
    ''' <param name="descending"></param>
    ''' <returns>
    ''' 当对应的字段没有建立数值范围索引的时候，函数返回<c>Nothing</c>
    ''' </returns>
    Public Function TryGetOrdered(field As String, descending As Boolean) As Integer()
        Dim name As String

        If Not TryGetHeaderName(field, name) Then
            Return Nothing
        End If

        Dim index As ValueIndex = m_valueindex.TryGetValue(name)

        If index Is Nothing Then
            Return Nothing
        End If

        Select Case index.UnderlyingType
            Case GetType(Double)
                Return DirectCast(index, RangeIndex(Of Double)).OrderedAddresses(descending).ToArray
            Case GetType(Integer)
                Return DirectCast(index, RangeIndex(Of Integer)).OrderedAddresses(descending).ToArray
            Case GetType(Date)
                Return DirectCast(index, RangeIndex(Of Date)).OrderedAddresses(descending).ToArray
            Case Else
                Return Nothing
        End Select
    End Function
End Class
