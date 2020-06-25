Namespace Runtime

    Public Class Environment

        ReadOnly symbols As New Dictionary(Of String, symbol)

        Public Function FindSymbol(name As String) As Symbol
            Return symbols.TryGetValue(name)
        End Function

    End Class
End Namespace