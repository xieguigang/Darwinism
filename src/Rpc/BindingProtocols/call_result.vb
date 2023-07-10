#Region "Microsoft.VisualBasic::03ff050c7d69ea60b7a7204035fd718d, Rpc\BindingProtocols\call_result.vb"

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

'     Class call_result
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes
Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' Results of callit
    ''' http://tools.ietf.org/html/rfc1833#section-3.1
    ''' </summary>
    Public Class call_result
        ''' <summary>
        ''' port of called program
        ''' </summary>
        <Order(0)>
        Public port As UInteger

        ''' <summary>
        ''' result
        ''' </summary>
        <Order(1), Var>
        Public res As Byte()
    End Class
End Namespace
