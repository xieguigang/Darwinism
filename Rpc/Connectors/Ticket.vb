#Region "Microsoft.VisualBasic::20a4f53dbbf2bdf65ad3d3dfccf0297b, Rpc\Connectors\Ticket.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:

'     Class Ticket
' 
'         Properties: Task, Xid
' 
'         Constructor: (+1 Overloads) Sub New
'         Sub: BuildRpcMessage, Cancel, Except, ReadResult
' 
' 
' /********************************************************************************/

#End Region

Imports System
Imports System.IO.XDR.Reading
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.Data.IO
Imports Rpc.MessageProtocol

Namespace Rpc.Connectors
    Friend Class Ticket(Of TReq, TResp)
        Implements ITicket

        Private _owner As ITicketOwner
        Private _callBody As call_body
        Private _reqArgs As TReq
        Private _taskSrc As TaskCompletionSource(Of TResp)
        Private _ctr As CancellationTokenRegistration
        Public Property Xid As UInteger Implements ITicket.Xid

        Public Sub New(owner As ITicketOwner, callBody As call_body, reqArgs As TReq, options As TaskCreationOptions, token As CancellationToken)
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

        Public Sub BuildRpcMessage(bw As IByteWriter) Implements ITicket.BuildRpcMessage
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

        Public Sub ReadResult(mr As IMsgReader, r As Reader, respMsg As rpc_msg) Implements ITicket.ReadResult
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

        Public Sub Except(ex As Exception) Implements ITicket.Except
            _ctr.Dispose()
            _taskSrc.TrySetException(ex)
        End Sub

        Private Sub Cancel()
            If _taskSrc.TrySetCanceled() Then _owner.RemoveTicket(Me)
        End Sub
    End Class
End Namespace
