Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.My.JavaScript

Namespace Runtime

    Public MustInherit Class DataSourceDriver

        Public MustOverride Function ReadFromUri(uri As String) As IEnumerable(Of Object)

    End Class

    Public Class DataFrameDriver : Inherits DataSourceDriver

        Public Overrides Iterator Function ReadFromUri(uri As String) As IEnumerable(Of Object)
            Dim dataframe As DataFrame = DataFrame.Load(uri)
            Dim obj As JavaScriptObject
            Dim headers As String() = dataframe.HeadTitles

            For Each row As RowObject In dataframe.Rows
                obj = New JavaScriptObject

                For i As Integer = 0 To headers.Length - 1
                    obj(headers(i)) = row(i)
                Next

                Yield obj
            Next
        End Function
    End Class
End Namespace