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

        Protected __host As TSocket
    End Class
End Namespace