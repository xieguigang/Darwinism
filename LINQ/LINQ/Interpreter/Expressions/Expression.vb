Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class Expression

        Public ReadOnly Property name As String
            Get
                Return MyClass.GetType.Name
            End Get
        End Property

        Public MustOverride Function Exec(env As Environment) As Object

    End Class
End Namespace