
''' <summary>
''' Docker image name
''' </summary>
Public Class Image

    Public Property Publisher As String
    Public Property Package As String

    Public Overrides Function ToString() As String
        Return $"{Publisher}/{Package}"
    End Function
End Class
