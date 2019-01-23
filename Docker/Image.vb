
''' <summary>
''' Docker image name
''' </summary>
Public Class Image

    Public Property Publisher As String
    Public Property Package As String

    Public Shared Function ParseEntry(text As String) As Image
        With text.Trim.Split("/"c)
            Dim user = .ElementAt(0)
            Dim name = .ElementAt(1)

            Return New Image With {
                .Package = name,
                .Publisher = user
            }
        End With
    End Function

    Public Overrides Function ToString() As String
        Return $"{Publisher}/{Package}"
    End Function
End Class
