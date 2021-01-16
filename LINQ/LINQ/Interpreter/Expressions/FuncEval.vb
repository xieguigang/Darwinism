Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class FuncEval : Inherits Expression

        Public Property func As Expression
        Public Property parameters As Expression()

        Sub New(func As Expression, parameters As IEnumerable(Of Expression))
            Me.func = func
            Me.parameters = parameters.ToArray
        End Sub

        Public Overrides Function Exec(context As ExecutableContext) As Object
            Dim invoke As Object = func.Exec(New ExecutableContext With {.env = context.env, .throwError = False})

            If invoke Is Nothing Then
                Throw New NullReferenceException
            ElseIf TypeOf invoke Is String Then
                invoke = context.env.FindInvoke(invoke)
            ElseIf TypeOf invoke Is SymbolReference Then
                invoke = context.env.FindInvoke(DirectCast(invoke, SymbolReference).symbolName)
            Else
                Throw New NotImplementedException
            End If

            Dim args As New List(Of Object)

            For Each item In parameters
                args.Add(item.Exec(context))
            Next

            Dim result As Object = invoke(args.ToArray)

            Return result
        End Function

        Public Overrides Function ToString() As String
            Return $"{func}({parameters.JoinBy(", ")})"
        End Function
    End Class
End Namespace