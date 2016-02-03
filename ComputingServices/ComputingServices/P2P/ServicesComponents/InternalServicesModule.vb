Imports Microsoft.VisualBasic.ComputingServices.ComponentModel

Namespace P2P.ServicesComponents

    Public Delegate Function RunShell(script As String) As Object
    ''' <summary>
    ''' 向控制台导入本服务的管理用的API
    ''' </summary>
    ''' <param name="console"></param>
    ''' <returns></returns>
    Public Delegate Function ShellImportsAPI(console As Object, type As Type) As Boolean

    Public MustInherit Class InternalServicesModule : Inherits IHostBase
        Implements System.IDisposable

        Protected _ShoalShellExec As RunShell
        Protected ProtocolHandler As Net.Protocol.Reflection.ProtocolHandler
        Protected _ShellImportsAPI As ShellImportsAPI

        Protected MustOverride Sub ImportsAPI()
        Protected MustOverride Function GetServicesPort() As Integer

        Protected Sub _runningServicesProtocol(Protocol As PbsProtocol)
            ProtocolHandler = New Net.Protocol.Reflection.ProtocolHandler(Protocol)
            __host = New Net.TcpSynchronizationServicesSocket(GetServicesPort)
            __host.Responsehandler = AddressOf ProtocolHandler.HandleRequest
            __host.Run()
        End Sub

        Protected Sub WaitForSocketStart()
            Do While __host Is Nothing
                Call Threading.Thread.Sleep(10)
            Loop
        End Sub

        Protected Sub _runningShoalShell()
            Call ImportsAPI()

            Do While Not __host Is Nothing
                Call Console.Write(">>> ")
                Dim cmdl As String = Console.ReadLine
                Call _ShoalShellExec(cmdl)
            Loop
        End Sub

        Public MustOverride Sub RunServices(argvs As CommandLine.CommandLine)

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call __host.Free
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