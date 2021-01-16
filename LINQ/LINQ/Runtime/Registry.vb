Namespace Runtime

    Public Class Registry

        Public Function GetTypeCodeName(type As Type) As String
            Select Case type
                Case GetType(Integer) : Return "i32"
                Case GetType(Long) : Return "i64"
                Case Else
                    Throw New MissingPrimaryKeyException
            End Select
        End Function

        Public Function GetReader(type As String) As DataSourceDriver
            If type = "row" Then
                Return New CsvDataFrameDriver
            Else
                Throw New MissingPrimaryKeyException
            End If
        End Function
    End Class
End Namespace

