Imports Microsoft.VisualBasic.Net.Protocols

Namespace SharedMemory

    Module Protocols

        Public Enum MemoryProtocols
            Read
            Write
        End Enum

        Public Function ReadValue(name As String) As RequestStream

        End Function

        Public Function WriteValue(name As String, value As Object) As RequestStream

        End Function
    End Module
End Namespace