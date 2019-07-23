Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON
Imports sciBASIC.ComputingServices.TaskHost

Public Module RemoteCall

    ''' <summary>
    ''' Invoke the function on the remote server.(远程服务器上面通过这个方法执行函数调用)
    ''' </summary>
    ''' <param name="params"></param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function Invoke(params As InvokeInfo) As Rtvl
        Dim rtvl As Rtvl

        Try
            Dim resultType As Type = Nothing
            Dim value As Object = doCall(params, resultType)

            rtvl = New Rtvl(value, resultType)
        Catch ex As Exception
            rtvl = New Rtvl(ex)
        End Try

        Return rtvl
    End Function

    ''' <summary>
    ''' A common function of invoke the method on the remote machine
    ''' </summary>
    ''' <param name="params">远程主机上面的函数指针</param>
    ''' <returns></returns>
    ''' 
    <Extension>
    Public Function doCall(params As InvokeInfo, Optional ByRef rtvlType As Type = Nothing) As Object
        Dim func As MethodInfo = params.GetMethod
        Dim paramsValue As Object() = params.parameters _
            .Select(Function(arg) arg.GetValue) _
            .ToArray
        Dim value As Object

        value = func.Invoke(Nothing, paramsValue)
        rtvlType = func.ReturnType

        Return value
    End Function
End Module
