#Region "Microsoft.VisualBasic::ffdbfff314cbbd439f5d9605be7da648, Rpc\BindingProtocols\netbuf.vb"

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

    '     Class netbuf
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.BindingProtocols
    ''' <summary>
    ''' netbuf structure, used to store the transport specific form of a universal transport address.
    ''' http://tools.ietf.org/html/rfc1833#section-2.1
    ''' </summary>
    Public Class netbuf
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(0)>
        Public maxlen As UInteger
        ''' <summary>
        ''' fixme: missing comment
        ''' </summary>
        <Order(1), Var>
        Public buf As Byte()
    End Class
End Namespace
