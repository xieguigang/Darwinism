Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language.Default

''' <summary>
''' 线程不安全的OSS文件系统对象
''' </summary>
Public Class FileSystem

    Public ReadOnly Property Root As String
    Public ReadOnly Property Bucket As Bucket

    Dim driver As CLI

    ''' <summary>
    ''' 在oss文件系统之中的当前的路径
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CurrentDirectory As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bucket$">Bucket name</param>
    ''' <param name="directory$">The root directory</param>
    ''' <param name="driver">ossutil cli tool model</param>
    Sub New(bucket$, directory$, driver As CLI, Optional current$ = Nothing)
        Me.Root = directory
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

    Private Sub New(bucket As Bucket, root$, current$, driver As CLI)
        Me.Bucket = bucket
        Me.Root = root
        Me.driver = driver
        Me.CurrentDirectory = current
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function CurrentDefault() As DefaultValue(Of String)
        Return CurrentDirectory
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetPath(rel As String) As String
        Return CurrentDirectory.ChangeFileSystemContext(Root, rel)
    End Function

    ''' <summary>
    ''' 使用相对路径修改了默认位置之后返回当前的文件系统的一个拷贝
    ''' </summary>
    ''' <param name="rel"></param>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ChangeDirectory(rel As String) As FileSystem
        Return New FileSystem(Bucket, Root, GetPath(rel), driver)
    End Function

    Public Function EnumerateFiles(Optional directory$ = Nothing) As IEnumerable(Of [Object])
        If directory.StringEmpty Then
            Return driver.ListObjects(Bucket.URI(CurrentDirectory))
        Else
            Return driver.ListObjects(Bucket.URI(GetPath(directory)))
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function EnumerateDirectories(Optional directory$ = Nothing) As IEnumerable(Of [Object])
        Return EnumerateFiles(directory).Where(AddressOf IsDirectory)
    End Function

    Public Overrides Function ToString() As String
        Return $"{CurrentDirectory} @ {Bucket.BucketName}"
    End Function
End Class
