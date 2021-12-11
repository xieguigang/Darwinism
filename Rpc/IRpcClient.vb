#Region "Microsoft.VisualBasic::669a88225bdf5b200a70b0664ce5bda1, Rpc\IRpcClient.vb"

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

    '     Interface IRpcClient
    ' 
    '         Function: CreateTask
    ' 
    '         Sub: Close
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
        ''' <param name="callBody">call body structure</param>
        ''' <param name="reqArgs">instance of request arguments</param>
        ''' <param name="options">task creation options</param>
        ''' <param name="token">cancellation token</param>
        ''' <typeparamname="TReq">type of request</typeparam>
        ''' <typeparamname="TResp">type of response</typeparam>
        Function CreateTask(Of TReq, TResp)(callBody As call_body, reqArgs As TReq, options As TaskCreationOptions, token As CancellationToken) As Task(Of TResp)
        ''' <summary>
        ''' Close this connection and cancel all queued tasks
        ''' </summary>
        Sub Close()
    End Interface
End Namespace
