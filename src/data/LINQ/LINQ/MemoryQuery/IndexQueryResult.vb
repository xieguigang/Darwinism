#Region "Microsoft.VisualBasic::b6b0bee1f1e2b0b1bdfa0e9d5b1f0c22, src\data\LINQ\LINQ\MemoryQuery\IndexQueryResult.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class IndexQueryResult
    ' 
    '         Properties: Addresses, Used
    ' 
    ' /********************************************************************************/

#End Region

''' <summary>
''' 索引查询结果：记录索引是否被真正使用，以及索引筛选出来的候选数据行偏移。
''' </summary>
''' <remarks>
''' 这个对象用于区别下面三种不同的查询状态：
''' 
''' + 未使用索引（<see cref="Used"/>=<c>False</c>）：调用方应当回退为全表扫描
''' + 索引查询无命中（<see cref="Used"/>=<c>True</c> 且 <see cref="Addresses"/> 为空数组）
''' + 索引查询有命中（<see cref="Used"/>=<c>True</c> 且 <see cref="Addresses"/> 为候选行偏移）
''' </remarks>
Public Class IndexQueryResult

    ''' <summary>
    ''' 索引是否被真正使用。当这个属性值为<c>False</c>的时候，调用方必须回退为全表扫描。
    ''' </summary>
    ''' <returns></returns>
    Public Property Used As Boolean

    ''' <summary>
    ''' 候选数据行的偏移。空数组表示索引判定为无命中。
    ''' </summary>
    ''' <returns></returns>
    Public Property Addresses As Integer()

End Class
