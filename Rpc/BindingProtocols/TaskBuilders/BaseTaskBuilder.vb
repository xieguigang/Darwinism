Imports System.Threading
Imports System.Threading.Tasks
Imports Rpc.MessageProtocol

Namespace Rpc.BindingProtocols.TaskBuilders
    ''' <summary>
    ''' operations of binding protocols
    ''' </summary>
    Public MustInherit Class BaseTaskBuilder
        Private Const Program As UInteger = 100000UI
        Private _conn As IRpcClient
        Private _attachedToParent As Boolean
        Private _token As CancellationToken

        Friend Sub New(ByVal conn As IRpcClient, ByVal token As CancellationToken, ByVal attachedToParent As Boolean)
            _conn = conn
            _attachedToParent = attachedToParent
            _token = token
        End Sub

        ''' <summary>
        ''' Gets the version of protocol
        ''' </summary>
        Protected MustOverride ReadOnly Property Version As UInteger

        Private Function CreateHeader(ByVal procNum As UInteger) As call_body
            Return New call_body() With {
                .rpcvers = 2,
                .prog = Program,
                .proc = procNum,
                .vers = Version,
                .cred = opaque_auth.None,
                .verf = opaque_auth.None
            }
        End Function

        ''' <summary>
        ''' Creates the task of request.
        ''' </summary>
        ''' <returns>
        ''' The queued task.
        ''' </returns>
        ''' <paramname="proc">
        ''' procedure number
        ''' </param>
        ''' <paramname="args">
        ''' instance of arguments of request
        ''' </param>
        ''' <typeparamname="TReq">
        ''' type of request
        ''' </typeparam>
        ''' <typeparamname="TResp">
        ''' type of response
        ''' </typeparam>
        Protected Function CreateTask(Of TReq, TResp)(ByVal proc As UInteger, ByVal args As TReq) As Task(Of TResp)
            Return _conn.CreateTask(Of TReq, TResp)(CreateHeader(proc), args, If(_attachedToParent, TaskCreationOptions.AttachedToParent, TaskCreationOptions.None), _token)
        End Function
    End Class
End Namespace
