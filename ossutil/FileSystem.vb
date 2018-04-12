Imports Microsoft.VisualBasic.Data.GraphTheory

''' <summary>
''' 线程不安全的OSS文件系统对象
''' </summary>
Public Class FileSystem

    Dim driver As CLI
    Dim tree As Tree(Of Dictionary(Of String, [Object]))

    ''' <summary>
    ''' 在oss文件系统之中的当前的路径
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CurrentDirectory As [Object]
    Public ReadOnly Property Bucket As Bucket
    Public ReadOnly Property Objects As [Object]()

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bucket$">Bucket name</param>
    ''' <param name="directory$">The root directory</param>
    ''' <param name="driver">ossutil cli tool model</param>
    Sub New(bucket$, directory$, driver As CLI)
        Me.driver = driver
        Me.Bucket = driver.GetBucketStorageDeviceList _
                          .Where(Function(b)
                                     Return b.BucketName.TextEquals(bucket)
                                 End Function) _
                          .FirstOrDefault

        If Me.Bucket Is Nothing Then
            Throw New InvalidExpressionException($"Bucket name `{bucket}` is invalid or invalid ossfs credential info!")
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return $"{CurrentDirectory} @ {Bucket.BucketName}"
    End Function
End Class
