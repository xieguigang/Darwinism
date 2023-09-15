Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' A descriptor of the specific resource file
''' </summary>
Public Class NodeMap

    Public Property resources As List(Of String)
    Public ReadOnly Property size As Integer
        Get
            If resources Is Nothing Then
                Return 0
            End If

            Return resources.Count
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return $"link_size:{size}"
    End Function

End Class
