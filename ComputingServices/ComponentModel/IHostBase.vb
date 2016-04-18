Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Abstract

Namespace ComponentModel

    Public MustInherit Class IHostBase : Inherits IMasterBase(Of TcpSynchronizationServicesSocket)

        Sub New(portal As Integer)
            __host = New TcpSynchronizationServicesSocket(portal)
        End Sub

        Sub New()
        End Sub
    End Class

    Public MustInherit Class IMasterBase(Of TSocket As IServicesSocket)

        Public MustOverride ReadOnly Property Portal As IPEndPoint

        Protected Friend __host As TSocket

        Public Shared Narrowing Operator CType(master As IMasterBase(Of TSocket)) As IPEndPoint
            Return master.Portal
        End Operator

        Public Shared Narrowing Operator CType(master As IMasterBase(Of TSocket)) As System.Net.IPEndPoint
            Return master.Portal.GetIPEndPoint
        End Operator

    End Class
End Namespace