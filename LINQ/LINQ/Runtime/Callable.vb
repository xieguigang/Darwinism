Imports System.Reflection

Namespace Runtime

    Public Class Callable

        Dim method As MethodInfo
        Dim parameters As ParameterInfo()

        Public ReadOnly Property name As String
            Get
                Return method.Name
            End Get
        End Property

        Sub New(method As MethodInfo)
            Me.method = method
            Me.parameters = method.GetParameters
        End Sub

        Sub New(math1 As Func(Of Double, Double))
            Call Me.New(math1.Method)
        End Sub

        Sub New(math2 As Func(Of Double, Double, Double))
            Call Me.New(math2.Method)
        End Sub

        Public Function Evaluate(params As Object()) As Object
            Dim args As New List(Of Object)

            For i As Integer = 0 To parameters.Length - 1
                If i >= params.Length Then
                    If parameters(i).IsOptional Then
                        args.Add(parameters(i).DefaultValue)
                    Else
                        Throw New InvalidExpressionException
                    End If
                Else
                    args.Add(CTypeDynamic(params(i), parameters(i).ParameterType))
                End If
            Next

            Return method.Invoke(Nothing, args.ToArray)
        End Function
    End Class
End Namespace