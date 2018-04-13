Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Default

''' <summary>
''' 对阿里云OSS文件系统进行抽象的线程不安全的OSS文件系统对象
''' </summary>
Public Class FileSystem

    ''' <summary>
    ''' OSS cloud file system I/O driver
    ''' </summary>
    Dim driver As CLI
    ''' <summary>
    ''' The tree graph of the <see cref="Objects"/>
    ''' </summary>
    Dim tree As Tree(Of [Object])

    ''' <summary>
    ''' 在oss文件系统之中的当前的路径
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CurrentDirectory As [Object]
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return tree.Data
        End Get
    End Property

    ''' <summary>
    ''' File system root entry/device entry
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Bucket As Bucket
    ''' <summary>
    ''' Jump points
    ''' </summary>
    ''' <returns></returns>
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
            tree = FilesTree(Objects, Me.Bucket.BucketName)
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

        tree = FilesTree(objects, bucket.BucketName)
    End Sub

    ''' <summary>
    ''' Clone
    ''' </summary>
    ''' <param name="bucket"></param>
    ''' <param name="objects"></param>
    ''' <param name="driver"></param>
    Sub New(bucket As Bucket, objects As [Object](), tree As Tree(Of [Object]), driver As CLI)
        Me.Bucket = bucket
        Me.Objects = objects
        Me.driver = driver
        Me.tree = tree
    End Sub

    Private Shared Function FilesTree(objects As [Object](), bucketName$) As Tree(Of [Object])
        Dim tokenTuples = objects.Select(Function(obj)
                                             Dim path$() = obj.ObjectName _
                                                              .Split("/"c) _
                                                              .Skip(3) _
                                                              .ToArray
                                             Return (path:=path, [Object]:=obj)
                                         End Function) _
                                 .OrderBy(Function(obj) obj.path.Length) _
                                 .ToArray
        Dim node As Tree(Of [Object])
        Dim key$
        Dim root As New Tree(Of [Object])("/") With {
            .Label = $"oss://{bucketName}",
            .Childs = New Dictionary(Of String, Tree(Of [Object])),
            .Data = New [Object] With {
                .meta = New Dictionary(Of String, String) From {
                    {NameOf([Object].ObjectName), "/"}
                }
            }
        }

        For Each obj As (path As String(), obj As [Object]) In tokenTuples
            node = root

            With obj.path
                For i As Integer = 0 To .Length - 1
                    key = .ByRef(i)

                    If key.StringEmpty AndAlso i = .Length - 1 AndAlso obj.obj.IsDirectory Then
                        node.Data = obj.obj
                    Else
                        If Not node.Childs.ContainsKey(key) Then
                            Dim newNode As New Tree(Of [Object])("/") With {
                                .Label = key,
                                .Childs = New Dictionary(Of String, Tree(Of [Object])),
                                .Parent = node
                            }

                            If i = .Length - 1 Then
                                ' 这是一个新的文件节点
                                newNode.Data = obj.obj
                            End If

                            node.Childs.Add(key, newNode)
                        End If

                        node = node.Childs(key)
                    End If
                Next
            End With
        Next

        Return root
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="directory">
    ''' 相对路径或者绝对路径
    ''' </param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 所有使用``/``起始的都是绝对路径
    ''' </remarks>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function ChangeDirectory(directory As String) As FileSystem
        Return New FileSystem(Bucket, Objects, GetTarget(path:=directory), driver)
    End Function

    Private Function GetTarget(path As String) As Tree(Of [Object])
        If path.First = "/"c Then
            ' 绝对路径
            Return tree.VisitTree(path.SplitPath)
        Else
            ' 相对路径
            Return tree.ChangeFileSystemContext(path.SplitPath)
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Function tempFile() As DefaultValue(Of String)
        Return New DefaultValue(Of String) With {
            .LazyValue = New Lazy(Of String)(Function() App.GetAppSysTempFile(".tmp", App.PID))
        }
    End Function

    ''' <summary>
    ''' Get file from OSS
    ''' </summary>
    ''' <param name="path$">远程对象的相对路径或者绝对路径，要求这个远程对象必须要存在</param>
    ''' <param name="save$"></param>
    ''' <returns></returns>
    Public Function [Get](path$, Optional save$ = Nothing) As String
        Dim target As Tree(Of [Object]) = GetTarget(path)

        With save Or tempFile()
            driver.Copy(from:=target.QualifyName, [to]:= .ByRef)
            ' returns normalized local filesystem full path
            path = .GetFullPath
        End With

        Return path
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="local$"></param>
    ''' <param name="remote$">不要求远程对象必须要存在</param>
    Public Sub Put(local$, remote$)
        Dim context As List(Of String)

        remote = remote _
            .Replace("\", "/") _
            .StringReplace("[/]{2,}", "/")

        If remote.First = "/"c Then
            ' 绝对路径
            ' 不进行任何处理？？
            context = New List(Of String)
        Else
            ' 需要将相对路径转换为绝对路径
            context = CurrentDirectory.ObjectName _
                                      .SplitPath _
                                      .Skip(2) _
                                      .AsList
        End If

        For Each name As String In remote.SplitPath
            If name = "." Then
                ' 不进行任何处理
            ElseIf name = ".." Then
                ' 访问父目录
                context.Pop()
            Else
                context += name
            End If
        Next

        remote = Bucket.URI(context.JoinBy("/"))
        driver.Copy(from:=local, [to]:=remote)
    End Sub

    Public Overrides Function ToString() As String
        Return CurrentDirectory.ToString
    End Function
End Class
