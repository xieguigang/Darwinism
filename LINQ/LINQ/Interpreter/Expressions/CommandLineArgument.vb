Namespace Interpreter.Expressions

    ''' <summary>
    ''' $"--opt"
    ''' </summary>
    Public Class CommandLineArgument : Inherits Expression

        Public ReadOnly Property ArgumentName As String

        Sub New(arg As String)
            ArgumentName = arg
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Return CType(App.CommandLine(ArgumentName), String)
        End Function

        Public Overrides Function ToString() As String
            Return $"$ARGS['{ArgumentName}']"
        End Function
    End Class
End Namespace