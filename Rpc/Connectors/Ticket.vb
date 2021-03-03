Imports System
Imports System.Threading
Imports System.Threading.Tasks
Imports Rpc.MessageProtocol
Imports Xdr

Namespace Rpc.Connectors
    Friend Class Ticket(Of TReq, TResp)
        Implements ITicket

        Private _owner As ITicketOwner
        Private _callBody As call_body
        Private _reqArgs As TReq
        Private _taskSrc As TaskCompletionSource(Of TResp)
        Private _ctr As CancellationTokenRegistration
        Public Property Xid As UInteger Implements ITicket.Xid

        Public Sub New(ByVal owner As ITicketOwner, ByVal callBody As call_body, ByVal reqArgs As TReq, ByVal options As TaskCreationOptions, ByVal token As CancellationToken)
            _owner = owner
            _callBody = callBody
            _reqArgs = reqArgs
            _taskSrc = New TaskCompletionSource(Of TResp)(options)

            If token.CanBeCanceled Then
                _ctr = token.Register(New Action(AddressOf Cancel))
            Else
                _ctr = New CancellationTokenRegistration()
            End If
        End Sub

        Public ReadOnly Property Task As Task(Of TResp)
            Get
                Return _taskSrc.Task
            End Get
        End Property

        Public Sub BuildRpcMessage(ByVal bw As IByteWriter) Implements ITicket.BuildRpcMessage
            Dim reqHeader As rpc_msg = New rpc_msg() With {
                .xid = Xid,
                .body = New body() With {
                    .mtype = msg_type.CALL,
                    .cbody = _callBody
                }
            }
            Dim xw = CreateWriter(bw)
            xw.Write(reqHeader)
            xw.Write(_reqArgs)
            _callBody = Nothing
            _reqArgs = Nothing
        End Sub

        Public Sub ReadResult(ByVal mr As IMsgReader, ByVal r As Reader, ByVal respMsg As rpc_msg) Implements ITicket.ReadResult
            _ctr.Dispose()

            Try
                ReplyMessageValidate(respMsg)
                Dim respArgs As TResp = r.Read(Of TResp)()
                mr.CheckEmpty()
                _taskSrc.TrySetResult(respArgs)
            Catch ex As Exception
                _taskSrc.TrySetException(ex)
            End Try
        End Sub

        Public Sub Except(ByVal ex As Exception) Implements ITicket.Except
            _ctr.Dispose()
            _taskSrc.TrySetException(ex)
        End Sub

        Private Sub Cancel()
            If _taskSrc.TrySetCanceled() Then _owner.RemoveTicket(Me)
        End Sub
    End Class
End Namespace
