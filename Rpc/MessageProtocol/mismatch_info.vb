#Region "Microsoft.VisualBasic::84d2797e504b3b24da51cc30e29b226a, Rpc\MessageProtocol\mismatch_info.vb"

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

'     Class mismatch_info
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' the lowest and highest version numbers of the remote program supported by the server
    ''' http://tools.ietf.org/html/rfc5531#section-9
    ''' </summary>
    Public Class mismatch_info
        ''' <summary>
        ''' lowest version number
        ''' </summary>
        <Order(0)>
        Public low As UInteger

        ''' <summary>
        ''' highest version number
        ''' </summary>
        <Order(1)>
        Public high As UInteger
    End Class
End Namespace
