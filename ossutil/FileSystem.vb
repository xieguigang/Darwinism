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
        Else
            Objects = driver.ListObjects(Me.Bucket.URI(directory)).ToArray
            CurrentDirectory = Objects.First
            tree = FilesTree(Objects)
        End If
    End Sub

    ''' <summary>
    ''' 测试用
    ''' </summary>
    ''' <param name="bucket"></param>
    ''' <param name="objects"></param>
    ''' <param name="driver"></param>
    Sub New(bucket As Bucket, objects As [Object](), driver As CLI)
        Me.driver = driver
        Me.Bucket = bucket
        Me.Objects = objects
        Me.CurrentDirectory = objects(0)

        tree = FilesTree(objects)
    End Sub

    Private Shared Function FilesTree(objects As [Object]()) As Tree(Of Dictionary(Of String, [Object]))
        Dim tokenTuples = objects.Select(Function(obj)
                                             Dim path$() = obj.ObjectName _
                                                              .Split("/"c) _
                                                              .Skip(3) _
                                                              .ToArray
                                             Return (path:=path, [Object]:=obj)
                                         End Function) _
                                 .OrderBy(Function(obj) obj.path.Length) _
                                 .ToArray

    End Function

    Public Overrides Function ToString() As String
        Return $"{CurrentDirectory} @ {Bucket.BucketName}"
    End Function
End Class
