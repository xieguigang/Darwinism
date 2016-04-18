Imports Microsoft.VisualBasic.ComputingServices.FileSystem
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Abstract

Namespace ComponentModel

    Public Interface IRemoteSupport

        ReadOnly Property FileSystem As FileSystemHost
    End Interface
End Namespace