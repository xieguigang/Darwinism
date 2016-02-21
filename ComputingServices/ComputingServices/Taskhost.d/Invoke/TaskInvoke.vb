Imports System.Reflection
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.ComputingServices.FileSystem
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.LINQ
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    <Protocol(GetType(TaskProtocols))>
    Public Class TaskInvoke : Inherits IHostBase
        Implements IRemoteSupport

        ''' <summary>
        ''' Running on local LAN
        ''' </summary>
        Dim _local As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="local">Program is running in a local server cluster which in the range of the same LAN network?</param>
        ''' <param name="port">You can suing function <see cref="GetFirstAvailablePort"/> to initialize this server object.</param>
        Sub New(Optional local As Boolean = True, Optional port As Integer = 1234)
            Call MyBase.New(port)
            _local = local
            __host.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
            FileSystem = New FileSystemHost(GetFirstAvailablePort)
        End Sub

        Public Function Run() As Integer
            Return __host.Run()
        End Function

        Public Overrides ReadOnly Property Portal As IPEndPoint
            Get
                Return Me.GetPortal(_local)
            End Get
        End Property

        Public ReadOnly Property FileSystem As FileSystemHost Implements IRemoteSupport.FileSystem

        ''' <summary>
        ''' Invoke the function on the remote server.(远程服务器上面通过这个方法执行函数调用)
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
        ''' A common function of invoke the method on the remote machine
        ''' </summary>
        ''' <param name="params">远程主机上面的函数指针</param>
        ''' <param name="value">value's <see cref="system.type"/></param>
        ''' <returns></returns>
        Private Shared Function __invoke(params As InvokeInfo, ByRef value As Type) As Object
            Dim func As MethodInfo = params.GetMethod
            Dim paramsValue As Object() = params.Parameters.ToArray(Function(arg) arg.GetValue)
            Dim x As Object = func.Invoke(Nothing, paramsValue)
            value = func.ReturnType
            Return x
        End Function

        Public Shared Function TryInvoke(info As InvokeInfo) As Object
            Return __invoke(info, Nothing)
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