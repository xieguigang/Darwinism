﻿Imports System.Reflection
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    <Protocol(GetType(TaskProtocols))>
    Public Class TaskInvoke : Inherits IHostBase

        ''' <summary>
        ''' Running on local lan
        ''' </summary>
        Dim _local As Boolean

        Sub New(Optional local As Boolean = True)
            Call MyBase.New(GetFirstAvailablePort)
            _local = local
            __host.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
            Call Parallel.Run(AddressOf __host.Run)
        End Sub

        Public Overrides ReadOnly Property Portal As IPEndPoint
            Get
                Return Me.GetPortal(_local)
            End Get
        End Property

        ''' <summary>
        ''' 远程服务器上面通过这个方法执行函数调用
        ''' </summary>
        ''' <param name="params"></param>
        ''' <returns></returns>
        Public Shared Function Invoke(params As InvokeInfo) As Rtvl
            Dim rtvl As Rtvl

            Try
                Dim rtvlType As Type = Nothing
                Dim value As Object = __invoke(params, rtvlType)
                rtvl = New Rtvl(value, rtvlType)
            Catch ex As Exception
                ex = New Exception(params.GetJson, ex)
                rtvl = New Rtvl(ex)
            End Try

            Return rtvl
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="params">远程主机上面的函数指针</param>
        ''' <param name="value">value's <see cref="system.type"/></param>
        ''' <returns></returns>
        Private Shared Function __invoke(params As InvokeInfo, ByRef value As Type) As Object
            Dim func As MethodInfo = params.GetMethod
            Dim paramsValue As Object() = InvokeInfo.GetParameters(func, params.Parameters)
            Dim x As Object = func.Invoke(Nothing, paramsValue)
            value = func.ReturnType
            Return x
        End Function

        <Protocol(TaskProtocols.Invoke)>
        Private Function Invoke(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = Serialization.LoadObject(Of InvokeInfo)(args.GetUTF8String)
            Dim value As Rtvl = Invoke(params)
            Return New RequestStream(value.GetJson)
        End Function

        ''' <summary>
        ''' linq池
        ''' </summary>
        ReadOnly __linq As New Dictionary(Of String, LinqProvider)

        <Protocol(TaskProtocols.Free)>
        Private Function Free(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim uid As String = args.GetUTF8String
            If __linq.ContainsKey(uid) Then
                Dim x As LinqProvider = __linq(uid)
                Call x.Free  ' 释放Linq数据源的指针
                Call __linq.Remove(uid)  ' 从哈希表之中移除数据源释放服务器资源
            End If
            Return NetResponse.RFC_OK  ' HTTP/200
        End Function

        ''' <summary>
        ''' 执行远程Linq代码
        ''' </summary>
        ''' <param name="CA">SSL证书编号</param>
        ''' <param name="args"></param>
        ''' <param name="remote"></param>
        ''' <returns></returns>
        <Protocol(TaskProtocols.InvokeLinq)>
        Private Function InvokeLinq(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = Serialization.LoadObject(Of InvokeInfo)(args.GetUTF8String) ' 得到远程函数指针信息
            Dim type As Type = Nothing
            Dim value As Object = __invoke(params, type)
            Dim source As IEnumerable = DirectCast(value, IEnumerable)
            Dim linq As New LinqProvider(source, type.GetArrayElement(True))  ' 创建 Linq 数据源
            Dim portal As IPEndPoint = linq.Portal
            Call __linq.Add(portal.ToString, linq)  ' 数据源添加入哈希表之中
            Dim svr As String = portal.GetJson  ' 返回数据源信息
            Return New RequestStream(svr)
        End Function
    End Class
End Namespace