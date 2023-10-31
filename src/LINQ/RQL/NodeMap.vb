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

    ''' <summary>
    ''' add resource with duplicated removes
    ''' </summary>
    ''' <param name="resource"></param>
    Public Sub add(resource As String)
        If resource Is Nothing Then
            Return
        ElseIf resources.IndexOf(resource) = -1 Then
            Call resources.Add(resource)
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return $"link_size:{size}"
    End Function

End Class
