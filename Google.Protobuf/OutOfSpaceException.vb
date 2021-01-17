Imports System.IO

Namespace Google.Protobuf

    ''' <summary>
    ''' Indicates that a CodedOutputStream wrapping a flat byte array
    ''' ran out of space.
    ''' </summary>
    Public NotInheritable Class OutOfSpaceException
        Inherits IOException

        Friend Sub New()
            MyBase.New("CodedOutputStream was writing to a flat byte array and ran out of space.")
        End Sub
    End Class
End Namespace