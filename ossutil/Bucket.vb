''' <summary>
''' Bucket storage device meta data for cloud file system
''' </summary>
Public Class Bucket : Inherits MetaData

    Public ReadOnly Property CreationTime As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property Region As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property StorageClass As String
        Get
            Return getValue()
        End Get
    End Property

    Public ReadOnly Property BucketName As String
        Get
            Return getValue()
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return {CreationTime, Region, StorageClass, BucketName}.JoinBy(vbTab)
    End Function
End Class
