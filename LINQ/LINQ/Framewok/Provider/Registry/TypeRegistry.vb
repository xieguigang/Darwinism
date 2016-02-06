Imports System.Reflection
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.LINQ.Framework.Provider

Namespace Framework.Provider

    ''' <summary>
    ''' Type registry table for loading the external LINQ entity assembly module.
    ''' (起始这个模块就是相当于一个类型缓存而已，因为程序可以直接读取dll文件里面的内容，但是直接读取的方法会造成性能下降，所以需要使用这个对象来缓存所需要的类型数据) 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TypeRegistry : Inherits ITextFile
        Implements IDisposable

        <Xml.Serialization.XmlElement> Public Property typeDefs As TypeEntry()
            Get
                Return _typeHash.Values.ToArray
            End Get
            Set(value As TypeEntry())
                If value Is Nothing Then
                    _typeHash = New Dictionary(Of String, TypeEntry)
                Else
                    _typeHash = value.ToDictionary(Function(x) x.name.ToLower)
                End If
            End Set
        End Property

        Dim _typeHash As Dictionary(Of String, TypeEntry)

        ''' <summary>
        ''' 返回包含有该类型的目标模块的文件路径
        ''' </summary>
        ''' <param name="name">LINQ Entity集合中的元素的简称或者别称，即Item中的Name属性</param>
        ''' <returns>If the key is not exists in this object, than the function will return a empty string.</returns>
        ''' <remarks></remarks>
        Public Function FindAssemblyPath(name As String) As Assembly
            Dim type As TypeEntry = Find(name)
            If type Is Nothing Then
                Return Nothing
            Else
                Return type.LoadAssembly
            End If
        End Function

        ''' <summary>
        ''' Return a registry item in the table using its specific name property.
        ''' (返回注册表中的一个指定名称的项目)
        ''' </summary>
        ''' <param name="name">大小写不敏感的</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Find(name As String) As TypeEntry
            If _typeHash.ContainsKey(name.ToLower.ShadowCopy(name)) Then
                Return _typeHash(name)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Registry the external LINQ entity assembly module in the LINQFramework
        ''' </summary>
        ''' <param name="assmPath">DLL file path</param>
        ''' <returns></returns>
        ''' <remarks>查询出目标元素的类型定义并获取信息</remarks>
        Public Function Register(assmPath As String) As Boolean
            Dim assm As Assembly = Assembly.LoadFrom(IO.Path.GetFullPath(assmPath)) 'Load external module
            Dim typeDefs As TypeInfo() =
                LQueryFramework.LoadAssembly(assm, LinqEntity.ILinqEntity) 'Get type define informations of LINQ entity

            If typeDefs.IsNullOrEmpty Then Return False

            Dim LQuery As IEnumerable(Of TypeEntry) =
                    From type As Type In typeDefs
                    Select New TypeEntry With {
                        .name = LinqEntity.GetEntityType(type),
                        .Assembly = assmPath,
                        .TypeId = type.FullName
                    }        'Generate the resitry item for each external type

            For Each x As TypeEntry In LQuery     'Update exists registry item or insrt new item into the table
                Dim exists As TypeEntry = Find(x.name)         '在注册表中查询是否有已注册的类型
                If exists Is Nothing Then
                    Call Me.typeDefs.Add(x)  'Insert new record.(添加数据)
                Else                                'Update exists data.(更新数据)
                    exists.Assembly = x.Assembly
                    exists.TypeId = x.TypeId
                End If
            Next
            Return True
        End Function

        ''' <summary>
        ''' 扫描安装应用程序文件夹之中的所有插件
        ''' </summary>
        Public Sub InstallCurrent()
            Dim dlls = FileIO.FileSystem.GetFiles(App.HOME, FileIO.SearchOption.SearchTopLevelOnly, "*.dll", "*.exe")
            For Each assm As String In dlls
                Call Register(assm)
            Next
        End Sub

        Public Shared Function Load(Path As String) As TypeRegistry
            If FileIO.FileSystem.FileExists(Path) Then
                Dim registry As TypeRegistry = Path.LoadTextDoc(Of TypeRegistry)()
                Return registry
            Else
                Return New TypeRegistry With {
                    .FilePath = Path,
                    .typeDefs = Nothing
                }
            End If
        End Function

        Public Shared Function LoadDefault() As TypeRegistry
            Return TypeRegistry.Load(LQueryFramework.DefaultFile)
        End Function

        Public Overrides Function Save(Optional FilePath As String = "", Optional Encoding As Encoding = Nothing) As Boolean
            Return Me.GetXml.SaveAsXml(getPath(FilePath), True, Encoding)
        End Function
    End Class
End Namespace