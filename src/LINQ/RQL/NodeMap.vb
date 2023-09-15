Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' A descriptor of the specific resource file
''' </summary>
Public Class NodeMap

    Public Property resources As List(Of String)
    Public ReadOnly Property size As Integer
        Get
            Return resources.Count
        End Get
    End Property

End Class
