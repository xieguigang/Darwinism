Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComputingServices.ComponentModel
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocols
Imports Microsoft.VisualBasic.Net.Protocols.Reflection
Imports Microsoft.VisualBasic.Serialization
Imports Microsoft.VisualBasic.Parallel
Imports Microsoft.VisualBasic.Scripting.InputHandler

Namespace TaskHost

    ''' <summary>
    ''' 执行得到数据集合然后分独传输数据元素
    ''' </summary>
    ''' 
    <Protocol(GetType(TaskProtocols))>
    Public Class LinqProvider : Inherits IHostBase
        Implements IDisposable

        ReadOnly _arrayType As Type
        ReadOnly _type As Type
        ReadOnly _source As Iterator
        ReadOnly _local As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="type">Element's <see cref="System.Type">type</see> in the <paramref name="source"/></param>
        Sub New(source As IEnumerable, type As Type, Optional local As Boolean = True)
            Call MyBase.New(Net.GetFirstAvailablePort)

            _type = type
            _source = New Iterator(source)
            _local = local
            __host.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
            _arrayType = type.MakeArrayType

            Call Runtask(AddressOf __host.Run)
        End Sub

        Public Overrides ReadOnly Property Portal As IPEndPoint
            Get
                Return Me.GetPortal(_local)
            End Get
        End Property

        Public Shared Function CreateObject(Of T)(source As IEnumerable(Of T)) As LinqProvider
            Return New LinqProvider(source, GetType(T))
        End Function

        Public Function GetReturns() As Rtvl
            Return New Rtvl(Portal, GetType(IPEndPoint))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="n">
        ''' 个数小于或者等于1，就只会返回一个对象；
        ''' 个数大于1的，会读取相应数量的元素然后返回一个集合类型
        ''' </param>
        ''' <param name="readDone"></param>
        ''' <returns></returns>
        Public Function Moves(n As Integer, ByRef readDone As Boolean) As Object
            If n <= 1 Then
                Dim value As Object = _source.Current
                readDone = _source.MoveNext()
                Return value
            Else
                Dim list As New List(Of Object)
                For i As Integer = 0 To n - 1
                    Call list.Add(_source.Current)
                    readDone = _source.MoveNext
                Next
                Return [DirectCast](list.ToArray, _type)
            End If
        End Function

        <Protocol(TaskProtocols.MoveNext)>
        Private Function __moveNext(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim readEnds As Boolean
            Dim value As Object = Moves(1, readEnds)
            Dim json As String = Serialization.GetJson(value, _type)
            Dim flag As Long = If(Not readEnds, Protocols.TaskProtocols.ReadsDone, HTTP_RFC.RFC_OK)
            Return New RequestStream(flag, flag, json)
        End Function

        <Protocol(TaskProtocols.Reset)>
        Private Function __reset(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Call _source.Reset()
            Return NetResponse.RFC_OK
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Call _source.Free
                    Call __host.Free
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