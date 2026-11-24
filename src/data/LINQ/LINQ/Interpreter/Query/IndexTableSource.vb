#Region "Microsoft.VisualBasic::a1b2c3d4e5f60718293a4b5c6d7e8f90, src\data\LINQ\LINQ\Interpreter\Query\IndexTableSource.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class IndexTableSource
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    ' /********************************************************************************/

#End Region

Namespace Interpreter.Query

    ''' <summary>
    ''' 带索引的数据源的规格描述对象：由脚本之中的``index(...)``函数创建，
    ''' 描述数据源文件的路径以及需要建立索引的字段集合。
    ''' </summary>
    Public Class IndexTableSource

        ''' <summary>
        ''' 数据源文件的路径
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Uri As String

        ''' <summary>
        ''' 需要建立词项哈希索引的字段集合
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Terms As String()

        ''' <summary>
        ''' 需要建立数值范围索引的字段集合
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Ranges As String()

        ''' <summary>
        ''' 需要建立全文搜索索引的字段集合
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FullText As String()

        ''' <summary>
        ''' 需要建立编辑距离索引的字段集合
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Levenshtein As String()

        Sub New(uri As String,
                Optional terms As String() = Nothing,
                Optional ranges As String() = Nothing,
                Optional fulltext As String() = Nothing,
                Optional levenshtein As String() = Nothing)

            Me.Uri = uri
            Me.Terms = terms
            Me.Ranges = ranges
            Me.FullText = fulltext
            Me.Levenshtein = levenshtein
        End Sub

        Public Overrides Function ToString() As String
            Return $"index(""{Uri}"", terms=[{Terms.JoinBy(", ")}], range=[{Ranges.JoinBy(", ")}], fulltext=[{FullText.JoinBy(", ")}])"
        End Function
    End Class
End Namespace
