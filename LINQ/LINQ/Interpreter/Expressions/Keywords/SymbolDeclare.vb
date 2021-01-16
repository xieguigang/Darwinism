Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class SymbolDeclare : Inherits KeywordExpression

        Public Property symbolName As String
        Public Property type As String

        Public Overrides ReadOnly Property keyword As String
            Get
                Return "Let"
            End Get
        End Property

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ToString() As String
            Return $"let {symbolName} as {type}"
        End Function
    End Class
End Namespace