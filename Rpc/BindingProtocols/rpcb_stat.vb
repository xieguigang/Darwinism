#Region "Microsoft.VisualBasic::4d1e0b765a4526667133f55c9cab334e, Rpc\BindingProtocols\rpcb_stat.vb"

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

    '     Class rpcb_stat
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Collections.Generic
Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' rpcbind statistics
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcb_stat
        ''' <summary>
        ''' # of procs in rpcbind V4 plus one
        ''' </summary>
        Public Const RPCBSTAT_HIGHPROC As UInteger = 13

        ''' <summary>
        ''' number of procedure calls by numbers
        ''' </summary>
        <Order(0), Fix(RPCBSTAT_HIGHPROC)>
        Public info As Integer()
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1)>
        Public setinfo As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(2)>
        Public unsetinfo As Integer
        ''' <summary>
        ''' list of all the stats about getport and getaddr
        ''' </summary>
        <Order(3)>
        Public addrinfo As List(Of rpcbs_addr)
        ''' <summary>
        ''' list of all the stats about rmtcall
        ''' </summary>
        <Order(4)>
        Public rmtinfo As List(Of rpcbs_rmtcall)
    End Class
End Namespace
