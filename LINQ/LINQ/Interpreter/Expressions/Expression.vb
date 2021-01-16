Imports LINQ.Runtime

Namespace Interpreter.Expressions

    Public MustInherit Class Expression

        Public ReadOnly Property name As String
            Get
                Return MyClass.GetType.Name.ToLower
            End Get
        End Property

        Public MustOverride Function Exec(context As ExecutableContext) As Object

    End Class
End Namespace