#Region "Microsoft.VisualBasic::994033b07575a170b0fab623612f07c5, src\CloudKit\ossutil\FileSystem.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 262
    '    Code Lines: 149
    ' Comment Lines: 79
    '   Blank Lines: 34
    '     File Size: 8.74 KB


    ' Class FileSystem
    ' 
    '     Properties: Bucket, CurrentDirectory, Objects
    ' 
    '     Constructor: (+4 Overloads) Sub New
    ' 
    '     Function: [Get], ChangeDirectory, FilesTree, GetContext, GetTarget
    '               ToString
    ' 
    '     Sub: Put
    ' 
    ' /********************************************************************************/

#End Region

#If netcore5 = 1 Then
Imports System.Data
#End If

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Default
Imports ThinkVB.FileSystem.OSS.Model

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

    Const InvalidConfig$ = "Bucket name `{0}` is invalid or invalid ossfs credential info!"

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
            Throw New InvalidExpressionException(String.Format(InvalidConfig, bucket))
        Else
            Objects = driver _
                .ListObjects(Me.Bucket.URI(directory)) _
                .ToArray
            tree = FilesTree(Objects, Me.Bucket.BucketName)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bucket$"></param>
    ''' <param name="directory$"></param>
    ''' <param name="driver$">The executable file location</param>
    Sub New(bucket$, directory$, driver$)
        Call Me.New(bucket, directory, New CLI(driver))
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
        Dim tokenTuples = objects _
            .Select(Function(obj)
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
            .label = $"oss://{bucketName}",
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
                                .label = key,
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
        ' context已经是一个经过归一化处理的完整路径了
        Dim context$() = GetContext(tree.Data, path)
        ' 绝对路径从root开始访问
        Return tree.BacktrackingRoot.VisitTree(context)
    End Function

    Private Shared Function GetContext(current As [Object], path$) As String()
        Dim context As List(Of String)

        path = path _
            .Replace("\", "/") _
            .StringReplace("[/]{2,}", "/")

        If path.First = "/"c Then
            ' 绝对路径
            ' 不进行任何处理？？
            context = New List(Of String)
        Else
            ' 需要将相对路径转换为绝对路径
            context = current.ObjectName _
                .SplitPath _
                .Skip(2) _
                .AsList
        End If

        For Each name As String In path.SplitPath
            If name = "." Then
                ' 不进行任何处理
            ElseIf name = ".." Then
                ' 访问父目录
                context.Pop()
            Else
                context += name
            End If
        Next

        Return context
    End Function

    ''' <summary>
    ''' 获取得到临时文件路径
    ''' </summary>
    Shared tempFile As New [Default](Of String)(AddressOf populateTempFile, isLazy:=False)

    ''' <summary>
    ''' Get file from OSS
    ''' </summary>
    ''' <param name="path$">远程对象的相对路径或者绝对路径，要求这个远程对象必须要存在</param>
    ''' <param name="save$"></param>
    ''' <returns></returns>
    Public Function [Get](path$, Optional save$ = Nothing) As String
        Dim target As Tree(Of [Object]) = GetTarget(path)

        With save Or tempFile
            driver.Copy(from:=target.QualifyName, [to]:= .ByRef)
            ' returns normalized local filesystem full path
            path = .GetFullPath
        End With

        Return path
    End Function

    ''' <summary>
    ''' File upload from local filesystem
    ''' </summary>
    ''' <param name="local$"></param>
    ''' <param name="remote$">不要求远程对象必须要存在</param>
    Public Sub Put(local$, remote$)
        Dim context = GetContext(path:=remote, current:=CurrentDirectory)
        remote = context.JoinBy("/")
        remote = Bucket.URI(remote)
        driver.Copy(from:=local, [to]:=remote)
    End Sub

    Public Overrides Function ToString() As String
        Return CurrentDirectory.ToString
    End Function
End Class
