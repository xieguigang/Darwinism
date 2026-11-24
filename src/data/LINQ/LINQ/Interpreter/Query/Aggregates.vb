#Region "Microsoft.VisualBasic::4d5e6f708192a3b4c5d6e7f8091a2b3c, src\data\LINQ\LINQ\Interpreter\Query\Aggregates.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Module Aggregates
    ' 
    '         Function: Aggregate, TryParse
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Globalization
Imports LINQ.Interpreter.Expressions
Imports Microsoft.VisualBasic.Linq
Imports any = Microsoft.VisualBasic.Scripting

Namespace Interpreter.Query

    ''' <summary>
    ''' 聚合函数类型
    ''' </summary>
    Public Enum AggregateKind
        Count
        Sum
        Average
        Min
        Max
        First
        Last
    End Enum

    ''' <summary>
    ''' 一个聚合函数调用的描述信息
    ''' </summary>
    Public Class AggregateCall

        Public ReadOnly Property Kind As AggregateKind
        ''' <summary>
        ''' 聚合函数的参数表达式。<c>Nothing</c>表示``count()``这种不带有参数的聚合调用。
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Argument As Expression
        Public ReadOnly Property Name As String

        Sub New(kind As AggregateKind, name As String, argument As Expression)
            Me.Kind = kind
            Me.Name = name
            Me.Argument = argument
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Name}({Argument})"
        End Function
    End Class

    ''' <summary>
    ''' 聚合函数的解析与求值
    ''' </summary>
    Public Module Aggregates

        ''' <summary>
        ''' 尝试将表达式解析为一个聚合函数的调用
        ''' </summary>
        ''' <param name="expr"></param>
        ''' <param name="call"></param>
        ''' <returns>
        ''' 当表达式不是一个已知的聚合函数调用的时候，函数返回<c>False</c>
        ''' </returns>
        Public Function TryParse(expr As Expression, ByRef aggCall As AggregateCall) As Boolean
            aggCall = Nothing

            If Not TypeOf expr Is FuncEval Then
                Return False
            End If

            Dim func As FuncEval = DirectCast(expr, FuncEval)
            Dim name As String = GetFunctionName(func.func)

            If String.IsNullOrEmpty(name) Then
                Return False
            End If

            Dim kind As AggregateKind

            Select Case name.ToLower
                Case "count" : kind = AggregateKind.Count
                Case "sum" : kind = AggregateKind.Sum
                Case "avg", "average", "mean" : kind = AggregateKind.Average
                Case "min" : kind = AggregateKind.Min
                Case "max" : kind = AggregateKind.Max
                Case "first" : kind = AggregateKind.First
                Case "last" : kind = AggregateKind.Last
                Case Else
                    Return False
            End Select

            Dim argument As Expression = Nothing

            If Not func.parameters.IsNullOrEmpty Then
                argument = func.parameters(0)
            End If

            aggCall = New AggregateCall(kind, name.ToLower, argument)

            Return True
        End Function

        Private Function GetFunctionName(func As Expression) As String
            If TypeOf func Is SymbolReference Then
                Return DirectCast(func, SymbolReference).symbolName
            ElseIf TypeOf func Is Literals Then
                Return any.ToString(DirectCast(func, Literals).value)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' 对给定的数据值集合求值聚合函数
        ''' </summary>
        ''' <param name="call"></param>
        ''' <param name="values">
        ''' 每一行的聚合参数值。对于``count()``这种没有参数的聚合调用，这个值为<c>Nothing</c>。
        ''' </param>
        ''' <param name="rowCount">当前分组之中所包含的数据行数量</param>
        ''' <returns></returns>
        Public Function Aggregate(aggCall As AggregateCall, values As Object(), rowCount As Integer) As Object
            Select Case aggCall.Kind
                Case AggregateKind.Count
                    If values Is Nothing Then
                        Return rowCount
                    Else
                        Return values.Count(Function(v) v IsNot Nothing)
                    End If
                Case AggregateKind.Sum
                    Dim numbers As Double()

                    If TryGetNumbers(values, numbers) Then
                        Return numbers.Sum
                    Else
                        Return Nothing
                    End If
                Case AggregateKind.Average
                    Dim numbers As Double()

                    If TryGetNumbers(values, numbers) Then
                        Return numbers.Average
                    Else
                        Return Nothing
                    End If
                Case AggregateKind.Min
                    Return GetExtremum(values, numbersAsc:=True)
                Case AggregateKind.Max
                    Return GetExtremum(values, numbersAsc:=False)
                Case AggregateKind.First
                    If values Is Nothing Then
                        Return Nothing
                    Else
                        Return values _
                            .Where(Function(v) v IsNot Nothing) _
                            .FirstOrDefault
                    End If
                Case AggregateKind.Last
                    If values Is Nothing Then
                        Return Nothing
                    Else
                        Return values _
                            .Where(Function(v) v IsNot Nothing) _
                            .LastOrDefault
                    End If
                Case Else
                    Throw New NotImplementedException(aggCall.Kind.ToString)
            End Select
        End Function

        ''' <summary>
        ''' 求最小值或者最大值：对于数值类型的数据使用数值比较，否则使用字符串比较
        ''' </summary>
        Private Function GetExtremum(values As Object(), numbersAsc As Boolean) As Object
            If values Is Nothing Then
                Return Nothing
            End If

            Dim numbers As Double()

            If TryGetNumbers(values, numbers) Then
                If numbersAsc Then
                    Return numbers.Min
                Else
                    Return numbers.Max
                End If
            End If

            Dim text As String() = values _
                .Where(Function(v) v IsNot Nothing) _
                .Select(AddressOf any.ToString) _
                .ToArray

            If text.Length = 0 Then
                Return Nothing
            ElseIf numbersAsc Then
                Return text.OrderBy(Function(s) s).First
            Else
                Return text.OrderByDescending(Function(s) s).First
            End If
        End Function

        Private Function TryGetNumbers(values As Object(), ByRef numbers As Double()) As Boolean
            If values Is Nothing Then
                numbers = Nothing
                Return False
            End If

            Dim list As New List(Of Double)

            For Each value As Object In values
                If value Is Nothing Then
                    Continue For
                End If

                Dim d As Double

                If Double.TryParse(any.ToString(value), NumberStyles.Float, CultureInfo.InvariantCulture, d) Then
                    list.Add(d)
                Else
                    numbers = Nothing
                    Return False
                End If
            Next

            numbers = list.ToArray

            Return list.Count > 0
        End Function
    End Module
End Namespace
