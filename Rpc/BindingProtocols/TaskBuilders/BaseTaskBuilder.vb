#Region "Microsoft.VisualBasic::1e9d6c6c0c23218b8562f5d016091a90, Rpc\BindingProtocols\TaskBuilders\BaseTaskBuilder.vb"

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

    '     Class BaseTaskBuilder
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateHeader, CreateTask
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
