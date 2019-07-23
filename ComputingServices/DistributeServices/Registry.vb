Imports Microsoft.VisualBasic.Net

Namespace DistributeServices

    ''' <summary>
    ''' The node registry in current grid
    ''' </summary>
    Public Class Registry : Implements IEnumerable(Of IPEndPoint)

        ''' <summary>
        ''' 这是一个网络范围,不是一个具体的IP端点
        ''' </summary>
        ReadOnly grid As IPEndPoint

        Sub New(ipRange$, port%)
            grid = New IPEndPoint(ipRange, port)
        End Sub

        Public Sub doScanForNodes()

        End Sub

        Public Function GetEnumerator() As IEnumerator(Of IPEndPoint) Implements IEnumerable(Of IPEndPoint).GetEnumerator
            Throw New NotImplementedException()
        End Function

        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace