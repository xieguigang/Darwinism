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
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace