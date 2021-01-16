Imports stdnum = System.Math

Namespace Runtime

    Friend Class Math

        Public Shared ReadOnly abs As New Callable(AddressOf stdnum.Abs)
        Public Shared ReadOnly min As New Callable(AddressOf stdnum.Min)
        Public Shared ReadOnly max As New Callable(AddressOf stdnum.Max)
        Public Shared ReadOnly pow As New Callable(AddressOf stdnum.Pow)

    End Class
End Namespace