#Region "Microsoft.VisualBasic::529332e50331bc840128c05759e740a9, src\data\LINQ\LINQ\Interpreter\Script\SyntaxImplements.vb"

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

    '   Total Lines: 308
    '    Code Lines: 263 (85.39%)
    ' Comment Lines: 1 (0.32%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 44 (14.29%)
    '     File Size: 13.29 KB


    '     Module SyntaxImplements
    ' 
    '         Function: CreateAggregateQuery, CreateProjectionQuery, GetParameters, GetProjection, GetSequence
    '                   GetVector, IsClosure, IsNumeric, JoinOperators, ParseExpression
    '                   ParseKeywordExpression, ParseToken, PopulateExpressions, PopulateQueryExpression
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports LINQ.Script.Builders
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Namespace Script

    Public Module SyntaxImplements

        <Extension>
        Private Function IsNumeric(t As Token) As Boolean
            Return t.name = Tokens.Integer OrElse t.name = Tokens.Number
        End Function

        <Extension>
        Private Function JoinOperators(tokenList As IEnumerable(Of Token)) As IEnumerable(Of Token)
            Dim list As New List(Of Token)(tokenList)
            Dim t As Token

            For i As Integer = 0 To list.Count - 1
                If i >= list.Count Then
                    Exit For
                Else
                    t = list(i)
                End If

                If t = (Tokens.Operator, "-") AndAlso list(i + 1).isNumeric Then
                    t = New Token(list(i + 1).name, -Val(list(i + 1).text))
                    list.RemoveAt(i + 1)
                    list.RemoveAt(i)
                    list.Insert(i, t)
                ElseIf t = (Tokens.Operator, ">") OrElse t = (Tokens.Operator, "<") Then
                    If list(i + 1) = (Tokens.Operator, "=") OrElse list(i + 1) = (Tokens.Operator, ">") Then
                        t = New Token(Tokens.Operator, t.text & list(i + 1).text)
                        list.RemoveAt(i + 1)
                        list.RemoveAt(i)
                        list.Insert(i, t)
                    End If
                End If
            Next

            Return list
        End Function

        ReadOnly sortOrders As Index(Of String) = {"descending", "ascending"}

        <Extension>
        Public Function PopulateQueryExpression(tokenList As IEnumerable(Of Token)) As Expression
            Dim blocks As List(Of Token()) = tokenList _
                .JoinOperators _
                .SplitByTopLevelStack _
                .AsList
            Dim import As New List(Of ImportDataDriver)

            If blocks(0)(Scan0) = (Tokens.keyword, "imports") Then
                import += New ImportDataDriver(blocks(0)(1).text)
            End If

            For i As Integer = 1 To blocks.Count - 1
                If i >= blocks.Count Then
                    Exit For
                End If

                If blocks(i).Length = 1 AndAlso blocks(i)(Scan0).name = Tokens.keyword Then
                    If blocks(i)(Scan0).text.ToLower Like sortOrders Then
                        blocks(i - 1) = blocks(i - 1) _
                            .JoinIterates(blocks(i)) _
                            .ToArray
                        blocks.RemoveAt(i)
                    End If
                ElseIf blocks(i)(Scan0) = (Tokens.keyword, "imports") Then
                    import += New ImportDataDriver(blocks(i)(1).text)
                End If
            Next

            blocks = (From block In blocks Where block(Scan0) <> (Tokens.keyword, "imports")).AsList

            If blocks(Scan0).First.isKeywordFrom Then
                Return blocks(Scan0).CreateProjectionQuery(blocks.Skip(1).ToArray).AddAttachDrivers(import)
            ElseIf blocks(Scan0).First.isKeywordAggregate Then
                Return blocks(Scan0).CreateAggregateQuery(blocks.Skip(1).ToArray).AddAttachDrivers(import)
            Else
                Throw New SyntaxErrorException
            End If
        End Function

        <Extension>
        Private Function CreateProjectionQuery(symbol As Token(), blocks As Token()()) As ProjectionExpression
            Dim symbolExpr As SymbolDeclare = symbol.ParseExpression
            Dim i As Integer = 0
            Dim seq As Expression = blocks.GetSequence(offset:=i)
            Dim exec As Expression() = blocks.Skip(i).PopulateExpressions.ToArray
            Dim proj As Expression = exec.Where(Function(t) TypeOf t Is OutputProjection).FirstOrDefault
            Dim opt As New Options(exec.Where(Function(t) TypeOf t Is PipelineKeyword))
            Dim execProgram As Expression() = exec _
                .Where(Function(t)
                           Return (Not TypeOf t Is PipelineKeyword) AndAlso (Not TypeOf t Is OutputProjection)
                       End Function) _
                .ToArray
            Dim Linq As New ProjectionExpression(symbolExpr, seq, execProgram, proj, opt)

            Return Linq
        End Function

        <Extension>
        Private Iterator Function PopulateExpressions(blocks As IEnumerable(Of Token())) As IEnumerable(Of Expression)
            For Each blockLine As Token() In blocks
                Yield ParseExpression(blockLine)
            Next
        End Function

        <Extension>
        Private Function GetSequence(blocks As Token()(), ByRef offset As Integer) As Expression
            If Not blocks(Scan0).First.isKeyword("in") Then
                Throw New SyntaxErrorException
            ElseIf blocks(Scan0).Length = 1 Then
                offset = 2
                Return blocks(1).ParseExpression
            Else
                offset = 1
                Return blocks(0).ParseExpression
            End If
        End Function

        <Extension>
        Private Function CreateAggregateQuery(symbol As Token(), blocks As Token()()) As AggregateExpression
            Dim symbolExpr As SymbolDeclare = symbol.ParseExpression
            Dim i As Integer = 0
            Dim seq As Expression = blocks.GetSequence(offset:=i)
            Dim exec As Expression() = blocks.Skip(i).PopulateExpressions.ToArray
            Dim into As AggregateInto = exec.OfType(Of AggregateInto).FirstOrDefault
            Dim groupBy As GroupBy = exec.OfType(Of GroupBy).FirstOrDefault
            Dim opt As New Options(exec.Where(Function(t) TypeOf t Is PipelineKeyword))
            Dim execProgram As Expression() = exec _
                .Where(Function(t)
                           Return (Not TypeOf t Is PipelineKeyword) AndAlso
                               (Not TypeOf t Is AggregateInto) AndAlso
                               (Not TypeOf t Is GroupBy)
                       End Function) _
                .ToArray

            If into Is Nothing Then
                Throw New SyntaxErrorException("missing 'INTO' statement for the aggregate query!")
            End If

            Return New AggregateExpression(symbolExpr, seq, execProgram, groupBy, into, opt)
        End Function

        <Extension>
        Friend Function ParseToken(t As Token) As Expression
            If t.name = Tokens.Symbol Then
                Return New SymbolReference(t.text)
            ElseIf t.name = Tokens.Boolean OrElse
                t.name = Tokens.Integer OrElse
                t.name = Tokens.Number OrElse
                t.name = Tokens.Literal Then

                Return New Literals(t)
            ElseIf t.name = Tokens.CommandLineArgument Then
                Return New CommandLineArgument(t.text)
            Else
                Throw New NotImplementedException
            End If
        End Function

        <Extension>
        Private Function GetProjection(tokenList As IEnumerable(Of Token)) As Expression
            Return New OutputProjection(tokenList.GetProjectionFields)
        End Function

        ''' <summary>
        ''' 解析投影表达式之中的字段列表
        ''' </summary>
        ''' <param name="tokenList"></param>
        ''' <returns></returns>
        <Extension>
        Private Function GetProjectionFields(tokenList As IEnumerable(Of Token)) As NamedValue(Of Expression)()
            Dim values As Expression() = tokenList.GetParameters.ToArray
            Dim fields As New List(Of NamedValue(Of Expression))

            For Each item As Expression In values
                If TypeOf item Is BinaryExpression Then
                    With DirectCast(item, BinaryExpression)
                        If .LikeValueAssign Then
                            fields.Add(New NamedValue(Of Expression)(DirectCast(.left, SymbolReference).symbolName, .right))
                        Else
                            fields.Add(New NamedValue(Of Expression)(item.ToString, item))
                        End If
                    End With
                ElseIf TypeOf item Is MemberReference Then
                    With DirectCast(item, MemberReference)
                        fields.Add(New NamedValue(Of Expression)(.memberName, item))
                    End With
                Else
                    fields.Add(New NamedValue(Of Expression)(item.ToString, item))
                End If
            Next

            Return fields.ToArray
        End Function

        <Extension>
        Private Function ParseKeywordExpression(tokenList As Token()) As Expression
            If tokenList(Scan0).isKeywordFrom OrElse tokenList(Scan0).isKeywordAggregate OrElse tokenList(Scan0).isKeyword("let") Then
                ' declare new symbol
                Dim name As String = tokenList(1).text
                Dim type As String = "any"
                Dim arguments As Expression() = Nothing

                If tokenList.Length > 2 Then
                    type = tokenList(3).text

                    If tokenList.Length > 4 Then
                        tokenList = tokenList.Skip(5).ToArray
                        tokenList = tokenList.Take(tokenList.Length - 1).ToArray

                        Dim values As Token()() = tokenList _
                            .SplitParameters _
                            .Select(Function(block)
                                        If block(Scan0).name = Tokens.Comma Then
                                            Return block.Skip(1).ToArray
                                        Else
                                            Return block
                                        End If
                                    End Function) _
                            .ToArray

                        arguments = values _
                            .Select(AddressOf ParseExpression) _
                            .ToArray
                    End If
                End If

                Return New SymbolDeclare With {
                    .symbolName = name,
                    .type = type,
                    .arguments = arguments
                }
            ElseIf tokenList(Scan0).isKeyword("where") Then
                Return New WhereFilter(ParseExpression(tokenList.Skip(1).ToArray))
            ElseIf tokenList(Scan0).isKeyword("in") Then
                Return ParseExpression(tokenList.Skip(1).ToArray)
            ElseIf tokenList(Scan0).isKeyword("select") Then
                Return tokenList.Skip(1).GetProjection
            ElseIf tokenList(Scan0).isKeyword("group") Then
                ' group by <key> [, <key> ...]
                Return New GroupBy(tokenList.Skip(2).GetProjectionFields)
            ElseIf tokenList(Scan0).isKeyword("into") Then
                ' into <name> = <aggregate expression> [, ...]
                Return New AggregateInto(tokenList.Skip(1).GetProjectionFields)
            ElseIf tokenList(Scan0).isKeyword("order") Then
                Dim sortKey = tokenList.Skip(2).ToArray
                Dim desc As Boolean

                If sortKey.Last.name = Tokens.keyword AndAlso sortKey.Last.text.ToLower Like sortOrders Then
                    desc = sortKey.Last.text.TextEquals("descending")
                    sortKey = sortKey.Take(sortKey.Length - 1).ToArray
                End If

                Return New OrderBy(ParseExpression(sortKey), desc)
            ElseIf tokenList(Scan0).isKeyword("take") Then
                Return New TakeItems(ParseExpression(tokenList.Skip(1).ToArray))
            ElseIf tokenList(Scan0).isKeyword("skip") Then
                Return New SkipItems(ParseExpression(tokenList.Skip(1).ToArray))
            Else
                Throw New SyntaxErrorException
            End If
        End Function

        <Extension>
        Private Function IsClosure(tokenList As Token()) As Boolean
            Return tokenList(Scan0).name = Tokens.Open AndAlso tokenList.Last.name = Tokens.Close
        End Function

        <Extension>
        Public Function ParseExpression(tokenList As Token()) As Expression
            If tokenList.Length = 1 Then
                Return tokenList(Scan0).ParseToken
            ElseIf tokenList(Scan0).name = Tokens.keyword Then
                Return tokenList.ParseKeywordExpression
            End If

            ' 赋值表达式需要优先解析：赋值表达式的右侧可能会是一个函数调用
            ' 例如``n = count(x)``，如果按照二元运算符的规则进行解析会失败
            Dim assignLeft As Token() = Nothing
            Dim assignRight As Token() = Nothing

            If TrySplitAssign(tokenList, assignLeft, assignRight) Then
                Dim target As Expression = ParseExpression(assignLeft)

                If TypeOf target Is SymbolReference Then
                    Return New BinaryExpression(target, ParseExpression(assignRight), "=")
                End If
            End If

            Dim blocks = tokenList.SplitByTopLevelStack.ToArray

            If blocks.Length = 1 Then
                tokenList = blocks(Scan0)

                If tokenList.First = (Tokens.Open, "[") OrElse tokenList.First = (Tokens.Open, "{") Then
                    Return tokenList.Skip(1).Take(tokenList.Length - 2).GetVector
                ElseIf tokenList.First = (Tokens.Open, "(") Then
                    tokenList = tokenList _
                        .Skip(1) _
                        .Take(tokenList.Length - 2) _
                        .ToArray
                End If
            ElseIf blocks.Length = 2 Then
                Dim name As Expression = ParseExpression(blocks(Scan0))

                If TypeOf name Is SymbolReference AndAlso blocks(1).IsClosure Then
                    Dim arguments As Token() = blocks(1) _
                        .Skip(1) _
                        .Take(blocks(1).Length - 2) _
                        .ToArray
                    Dim func As String = DirectCast(name, SymbolReference).symbolName

                    If String.Equals(func, "index", StringComparison.OrdinalIgnoreCase) Then
                        ' index函数带有命名参数，无法通过FuncEval来实现
                        ' 在这里创建专用的数据源表达式对象
                        Return CreateIndexSource(arguments)
                    Else
                        Return New FuncEval(name, arguments.GetParameters)
                    End If
                End If
            End If

            Return tokenList.ParseBinary
        End Function

        ''' <summary>
        ''' 尝试在最顶层按照``=``运算符将表达式分割为左值和右值
        ''' </summary>
        ''' <param name="tokenList"></param>
        ''' <param name="left"></param>
        ''' <param name="right"></param>
        ''' <returns></returns>
        Private Function TrySplitAssign(tokenList As Token(), ByRef left As Token(), ByRef right As Token()) As Boolean
            Dim stack As Integer = 0

            For i As Integer = 0 To tokenList.Length - 1
                Dim t As Token = tokenList(i)

                If t.name = Tokens.Open Then
                    stack += 1
                ElseIf t.name = Tokens.Close Then
                    stack -= 1
                ElseIf stack = 0 AndAlso t.name = Tokens.Operator AndAlso t.text = "=" Then
                    left = tokenList.Take(i).ToArray
                    right = tokenList.Skip(i + 1).ToArray

                    Return left.Length > 0 AndAlso right.Length > 0
                End If
            Next

            left = Nothing
            right = Nothing

            Return False
        End Function

        ''' <summary>
        ''' 解析``index(&lt;uri>, terms = [...], range = [...], fulltext = [...])``数据源表达式
        ''' </summary>
        ''' <param name="arguments"></param>
        ''' <returns></returns>
        <Extension>
        Private Function CreateIndexSource(arguments As Token()) As Expression
            Dim blocks As Token()() = arguments _
                .SplitParameters _
                .Where(Function(b) Not b.IsNullOrEmpty) _
                .Select(Function(b) If(b(Scan0).name = Tokens.Comma, b.Skip(1).ToArray, b)) _
                .Where(Function(b) Not b.IsNullOrEmpty) _
                .ToArray
            Dim path As Expression = Nothing
            Dim specs As New List(Of NamedValue(Of Expression))

            For i As Integer = 0 To blocks.Length - 1
                If i = 0 Then
                    path = ParseExpression(blocks(i))
                Else
                    Dim assign As Expression = ParseExpression(blocks(i))

                    If TypeOf assign Is BinaryExpression AndAlso DirectCast(assign, BinaryExpression).LikeValueAssign Then
                        With DirectCast(assign, BinaryExpression)
                            specs.Add(New NamedValue(Of Expression)(
                                DirectCast(.left, SymbolReference).symbolName,
                                .right
                            ))
                        End With
                    Else
                        Throw New SyntaxErrorException($"invalid index field specification: '{blocks(i).Select(Function(t) t.text).JoinBy(" ")}'!")
                    End If
                End If
            Next

            If path Is Nothing Then
                Throw New SyntaxErrorException("missing data source file for the index function!")
            End If

            Return New IndexSource(path, specs)
        End Function

        <Extension>
        Private Iterator Function GetParameters(tokenList As IEnumerable(Of Token)) As IEnumerable(Of Expression)
            Dim blocks As Token()() = tokenList _
                .SplitParameters _
                .Select(Function(b)
                            If b(Scan0).name = Tokens.Comma Then
                                Return b.Skip(1).ToArray
                            Else
                                Return b
                            End If
                        End Function) _
                .ToArray

            For Each block As Token() In blocks
                Yield ParseExpression(block)
            Next
        End Function

        <Extension>
        Private Function GetVector(tokenList As IEnumerable(Of Token)) As Expression
            Dim elements As Expression() = tokenList.GetParameters.ToArray
            Dim vec As New ArrayExpression(elements)

            Return vec
        End Function
    End Module
End Namespace
