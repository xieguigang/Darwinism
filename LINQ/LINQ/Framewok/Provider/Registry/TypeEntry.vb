Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic

Namespace Framework.Provider

    ''' <summary>
    ''' item in the type registry table
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TypeEntry : Implements sIdEnumerable

        ''' <summary>
        ''' 类型的简称或者别称，即本属性为LINQEntity自定义属性中的构造函数的参数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property name As String Implements sIdEnumerable.Identifier
        ''' <summary>
        ''' 建议使用相对路径，以防止移动程序的时候任然需要重新注册方可以使用
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property Assembly As String
        ''' <summary>
        ''' Full type name for the target LINQ entity type.(目标LINQEntity集合中的类型全称)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Xml.Serialization.XmlAttribute> Public Property TypeId As String

        Public Function LoadAssembly() As Assembly
            Dim path As String = FileIO.FileSystem.GetFileInfo(Assembly).FullName
            Dim assm As Assembly = System.Reflection.Assembly.LoadFile(path)
            Return assm
        End Function

        Public Overloads Function [GetType]() As Type
            Dim assm As Assembly = LoadAssembly()
            Dim type As Type = assm.GetType(TypeId)
            Return type
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("({0}) {1}!{2}", name, Assembly, TypeId)
        End Function
    End Class
End Namespace