Imports Microsoft.VisualBasic.Language

Namespace Interpreter.Expressions

    Public Class ImportDataDriver : Inherits KeywordExpression

        Public ReadOnly Property dllName As String

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Imports"
            End Get
        End Property

        Sub New(dllName As String)
            Me.dllName = dllName
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            If dllName.FileExists Then
                Return dllName
            End If

            Dim fileName As Value(Of String) = ""
            Dim driver As String = dllName

            If driver.ExtensionSuffix <> "dll" Then
                driver = $"{driver}.dll"
            End If

            For Each dir As String In {App.HOME, App.CurrentDirectory}
                If (fileName = $"{dir}/{driver}").FileExists Then
                    Return fileName
                End If
            Next

            Throw New BadImageFormatException($"driver module '{driver}' not found!")
        End Function

        Public Overrides Function ToString() As String
            Return $"load driver: {dllName}"
        End Function
    End Class
End Namespace