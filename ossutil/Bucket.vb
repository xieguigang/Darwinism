''' <summary>
''' Bucket storage device meta data for cloud file system
''' </summary>
Public Class Bucket : Inherits MetaData

    Public Const Protocol$ = "oss://"

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
            Return Mid(getValue(), Protocol.Length + 1)
        End Get
    End Property

    Public Function URI(path As String) As String
        Return $"oss://{BucketName}/{path}"
    End Function

    Public Overrides Function ToString() As String
        Return $"{BucketName} @ {Region}"
    End Function
End Class
