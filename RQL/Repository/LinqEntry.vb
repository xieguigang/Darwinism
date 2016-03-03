Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.ComputingServices.TaskHost

''' <summary>
''' 类型信息是所查询的对象的类型信息
''' </summary>
Public Class LinqEntry : Inherits MetaData.TypeInfo

    ''' <summary>
    ''' 唯一标识当前的这个查询的哈希值
    ''' </summary>
    ''' <returns></returns>
    Public Property uid As String
    ''' <summary>
    ''' 除了使用上面的uid进行url查询，也可以使用这个地址来使用socket查询，具体的协议已经封装在<see cref="ILinq(Of T)"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property Linq As IPEndPoint

End Class
