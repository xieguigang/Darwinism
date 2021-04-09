#Region "Microsoft.VisualBasic::4c0d0aa5a6ac02fc9576e0bb604ba151, Rpc\BindingProtocols\rpcbs_rmtcall.vb"

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

    '     Class rpcbs_rmtcall
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' the stats about rmtcall
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcbs_rmtcall
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(0)>
        Public prog As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1)>
        Public vers As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(2)>
        Public proc As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(3)>
        Public success As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(4)>
        Public failure As Integer
        ''' <summary>
        ''' whether callit or indirect
        ''' </summary>
        <Order(5)>
        Public indirect As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(6), Var>
        Public netid As String
    End Class
End Namespace
