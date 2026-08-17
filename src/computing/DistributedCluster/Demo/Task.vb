Imports System.Threading
Imports Microsoft.VisualBasic.Math

Public Class Task

    ''' <summary>
    ''' 计算相关性网络
    ''' </summary>
    ''' <param name="input"></param>
    ''' <param name="blockId"></param>
    ''' <param name="jobRoot"></param>
    ''' <returns></returns>
    Public Function CorrelationNetwork(input As Byte(), blockId As String, jobRoot As String) As Integer
        Dim delay As Double = RandomExtensions.NextDouble() * 120
        Call Thread.Sleep(delay * 1000)
        Return 0
    End Function
End Class
