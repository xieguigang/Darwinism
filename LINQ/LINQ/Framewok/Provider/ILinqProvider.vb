Namespace Framework.Provider

    ''' <summary>
    ''' Get a Collection of the target LINQ entity from file object.(从文件对象获取目标LINQ实体对象的集合)
    ''' </summary>
    ''' <param name="uri">File path or resource from url</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Delegate Function GetLinqResource(uri As String) As IEnumerable

    Public Module RegistryReader

        ''' <summary>
        ''' 数据源的示例函数
        ''' </summary>
        ''' <param name="uri"></param>
        ''' <returns></returns>
        <LinqEntity("typeDef", GetType(TypeEntry))>
        Public Function GetResource(uri As String) As IEnumerable
            Dim registry As TypeRegistry = TypeRegistry.Load(uri)
            Return registry.typeDefs
        End Function
    End Module
End Namespace