#Region "Microsoft.VisualBasic::48c2bdf3c92b618bdcde81c3d468fb1d, Rpc\Exceptions\AuthenticateException.vb"

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

    '     Class AuthenticateException
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
    ''' Authenticate error
    ''' </summary>
    Public Class AuthenticateException
        Inherits ReplyException
        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <param name="replyBody"></param>
        Public Sub New(replyBody As reply_body)
            MyBase.New(replyBody)
        End Sub

        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <param name="replyBody"></param>
        ''' <param name="message"></param>
        Public Sub New(replyBody As reply_body, message As String)
            MyBase.New(replyBody, message)
        End Sub

        ''' <summary>
        ''' Authenticate error
        ''' </summary>
        ''' <param name="replyBody"></param>
        ''' <param name="message"></param>
        ''' <param name="innerEx"></param>
        Public Sub New(replyBody As reply_body, message As String, innerEx As Exception)
            MyBase.New(replyBody, message, innerEx)
        End Sub
    End Class
End Namespace
