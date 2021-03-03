Imports System
Imports Rpc.MessageProtocol

Namespace Rpc
    ''' <summary>
    ''' Error received in response RPC message
    ''' </summary>
    Public Class ReplyException
        Inherits RpcException
        ''' <summary>
        ''' Body of a reply to an RPC call for details
        ''' </summary>
        Private _ReplyBody As Rpc.MessageProtocol.reply_body

        Public Property ReplyBody As reply_body
            Get
                Return _ReplyBody
            End Get
            Private Set(ByVal value As reply_body)
                _ReplyBody = value
            End Set
        End Property

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <paramname="replyBody"></param>
        Public Sub New(ByVal replyBody As reply_body)
            Me.ReplyBody = replyBody
        End Sub

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <paramname="replyBody"></param>
        ''' <paramname="message"></param>
        Public Sub New(ByVal replyBody As reply_body, ByVal message As String)
            MyBase.New(message)
            Me.ReplyBody = replyBody
        End Sub

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <paramname="replyBody"></param>
        ''' <paramname="message"></param>
        ''' <paramname="innerEx"></param>
        Public Sub New(ByVal replyBody As reply_body, ByVal message As String, ByVal innerEx As Exception)
            MyBase.New(message, innerEx)
            Me.ReplyBody = replyBody
        End Sub
    End Class
End Namespace
