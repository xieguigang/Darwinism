Imports System.IO
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace FileSystem.Protocols

    Public Module API

        Public ReadOnly Property ProtocolEntry As Long =
            New Protocol(GetType(FileSystemAPI)).EntryPoint

        Public Function OpenHandle(file As String, mode As FileMode, access As FileAccess) As RequestStream
            Dim params As New FileOpen With {
                .FileName = file,
                .Mode = mode,
                .Access = access
            }
            Return RequestStream.CreateProtocol(ProtocolEntry, FileSystemAPI.OpenHandle, params)
        End Function
    End Module
End Namespace