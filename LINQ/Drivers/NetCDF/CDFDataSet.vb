Imports LINQ.Runtime.Drivers

<DriverFlag("dataframe")>
Public Class CDFDataSet : Inherits DataSourceDriver

    Public Sub New(arguments() As String)
        MyBase.New(arguments)
    End Sub

    Public Overrides Function ReadFromUri(uri As String) As IEnumerable(Of Object)
        Throw New NotImplementedException()
    End Function
End Class
