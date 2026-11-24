#Region "Microsoft.VisualBasic::1a2b3c4d5e6f708192a3b4c5d6e7f809, src\data\LINQ\LINQ\Interpreter\Query\WhereIndexTranslator.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed

    ' Summaries:

    '     Module WhereIndexTranslator
    ' 
    '         Function: Translate, TryGetConstant, TryGetField, TryTranslate
    ' 
    ' /********************************************************************************/

#End Region

Imports LINQ.Interpreter.Expressions

Namespace Interpreter.Query

    ''' <summary>
    ''' 将``WHERE``子句之中的条件表达式翻译为索引查询条件。
    ''' </summary>
    ''' <remarks>
    ''' 索引查询只是用于缩小候选的数据行集合，最终的数据行仍然会经过条件表达式的复核，
    ''' 所以这里对于无法确定语义的条件(例如``or``以及``not``)只会放弃索引收窄，而不会影响查询结果的正确性。
    ''' </remarks>
    Public Module WhereIndexTranslator

        ''' <summary>
        ''' 将查询条件表达式翻译为索引查询条件集合
        ''' </summary>
        ''' <param name="expr">条件表达式</param>
        ''' <param name="symbolName">查询的数据源符号名称</param>
        ''' <returns>
        ''' 当条件表达式无法使用索引进行收窄的时候，函数返回<c>Nothing</c>
        ''' </returns>
        Public Function Translate(expr As Expression, symbolName As String) As LINQ.Query()
            Dim queries As New List(Of LINQ.Query)

            If TryTranslate(expr, symbolName, queries) AndAlso queries.Count > 0 Then
                Return queries.ToArray
            Else
                Return Nothing
            End If
        End Function

        Private Function TryTranslate(expr As Expression, symbolName As String, ByRef queries As List(Of LINQ.Query)) As Boolean
            If TypeOf expr Is BinaryExpression Then
                Dim bin As BinaryExpression = DirectCast(expr, BinaryExpression)

                Select Case bin.Operator
                    Case "and"
                        ' 逻辑与运算：两个子条件都必须可以使用索引
                        ' 最终在索引层面对候选集合取交集
                        Return TryTranslate(bin.left, symbolName, queries) AndAlso
                            TryTranslate(bin.right, symbolName, queries)
                    Case "=", ">", ">=", "<", "<="
                        Dim field As String = Nothing
                        Dim value As Object = Nothing

                        If Not TryGetField(bin.left, symbolName, field) Then
                            Return False
                        End If
                        If Not TryGetConstant(bin.right, value) Then
                            Return False
                        End If

                        Call queries.Add(New LINQ.Query With {
                            .field = field,
                            .value = value,
                            .search = GetSearchType(bin.Operator),
                            .strict = bin.Operator = ">" OrElse bin.Operator = "<"
                        })

                        Return True
                    Case Else
                        ' or / not / <> 等条件不进行索引收窄
                        Return False
                End Select
            Else
                Return False
            End If
        End Function

        Private Function GetSearchType(op As String) As LINQ.Query.Type
            Select Case op
                Case "="
                    Return LINQ.Query.Type.ValueMatch
                Case ">", ">="
                    Return LINQ.Query.Type.ValueRangeGreaterThan
                Case "<", "<="
                    Return LINQ.Query.Type.ValueRangeLessThan
                Case Else
                    Throw New NotImplementedException(op)
            End Select
        End Function

        ''' <summary>
        ''' 尝试从表达式之中解析出数据源字段的名称：``x.field``或者``$x->field``
        ''' </summary>
        ''' <param name="expr"></param>
        ''' <param name="symbolName"></param>
        ''' <param name="field"></param>
        ''' <returns></returns>
        Public Function TryGetField(expr As Expression, symbolName As String, ByRef field As String) As Boolean
            If TypeOf expr Is MemberReference Then
                Dim member As MemberReference = DirectCast(expr, MemberReference)

                If TypeOf member.symbol Is SymbolReference AndAlso
                    String.Equals(DirectCast(member.symbol, SymbolReference).symbolName, symbolName, StringComparison.OrdinalIgnoreCase) Then

                    field = member.memberName
                    Return True
                End If
            End If

            field = Nothing
            Return False
        End Function

        ''' <summary>
        ''' 尝试从表达式之中解析出常量值
        ''' </summary>
        ''' <param name="expr"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function TryGetConstant(expr As Expression, ByRef value As Object) As Boolean
            If TypeOf expr Is Literals Then
                value = DirectCast(expr, Literals).value
                Return True
            Else
                value = Nothing
                Return False
            End If
        End Function

    End Module
End Namespace
