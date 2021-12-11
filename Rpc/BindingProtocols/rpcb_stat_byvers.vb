#Region "Microsoft.VisualBasic::db624fd5fcb3954855677818410712d3, Rpc\BindingProtocols\rpcb_stat_byvers.vb"

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

'     Class rpcb_stat_byvers
' 
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO.XDR.Attributes

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' One rpcb_stat structure is returned for each version of rpcbind being monitored.
    ''' Provide only for rpcbind V2, V3 and V4.
    ''' typedef rpcb_stat rpcb_stat_byvers[RPCBVERS_STAT];
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_stat_byvers
        ''' <summary>
        ''' rpcbind V2 statistics
        ''' </summary>
        <Order(0)>
        Public V2 As rpcb_stat
        ''' <summary>
        ''' rpcbind V3 statistics
        ''' </summary>
        <Order(1)>
        Public V3 As rpcb_stat
        ''' <summary>
        ''' rpcbind V4 statistics
        ''' </summary>
        <Order(2)>
        Public V4 As rpcb_stat
    End Class
End Namespace
