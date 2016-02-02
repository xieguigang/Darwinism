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

        Public Shared Function CreateObject(Of T)(x As T) As Returns
            Return New Returns(x, GetType(T))
        End Function
    End Class

    Public Class ILinq(Of T) : Implements IEnumerable(Of T)

        Public ReadOnly Property Type As Type = GetType(T)
        Public ReadOnly Property Portal As IPEndPoint

        ReadOnly invoke As AsynInvoke
        ReadOnly req As New RequestStream(Protocols.ProtocolEntry, TaskProtocols.MoveNext)

        Sub New(portal As IPEndPoint)
            Me.Portal = portal
            Me.invoke = New AsynInvoke(portal)
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Type.FullName}@{Portal.ToString}"
        End Function

        Public Iterator Function AsQuerable() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Call invoke.SendMessage(Protocols.LinqReset)  ' resets the remote linq source read position

            Do While True
                Dim rep As RequestStream = invoke.SendMessage(req)
                Dim json As String = rep.GetUTF8String
                Dim value As Object = Serialization.LoadObject(json, Type)
                Dim x As T = DirectCast(value, T)

                Yield x

                If rep.ProtocolCategory = TaskProtocols.ReadsDone Then
                    Exit Do
                End If
            Loop
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield AsQuerable()
        End Function
    End Class
End Namespace