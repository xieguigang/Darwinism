Imports stdnum = System.Math

Namespace Runtime

    Friend Class Math

        Public Shared ReadOnly abs As New Callable(AddressOf stdnum.Abs)
        Public Shared ReadOnly min As New Callable(AddressOf stdnum.Min)
        Public Shared ReadOnly max As New Callable(AddressOf stdnum.Max)

    End Class
End Namespace