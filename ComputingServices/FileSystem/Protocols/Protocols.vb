Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace FileSystem.Protocols

    ''' <summary>
    ''' 
    ''' </summary>
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

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="pos">-100表示set</param>
        ''' <param name="handle"></param>
        ''' <returns></returns>
        Public Function GetSetReadPosition(pos As Long, handle As FileHandle) As RequestStream
            Dim args As New FileStreamPosition(handle) With {.Position = pos}
            Return RequestStream.CreateProtocol(ProtocolEntry, FileSystemAPI.FilePosition, args)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="handle"></param>
        ''' <returns></returns>
        Public Function GetSetLength(length As Long, handle As FileHandle) As RequestStream
            Dim args As New FileStreamPosition(handle) With {.Position = length}
            Return RequestStream.CreateProtocol(ProtocolEntry, FileSystemAPI.FileStreamLength, args)
        End Function
    End Module
End Namespace