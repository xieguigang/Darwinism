Imports Microsoft.VisualBasic.Scripting.MetaData

Namespace Framework.Provider.ImportsAPI

    ''' <summary>
    ''' 导入的命名空间
    ''' </summary>
    Public Class ImportsNs : Inherits PackageNamespace

        ''' <summary>
        ''' {namespace, typeinfo}
        ''' </summary>
        ''' <returns></returns>
        Public Property Modules As Dictionary(Of String, TypeInfo)
    End Class
End Namespace