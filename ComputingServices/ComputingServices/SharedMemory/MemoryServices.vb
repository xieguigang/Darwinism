Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Serialization

Namespace SharedMemory

    ''' <summary>
    ''' Shared the memory with the remote machine.
    ''' </summary>
    Public Class MemoryServices : Implements IDisposable

        ''' <summary>
        ''' Gets the memory data from remote machine.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="name">建议使用NameOf来设置或者获取参数值</param>
        ''' <returns></returns>
        Public Function GetValue(Of T)(name As String) As T
            Return __remote.ReadValue(Of T)(name)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="name">建议使用NameOf来设置或者获取参数值</param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        Public Function SetValue(Of T)(name As String, value As T) As Boolean
            Return __remote.WriteValue(name, value)
        End Function

        ReadOnly __remote As IPEndPoint
        ReadOnly __localSvr As SharedSvr

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="remote"></param>
        ''' <param name="local">local services port that provides to the remote</param>
        Sub New(remote As IPEndPoint, local As Integer)
            __remote = remote
            __localSvr = New SharedSvr(local)
        End Sub

        Public Function Allocate(name As String, value As Object, Optional [overrides] As Boolean = False) As Boolean
            Return __localSvr.Allocate(name, value, [overrides])
        End Function

        Public Overrides Function ToString() As String
            Return __remote.GetJson
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call __localSvr.Dispose()
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