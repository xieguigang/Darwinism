Imports System.Reflection

Namespace Runtime

    Public Class InternalInvoke

        Shared ReadOnly invokes As New Dictionary(Of String, Callable)

        Shared Sub New()
            Call loadInternal(Of Math)()
        End Sub

        Private Shared Sub loadInternal(Of T As Class)()
            Dim type As TypeInfo = GetType(T)
            Dim fields As FieldInfo() = type.DeclaredFields _
                .Where(Function(m)
                           Return m.IsStatic AndAlso m.FieldType Is GetType(Callable)
                       End Function) _
                .ToArray

            For Each item As FieldInfo In fields
                invokes(item.Name) = item.GetValue(Nothing)
            Next
        End Sub

        Public Shared Function FindInvoke(name As String) As Callable
            Return invokes.TryGetValue(name)
        End Function
    End Class
End Namespace