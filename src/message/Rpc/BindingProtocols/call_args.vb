﻿#Region "Microsoft.VisualBasic::8308f7e1795515441aaf357dbae81c2a, Rpc\BindingProtocols\call_args.vb"

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

'     Class call_args
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' arguments to callit
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Class call_args
        ''' <summary>
        ''' program
        ''' </summary>
        <Order(0)>
        Public prog As UInteger

        ''' <summary>
        ''' version
        ''' </summary>
        <Order(1)>
        Public vers As UInteger

        ''' <summary>
        ''' procedure
        ''' </summary>
        <Order(2)>
        Public proc As UInteger

        ''' <summary>
        ''' arguments
        ''' </summary>
        <Order(3), Var>
        Public args As Byte()
    End Class
End Namespace
