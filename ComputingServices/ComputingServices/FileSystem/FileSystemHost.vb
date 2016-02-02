Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Net

Namespace FileSystem

    Public Class FileSystemHost : Inherits IHostBase

        Sub New(port As Integer)
            __host = New TcpSynchronizationServicesSocket(port)
        End Sub

        Public Overrides ReadOnly Property Portal As IPEndPoint
            Get
                Return New IPEndPoint(AsynInvoke.LocalIPAddress, __host.LocalPort)
            End Get
        End Property
    End Class
End Namespace