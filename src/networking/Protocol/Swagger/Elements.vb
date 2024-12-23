Namespace Protocols.Swagger

    Public Class definition

        Public Property type As String
        Public Property properties As Dictionary(Of String, [property])
        Public Property title As String

    End Class

    Public Class [property]

        Public Property type As String
        Public Property example As String

    End Class

    Public Class info

        Public Property version As String
        Public Property title As String

    End Class

    Public Class path

        Public Property [get] As handler

    End Class

    Public Class handler

        Public Property parameters As parameter()
        Public Property responses As Dictionary(Of String, response)
        Public Property produces As String()

    End Class

    Public Class response

        Public Property description As String
        Public Property schema As Dictionary(Of String, schema)

    End Class

    Public Class schema

        Public Property schema As Dictionary(Of String, String)

    End Class

    Public Class parameter

        Public Property name As String
        Public Property [in] As String
        Public Property type As String
        Public Property [default] As String

    End Class
End Namespace