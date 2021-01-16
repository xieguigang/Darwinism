Imports System.Runtime.CompilerServices
Imports LINQ.Language
Imports Microsoft.VisualBasic.Language

Module StackParser

    <Extension>
    Private Function isKeywordFrom(t As Token) As Boolean
        Return isKeyword(t, "from")
    End Function

    Private Function isKeyword(t As Token, text As String) As Boolean
        Return t.name = Tokens.keyword AndAlso t.text.TextEquals(text)
    End Function

    <Extension>
    Private Function isKeywordSelect(t As Token) As Boolean
        Return isKeyword(t, "select")
    End Function

    ''' <summary>
    ''' 根据最顶端的关键词以及括号进行栈片段的分割
    ''' </summary>
    ''' <param name="tokenList"></param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function SplitByTopLevelStack(tokenList As IEnumerable(Of Token)) As IEnumerable(Of Token())
        Dim block As New List(Of Token)
        Dim stack As New Stack(Of String)

        For Each item As Token In tokenList
            If item.name = Tokens.keyword Then
                If stack.Count > 0 Then
                    block.Add(item)
                Else
                    If block > 0 Then
                        Yield block.PopAll
                    End If

                    block.Add(item)
                End If
            ElseIf item.name = Tokens.Open Then
                stack.Push(item.text)

                If stack.Count > 1 Then
                    block.Add(item)
                Else
                    If block > 0 Then
                        Yield block.PopAll
                    End If

                    block.Add(item)
                End If
            ElseIf item.name = Tokens.Close Then
                Dim parent As String = stack.Pop

                If parent <> item.text Then
                    Throw New SyntaxErrorException
                End If

                If stack.Count > 1 Then
                    block.Add(item)
                Else
                    block.Add(item)

                    If block > 0 Then
                        Yield block.PopAll
                    End If
                End If
            Else
                block.Add(item)
            End If
        Next

        If stack.Count > 0 Then
            Throw New SyntaxErrorException
        ElseIf block > 0 Then
            Yield block.PopAll
        End If
    End Function
End Module
