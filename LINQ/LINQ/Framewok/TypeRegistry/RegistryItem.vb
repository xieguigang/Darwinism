Namespace Framework

    ''' <summary>
    ''' item in the type registry table
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RegistryItem

        ''' <summary>
        ''' 类型的简称或者别称，即本属性为LINQEntity自定义属性中的构造函数的参数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property Name As String
        ''' <summary>
        ''' 建议使用相对路径，以防止移动程序的时候任然需要重新注册方可以使用
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property AssemblyPath As String
        ''' <summary>
        ''' Full type name for the target LINQ entity type.(目标LINQEntity集合中的类型全称)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property TypeId As String

        Public ReadOnly Property AssemblyFullPath As String
            Get
                Return IO.Path.GetFullPath(AssemblyPath)
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("({0}) {1}!{2}", Name, AssemblyPath, TypeId)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Obj">Name, TypeId, AssemblyPath, IsInnerType</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Widening Operator CType(Obj As Object()) As RegistryItem
            Dim RegistryItem As RegistryItem = New RegistryItem
            RegistryItem.Name = Obj(0).ToString
            RegistryItem.TypeId = Obj(1).ToString
            RegistryItem.AssemblyPath = Obj(2).ToString

            Return RegistryItem
        End Operator
    End Class
End Namespace