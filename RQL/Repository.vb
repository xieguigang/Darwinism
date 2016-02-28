Imports Microsoft.VisualBasic.ComputingServices.TaskHost

Public Class Repository

    ''' <summary>
    ''' {lower_case.url, type_info}
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Models As Dictionary(Of String, TypeInfo)




End Class



''' <summary>
''' 实体对象
''' </summary>
Public Class EntityProvider : Inherits TypeInfo

    ''' <summary>
    ''' 存储的方法
    ''' </summary>
    ''' <returns></returns>
    Public Property Tek As StorageTeks


End Class