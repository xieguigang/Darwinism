Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Runtime

    Public Class GlobalEnvironment : Inherits Environment

        Protected ReadOnly registry As Registry

        Sub New(registry As Registry, ParamArray values As NamedValue(Of Object)())
            Call MyBase.New(Nothing)

            Dim typeCode As String
            Dim symbol As Symbol

            For Each item In values
                typeCode = registry.GetTypeCodeName(item.Value.GetType)
                symbol = AddSymbol(item.Name, typeCode)
                symbol.value = item.Value
            Next

            Me.registry = registry
        End Sub

        Public Function GetDriverByCode(code As String) As DataSourceDriver
            Return registry.GetReader(code)
        End Function
    End Class
End Namespace