Imports System.Dynamic
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.LINQ.Framework

Namespace Script

    Public Class Variable : Implements sIdEnumerable

        Public Property Name As String Implements sIdEnumerable.Identifier
        Public Property Data As IEnumerable

        Public Overrides Function ToString() As String
            Return Name
        End Function
    End Class
End Namespace