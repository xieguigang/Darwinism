Imports System.Threading
Imports System.Threading.Tasks
Imports Rpc.MessageProtocol

Namespace Rpc
    ''' <summary>
    ''' creates tasks for the control request to the RPC server
    ''' </summary>
    Public Interface IRpcClient
        ''' <summary>
        ''' creates the task for the control request to the RPC server
        ''' </summary>
        ''' <returns>
        ''' the runned task
        ''' </returns>
        ''' <paramname="callBody">call body structure</param>
        ''' <paramname="reqArgs">instance of request arguments</param>
        ''' <paramname="options">task creation options</param>
        ''' <paramname="token">cancellation token</param>
        ''' <typeparamname="TReq">type of request</typeparam>
        ''' <typeparamname="TResp">type of response</typeparam>
        Function CreateTask(Of TReq, TResp)(ByVal callBody As call_body, ByVal reqArgs As TReq, ByVal options As TaskCreationOptions, ByVal token As CancellationToken) As Task(Of TResp)
        ''' <summary>
        ''' Close this connection and cancel all queued tasks
        ''' </summary>
        Sub Close()
    End Interface
End Namespace
