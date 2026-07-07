Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Inspect

    Public Class InspectLayer : Implements INamedValue

        Public Property Architecture As String
        Public Property Created As String
        Public Property Id As String Implements INamedValue.Key
        Public Property Os As String
        Public Property Parent As String
        Public Property Size As Long
        Public Property Config As LayerConfig
        Public Property GraphDriver As GraphDriver
        Public Property RootFS As RootFS
        Public Property Metadata As Metadata

        ' ps@ps:/mnt$ docker inspect 48686c37b882
        ' [
        '   {
        '     "Architecture": "amd64",
        '     "Config": {
        '       "Cmd": [
        '         "/bin/bash"
        '       ],
        '       "WorkingDir": "/mnt/sdb/metagenomics_LLMs"
        '     },
        '     "Created": "2026-02-26T05:09:18.70362583Z",
        '     "GraphDriver": {
        '       "Data": {
        '         "LowerDir": "/var/lib/docker/overlay2/97da2b370c69db8efaf0ff2d3c3f42d5cde89be9e4e7eca08395e5af8d6b7085/diff:/var/lib/docker/overlay2/4db5e84f058c7d220f7114a527e50339b110e637356e9f625838c54793e09f7b/diff",
        '         "MergedDir": "/var/lib/docker/overlay2/fd6407ccce4943cd51d9c3dc78f1835afe39853bc43dcfc9f65bb25a584fdafa/merged",
        '         "UpperDir": "/var/lib/docker/overlay2/fd6407ccce4943cd51d9c3dc78f1835afe39853bc43dcfc9f65bb25a584fdafa/diff",
        '         "WorkDir": "/var/lib/docker/overlay2/fd6407ccce4943cd51d9c3dc78f1835afe39853bc43dcfc9f65bb25a584fdafa/work"
        '       },
        '       "Name": "overlay2"
        '     },
        '     "Id": "sha256:48686c37b882eccb90efbe72b73a5397d09118b6d067528750f82f5121a53df8",
        '     "Metadata": {
        '       "LastTagTime": "2026-02-26T13:09:18.717659063+08:00"
        '     },
        '     "Os": "linux",
        '     "Parent": "sha256:80c088195ee82e1671991d120be08dfc37929342e86887d22c9a878238ff45f4",
        '     "RepoDigests": [],
        '     "RepoTags": [],
        '     "RootFS": {
        '       "Layers": [
        '         "sha256:56e3c8b0968beb76951750ee599dbe9decd5235fbf1589de7707b545d918c05e",
        '         "sha256:768bb212ea574e52f685481b58a33fc3e1de81e0833646c2ad671b6524ecce95",
        '         "sha256:20fd619d6816dd4d602e0e523f3e786e206be3c952422fa811173f355cb9cb6f"
        '       ],
        '       "Type": "layers"
        '     },
        '     "Size": 21805111078
        '   }
        ' ]

        Public Shared Function ParseJSON(jsonstr As String) As InspectLayer
            Return jsonstr.LoadJSON(Of InspectLayer())()(0)
        End Function

        ''' <summary>
        ''' 递归式的找出目标镜像所有的子镜像以及孙子镜像的id集合
        ''' </summary>
        ''' <param name="images">docker images -aq => docker inspect得到的inspect结果集合</param>
        ''' <param name="target">需要进行分析的目标镜像的id</param>
        ''' <returns>递归式的找出目标镜像所有的子镜像以及孙子镜像的id集合</returns>
        Public Shared Function ImageChilds(images As InspectLayer(), target As String) As IEnumerable(Of String)
            Dim childs As New List(Of String)
            Dim visited As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            If images Is Nothing OrElse images.Length = 0 OrElse String.IsNullOrEmpty(target) Then
                Return childs
            End If

            ' 找出目标镜像（支持短ID和完整ID匹配）
            ' 注意：不能用 images.KeyItem(target)，因为 KeyItem 做的是精确匹配，
            ' 而 docker rmi 传入的是短ID，inspect 返回的是 sha256: 前缀的完整ID
            Dim targetImg As InspectLayer = images.FirstOrDefault(
                Function(img) Not String.IsNullOrEmpty(img.Id) AndAlso IdMatch(img.Id, target))

            If targetImg Is Nothing Then
                Return childs
            End If

            ' 从目标镜像开始，递归向下查找所有子镜像
            Call FindChildsRecursive(images, targetImg.Id, childs, visited)

            Return childs
        End Function

        ''' <summary>
        ''' 递归查找所有依赖指定父镜像的子镜像
        ''' </summary>
        ''' <param name="images">全部镜像集合</param>
        ''' <param name="parentId">父镜像ID</param>
        ''' <param name="childs">用于收集子镜像ID的列表</param>
        ''' <param name="visited">已访问的镜像ID集合，防止循环引用导致无限递归</param>
        Private Shared Sub FindChildsRecursive(images As InspectLayer(),
                                               parentId As String,
                                               childs As List(Of String),
                                               visited As HashSet(Of String))
            ' 找出所有 Parent 指向 parentId 的镜像，这些就是 parentId 的直接子镜像
            Dim directChilds As IEnumerable(Of InspectLayer) =
                images.Where(Function(img) Not String.IsNullOrEmpty(img.Parent) AndAlso
                                          IdMatch(img.Parent, parentId))

            For Each child As InspectLayer In directChilds
                ' 防止循环引用（虽然正常情况下不会出现，但做防御性处理）导致无限递归
                If visited.Contains(child.Id) Then
                    Continue For
                End If

                Call visited.Add(child.Id)
                Call childs.Add(child.Id)

                ' 以当前子镜像为父节点，继续递归查找孙子镜像
                Call FindChildsRecursive(images, child.Id, childs, visited)
            Next
        End Sub

        ''' <summary>
        ''' 比较两个镜像ID是否匹配
        ''' 支持短ID前缀匹配（如 docker rmi 5cdc6535eec6），并忽略 sha256: 前缀
        ''' </summary>
        ''' <param name="id1">镜像ID1</param>
        ''' <param name="id2">镜像ID2</param>
        ''' <returns>如果两个ID匹配则返回True</returns>
        Private Shared Function IdMatch(id1 As String, id2 As String) As Boolean
            If String.IsNullOrEmpty(id1) OrElse String.IsNullOrEmpty(id2) Then
                Return False
            End If

            ' 去除 sha256: 前缀，统一比较格式
            Dim normalized1 As String = id1.Replace("sha256:", "")
            Dim normalized2 As String = id2.Replace("sha256:", "")

            ' 支持短ID前缀匹配
            ' 例如 "5cdc6535eec6" 可以匹配 "5cdc6535eec6xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            Return normalized1.StartsWith(normalized2, StringComparison.OrdinalIgnoreCase) OrElse
                   normalized2.StartsWith(normalized1, StringComparison.OrdinalIgnoreCase)
        End Function


    End Class

    Public Class LayerConfig

        Public Property Cmd As String()
        Public Property WorkingDir As String

    End Class

    Public Class GraphDriver

        Public Property Name As String
        Public Property Data As Dictionary(Of String, String)

    End Class

    Public Class Metadata

        Public Property LastTagTime As String

    End Class

    Public Class RootFS

        Public Property Layers As String()
        Public Property Type As String

    End Class
End Namespace