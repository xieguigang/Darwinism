#Region "Microsoft.VisualBasic::2b3c4d5e6f708192a3b4c5d6e7f8091a, src\data\LINQ\LINQ\Interpreter\Expressions\Keywords\GroupBy.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class GroupBy
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: Exec, ToString
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Runtime
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Interpreter.Expressions

    ''' <summary>
    ''' 聚合查询的分组操作：``GROUP BY &lt;key> [, &lt;key> ...]``
    ''' </summary>
    Public Class GroupBy : Inherits KeywordExpression

        ''' <summary>
        ''' 分组键表达式集合
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Keys As NamedValue(Of Expression)()

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Group By"
            End Get
        End Property

        Sub New(keys As IEnumerable(Of NamedValue(Of Expression)))
            Me.Keys = keys.ToArray
        End Sub

        ''' <summary>
        ''' 求值当前数据行的分组键
        ''' </summary>
        ''' <param name="context"></param>
        ''' <returns></returns>
        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim tuple As New List(Of Object)

            For Each key As NamedValue(Of Expression) In Keys
                tuple.Add(key.Value.Exec(context))
            Next

            Return tuple.ToArray
        End Function

        Public Overrides Function ToString() As String
            Return $"group by {Keys.Select(Function(a) $"{a.Name} = {a.Value}").JoinBy(", ")}"
        End Function
    End Class
End Namespace
