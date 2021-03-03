Imports System
Imports Rpc.MessageProtocol

Namespace Rpc
    ''' <summary>
    ''' Authenticate error
    ''' </summary>
    Public Class AuthenticateException
        Inherits ReplyException
        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <paramname="replyBody"></param>
        Public Sub New(ByVal replyBody As reply_body)
            MyBase.New(replyBody)
        End Sub

        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <paramname="replyBody"></param>
        ''' <paramname="message"></param>
        Public Sub New(ByVal replyBody As reply_body, ByVal message As String)
            MyBase.New(replyBody, message)
        End Sub

        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <paramname="replyBody"></param>
        ''' <paramname="message"></param>
        ''' <paramname="innerEx"></param>
        Public Sub New(ByVal replyBody As reply_body, ByVal message As String, ByVal innerEx As Exception)
            MyBase.New(replyBody, message, innerEx)
        End Sub
    End Class
End Namespace
