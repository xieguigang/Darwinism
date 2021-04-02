#Region "Microsoft.VisualBasic::76ccd68197ecb13e4206b4aeb7478c08, Rpc\BindingProtocols\rpcbs_addr.vb"

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

    '     Class rpcbs_addr
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' the stat about getport and getaddr
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class rpcbs_addr
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
        Public success As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(3)>
        Public failure As Integer
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(4), Var>
        Public netid As String
    End Class
End Namespace

