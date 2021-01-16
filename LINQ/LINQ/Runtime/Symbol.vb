Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository

Namespace Runtime

    Public Class Symbol : Implements INamedValue

        Public Property SymbolKey As String Implements IKeyedEntity(Of String).Key
        Public Property type As String
        Public Property value As Object

        Public ReadOnly Property valueType As Type
            Get
                If value Is Nothing Then
                    Return GetType(Void)
                Else
                    Return value.GetType
                End If
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return $"let {SymbolKey} as {type} = ({valueType.Name.ToLower}) {value}"
        End Function

    End Class
End Namespace