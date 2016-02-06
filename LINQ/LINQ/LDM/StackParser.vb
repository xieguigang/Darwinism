Imports Microsoft.VisualBasic.LINQ.TokenIcer

Namespace LDM

    Public Module StackParser

        Public Function Parser(source As Queue(Of Token)) As Func()

        End Function

        Public Function Parsing(source As Queue(Of Token)) As Func()
            Dim list As New List(Of Func)

            Do While Not source.IsNullOrEmpty
                Dim x As Token = source.Dequeue
                Dim peek As Token = source.Peek
                If peek Is Nothing Then
                    Call list.Add(New Func With {.Caller = x})
                    Exit Do
                End If
                If peek.TokenName = TokenParser.Tokens.LPair Then  ' 向下一层堆栈
                    Call source.Dequeue()

                ElseIf peek.TokenName = TokenParser.Tokens.RPair Then  ' 向上一层退栈
                Else
                    Call list.Add(New Func With {.Caller = x})
                End If
            Loop

            Return list.ToArray
        End Function
    End Module

    Public Class Func

        Public Property Caller As TokenIcer.Token
        Public Property Args As Func()
    End Class
End Namespace


