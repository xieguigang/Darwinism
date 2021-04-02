#Region "Microsoft.VisualBasic::18e6426cd429f1800568feab377d4a0b, Rpc\MessageProtocol\opaque_auth.vb"

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

    '     Structure opaque_auth
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.IO.Xdr

Namespace Rpc.MessageProtocol
    ''' <summary>
    ''' 
    ''' http://tools.ietf.org/html/rfc5531#section-8.2
    ''' </summary>
    Public Structure opaque_auth
        ''' <summary>
        ''' Null Authentication
        ''' http://tools.ietf.org/html/rfc5531#section-10.1
        ''' </summary>
        Public Shared ReadOnly None As opaque_auth = New opaque_auth() With {
            .flavor = auth_flavor.AUTH_NONE,
            .body = New Byte(-1) {}
        }

        ''' <summary>
        ''' Authentication flavor
        ''' </summary>
        <Order(0)>
        Public flavor As auth_flavor

        ''' <summary>
        ''' The interpretation and semantics of the data contained within the 
        ''' authentication fields are specified by individual, independent
        ''' authentication protocol specifications.
        ''' </summary>
        <Order(1), Var(400)>
        Public body As Byte()
    End Structure
End Namespace

