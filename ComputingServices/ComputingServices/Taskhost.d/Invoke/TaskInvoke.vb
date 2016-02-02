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
            Dim func As MethodInfo = params.GetMethod
            Dim paramsValue As Object() = InvokeInfo.GetParameters(func, params.Parameters)
            Dim rtvl As Rtvl
            Try
                Dim value As Object = func.Invoke(Nothing, paramsValue)
                rtvl = New Rtvl(value, func.ReturnType)
            Catch ex As Exception
                ex = New Exception(params.GetJson, ex)
                rtvl = New Rtvl(ex)
            End Try

            Return rtvl
        End Function

        <Protocol(TaskProtocols.Invoke)>
        Private Function Invoke(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream

        End Function
    End Class
End Namespace