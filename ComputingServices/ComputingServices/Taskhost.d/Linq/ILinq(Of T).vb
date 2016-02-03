Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol

Namespace TaskHost

    ''' <summary>
    ''' Remote LINQ source reader
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class ILinq(Of T) : Implements IEnumerable(Of T)
        Implements IDisposable

        ''' <summary>
        ''' Element type in the source collection.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Type As Type = GetType(T)
        ''' <summary>
        ''' Remote entry point
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Portal As IPEndPoint

        ReadOnly invoke As AsynInvoke
        ReadOnly req As New RequestStream(Protocols.ProtocolEntry, TaskProtocols.MoveNext)

        ''' <summary>
        ''' Creates a linq source reader from the remote entry point
        ''' </summary>
        ''' <param name="portal"></param>
        Sub New(portal As IPEndPoint)
            Me.Portal = portal
            Me.invoke = New AsynInvoke(portal)
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Type.FullName}@{Portal.ToString}"
        End Function

#Region "Implements IEnumerable(Of T)"

        Public Iterator Function AsQuerable() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
            Call invoke.SendMessage(Protocols.LinqReset)  ' resets the remote linq source read position

            Do While True
                Dim rep As RequestStream = invoke.SendMessage(req)
                Dim json As String = rep.GetUTF8String
                Dim value As Object = Serialization.LoadObject(json, Type)
                Dim x As T = DirectCast(value, T)

                If rep.ProtocolCategory = TaskProtocols.ReadsDone Then
                    Exit Do
                Else
                    Yield x
                End If
            Loop
        End Function

        Private Iterator Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Yield AsQuerable()
        End Function

#End Region

        ''' <summary>
        ''' Automatically free the remote resource.(释放远程主机上面的资源)
        ''' </summary>
        Private Sub __free()
            Dim uid As String = Portal.ToString
            Dim req As New RequestStream(ProtocolEntry, TaskProtocols.Free, uid)
            Call invoke.SendMessage(req)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call __free()
                    Call invoke.Free
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace