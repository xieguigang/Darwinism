Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace TaskHost

    ''' <summary>
    ''' The returns value.(远端调用的函数返回)
    ''' </summary>
    Public Class Rtvl

        Public Property errCode As Integer
        ''' <summary>
        ''' Exception Message
        ''' </summary>
        ''' <returns></returns>
        Public Property ex As String
        ''' <summary>
        ''' Json value
        ''' </summary>
        ''' <returns></returns>
        Public Property value As String

        Sub New()
        End Sub

        Sub New(ex As Exception)
            _errCode = HTTP_RFC.RFC_INTERNAL_SERVER_ERROR
            _ex = ex.ToString
        End Sub

        Sub New(value As Object, type As Type)
            _errCode = HTTP_RFC.RFC_OK
            _value = Serialization.GetJson(value, type)
        End Sub

        ''' <summary>
        ''' If the remote execute raising a exception, then a exception will be throw from this function.
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Public Function GetValue(type As Type) As Object
            If errCode <> HTTP_RFC.RFC_OK Then
                Throw New Exception(ex)
            End If
            Return Serialization.LoadObject(value, type)
        End Function

        Public Function GetValue(func As [Delegate]) As Object
            Dim type As Type = func.Method.ReturnType
            Return GetValue(type)
        End Function

        Public Shared Function CreateObject(Of T)(x As T) As Rtvl
            Return New Rtvl(x, GetType(T))
        End Function
    End Class
End Namespace