Imports System.Reflection
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Net.SSL
Imports Microsoft.VisualBasic.Net.TCPExtensions

Namespace Asymmetric

    ''' <summary>
    ''' 服务实例，只是和管理节点之间的通信的通道
    ''' </summary>
    Public MustInherit Class Instance : Inherits IMasterBase(Of SSLSynchronizationServicesSocket)

        Protected ReadOnly _invokeCA As Net.SSL.Certificate

        Sub New(CLI As CommandLine.CommandLine)
            Call CLI.__DEBUG_ECHO

            _invokeCA = CLI(Protocols.OAuth).GetCA
            __host = New Net.SSL.SSLSynchronizationServicesSocket(
                Net.TCPExtensions.GetFirstAvailablePort,
                _invokeCA,
                container:=Me,
                exHandler:=AddressOf __handleException)
            Call (Sub() __returnPortal(CLI)).BeginInvoke(Nothing, Nothing)
        End Sub

        Protected MustOverride Function __getExternalSvrPortal() As Integer

        Public Sub Run()
            Call __host.Install(_invokeCA, [overrides]:=True)
            Call __host.Run()
        End Sub

        Private Sub __returnPortal(cli As CommandLine.CommandLine)
            Call __host.WaitForStart()
            Call Parallel.ReturnPortal(cli, __host.LocalPort)
        End Sub

        Protected Overridable Sub __handleException(ex As Exception)
            Call App.LogException(ex, MethodBase.GetCurrentMethod.GetFullName)
        End Sub
    End Class
End Namespace