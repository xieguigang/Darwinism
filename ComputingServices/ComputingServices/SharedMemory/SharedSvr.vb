Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection

Namespace SharedMemory

    ''' <summary>
    ''' Memory shared services.
    ''' </summary>
    ''' 
    <Protocol(GetType(Protocols.MemoryProtocols))>
    Public Class SharedSvr : Implements IDisposable
        Implements IObjectModel_Driver

        ReadOnly __localSvr As TcpSynchronizationServicesSocket

        Sub New(local As Integer)
            __localSvr = New TcpSynchronizationServicesSocket(local)
            __localSvr.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
        End Sub

        <Protocol(MemoryProtocols.Read)>
        Private Function Read(CA As Long, args As RequestStream, remote As IPEndPoint) As RequestStream

        End Function

        <Protocol(MemoryProtocols.Write)>
        Private Function Write(CA As Long, args As RequestStream, remote As IPEndPoint) As RequestStream

        End Function

        Public Overrides Function ToString() As String
            Return __localSvr.ToString
        End Function

        Public Function Run() As Integer Implements IObjectModel_Driver.Run
            Return __localSvr.Run
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    __localSvr.Dispose()
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