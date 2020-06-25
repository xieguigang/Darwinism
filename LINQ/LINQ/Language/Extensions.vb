Namespace Language

    <HideModuleName>
    Public Module Extensions

        Public Function GetTokens(script As String) As IEnumerable(Of Token)
            Return New Tokenizer(code:=script).GetTokens
        End Function
    End Module
End Namespace