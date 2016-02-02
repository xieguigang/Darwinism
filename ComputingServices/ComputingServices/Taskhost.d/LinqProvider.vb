Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Net
Imports Microsoft.VisualBasic.Net.Protocol
Imports Microsoft.VisualBasic.Net.Protocol.Reflection
Imports Microsoft.VisualBasic.Serialization

Namespace TaskHost

    ''' <summary>
    ''' 执行得到数据集合然后分独传输数据元素
    ''' </summary>
    ''' 
    <Protocol(GetType(TaskProtocols))>
    Public Class LinqProvider

        ReadOnly __host As TcpSynchronizationServicesSocket =
            New TcpSynchronizationServicesSocket(GetFirstAvailablePort)
        ReadOnly _type As Type
        ReadOnly _source As Iterator
        ReadOnly _local As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="type">Element's <see cref="System.Type">type</see> in the <paramref name="source"/></param>
        Sub New(source As IEnumerable, type As Type, Optional local As Boolean = True)
            __host.Responsehandler = AddressOf New ProtocolHandler(Me).HandleRequest
            _type = type
            _source = New Iterator(source)
            _local = local

            Call Parallel.Run(AddressOf __host.Run)
        End Sub

        Public ReadOnly Property Portal As IPEndPoint
            Get
                If _local Then
                    Return New IPEndPoint(AsynInvoke.LocalIPAddress, __host.LocalPort)
                Else
                    Return New IPEndPoint(GetMyIPAddress, __host.LocalPort)
                End If
            End Get
        End Property

        Public Shared Function CreateObject(Of T)(source As IEnumerable(Of T)) As LinqProvider
            Return New LinqProvider(source, GetType(T))
        End Function

        Public Function GetReturns() As Returns
            Return New Returns(Portal, GetType(IPEndPoint))
        End Function

        <Protocol(TaskProtocols.MoveNext)>
        Private Function __moveNext(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Dim value As Object = _source.Current
            Dim readEnds As Boolean = _source.MoveNext()
            Dim json As String = Serialization.GetJson(value, _type)
            Dim flag As Long = If(Not readEnds, Protocols.TaskProtocols.ReadsDone, HTTP_RFC.RFC_OK)
            Return New RequestStream(flag, flag, json)
        End Function

        <Protocol(TaskProtocols.Reset)>
        Private Function __reset(CA As Long, args As RequestStream, remote As System.Net.IPEndPoint) As RequestStream
            Call _source.Reset()
            Return NetResponse.RFC_OK
        End Function
    End Class
End Namespace