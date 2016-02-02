Imports System.Reflection
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    ''' <summary>
    ''' 由于是远程调用，所以运行的环境可能会很不一样，所以在设计程序的时候请尽量避免或者不要使用模块变量，以免出现难以调查的BUG
    ''' </summary>
    Public Class TaskHost

        Dim _remote As IPEndPoint

        Sub New(remote As IPEndPoint)
            _remote = remote
        End Sub

        ''' <summary>
        ''' 相当于Sub，调用远程的命令行程序，只会返回0或者错误代码
        ''' </summary>
        ''' <param name="exe"></param>
        ''' <param name="args"></param>
        ''' <returns></returns>
        Public Function Shell(exe As String, args As String) As Integer

        End Function

        ''' <summary>
        ''' 本地服务器通过这个方法调用远程主机上面的函数
        ''' </summary>
        ''' <param name="target"></param>
        ''' <param name="args"></param>
        ''' <returns></returns>
        Public Function Invoke(target As [Delegate], ParamArray args As Object()) As Object
            Dim params As InvokeInfo = InvokeInfo.CreateObject(target, args)
            Dim value As String = Serialization.GetJson(params)
            Dim req As RequestStream = New RequestStream(ProtocolEntry, TaskProtocols.Invoke, value)
            Dim rep As RequestStream = New AsynInvoke(_remote).SendMessage(req)
            Dim rtvl As Rtvl = Serialization.LoadObject(Of Rtvl)(rep.GetUTF8String)
            Dim obj As Object = rtvl.GetValue(target)
            Return obj
        End Function

        Public Function Invoke(Of T)(target As [Delegate], ParamArray args As Object()) As T
            Dim value As Object = Invoke(target, args)
            If value Is Nothing Then
                Return Nothing
            Else
                Return DirectCast(value, T)
            End If
        End Function

        ''' <summary>
        ''' 执行远程机器上面的代码，然后返回数据查询接口
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="target">远程机器上面的函数指针</param>
        ''' <param name="args"></param>
        ''' <returns></returns>
        Public Function AsLinq(Of T)(target As [Delegate], ParamArray args As Object()) As ILinq(Of T)
            Dim params As InvokeInfo = InvokeInfo.CreateObject(target, args)
            Dim jparam As String = params.GetJson
            Dim req As New RequestStream(ProtocolEntry, TaskProtocols.InvokeLinq, jparam)
            Dim rep As RequestStream = New AsynInvoke(_remote).SendMessage(req)
            Dim svr As IPEndPoint = rep.GetUTF8String.LoadObject(Of IPEndPoint)
            Return New ILinq(Of T)(svr)
        End Function
    End Class
End Namespace