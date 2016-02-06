Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace LDM

    Public Module StackParser

        ''' <summary>
        ''' 返回顶层的根节点
        ''' </summary>
        ''' <param name="source"></param>
        ''' <returns></returns>
        <Extension> Public Function Parsing(source As Queue(Of Token)) As Func
            Dim pretend As Token = New Token(TokenParser.Tokens.Pretend, "Pretend")
            Dim root As Func = New Func With {
                .Caller = New List(Of Token) From {pretend},
                .Args = __parsing(source)
            }
            Return root
        End Function

        ''' <summary>
        ''' 主要是解析当前的栈层之中的使用，逗号分隔的参数列表
        ''' </summary>
        ''' <param name="source"></param>
        ''' <returns></returns>
        Private Function __parsing(source As Queue(Of Token)) As Func()
            Dim list As New List(Of Func)
            Dim current As Func = New Func With {
                .Caller = New List(Of Token)
            }

            Do While Not source.IsNullOrEmpty
                Dim x As Token = source.Dequeue

                If Not x.TokenName = TokenParser.Tokens.ParamDeli Then
                    Call current.Caller.Add(x)
                End If

                If source.Count = 0 Then
                    Call list.Add(current)
                    Exit Do
                End If

                Dim peek As Token = source.Peek

                If peek.TokenName = TokenParser.Tokens.LPair Then  ' 向下一层堆栈
                    Call source.Dequeue()
                    current.Args = __parsing(source)
                ElseIf peek.TokenName = TokenParser.Tokens.RPair Then  ' 向上一层退栈
                    Call source.Dequeue()
                    Call list.Add(current)
                    Exit Do
                ElseIf x.TokenName = TokenParser.Tokens.ParamDeli Then
                    Call list.Add(current)
                    current = New Func With {
                        .Caller = New List(Of Token)
                    }
                End If
            Loop

            Return list.ToArray
        End Function
    End Module

    Public Class Func

        Public Property Caller As List(Of Token)
        Public Property Args As Func()

        Public Overrides Function ToString() As String
            If Args.IsNullOrEmpty Then
                Return String.Join(" ", Caller.ToArray(Function(x) x.TokenValue))
            Else
                Dim caller As String = String.Join(" ", Me.Caller.ToArray(Function(x) x.TokenValue))
                Dim params As String() = Me.Args.ToArray(Function(x) x.ToString)
                Dim args As String = String.Join(", ", params)
                Return $"{caller}({args})"
            End If
        End Function
    End Class
End Namespace


