Imports System.Threading
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Parallel.Tasks

Namespace TaskHost

    Public Class LinqPool : Implements IDisposable

        ''' <summary>
        ''' linq池
        ''' </summary>
        ReadOnly __linq As New Dictionary(Of String, LinqProvider)
        ReadOnly __openQuerys As New TaskQueue(Of IPEndPoint)

        Public Function GetLinq(uid As String) As LinqProvider
            Return __linq(uid)
        End Function

        ''' <summary>
        ''' uid参数是Linq Portal的Tostring函数的结果
        ''' </summary>
        ''' <param name="uid"></param>
        Public Sub Free(uid As String)
            If Not __linq.ContainsKey(uid) Then Return

            Dim x As LinqProvider = __linq(uid)
            Call x.Free  ' 释放Linq数据源的指针

            SyncLock __linq
                Call __linq.Remove(uid)  ' 从哈希表之中移除数据源释放服务器资源
            End SyncLock
        End Sub

        ''' <summary>
        '''
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="type">
        ''' 这里应该是集合的类型，函数会自动从这个类型信息之中得到元素的类型；
        ''' 假若函数获取得到集合之中的元素的类型失败的话，则会直接使用所传入的类型参数作为集合之中的元素类型
        ''' </param>
        ''' <returns></returns>
        Public Function OpenQuery(source As IEnumerable, type As Type) As IPEndPoint
            Dim elType As Type = type.GetTypeElement(True)
            If elType Is Nothing Then
                elType = type
            End If

            Dim task As New __openTask(__linq) With {
                .elType = elType,
                .source = source
            }
            Return __openQuerys.Join(AddressOf task.OpenQuery)
        End Function

        Private Class __openTask

            ReadOnly __linq As Dictionary(Of String, LinqProvider)

            Public source As IEnumerable
            Public elType As Type

            Sub New(ByRef linq As Dictionary(Of String, LinqProvider))
                __linq = linq
            End Sub

            Public Function OpenQuery() As IPEndPoint
                Dim linq As New LinqProvider(source, elType)  ' 创建 Linq 数据源
                Dim portal As IPEndPoint = linq.Portal
                Dim uid As String = portal.ToString

                Call __linq.Add(portal.ToString, linq)  ' 数据源添加入哈希表之中

                Return portal
            End Function
        End Class

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