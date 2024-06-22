Public Class TermHashIndex : Inherits SearchIndex

    ReadOnly hashIndex As New Dictionary(Of String, List(Of Integer))

    Public Sub New(documents As DocumentPool)
        MyBase.New(documents)
    End Sub


End Class
