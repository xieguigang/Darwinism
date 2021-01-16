Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class SymbolReference : Inherits Expression

        Dim symbolName As String

        Sub New(name As String)
            Me.symbolName = name
        End Sub

        Public Overrides Function Exec(env As Environment) As Object
            Dim symbol As Symbol = env.FindSymbol(symbolName)

            If symbol Is Nothing Then
            Else

            End If
        End Function

        Public Overrides Function ToString() As String
            Return $"&{symbolName}"
        End Function
    End Class
End Namespace