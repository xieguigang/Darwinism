Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace TaskHost

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