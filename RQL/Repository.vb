Imports Microsoft.VisualBasic.ComputingServices.TaskHost

Public Class Repository

    ''' <summary>
    ''' {lower_case.url, type_info}
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Models As Dictionary(Of String, TypeInfo)




End Class

''' <summary>
''' Entity storage technology.(实体对象所存储的方法)
''' </summary>
Public Enum StorageTek As Integer
    ''' <summary>
    ''' Individual files in a directory.(以单独的文件的形式保存在一个文件夹之中)
    ''' </summary>
    DIR = 2
    ''' <summary>
    ''' Csv rows.(Csv文件的行映射为某一个实体对象)
    ''' </summary>
    Tabular = 4
    ''' <summary>
    ''' Xml文件之中的List之中的某一个对象映射为某一个实体对象
    ''' </summary>
    Xml = 8
    ''' <summary>
    ''' Json文件之中的list之中的某一个对象映射为某一个实体对象
    ''' </summary>
    Json = 16
    ''' <summary>
    ''' 实体对象是存储在MySQL数据库的某一张表之中的
    ''' </summary>
    SQL
End Enum

''' <summary>
''' 实体对象
''' </summary>
Public Class EntityProvider : Inherits TypeInfo

    ''' <summary>
    ''' 存储的方法
    ''' </summary>
    ''' <returns></returns>
    Public Property Tek As StorageTek


End Class