Imports System.Reflection

Namespace Runtime

    Public Class Callable

        Dim method As MethodInfo

        Public ReadOnly Property name As String
            Get
                Return method.Name
            End Get
        End Property

        Sub New(method As MethodInfo)
            Me.method = method
        End Sub

        Sub New(math1 As Func(Of Double, Double))
            Call Me.New(math1.Method)
        End Sub

        Sub New(math2 As Func(Of Double, Double, Double))
            Call Me.New(math2.Method)
        End Sub

        Public Function Evaluate(params As Object()) As Object

        End Function
    End Class
End Namespace