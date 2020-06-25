Imports LINQ.Interpreter.Expressions
Imports LINQ.Script

Namespace Interpreter

    Public Class Program

        ReadOnly execQueue As Expression()

        Friend Sub New(exec As IEnumerable(Of Expression))
            execQueue = exec.ToArray
        End Sub

        Public Overrides Function ToString() As String
            Return execQueue.JoinBy(" ;" & vbCrLf)
        End Function

        Public Shared Function CreateProgram(script As String) As Program
            Return New Program(Language.GetTokens(script).PopulateQueryExpressions)
        End Function

    End Class
End Namespace