Namespace TaskHost

    Public Class MemoryHash

        ReadOnly __innerHash As New SortedDictionary(Of Long, Object)
        ReadOnly __innerAddr As New SortedDictionary(Of Long, ObjectAddress)

        Public Function GetObject(addr As Long) As Object
            If __innerHash.ContainsKey(addr) Then
                Return __innerHash(addr)
            Else
                Throw New NullReferenceException($"Address {addr} point to a invalid memory location!")
            End If
        End Function

    End Class
End Namespace