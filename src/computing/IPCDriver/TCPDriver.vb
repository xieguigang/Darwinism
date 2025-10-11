Imports System.Runtime.CompilerServices
Imports Darwinism.HPC.Parallel
Imports Darwinism.IPC.Networking.Protocols.Reflection
Imports Darwinism.IPC.Networking.Tcp
Imports Microsoft.VisualBasic.CommandLine.InteropService.Pipeline
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.Parallel

Public Class TCPDriver : Implements ITaskDriver, IDisposable

    ReadOnly app As ProtocolHandler
    ReadOnly socket As TcpServicesSocket

    Private disposedValue As Boolean

    Public ReadOnly Property TcpPort As Integer
        Get
            Return socket.LocalPort
        End Get
    End Property

    Sub New(app As ProtocolHandler,
            Optional debugPort As Integer? = Nothing,
            Optional masterPid As String = Nothing)

        Dim port As Integer = If(debugPort Is Nothing, IPCSocket.GetFirstAvailablePort(), debugPort)
        Dim callback As DataRequestHandler = AddressOf app.HandleRequest

        Me.app = app
        Me.socket = New TcpServicesSocket(port, debug:=Not debugPort Is Nothing) With {
            .KeepsAlive = False,
            .ResponseHandler = callback
        }

        Call RunSlavePipeline.SendMessage($"socket={TcpPort}")
        Call BackgroundTaskUtils.BindToMaster(masterPid, Me)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Run() As Integer Implements ITaskDriver.Run
        Return socket.Run
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                Call socket.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
