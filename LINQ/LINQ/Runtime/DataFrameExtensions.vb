Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.My.JavaScript
Imports any = Microsoft.VisualBasic.Scripting

Namespace Runtime

    Public Module DataFrameExtensions

        Public Function CreateTableDataSet(output As JavaScriptObject()) As DataFrame
            Dim allNames As String() = output.Select(Function(obj) obj.GetNames).IteratesALL.Distinct.ToArray
            Dim dataset As New DataFrame(allNames)
            Dim row As RowObject

            For Each item As JavaScriptObject In output
                row = allNames.Select(Function(name) any.ToString(item(name))).DoCall(Function(cells) New RowObject(cells))
                dataset.AppendLine(row)
            Next

            Return dataset.MeasureTypeSchema
        End Function
    End Module
End Namespace