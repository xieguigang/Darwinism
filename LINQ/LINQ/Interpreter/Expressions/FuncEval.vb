Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public Class FuncEval : Inherits Expression

        Public Property func As Expression
        Public Property parameters As Expression()

        Public Overrides Function Exec(env As Environment) As Object
            Dim invoke As Object = func.Exec(env)

            If invoke Is Nothing Then
                Throw New NullReferenceException
            ElseIf TypeOf invoke Is String Then
                invoke = env.FindInvoke(invoke)
            ElseIf TypeOf invoke Is SymbolReference Then
                invoke = env.FindInvoke(DirectCast(invoke, SymbolReference).symbolName)
            Else
                Throw New NotImplementedException
            End If

            Dim args As New List(Of Object)

            For Each item In parameters
                args.Add(item.Exec(env))
            Next

            Dim result As Object = invoke(args.ToArray)

            Return result
        End Function
    End Class
End Namespace