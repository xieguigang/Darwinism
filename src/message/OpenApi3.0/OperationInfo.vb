Imports Darwinism.IPC.OpenApi3.Models

''' <summary>
''' 操作信息辅助类
''' </summary>
Public Class OperationInfo
    Public Property PathTemplate As String
    Public Property HttpMethod As String
    Public Property Operation As OperationObject
    Public Property PathLevelParameters As List(Of ParameterObject)
End Class