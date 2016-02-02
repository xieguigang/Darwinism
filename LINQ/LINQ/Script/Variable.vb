Imports System.Dynamic
Imports Microsoft.VisualBasic.LINQ.Framework

Namespace Script

    Public Class Variable
        Public Property Name As String
        Public Property Data As Object()

        Public Overrides Function ToString() As String
            Return Name
        End Function

        Public Shared Widening Operator CType(v As Object()) As Variable
            Return New Variable With {
                .Name = v(0).ToString,
                .Data = v(1)
            }
        End Operator
    End Class
End Namespace