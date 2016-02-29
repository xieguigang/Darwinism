Imports Microsoft.VisualBasic.ComputingServices.TaskHost
Imports Microsoft.VisualBasic.Linq.Framework.Provider
Imports Microsoft.VisualBasic.Linq.Framework.Provider.ImportsAPI
Imports Microsoft.VisualBasic.Linq.LDM.Expression
Imports Microsoft.VisualBasic.Scripting

Namespace StorageTek

    ''' <summary>
    ''' 实体对象
    ''' </summary>
    Public Class EntityProvider : Inherits MetaData.TypeInfo

        ''' <summary>
        ''' 存储的方法
        ''' </summary>
        ''' <returns></returns>
        Public Property Tek As StorageTeks
        ''' <summary>
        ''' 映射的实际的存储位置
        ''' </summary>
        ''' <returns></returns>
        Public Property MapFileIO As String

        Public Function GetRepository() As IEnumerable
            Dim api As IRepository = StorageTek.API.InternalAPIs(Tek)
            Dim type As Type = Me.GetType
            Dim source As IEnumerable = api(MapFileIO, type)
            Return source
        End Function

        Public Overrides Function ToString() As String
            Return $"[{Tek.ToString}] {MapFileIO}  //{MyBase.ToString}"
        End Function

        Public Function LinqWhere(where As String, types As TypeRegistry, api As APIProvider) As IEnumerable
            Dim Closure = WhereClosure.CreateLinqWhere([GetType], where)
            Dim LQuery = (From x As Object In Me.GetRepository()
                          Where True = Closure.WhereTest(x)
                          Select x)
            Return LQuery
        End Function
    End Class
End Namespace