

Public Module API

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="type">单个元素的类型</param>
    ''' <returns></returns>
    Public Function XmlList(url As String, type As Type) As IEnumerable
        Dim listType As Type = GetType(List(Of )).MakeGenericType(type)
        Dim obj As Object = LoadXml(Of ()
    End Function
End Module
