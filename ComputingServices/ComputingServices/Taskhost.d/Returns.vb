Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace TaskHost

    ''' <summary>
    ''' 远端调用的函数返回
    ''' </summary>
    Public Class Returns

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
            errCode = HTTP_RFC.RFC_OK
            value = Serialization.GetJson(value, type)
        End Sub

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
    End Class

    Public Class Returns(Of T) : Inherits Returns

        Public ReadOnly Property Type As Type = GetType(T)

        Public Overloads Function GetValue() As T
            Dim value As Object = GetValue(GetType(T))
            Return DirectCast(value, T)
        End Function

        Public Iterator Function AsQuerable() As IEnumerable(Of T)
            Dim portal As IPEndPoint = Serialization.LoadObject(Of IPEndPoint)(value)
            Dim invoke As New AsynInvoke(portal)
            Dim req As New RequestStream(Protocols.ProtocolEntry, TaskProtocols.MoveNext)

            Do While True
                Dim rep As RequestStream = invoke.SendMessage(req)
                If rep.ProtocolCategory = TaskProtocols.ReadsDone Then
                    Exit Do
                End If
                Dim json As String = rep.GetUTF8String
                Dim value As Object = Serialization.LoadObject(json, Type)
                Dim x As T = DirectCast(value, T)

                Yield x
            Loop
        End Function
    End Class
End Namespace