#Region "Microsoft.VisualBasic::4f25adfd73df3b72fcc481dadc2ceaa1, Rpc\Exceptions\ReplyException.vb"

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

    '     Class ReplyException
    ' 
    '         Properties: ReplyBody
    ' 
    '         Constructor: (+3 Overloads) Sub New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

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
            Private Set(value As reply_body)
                _ReplyBody = value
            End Set
        End Property

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <param name="replyBody"></param>
        Public Sub New(replyBody As reply_body)
            Me.ReplyBody = replyBody
        End Sub

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <param name="replyBody"></param>
        ''' <param name="message"></param>
        Public Sub New(replyBody As reply_body, message As String)
            MyBase.New(message)
            Me.ReplyBody = replyBody
        End Sub

        ''' <summary>
        ''' Error received in response RPC message
        ''' </summary>
        ''' <param name="replyBody"></param>
        ''' <param name="message"></param>
        ''' <param name="innerEx"></param>
        Public Sub New(replyBody As reply_body, message As String, innerEx As Exception)
            MyBase.New(message, innerEx)
            Me.ReplyBody = replyBody
        End Sub
    End Class
End Namespace
