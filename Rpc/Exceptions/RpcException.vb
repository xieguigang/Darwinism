Imports System

Namespace Rpc
    ''' <summary>
    ''' Error associated with work on RPC protocol
    ''' </summary>
    Public Class RpcException
        Inherits SystemException
        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        ''' <paramname="message"></param>
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        ''' <summary>
        ''' Error associated with work on RPC protocol
        ''' </summary>
        ''' <paramname="message"></param>
        ''' <paramname="innerEx"></param>
        Public Sub New(ByVal message As String, ByVal innerEx As Exception)
            MyBase.New(message, innerEx)
        End Sub
    End Class
End Namespace
