#Region "Microsoft.VisualBasic::1fde0edb903700a2434d2a359f6875d1, Rpc\MessageProtocol\call_body.vb"

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

'     Class call_body
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' Body of an RPC call
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class call_body
        ''' <summary>
        ''' MUST be equal to 2
        ''' </summary>
        <Order(0)>
        Public rpcvers As UInteger

        ''' <summary>
        ''' the remote program
        ''' </summary>
        <Order(1)>
        Public prog As UInteger

        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(2)>
        Public vers As UInteger

        ''' <summary>
        ''' the procedure within the remote program to be called
        ''' </summary>
        <Order(3)>
        Public proc As UInteger

        ''' <summary>
        ''' authentication credential
        ''' </summary>
        <Order(4)>
        Public cred As opaque_auth

        ''' <summary>
        ''' authentication verifier
        ''' </summary>
        <Order(5)>
        Public verf As opaque_auth
    End Class
End Namespace
