Namespace Runtime

    Public Class Environment

        ReadOnly symbols As New Dictionary(Of String, Symbol)

        ReadOnly parent As Environment

        Sub New(parent As Environment)
            Me.parent = parent
        End Sub

        Public Sub AddSymbol(name As String, type As String)
            symbols.Add(name, New Symbol With {.SymbolKey = name, .type = type})
        End Sub

        Public Function FindSymbol(name As String) As Symbol
            If symbols.ContainsKey(name) Then
                Return symbols(name)
            Else
                Return parent.FindSymbol(name)
            End If
        End Function

    End Class
End Namespace