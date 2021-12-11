#Region "Microsoft.VisualBasic::4083fbf912c7f2085145f8c3f8e8ab25, Rpc\BindingProtocols\rpcb_rmtcallargs.vb"

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

'     Class rpcb_rmtcallargs
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Arguments of remote calls
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_rmtcallargs
        ''' <summary>
        ''' program number
        ''' </summary>
        <Order(0)>
        Public prog As ULong
        ''' <summary>
        ''' version number
        ''' </summary>
        <Order(1)>
        Public vers As ULong
        ''' <summary>
        ''' procedure number
        ''' </summary>
        <Order(2)>
        Public proc As ULong
        ''' <summary>
        ''' argument
        ''' </summary>
        <Order(3), Var>
        Public args As Byte()
    End Class
End Namespace
