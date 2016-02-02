Imports System.Reflection
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
        ''' <param name="params"></param>
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
        ReadOnly __linq As New List(Of LinqProvider)

        ''' <summary>
        ''' 执行远程Linq代码
        ''' </summary>
        ''' <param name="CA"></param>
        ''' <param name="args"></param>
        ''' <param name="remote"></param>
        ''' <returns></returns>
        <Protocol(TaskProtocols.InvokeLinq)>
        Private Function InvokeLinq(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim params As InvokeInfo = Serialization.LoadObject(Of InvokeInfo)(args.GetUTF8String)
            Dim type As Type = Nothing
            Dim value As Object = __invoke(params, type)
            Dim source As IEnumerable = DirectCast(value, IEnumerable)
            Dim linq As New LinqProvider(source, type.GetArrayElement(True))
            Call __linq.Add(linq)
            Dim svr As String = linq.Portal.GetJson
            Return New RequestStream(svr)
        End Function
    End Class
End Namespace