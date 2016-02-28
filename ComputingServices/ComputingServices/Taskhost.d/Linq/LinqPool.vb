Imports Microsoft.VisualBasic.Net

Namespace TaskHost

    Public Class LinqPool : Implements IDisposable

        ''' <summary>
        ''' linq池
        ''' </summary>
        ReadOnly __linq As New Dictionary(Of String, LinqProvider)

        Public Sub Free(uid As String)
            If Not __linq.ContainsKey(uid) Then Return

            Dim x As LinqProvider = __linq(uid)
            Call x.Free  ' 释放Linq数据源的指针
            Call __linq.Remove(uid)  ' 从哈希表之中移除数据源释放服务器资源
        End Sub

        Public Function OpenQuery(source As IEnumerable, type As Type) As IPEndPoint
            Dim linq As New LinqProvider(source, type.GetArrayElement(True), False)  ' 创建 Linq 数据源
            Dim portal As IPEndPoint = linq.Portal
            Call __linq.Add(portal.ToString, linq)  ' 数据源添加入哈希表之中
            Return portal
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).

                    For Each x In __linq
                        Call Free(x.Key)
                    Next

                    Call __linq.Free
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace