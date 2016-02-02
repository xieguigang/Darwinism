Imports System.Reflection
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    Public Class TaskInvoke : Inherits IHostBase

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
    End Class
End Namespace