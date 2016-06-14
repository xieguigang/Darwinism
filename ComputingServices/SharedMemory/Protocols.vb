Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Serialization

Namespace SharedMemory

    Module Protocols

        Public Enum MemoryProtocols
            Read
            Write
            [TypeOf]
        End Enum

        Public ReadOnly Property ProtocolEntry As Long = New Protocol(GetType(MemoryProtocols)).EntryPoint

        <Extension>
        Public Function [TypeOf](remote As IPEndPoint, name As String) As Type
            Dim req As New RequestStream(ProtocolEntry, MemoryProtocols.TypeOf, name)
            Dim ref As TypeInfo = req.LoadObject(Of TypeInfo)
            Return ref.GetType(True)
        End Function

        Public Function ReadValue(name As String) As RequestStream
            Return New RequestStream(ProtocolEntry, MemoryProtocols.Read, name)
        End Function

        Public Function WriteValue(name As String, value As Object) As RequestStream
            Dim json As New Argv(name, value)
            Dim req As New RequestStream(ProtocolEntry, MemoryProtocols.Write, json.GetJson)
            Return req
        End Function

        <Extension>
        Public Function ReadValue(remote As IPEndPoint, name As String, type As Type) As Object
            Dim req As RequestStream = ReadValue(name)
            Dim rep As RequestStream = New AsynInvoke(remote).SendMessage(req)
            Return JsonContract.LoadObject(rep.GetUTF8String, type)
        End Function

        <Extension>
        Public Function ReadValue(Of T)(remote As IPEndPoint, name As String) As T
            Return DirectCast(remote.ReadValue(name, GetType(T)), T)
        End Function

        <Extension>
        Public Function WriteValue(remote As IPEndPoint, name As String, value As Object) As Boolean
            Dim req As RequestStream = WriteValue(name, value)
            Dim rep As RequestStream = New AsynInvoke(remote).SendMessage(req)
            Return rep.Protocol = HTTP_RFC.RFC_OK
        End Function
    End Module
End Namespace