Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository

Namespace Runtime

    Public Class Symbol : Implements INamedValue

        Public Property SymbolKey As String Implements IKeyedEntity(Of String).Key

    End Class
End Namespace