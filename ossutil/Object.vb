''' <summary>
''' File/Directory object
''' </summary>
Public Class [Object] : Inherits MetaData

    Public ReadOnly Property LastModifiedTime As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property Size As Long
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property StorageClass As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property ETAG As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property ObjectName As String
        Get
            Return getValue()
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return {LastModifiedTime, Size, StorageClass, ETAG, ObjectName}.JoinBy(vbTab)
    End Function
End Class
