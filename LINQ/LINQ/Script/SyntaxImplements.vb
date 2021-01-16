Imports System.Runtime.CompilerServices
Imports LINQ.Interpreter.Expressions
Imports LINQ.Interpreter.Query
Imports LINQ.Language
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Script

    Public Module SyntaxImplements

        <Extension>
        Public Function PopulateQueryExpression(tokens As IEnumerable(Of Token)) As Expression
            Dim blocks = tokens.SplitByTopLevelStack.ToArray

            If blocks(Scan0).First.isKeywordFrom Then
                Return blocks(Scan0).CreateProjectionQuery(blocks.Skip(1).ToArray)
            ElseIf blocks(Scan0).First.isKeywordAggregate Then
                Return blocks(Scan0).CreateAggregateQuery(blocks.Skip(1).ToArray)
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
            Dim proj As New ProjectionExpression(symbolExpr, seq, exec)

            Return proj
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
            Dim seq As Expression
        End Function

        <Extension>
        Private Function ParseToken(t As Token) As Expression
            If t.name = Tokens.Symbol Then
                Return New SymbolReference(t.text)
            ElseIf t.name = Tokens.Boolean OrElse
                t.name = Tokens.Integer OrElse
                t.name = Tokens.Number OrElse
                t.name = Tokens.Literal Then

                Return New Literals(t)
            Else
                Throw New NotImplementedException
            End If
        End Function

        <Extension>
        Private Function GetProjection(tokenList As IEnumerable(Of Token)) As Expression
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
                Else
                    fields.Add(New NamedValue(Of Expression)(item.ToString, item))
                End If
            Next

            Return New OutputProjection(fields)
        End Function

        <Extension>
        Private Function ParseKeywordExpression(tokenList As Token()) As Expression
            If tokenList(Scan0).isKeywordFrom OrElse tokenList(Scan0).isKeywordAggregate OrElse tokenList(Scan0).isKeyword("let") Then
                ' declare new symbol
                Dim name As String = tokenList(1).text
                Dim type As String = "any"

                If tokenList.Length > 2 Then
                    type = tokenList(3).text
                End If

                Return New SymbolDeclare With {.symbolName = name, .type = type}
            ElseIf tokenList(Scan0).isKeyword("where") Then
                Return New WhereFilter(ParseExpression(tokenList.Skip(1).ToArray))
            ElseIf tokenList(Scan0).isKeyword("in") Then
                Return ParseExpression(tokenList.Skip(1).ToArray)
            ElseIf tokenList(Scan0).isKeyword("select") Then
                Return tokenList.Skip(1).GetProjection
            ElseIf tokenList(Scan0).isKeyword("order") Then
                Return New OrderBy(ParseExpression(tokenList.Skip(2).ToArray))
            Else
                Throw New SyntaxErrorException
            End If
        End Function

        <Extension>
        Public Function ParseExpression(tokenList As Token()) As Expression
            If tokenList.Length = 1 Then
                Return tokenList(Scan0).ParseToken
            ElseIf tokenList(Scan0).name = Tokens.keyword Then
                Return tokenList.ParseKeywordExpression
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
            End If

            Return tokenList.ParseBinary
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