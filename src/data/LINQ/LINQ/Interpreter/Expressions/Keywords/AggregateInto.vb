#Region "Microsoft.VisualBasic::3c4d5e6f708192a3b4c5d6e7f8091a2b, src\data\LINQ\LINQ\Interpreter\Expressions\Keywords\AggregateInto.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Class AggregateInto
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
    ''' 聚合查询的结果输出: ``INTO &lt;name> = &lt;aggregate expression> [, ...]``
    ''' </summary>
    ''' <remarks>
    ''' 这个表达式对象本身不参与数据行的求值运算，而是由<see cref="Query.AggregateExpression"/>
    ''' 在完成数据行的分组之后，针对每一个分组求值其中的聚合函数。
    ''' </remarks>
    Public Class AggregateInto : Inherits KeywordExpression

        ''' <summary>
        ''' 聚合输出的字段列表
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Fields As NamedValue(Of Expression)()

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Into"
            End Get
        End Property

        Sub New(fields As IEnumerable(Of NamedValue(Of Expression)))
            Me.Fields = fields.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"into {Fields.Select(Function(a) $"{a.Name} = {a.Value}").JoinBy(", ")}"
        End Function
    End Class
End Namespace
