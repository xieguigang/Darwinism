Namespace Interpreter.Expressions

    ''' <summary>
    ''' $"--opt"
    ''' </summary>
    Public Class CommandLineArgument : Inherits Expression

        Public ReadOnly Property ArgumentName As String

        Sub New()

        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace